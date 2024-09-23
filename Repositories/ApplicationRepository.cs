using EmployMe.Data;
using EmployMe.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployMe.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly EmployMeAuthDbContext _context;

        public ApplicationRepository(EmployMeAuthDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> CreateApplicationAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
            return await SaveAsync();
        }

        public async Task<bool> DeleteApplicationAsync(int empolyeeId, int jobId)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.JobId == jobId && a.EmployeeId == empolyeeId);

            if (application == null)
                return false;

            _context.Applications.Remove(application);
            return await SaveAsync();
        }

        public async Task<ICollection<Application>> GetAllApplicationsAsync(bool? getAccepted)
        {
             var applications = _context.Applications
                .Include(a => a.Employee)
                .Include(a => a.Job).ThenInclude(j => j.Company).AsQueryable();

            var applicationsDM = getAccepted == null ? await applications.ToListAsync()
                : getAccepted == true ? await applications.Where(a => a.Accepted == true).ToListAsync()
                : await applications.Where(a => a.Accepted == false).ToListAsync();

            return applicationsDM;

        }

        public async Task<ICollection<Employee>?> GetApplicantsByJobAsync(int jobId)
        {
            if (!await _context.jobs.AnyAsync(j => j.Id == jobId))
                return null;
            var applicants = await _context.Applications.Include("Employee")
                .Where(a => a.JobId == jobId).Select(a => a.Employee).ToListAsync();

            return applicants;
        }

        public async Task<ICollection<Application>?> GetCompanyApplicationsAsync(int companyId)
        {
            // Check if company exists
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            if (company == null)
                return null;

            var applications = await _context.Applications
                .Include(a => a.Employee)
                .Include(a => a.Job)
                .Where(a => a.Job.CompanyId == companyId).ToListAsync();
            
            return applications;
        }

        public async Task<ICollection<AvailableJob>?> GetJobsByEmployeeAsync(int employeeId)
        {
            if (!await _context.Employees.AnyAsync(e => e.Id == employeeId))
                return null;

            var jobs = await _context.Applications.Include(a => a.Job).ThenInclude(j => j.Company)
                .Where(a => a.EmployeeId == employeeId).Select(a => a.Job).ToListAsync();

            return jobs;
        }

        public async Task<Application?> GetApplicationAsync(int empId, int jobId)
        {
            var application = await _context.Applications
                .Include(a => a.Employee).
                Include(a => a.Job).ThenInclude(j => j.Company).
                FirstOrDefaultAsync(a => a.EmployeeId == empId && a.JobId == jobId);
            if (application is null)
                return null;
            return application;
        }

        public async Task<bool> AcceptApplicantAsync(int empId, int jobId)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.EmployeeId == empId && a.JobId == jobId);

            if (application is null)
                return false;

            application.Accepted = true;
            return await SaveAsync();
        }
    }
}
