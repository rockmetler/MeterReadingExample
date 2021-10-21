using AccountContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace MeterAPI.BL.Interface
{
    [TransientService]
    public interface IAccountService
    {
        Task<AccountResponse> UpsertAccount(AccountRequest request);
        AccountResponse GetAccount(string AccountId);
        Task<bool> DeleteAccount(string AccountId);
    }
}
