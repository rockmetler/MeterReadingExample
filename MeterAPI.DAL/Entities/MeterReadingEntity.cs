using MeterContracts;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace MeterAPI.DAL.Entities
{
    public partial class MeterReadingEntity
    {
        [Key]
        public string MeterReadingId { get; set; }
        public string AccountId { get; set; }
        public long? Reading { get; set; }

        public virtual TestAccountEntity Account { get; set; }

        public static explicit operator MeterReadingEntity(MeterRequest v)
        {
            MeterReadingEntity entity = new MeterReadingEntity()
            {
                AccountId = v.AccountId,
                MeterReadingId = v.MeterReadingId,
                Reading = v.Reading
            };
            return entity;
        }
    }
}
