using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeInfo>>(employees);
        }

        public async Task<EmployeeInfo> GetEmployeeByCodeAsync(string employeeCode)
        {
            var employee = await _employeeRepository.GetByCodeAsync(employeeCode);
            return _mapper.Map<EmployeeInfo>(employee);
        }

        public async Task<bool> SaveEmployeeAsync(EmployeeInfo employeeInfo)
        {
            var employee = _mapper.Map<Employee>(employeeInfo);
            return await _employeeRepository.SaveEmployeeAsync(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(string employeeCode, EmployeeInfo employeeInfo)
        {
            var employee = _mapper.Map<Employee>(employeeInfo);
            return await _employeeRepository.UpdateEmployeeAsync(employeeCode, employee);
        }

        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            return await _employeeRepository.DeleteEmployeeAsync(employeeCode);
        }

        public async Task<EmployeeClientInfo> GetEmployeeWithCompanyAsync(string employeeCode)
        {
            var employee = await _employeeRepository.GetByCodeAsync(employeeCode);
            if (employee == null)
            {
                return null;
            }

            var company = await _companyRepository.GetByCodeAsync(employee.CompanyCode);
            var employeeClientDto = new EmployeeClientInfo
            {
                EmployeeCode = employee.EmployeeCode,
                EmployeeName = employee.EmployeeName,
                CompanyName = company?.CompanyName,
                OccupationName = employee.Occupation,
                EmployeeStatus = employee.EmployeeStatus,
                EmailAddress = employee.EmailAddress,
                PhoneNumber = employee.Phone,
                LastModifiedDateTime = employee.LastModified
            };

            return employeeClientDto;
        }
    }
}
