using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountContracts
{
    public class AccountRequest
    {
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }

    public class AccountResponse : AccountRequest
    {
        public bool Success { get; set; }
    }
}
