using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper, ILoggerManager logger)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/<controller>
        [HttpGet]
        public async Task<IEnumerable<CompanyDto>> GetAll()
        {
            _logger.LogInfo("GET api/company called.");

            var items = await _companyService.GetAllCompaniesAsync();
            _logger.LogInfo($"Retrieved {items?.Count()} companies.");

            return _mapper.Map<IEnumerable<CompanyDto>>(items);
        }

        // GET api/<controller>/5
        [HttpGet]
        public async Task<IHttpActionResult> Get(string companyCode)
        {
            _logger.LogInfo($"GET api/company/{companyCode} called.");

            try
            {
                var item = await _companyService.GetCompanyByCodeAsync(companyCode);

                if (item != null)
                {
                    _logger.LogInfo($"Company with code {companyCode} retrieved.");
                    return Ok(_mapper.Map<CompanyDto>(item));
                }

                _logger.LogWarn($"Company with code {companyCode} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving company with code {companyCode}: {ex.Message}");
                return InternalServerError();
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] CompanyDto companyDto)
        {
            _logger.LogInfo("POST api/company called.");

            if (companyDto == null)
            {
                _logger.LogWarn("Invalid data for creating company.");
                return BadRequest("Invalid data.");
            }

            try
            {
                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                var result = await _companyService.SaveCompanyAsync(companyInfo);

                if (result)
                {
                    _logger.LogInfo("Company created successfully.");
                    return Ok("Company created successfully.");
                }
                else
                {
                    _logger.LogError("Failed to create company.");
                    return BadRequest("Failed to create company.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating company: {ex.Message}");
                return InternalServerError();
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        public async Task<IHttpActionResult> Put(string companyCode, [FromBody] CompanyDto companyDto)
        {
            _logger.LogInfo($"PUT api/company/{companyCode} called.");

            if (companyDto == null)
            {
                _logger.LogWarn("Invalid data for updating company.");
                return BadRequest("Invalid data.");
            }

            try
            {
                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                var result = await _companyService.UpdateCompanyAsync(companyCode, companyInfo);

                if (result)
                {
                    _logger.LogInfo($"Company with code {companyCode} updated successfully.");
                    return Ok("Company updated successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to update company with code {companyCode}.");
                    return BadRequest("Failed to update company.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating company with code {companyCode}: {ex.Message}");
                return InternalServerError();
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string companyCode)
        {
            _logger.LogInfo($"DELETE api/company/{companyCode} called.");

            try
            {
                var result = await _companyService.DeleteCompanyAsync(companyCode);

                if (result)
                {
                    _logger.LogInfo($"Company with code {companyCode} deleted successfully.");
                    return Ok("Company deleted successfully.");
                }
                else
                {
                    _logger.LogError($"Failed to delete company with code {companyCode}.");
                    return BadRequest("Failed to delete company.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting company with code {companyCode}: {ex.Message}");
                return InternalServerError();
            }
        }
    }
}