using LTS.Application.Services;
using LTS.Common.Interfaces;
using LTS.Common.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace LTS.Tests.Services
{
    public class SupplierServiceTests
    {
        private readonly Mock<ISupplierRepository>
            _repoMock;

        private readonly SupplierService
            _service;

        public SupplierServiceTests()
        {
            _repoMock =
                new Mock<ISupplierRepository>();

            _service =
                new SupplierService(
                    _repoMock.Object);
        }

        [Fact]
        public void AddSupplier_ShouldThrow_WhenNameEmpty()
        {
            var supplier = new Supplier
            {
                SupplierName = "",
                ContactPerson = "John",
                Email = "john@test.com"
            };

            var ex =
                Assert.Throws<Exception>(() =>
                    _service.AddSupplier(supplier));

            Assert.Equal(
                "Supplier name required",
                ex.Message);
        }

        [Fact]
        public void AddSupplier_ShouldCallRepository()
        {
            var supplier = new Supplier
            {
                SupplierName = "Intel",
                ContactPerson = "John",
                Email = "john@test.com"
            };

            _service.AddSupplier(supplier);

            _repoMock.Verify(
                x => x.Add(It.IsAny<Supplier>()),
                Times.Once);
        }

        [Fact]
        public void GetSuppliers_ShouldReturnList()
        {
            _repoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Supplier>
                {
                    new Supplier
                    {
                        SupplierId = 1,
                        SupplierName = "Intel"
                    }
                });

            var result =
                _service.GetSuppliers();

            Assert.Single(result);

            Assert.Equal(
                "Intel",
                result[0].SupplierName);
        }
    }
}