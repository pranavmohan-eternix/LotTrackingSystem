using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using LTS.Application.Services;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Tests.Services
{
    public class CarrierServiceTests
    {
        private readonly Mock<ICarrierRepository>
            repoMock;

        private readonly CarrierService service;

        public CarrierServiceTests()
        {
            repoMock =
                new Mock<ICarrierRepository>();

            service =
                new CarrierService(
                    repoMock.Object);
        }

        // TEST 1

        [Fact]
        public void AddCarrier_ShouldThrow_WhenCodeEmpty()
        {
            var carrier = new Carrier
            {
                CarrierCode = "",
                Capacity = 10
            };

            var ex =
                Assert.Throws<Exception>(() =>
                    service.AddCarrier(carrier));

            Assert.Equal(
                "Carrier Code is required",
                ex.Message);
        }

        // TEST 2

        [Fact]
        public void AddCarrier_ShouldThrow_WhenCapacityInvalid()
        {
            var carrier = new Carrier
            {
                CarrierCode = "C01",
                Capacity = 0
            };

            var ex =
                Assert.Throws<Exception>(() =>
                    service.AddCarrier(carrier));

            Assert.Equal(
                "Capacity must be greater than 0",
                ex.Message);
        }

        // TEST 3

        [Fact]
        public void AddCarrier_ShouldAddSuccessfully()
        {
            var carrier = new Carrier
            {
                CarrierCode = "C01",
                Capacity = 10
            };

            service.AddCarrier(carrier);

            repoMock.Verify(
                x => x.Add(
                    It.IsAny<Carrier>()),
                Times.Once);
        }

        // TEST 4

        [Fact]
        public void GetCarriers_ShouldReturnData()
        {
            repoMock
                .Setup(x => x.GetAll())
                .Returns(
                    new List<Carrier>
                    {
                        new Carrier
                        {
                            CarrierId = 1,
                            CarrierCode = "C01",
                            Capacity = 10
                        }
                    });

            var result =
                service.GetCarriers();

            Assert.Single(result);

            Assert.Equal(
                "C01",
                result[0].CarrierCode);
        }
    }
}