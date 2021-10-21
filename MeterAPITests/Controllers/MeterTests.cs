

using FluentAssertions;

using MeterAPI.BL;
using MeterAPI.DAL;
using MeterAPI.DAL.Entities;
using MeterAPI.DAL.Interface;
using MeterAPI.Helpers.Interfaces;

using MeterContracts;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace MeterAPI.Controllers.Tests
{
    public class MeterTests
    {
        [Fact()]
        public async void UpsertMeterTest()
        {
            string accountGuid = Guid.NewGuid().ToString();
            string meterGuid = Guid.NewGuid().ToString();
            var Entity = new MeterReadingEntity() { AccountId = accountGuid, MeterReadingId = meterGuid, Reading = 1111 };
            Meter meter = new Meter();
            var returnData = await meter.UpsertMeterReading(Entity);
            returnData.Should().BeOfType(typeof(MeterResponse));
        }

        [Fact()]
        public async void DeleteMeterTest()
        {
            string MeterId = "47596d95-9e02-4c96-a003-6d808905b1b5";

            Mock<IMeter> mockRepo = new Mock<IMeter>();
            Mock<IHelpers> mockHelpers = new Mock<IHelpers>();

            mockRepo.Setup(x => x.DeleteMeterReading(MeterId)).ReturnsAsync(true);

            var serviceObject = new MeterService(mockRepo.Object,mockHelpers.Object);

            var returnData = await serviceObject.DeleteMeterReading(MeterId);
            Assert.True(returnData);
        }

        [Fact()]
        public void GetMeterTest()
        {
            string MeterId = "47596d95-9e02-4c96-a003-6d808905b1b5";

            Mock<IMeter> mockRepo = new Mock<IMeter>();
            Mock<IHelpers> mockHelpers = new Mock<IHelpers>();

            mockRepo.Setup(x => x.GetByMeterReadingId(MeterId)).Returns(new MeterResponse() { AccountId = "test", MeterReadingId = "meterTest", Reading = 1111, Success = true });

            var serviceObject = new MeterService(mockRepo.Object, mockHelpers.Object);
            var returnData = serviceObject.GetMeterReading(MeterId);
            returnData.Should().BeEquivalentTo(new MeterResponse() { AccountId = "test", MeterReadingId = "meterTest", Reading = 1111, Success = true });
        }

        [Fact()]
        public async System.Threading.Tasks.Task UploadMeterReadingsTestAsync()
        {
            string accountGuid = "d448ea0b-6387-4966-8e02-6f195ec53140";
            List<MeterReadingEntity> validEntityList = new List<MeterReadingEntity>();
            List<MeterReadingEntity> invalidEntityList = new List<MeterReadingEntity>();
            for (int i = 1111; i < 1116; i++)
            {
                validEntityList.Add(new MeterReadingEntity() { AccountId = accountGuid, MeterReadingId = Guid.NewGuid().ToString(), Reading = i });
            }
            for (int i = 1111; i < 1116; i++)
            {
                invalidEntityList.Add(new MeterReadingEntity() { AccountId = accountGuid, MeterReadingId = validEntityList.First().MeterReadingId.ToString(), Reading = i });
            }
            EntityContainer container = new EntityContainer()
            {
                InvalidList = invalidEntityList,
                ValidList = validEntityList
            };
            Meter meter = new Meter();
            var returnData = await meter.CreateMeterReadings(container);
            returnData.Should().BeOfType(typeof(List<MeterResponse>));
        }
    }
}