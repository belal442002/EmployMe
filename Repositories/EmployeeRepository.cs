using EmployMe.Data;
using EmployMe.Models.Domain;
using EmployMe.Models.DTO.EmployeeDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection.Emit;

namespace EmployMe.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployMeAuthDbContext dbContext;
        
        public EmployeeRepository(EmployMeAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }
        
        public async Task<List<Employee>> GetAllAsync()
        {
            return await dbContext.Employees.ToListAsync();
        }
        
        public async Task<List<Employee>> GetAllInfoAsync()
        {
            return await dbContext.Employees.Include(employee => employee.Profile).Include(employee => employee.Account).ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await dbContext.Employees.Include(employee => employee.Profile).FirstOrDefaultAsync(i => i.Id == id);
        }
        
        public async Task<Employee?> GetByIdInfoAsync(int id)
        {
            return await dbContext.Employees.Include(employee => employee.Profile).Include(employee => employee.Account).FirstOrDefaultAsync(r => r.Id == id);
        }
        
        public async Task<Employee> CreateAsync(Employee employeeModel)
        {
            await dbContext.Employees.AddAsync(employeeModel);
            await dbContext.SaveChangesAsync();
            return employeeModel;
        }
        
        public async Task<Employee?> DeleteAsync(int id)
        {
            var employeeModel = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employeeModel == null)
            {
                return null;
            }
            dbContext.Employees.Remove(employeeModel);
            await dbContext.SaveChangesAsync();
            return employeeModel;
        }

        public async Task<Employee?> UpdateAsync(int id, Employee employeeModel)
        {
            var existingEmployee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (existingEmployee == null)
            {
                return null;
            }
            existingEmployee.FirstName = employeeModel.FirstName;
            existingEmployee.LastName = employeeModel.LastName;
            existingEmployee.ContactEmail = employeeModel.ContactEmail;
            existingEmployee.CVurl = employeeModel.CVurl;
            existingEmployee.ZipCode = employeeModel.ZipCode;
            existingEmployee.City = employeeModel.City;
            existingEmployee.Location = employeeModel.Location;
            existingEmployee.PhoneNumber = employeeModel.PhoneNumber;

            await dbContext.SaveChangesAsync();
            return existingEmployee;
        }
    }
}
