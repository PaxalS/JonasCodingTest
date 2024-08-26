using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbWrapper<Company> _companyDbWrapper;

        public CompanyRepository(IDbWrapper<Company> companyDbWrapper)
        {
            _companyDbWrapper = companyDbWrapper;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _companyDbWrapper.FindAllAsync();
        }

        public async Task<Company> GetByCodeAsync(string companyCode)
        {
            var result = await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode));
            return result.FirstOrDefault();
        }

        public async Task<bool> SaveCompanyAsync(Company company)
        {
            return await _companyDbWrapper.InsertAsync(company);
        }

        public async Task<bool> UpdateCompanyAsync(string companyCode, Company updatedCompany)
        {
            var existingCompany = await GetByCodeAsync(companyCode);

            if (existingCompany != null)
            {
                existingCompany.CompanyName = updatedCompany.CompanyName;
                existingCompany.AddressLine1 = updatedCompany.AddressLine1;
                existingCompany.AddressLine2 = updatedCompany.AddressLine2;
                existingCompany.AddressLine3 = updatedCompany.AddressLine3;
                existingCompany.Country = updatedCompany.Country;
                existingCompany.EquipmentCompanyCode = updatedCompany.EquipmentCompanyCode;
                existingCompany.FaxNumber = updatedCompany.FaxNumber;
                existingCompany.PhoneNumber = updatedCompany.PhoneNumber;
                existingCompany.PostalZipCode = updatedCompany.PostalZipCode;
                existingCompany.LastModified = updatedCompany.LastModified;

                return await _companyDbWrapper.UpdateAsync(existingCompany);
            }

            return false;
        }

        public async Task<bool> DeleteCompanyAsync(string companyCode)
        {
            return await _companyDbWrapper.DeleteAsync(c => c.CompanyCode.Equals(companyCode));
        }
    }
}
