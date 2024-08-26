using System;

namespace DataAccessLayer.Model.Models
{
    public class EmployeeClient : DataEntity
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName { get; set; }
        public string OccupationName { get; set; }
        public string EmployeeStatus { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
    }
}