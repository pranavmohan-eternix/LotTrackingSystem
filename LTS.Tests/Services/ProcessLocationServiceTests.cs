using Xunit;
using Moq;
using System.Collections.Generic;
using LTS.Application.Services;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Tests.Services
{
    public class ProcessLocationServiceTests
    {
        private readonly Mock<IProcessLocationRepository>
            _repoMock;

        private readonly ProcessLocationService
            _service;

        public ProcessLocationServiceTests()
        {
            _repoMock =
                new Mock<IProcessLocationRepository>();

            _service =
                new ProcessLocationService(
                    _repoMock.Object);
        }

        [Fact]
        public void GetLocations_ShouldReturnList()
        {
            _repoMock
                .Setup(x => x.GetAll())
                .Returns(new List<ProcessLocation>
                {
                    new ProcessLocation
                    {
                        ProcessLocationId = 1,
                        StationName = "S-01"
                    }
                });

            var result =
                _service.GetLocations();

            Assert.Single(result);

            Assert.Equal(
                "S-01",
                result[0].StationName);
        }
    }
}