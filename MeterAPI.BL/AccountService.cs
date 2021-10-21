using AccountContracts;

using MeterAPI.BL.Interface;
using MeterAPI.DAL.Entities;
using MeterAPI.DAL.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterAPI.BL
{
    public class AccountService : IAccountService
    {
        private readonly IAccount _Account;
        public AccountService(IAccount Account)
        {
            _Account = Account;
        }


        /// <summary>
        /// Deletes a Account from the database
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAccount(string AccountId)
        {
            var response = await _Account.Delete(AccountId);
            return response;
        }


        /// <summary>
        /// Gets a Account from the database
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public AccountResponse GetAccount(string AccountId)
        {
            var response = _Account.Get(AccountId);
            return response;
        }

        /// <summary>
        /// Updates the data stored related to a Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AccountResponse> UpsertAccount(AccountRequest request)
        {
            var Entity = new TestAccountEntity()
            {
                AccountId = request.AccountId,
                Surname = request.Surname,
                FirstName = request.FirstName,
                Email = request.Email
            };
            var response = await _Account.Upsert(Entity);
            return response;
        }

    }
}
