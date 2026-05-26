using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using LTS.Application.Services;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Tests.Services
{
    public class LotServiceTests
    {
        private readonly Mock<ILotRepository> repoMock;

        private readonly Mock<ICarrierRepository>
            carrierRepoMock;

        private readonly Mock<IWaferRepository>
            waferRepoMock;

        private readonly Mock<IProcessLocationRepository>
            processRepoMock;

        private readonly Mock<ILotHistoryRepository>
            historyRepoMock;

        private readonly Mock<ILogRepository>
            logRepoMock;

        private readonly LotService service;

        public LotServiceTests()
        {
            repoMock =
                new Mock<ILotRepository>();

            carrierRepoMock =
                new Mock<ICarrierRepository>();

            waferRepoMock =
                new Mock<IWaferRepository>();

            processRepoMock =
                new Mock<IProcessLocationRepository>();

            historyRepoMock =
                new Mock<ILotHistoryRepository>();

            logRepoMock =
                new Mock<ILogRepository>();

            var logService =
                new LogService(
                    logRepoMock.Object);

            service = new LotService(
                repoMock.Object,
                carrierRepoMock.Object,
                waferRepoMock.Object,
                processRepoMock.Object,
                historyRepoMock.Object,
                logService);
        }

        // TEST 1

        [Fact]
        public void AddLot_ShouldThrow_WhenLotCodeEmpty()
        {
            var lot = new Lot
            {
                LotCode = "",
                CarrierId = 1
            };

            var wafers =
                new List<Wafer>
                {
                    new Wafer()
                };

            var ex =
                Assert.Throws<Exception>(() =>
                    service.AddLot(
                        lot,
                        wafers));

            Assert.Equal(
                "Lot Code required",
                ex.Message);
        }

        // TEST 2

        [Fact]
        public void AddLot_ShouldThrow_WhenNoWafers()
        {
            var lot = new Lot
            {
                LotCode = "LOT001",
                CarrierId = 1
            };

            var wafers =
                new List<Wafer>();

            var ex =
                Assert.Throws<Exception>(() =>
                    service.AddLot(
                        lot,
                        wafers));

            Assert.Equal(
                "Select wafers",
                ex.Message);
        }

        // TEST 3

        [Fact]
        public void AddLot_ShouldThrow_WhenCarrierCapacityExceeded()
        {
            var carrier = new Carrier
            {
                CarrierId = 1,
                Capacity = 1,
                Status = "Available"
            };

            carrierRepoMock
                .Setup(x => x.GetAll())
                .Returns(
                    new List<Carrier>
                    {
                        carrier
                    });

            var wafers =
                new List<Wafer>
                {
                    new Wafer(),
                    new Wafer()
                };

            var lot = new Lot
            {
                LotCode = "LOT1",
                CarrierId = 1
            };

            var ex =
                Assert.Throws<Exception>(() =>
                    service.AddLot(
                        lot,
                        wafers));

            Assert.Equal(
                "Carrier capacity exceeded",
                ex.Message);
        }

        // TEST 4

        [Fact]
        public void AddLot_ShouldThrow_WhenCarrierOccupied()
        {
            var carrier = new Carrier
            {
                CarrierId = 1,
                Capacity = 5,
                Status = "Occupied"
            };

            carrierRepoMock
                .Setup(x => x.GetAll())
                .Returns(
                    new List<Carrier>
                    {
                        carrier
                    });

            var wafers =
                new List<Wafer>
                {
                    new Wafer()
                };

            var lot = new Lot
            {
                LotCode = "LOT1",
                CarrierId = 1
            };

            var ex =
                Assert.Throws<Exception>(() =>
                    service.AddLot(
                        lot,
                        wafers));

            Assert.Equal(
                "Carrier already occupied",
                ex.Message);
        }

        // TEST 5

        [Fact]
        public void AddLot_ShouldAddSuccessfully()
        {
            var carrier = new Carrier
            {
                CarrierId = 1,
                Capacity = 5,
                Status = "Available"
            };

            carrierRepoMock
                .Setup(x => x.GetAll())
                .Returns(
                    new List<Carrier>
                    {
                        carrier
                    });

            repoMock
                .Setup(x =>
                    x.Add(It.IsAny<Lot>()))
                .Returns(1);

            var wafers =
                new List<Wafer>
                {
                    new Wafer
                    {
                        WaferId = 1
                    }
                };

            var lot = new Lot
            {
                LotCode = "LOT1",
                CarrierId = 1
            };

            service.AddLot(
                lot,
                wafers);

            repoMock.Verify(
                x => x.Add(It.IsAny<Lot>()),
                Times.Once);
        }

        // TEST 6

        [Fact]
        public void StartLot_ShouldThrow_WhenLotNotFound()
        {
            repoMock
                .Setup(x => x.GetAll())
                .Returns(
                    new List<Lot>());

            var ex =
                Assert.Throws<Exception>(() =>
                    service.StartLot(1));

            Assert.Equal(
                "Lot not found",
                ex.Message);
        }

        // TEST 7

        [Fact]
        public void StartLot_ShouldThrow_WhenNoRouteSelected()
        {
            var lot = new Lot
            {
                LotId = 1,
                LotCode = "LOT1",
                RouteStations = ""
            };

            repoMock
                .Setup(x => x.GetAll())
                .Returns(
                    new List<Lot>
                    {
                        lot
                    });

            var ex =
                Assert.Throws<Exception>(() =>
                    service.StartLot(1));

            Assert.Equal(
                "No route selected",
                ex.Message);
        }

        // TEST 8

        [Fact]
        public void StartLot_ShouldQueue_WhenStationUnavailable()
        {
            var lot = new Lot
            {
                LotId = 1,
                LotCode = "LOT1",
                RouteStations = "1",
                WaferCount = 2
            };

            repoMock
                .Setup(x => x.GetAll())
                .Returns(
                    new List<Lot>
                    {
                        lot
                    });

            processRepoMock
                .Setup(x =>
                    x.IsStationAvailable(1))
                .Returns(false);

            service.StartLot(1);

            repoMock.Verify(
                x => x.UpdateStatus(
                    1,
                    "Queued"),
                Times.Once);
        }

        // TEST 9

        [Fact]
        public void StartLot_ShouldStartSuccessfully()
        {
            var lot = new Lot
            {
                LotId = 1,
                LotCode = "LOT1",
                RouteStations = "1",
                WaferCount = 2,
                CarrierId = 1
            };

            repoMock
                .Setup(x => x.GetAll())
                .Returns(
                    new List<Lot>
                    {
                        lot
                    });

            processRepoMock
                .Setup(x =>
                    x.IsStationAvailable(1))
                .Returns(true);

            service.StartLot(1);

            repoMock.Verify(
                x => x.MoveNext(
                    1,
                    1,
                    "InProgress"),
                Times.Once);
        }
    }
}