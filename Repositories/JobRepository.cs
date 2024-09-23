using EmployMe.Data;
using EmployMe.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace EmployMe.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly EmployMeAuthDbContext _context;

        public JobRepository(EmployMeAuthDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
        public async Task<bool?> CreateJobAsync(AvailableJob job)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _context.jobs.AddAsync(job);
                    var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == job.CompanyId && x.active);
                    if (company == null)
                    {
                        return null;
                    }
                    if (company.Max_Vacancies == 0)
                    {
                        return null;
                    }
                    company.Max_Vacancies = company.Max_Vacancies - 1;

                    // Save changes to database
                    var result = await SaveAsync();

                    // Complete the transaction
                    transactionScope.Complete();

                    return result;
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed
                    // If an exception occurs, the transaction will automatically be rolled back
                    return null;
                }
            }
        }
        public async Task<bool> DeleteJobAsync(int id)
        {
            var job = await _context.jobs.FirstOrDefaultAsync(j => j.Id == id);
            if (job is null)
                return false;

            _context.jobs.Remove(job);
            return await SaveAsync();
        }
        public async Task<ICollection<AvailableJob>> GetAllJobsAsync(String? query = null)
        {
            var jobs =  _context.jobs.Include(j => j.Company).AsQueryable();

            if(!String.IsNullOrEmpty(query))
            {
                jobs = jobs.Where(j => j.Name.ToLower().Contains(query.ToLower()));
            }
            return await jobs.ToListAsync();
        }
        public async Task<ICollection<AvailableJob>> GetAllJobsCompanyAsync(int CompanyId, String? query = null)
        {
            var jobs = _context.jobs.Where(j => j.CompanyId == CompanyId).AsQueryable();

            if (!String.IsNullOrEmpty(query))
            {
                jobs = jobs.Where(j => j.Name.ToLower().Contains(query.ToLower()));
            }
            return await jobs.ToListAsync();
        }
        public async Task<AvailableJob?> GetJobAsync(int id)
        {
            //var job = await _context.jobs.FirstOrDefaultAsync(j => j.Id == id);
            var job = await _context.jobs.Include(j => j.Company).FirstOrDefaultAsync(j => j.Id == id);
            if (job is null)
                return null;
            return job;
        }
        public async Task<bool> UpdateJobAsync(int id, AvailableJob job)
        {
            var jobToUpdate = await _context.jobs.FirstOrDefaultAsync(j => j.Id == id);
            if (jobToUpdate is null)
                return false;

            jobToUpdate.Duties = job.Duties;
            jobToUpdate.Qualifications = job.Qualifications;
            jobToUpdate.Description = job.Description;
            jobToUpdate.Name = job.Name;

            return await SaveAsync();
        }
    }
}
