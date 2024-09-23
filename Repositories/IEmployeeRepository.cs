using EmployMe.Models.Domain;
using EmployMe.Models.DTO.EmployeeDto;

namespace EmployMe.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<List<Employee>> GetAllInfoAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee?> GetByIdInfoAsync(int id);
        Task<Employee> CreateAsync(Employee employeeModel);
        Task<Employee?> UpdateAsync(int id, Employee employeeModel);
        Task<Employee?> DeleteAsync(int id);   
    }
}
