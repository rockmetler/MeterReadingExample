using AccountContracts;

using MeterAPI.DAL.Entities;
using MeterAPI.DAL.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterAPI.DAL
{
    public class Account : IAccount
    {
        public async Task<bool> Delete(string AccountId)
        {
            using (var db = new MeterDBContext())
            {
                if (AccountId != null)
                {
                    var Item = db.TestAccounts.Where(x => x.AccountId == AccountId).First();
                    var result = db.Remove(Item);
                    if (await db.SaveChangesAsync() > 0)
                    {
                        return true;
                    };
                }
                return false;
            }
        }

        /// <summary>
        /// Gets a Account with the specified AccountId.
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public AccountResponse Get(string AccountId)
        {
            using (var db = new MeterDBContext())
            {
                if (AccountId != null)
                {
                    var result = db.TestAccounts.Where(x => x.AccountId == AccountId).First();
                    AccountResponse response = new AccountResponse()
                    {
                        AccountId = result.AccountId,
                        Email = result.Email,
                        FirstName = result.FirstName,
                        Surname = result.Surname,
                        Success = true
                    };
                    return response;
                }
                return new AccountResponse();
            }
        }

        /// <summary>
        /// Connects to the DB using EFCore and upserts a row with the data defined in the request method.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AccountResponse> Upsert(TestAccountEntity Entity)
        {
            using (var db = new MeterDBContext())
            {
                if (Entity.AccountId != null)
                {
                    var existingRec = db.TestAccounts.Where(x => x.AccountId == Entity.AccountId).FirstOrDefault();
                    if (existingRec != null)
                    {
                        //I came back here during testing, i'd use this next time: https://github.com/mono/entityframework/blob/master/src/EntityFramework/Migrations/DbSetMigrationsExtensions.cs#L79
                        db.Entry(existingRec).CurrentValues.SetValues(Entity);
                    }
                    else
                    {
                        await db.AddAsync(Entity);
                    }
                }
                try
                {
                    int alteredRows = await db.SaveChangesAsync();
                    var insertedRec = db.TestAccounts.Last();
                    AccountResponse response = new AccountResponse()
                    {
                        AccountId = insertedRec.AccountId,
                        Email = insertedRec.Email,
                        FirstName = insertedRec.FirstName,
                        Surname = insertedRec.Surname,
                        Success = alteredRows > 0
                    };
                    return response;
                }
                catch (Exception ex)
                {
                    return new AccountResponse()
                    {
                        Success = false
                    };
                }
            }

        }
    }
}
