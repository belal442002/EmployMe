using EmployMe.Data;
using EmployMe.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployMe.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly EmployMeAuthDbContext _context;

        public PackageRepository(EmployMeAuthDbContext context)
        {
            _context = context;
        }
        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
        
        public async Task<bool> CreatePackageAsync(Package package)
        {
            await _context.Packages.AddAsync(package);
            return await SaveAsync();
        }

        public async Task<bool> DeletePackageAsync(int id)
        {
            var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == id);
            if (package is null)
                return false;

            _context.Packages.Remove(package);
            return await SaveAsync();
        }

        public async Task<ICollection<Package>> GetAllPackagesAsync(bool activePackage = true)
        {
            var packages = _context.Packages.AsQueryable();

            packages = activePackage ?  packages.Where(p => p.Active_YN == true)
                :  packages.Where(p => p.Active_YN == false);

            return await packages.ToListAsync();
        }

        public async Task<Package?> GetPackageAsync(int id)
        {
            
            return await _context.Packages.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> UpdatePackageAsync(int id, Package package)
        {
            var packageToUpdate = await _context.Packages.FirstOrDefaultAsync(p => p.Id == id);
            if (packageToUpdate is null)
                return false;
            packageToUpdate.Description = package.Description;
            packageToUpdate.Name = package.Name;
            packageToUpdate.Max_Vacancies = package.Max_Vacancies;
            packageToUpdate.Max_Cv_Recommendation = package.Max_Cv_Recommendation;
            packageToUpdate.Price = package.Price;
            packageToUpdate.Max_Job_Interviews = package.Max_Job_Interviews;
            package.Active_YN = package.Active_YN;

            return await SaveAsync();
        }

        public async Task<bool> Active_YN(int id)
        {
            var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == id);
            if (package is null)
                return false;
            package.Active_YN = package.Active_YN == false ? true : false;

            return await SaveAsync();
        }

        public async Task<bool> ExistAsync(string name)
        {
            return await _context.Packages.AnyAsync(p => p.Name == name);
        }
    }
}
