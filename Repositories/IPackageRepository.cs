using EmployMe.Models.Domain;

namespace EmployMe.Repositories
{
    public interface IPackageRepository
    {
        Task<Package?> GetPackageAsync(int id);
        Task<ICollection<Package>> GetAllPackagesAsync(bool activePackage = true);
        Task<bool> CreatePackageAsync(Package package);
        Task<bool> UpdatePackageAsync(int id, Package package);
        Task<bool> DeletePackageAsync(int id);
        Task<bool> Active_YN(int id);
        Task<bool> ExistAsync(String name);
    }
}
