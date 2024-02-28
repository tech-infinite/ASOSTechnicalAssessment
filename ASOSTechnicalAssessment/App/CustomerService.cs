using ASOSTechnicalAssessment.App;
using System;

namespace App
{
    public class CustomerService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICustomerCreditService _customerCreditService;

        public CustomerService(ICompanyRepository companyRepository, ICustomerCreditService customerCreditService)
        {
            _companyRepository = companyRepository;
            _customerCreditService = customerCreditService;
        }

        public CustomerService()
        {
        }

        public bool AddCustomer(string firstName, string surname, string email, DateTime dateOfBirth, int companyId)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(surname) || !IsValidEmail(email) || !IsOldEnough(dateOfBirth))
            {
                return false;
            }

            var company = _companyRepository.GetById(companyId);
            if (company == null)
            {
                return false;
            }

            var customer = new Customer
            {
                Company = company,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                Surname = surname
            };

            SetCreditLimit(customer);

            if (customer.HasCreditLimit && customer.CreditLimit < 500)
            {
                return false;
            }

            CustomerDataAccess.AddCustomer(customer);

            return true;
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private bool IsOldEnough(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age >= 21;
        }

        private void SetCreditLimit(Customer customer)
        {
            if (customer.Company.Name == "VeryImportantClient")
            {
                customer.HasCreditLimit = false;
            }
            else
            {
                customer.HasCreditLimit = true;
                var creditLimit = _customerCreditService.GetCreditLimit(customer.FirstName, customer.Surname, customer.DateOfBirth).Result;
                if (customer.Company.Name == "ImportantClient")
                {
                    creditLimit *= 2;
                }
                customer.CreditLimit = creditLimit;
            }
        }
    }
}
