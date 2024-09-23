
using EmployMe.Data;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO.CompanyDto;
using Microsoft.EntityFrameworkCore;

namespace EmployMe.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        
        private readonly EmployMeAuthDbContext dbContext;

        public CompanyRepository(EmployMeAuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Company>> GetAllAsync()
        {
            return await dbContext.Companies.ToListAsync();
        }
        public async Task<List<Company>> GetAllActiveAsync()
        {
            return await dbContext.Companies.Where(c => c.active).Include(company => company.Profile).Include(company => company.Account).ToListAsync();
        }
        public async Task<List<Company>> GetAllInactiveAsync()
        {
            return await dbContext.Companies.Where(c => !c.active).Include(company => company.Profile).Include(company => company.Account).ToListAsync();
        }
        public async Task<List<Company>> GetAllInfoAsync()
        {
            return await dbContext.Companies.Include(company => company.Profile).Include(company => company.Account).ToListAsync();
        }
        public async Task<Company?> GetByIdAsync(int id)
        {
            return await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id && x.active);
        }
        public async Task<Company?> GetByIdInfoAsync(int id)
        {
            return await dbContext.Companies.Include(company => company.Profile).Include(company => company.Account).FirstOrDefaultAsync(x => x.Id == id && x.active);
        }
        public async Task<Company?> GetByAccountIdAsync(string id)
        {
            return await dbContext.Companies.FirstOrDefaultAsync(r => r.AccountId == id);
        }
        public async Task<Company> CreateAsync(Company company)
        {
            await dbContext.Companies.AddAsync(company);  //THIS STEP ASSIGN ID VALUE TO region.id
            await dbContext.SaveChangesAsync();
            return company;
        }
        public async Task<Company?> UpdateAsync(int id, Company company)
        {
            var existingCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id && x.active);
            if (existingCompany == null) { return null; }
            //assign new values to existingRegion
            existingCompany.HR_Email = company.HR_Email;
            existingCompany.Available_CV_Recommendations = existingCompany.Available_CV_Recommendations;
            existingCompany.Available_Job_Interviews  = existingCompany.Available_Job_Interviews;
            existingCompany.Description = company.Description;
            existingCompany.Name = company.Name;
            existingCompany.ContactEmail = company.ContactEmail;

            await dbContext.SaveChangesAsync();  //dbContext upates row of 'id' with existingRegion values
            return existingCompany;
        }
        public async Task<Company?> UpdateCVRecommendationsAsync(int id, Company company)
        {
            var existingCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id && x.active);
            if (existingCompany == null) { return null; }
            existingCompany.Available_CV_Recommendations = company.Available_CV_Recommendations;

            await dbContext.SaveChangesAsync();  //dbContext upates row of 'id' with existingRegion values
            return existingCompany;
        }
        public async Task<Company?> UpdateAvailableJobInterviewsAsync(int id, Company company)
        {
            var existingCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id && x.active);
            if (existingCompany == null) { return null; }
            existingCompany.Available_Job_Interviews = company.Available_Job_Interviews;

            await dbContext.SaveChangesAsync();  //dbContext upates row of 'id' with existingRegion values
            return existingCompany;
        }
        public async Task<Company?> UpdateCVRecommendations_JobInterviewsAsync(int id, Company company)
        {
            var existingCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id && x.active);
            if (existingCompany == null) { return null; }
            existingCompany.Available_CV_Recommendations = company.Available_CV_Recommendations;
            existingCompany.Available_Job_Interviews  = company.Available_Job_Interviews;
            await dbContext.SaveChangesAsync();  //dbContext upates row of 'id' with existingRegion values
            return existingCompany;
        }
        public async Task<Company?> UpdatePackage(int id, Package package)
        {
            var existingCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id && x.active);
            if (existingCompany == null) { return null; }
            existingCompany.PackageId = package.Id;
            existingCompany.Available_CV_Recommendations = package.Max_Cv_Recommendation;
            existingCompany.Available_Job_Interviews = package.Max_Job_Interviews;
            existingCompany.Max_Vacancies = package.Max_Vacancies;
            await dbContext.SaveChangesAsync();
            return existingCompany;
        }
        public async Task<Company?> ApproveCompanyAsync(int id)
        {
            var existingCompany = await dbContext.Companies.Include(company => company.Account).FirstOrDefaultAsync(x => x.Id == id && !x.active);
            if (existingCompany == null) { return null; }
            existingCompany.active = true;
            await dbContext.SaveChangesAsync();
            return existingCompany;
        }
        public async Task<Company?> CheckRejectCompanyAsync(int id)
        {
            var existingCompany = await dbContext.Companies.Include(company => company.Account).FirstOrDefaultAsync(x => x.Id == id && !x.active);
            if (existingCompany == null) { return null; }
            return existingCompany;
        }
        public async Task<Company?> DeleteAsync(int id)
        {
            var existingCompany = await dbContext.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCompany == null) { return null; }
            dbContext.Companies.Remove(existingCompany); //remove doesnot have async mathod it still syncronous
            await dbContext.SaveChangesAsync();
            return existingCompany;

        }
        
    }
}
