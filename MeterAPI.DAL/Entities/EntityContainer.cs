using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterAPI.DAL.Entities
{
    public class EntityContainer
    {
        public List<MeterReadingEntity> ValidList { get; set; }
        public List<MeterReadingEntity> InvalidList { get; set; }
    }
}
