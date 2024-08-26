using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper)
        {
            _employeeDbWrapper = employeeDbWrapper;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeDbWrapper.FindAllAsync();
        }

        public async Task<Employee> GetByCodeAsync(string employeeCode)
        {
            var result = await _employeeDbWrapper.FindAsync(e => e.EmployeeCode.Equals(employeeCode));
            return result.FirstOrDefault();
        }

        public async Task<bool> SaveEmployeeAsync(Employee employee)
        {
            return await _employeeDbWrapper.InsertAsync(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(string employeeCode, Employee employee)
        {
            var existingEmployee = await GetByCodeAsync(employeeCode);
            if (existingEmployee != null)
            {
                existingEmployee.EmployeeName = employee.EmployeeName;
                existingEmployee.Occupation = employee.Occupation;
                existingEmployee.EmployeeStatus = employee.EmployeeStatus;
                existingEmployee.EmailAddress = employee.EmailAddress;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.LastModified = employee.LastModified;

                return await _employeeDbWrapper.UpdateAsync(existingEmployee);
            }

            return false;
        }

        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            return await _employeeDbWrapper.DeleteAsync(e => e.EmployeeCode.Equals(employeeCode));
        }
    }
}
