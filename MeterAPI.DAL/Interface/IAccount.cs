using AccountContracts;

using MeterAPI.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace MeterAPI.DAL.Interface
{
    [TransientService]
    public interface IAccount
    {
        AccountResponse Get(string AccountReadingId);
        Task<AccountResponse> Upsert(TestAccountEntity Entity);
        Task<bool> Delete(string AccountReadingId);
    }
}
