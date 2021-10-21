using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MeterAPI.DAL.Entities
{
    public partial class TestAccountEntity
    {
        public TestAccountEntity()
        {
            MeterReadings = new HashSet<MeterReadingEntity>();
        }
        [Key]
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public virtual ICollection<MeterReadingEntity> MeterReadings { get; set; }
    }
}
