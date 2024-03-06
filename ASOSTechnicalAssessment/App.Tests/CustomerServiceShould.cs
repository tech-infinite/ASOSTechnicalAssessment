using ASOSTechnicalAssessment.App;
using Moq;
using NUnit.Framework;

namespace App.Tests
{
    [TestFixture]
    public sealed class CustomerServiceShould
    {
        [Test]
        public void TestMethod1()
        {
            // Arrange
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            companyRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(new Company());

            var customerCreditServiceMock = new Mock<ICustomerCreditService>();
            customerCreditServiceMock.Setup(service => service.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(1000);

            var customerService = new CustomerService(companyRepositoryMock.Object, customerCreditServiceMock.Object);

            // Act
            bool result = customerService.AddCustomer("Joe", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ReturnFalseWhenAddingInvalidCustomer()
        {
            // Arrange
            var companyRepositoryMock = new Mock<ICompanyRepository>();
            var customerCreditServiceMock = new Mock<ICustomerCreditService>();
            var customerService = new CustomerService(companyRepositoryMock.Object, customerCreditServiceMock.Object);

            // Act
            bool result = customerService.AddCustomer("", "Bloggs", "joe.bloggs@adomain.com", new DateTime(1980, 3, 27), 4);

            // Assert
            Assert.IsFalse(result);
        }
    }
}

