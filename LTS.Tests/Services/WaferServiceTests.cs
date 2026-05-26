using LTS.Application.Services;
using LTS.Common.Interfaces;
using LTS.Common.Models;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace LTS.Tests.Services
{
    public class WaferServiceTests
    {
        private readonly Mock<IWaferRepository>
            _repoMock;

        private readonly WaferService
            _service;

        public WaferServiceTests()
        {
            _repoMock =
                new Mock<IWaferRepository>();

            _service =
                new WaferService(
                    _repoMock.Object);
        }

        [Fact]
        public void AddWafer_ShouldThrow_WhenSerialEmpty()
        {
            var wafer = new Wafer
            {
                WaferSerialNo = "",
                SupplierId = 1
            };

            var ex =
                Assert.Throws<Exception>(() =>
                    _service.AddWafer(wafer));

            Assert.Equal(
                "Wafer Serial No required",
                ex.Message);
        }

        [Fact]
        public void AddWafer_ShouldCallRepository()
        {
            var wafer = new Wafer
            {
                WaferSerialNo = "WF-001",
                SupplierId = 1
            };

            _service.AddWafer(wafer);

            _repoMock.Verify(
                x => x.Add(It.IsAny<Wafer>()),
                Times.Once);
        }

        [Fact]
        public void GetWafers_ShouldReturnList()
        {
            _repoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Wafer>
                {
                    new Wafer
                    {
                        WaferId = 1,
                        WaferSerialNo = "WF-001"
                    }
                });

            var result =
                _service.GetWafers();

            Assert.Single(result);

            Assert.Equal(
                "WF-001",
                result[0].WaferSerialNo);
        }

        
        [Fact]
        public void DeleteWafer_ShouldCallRepository()
        {
            // ARRANGE

            _repoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Wafer>
                {
            new Wafer
            {
                WaferId = 1,
                WaferSerialNo = "WF-001"
            }
                });

            // ACT

            _service.DeleteWafer(1);

            // ASSERT

            _repoMock.Verify(
                x => x.Delete(1),
                Times.Once);
        }
    }
}