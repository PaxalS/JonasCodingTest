using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILoggerManager logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/employee
        [HttpGet]
        public async Task<IEnumerable<EmployeeDto>> GetAll()
        {
            _logger.LogInfo("GET api/employee called.");

            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                _logger.LogInfo("Retrieved all employees successfully.");
                return (IEnumerable<EmployeeDto>)Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAll action: {ex.Message}");
                return (IEnumerable<EmployeeDto>)InternalServerError();
            }
        }

        // GET api/employee/5
        [HttpGet]
        public async Task<IHttpActionResult> Get(string employeeCode)
        {
            _logger.LogInfo("GET api/employee/5 called.");

            try
            {
                var employee = await _employeeService.GetEmployeeByCodeAsync(employeeCode);
                if (employee == null)
                {
                    _logger.LogWarn($"Employee with code {employeeCode} was not found.");
                    return NotFound();
                }
                _logger.LogInfo($"Retrieved employee with code {employeeCode} successfully.");
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Get action: {ex.Message}");
                return InternalServerError();
            }
        }

        // POST api/employee
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] EmployeeDto employeeDto)
        {
            _logger.LogInfo("POST api/employee called.");

            if (employeeDto == null)
            {
                _logger.LogWarn("Received an invalid POST request with null data.");
                return BadRequest("Invalid data.");
            }

            try
            {
                var employeeInfo = _mapper.Map<EmployeeInfo>(employeeDto);
                var result = await _employeeService.SaveEmployeeAsync(employeeInfo);

                if (result)
                {
                    _logger.LogInfo("Employee created successfully.");
                    return Ok("Employee created successfully.");
                }
                else
                {
                    _logger.LogError("Failed to create employee.");
                    return BadRequest("Failed to create employee.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Post action: {ex.Message}");
                return InternalServerError();
            }
        }

        // PUT api/employee/5
        [HttpPut]
        public async Task<IHttpActionResult> Put(string employeeCode, [FromBody] EmployeeDto employeeDto)
        {
            _logger.LogInfo("PUT api/employee/5 called.");

            if (employeeDto == null)
            {
                _logger.LogWarn("Received an invalid PUT request with null data.");
                return BadRequest("Invalid data.");
            }

            try
            {
                var employeeInfo = _mapper.Map<EmployeeInfo>(employeeDto);
                var result = await _employeeService.UpdateEmployeeAsync(employeeCode, employeeInfo);

                if (result)
                {
                    _logger.LogInfo($"Employee with code {employeeCode} updated successfully.");
                    return Ok("Employee updated successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to update employee with code {employeeCode}.");
                    return BadRequest("Failed to update employee.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Put action: {ex.Message}");
                return InternalServerError();
            }
        }

        // DELETE api/employee/5
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string employeeCode)
        {
            _logger.LogInfo("DELETE api/employee/5 called.");

            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(employeeCode);

                if (result)
                {
                    _logger.LogInfo($"Employee with code {employeeCode} deleted successfully.");
                    return Ok("Employee deleted successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to delete employee with code {employeeCode}.");
                    return BadRequest("Failed to delete employee.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Delete action: {ex.Message}");
                return InternalServerError();
            }
        }

        // GET api/employee/client/5
        [HttpGet]
        [Route("api/employee/client/{employeeCode}")]
        public async Task<IHttpActionResult> GetEmployeeForClient(string employeeCode)
        {
            _logger.LogInfo("GET api/employee/client/5 called.");

            try
            {
                var employeeClientDto = await _employeeService.GetEmployeeWithCompanyAsync(employeeCode);

                if (employeeClientDto == null)
                {
                    _logger.LogWarn($"Employee with code {employeeCode} was not found.");
                    return NotFound();
                }

                _logger.LogInfo($"Successfully retrieved employee with code {employeeCode}.");
                return Ok(employeeClientDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching employee with code {employeeCode}: {ex.Message}");
                return InternalServerError();
            }
        }
    }
}
