using AccountContracts;

using FluentAssertions;

using MeterAPI.BL;
using MeterAPI.DAL.Interface;
using MeterAPI.DAL;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using MeterAPI.DAL.Entities;

namespace MeterAPITests.Controllers
{
    public class AccountTests
    {
        [Fact()]
        public async void UpsertAccountTest()
        {
            string accountGuid = Guid.NewGuid().ToString();
            string AccountGuid = Guid.NewGuid().ToString();
            string userEmail = TestUtil.RandomString(8, true) + "@account.com";
            var Entity = new TestAccountEntity() { AccountId = "test", Email = userEmail, FirstName = "test", Surname = "McTestyFace" };
            var request = new AccountRequest() { AccountId = "test", Email = userEmail, FirstName = "test", Surname = "McTestyFace" };
            Account Account = new Account();
            var returnData = await Account.Upsert(Entity);
            returnData.Should().BeOfType(typeof(AccountResponse));
        }

        [Fact()]
        public async void DeleteAccountTest()
        {
            string AccountId = "47596d95-9e02-4c96-a003-6d808905b1b5";

            Mock<IAccount> mockRepo = new Mock<IAccount>();

            mockRepo.Setup(x => x.Delete(AccountId)).ReturnsAsync(true);

            var serviceObject = new AccountService(mockRepo.Object);

            var returnData = await serviceObject.DeleteAccount(AccountId);
            Assert.True(returnData);
        }

        [Fact()]
        public void GetAccountTest()
        {
            string AccountId = "47596d95-9e02-4c96-a003-6d808905b1b5";

            Mock<IAccount> mockRepo = new Mock<IAccount>();

            mockRepo.Setup(x => x.Get(AccountId)).Returns(new AccountResponse() { AccountId = "test", Email = "test@account.com", FirstName = "test", Surname = "McTestyFace", Success = true });

            var serviceObject = new AccountService(mockRepo.Object);
            var returnData = serviceObject.GetAccount(AccountId);
            returnData.Should().BeEquivalentTo(new AccountResponse() { AccountId = "test", Email = "test@account.com", FirstName = "test", Surname = "McTestyFace", Success = true });
        }
    }

    public class TestUtil
    {
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}
