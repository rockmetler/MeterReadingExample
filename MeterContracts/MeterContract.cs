using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeterContracts
{
    public class MeterRequestContainer
    {
        public List<MeterRequest> MeterRequest { get; set; }
        public List<MeterRequest> InvalidValues { get; set; }
    }

    public class MeterResponseContainer
    {
        public List<MeterResponse> MeterRequest { get; set; }
        public List<MeterResponse> InvalidValues { get; set; }
    }

    public class MeterRequest
    {
        public string MeterReadingId { get; set; }
        public string AccountId { get; set; }
        public long? Reading { get; set; }

        public static MeterRequest FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            MeterRequest MeterReadingEntity = new MeterRequest();
            try
            {
                MeterReadingEntity.MeterReadingId = Convert.ToString(values[0]);
                MeterReadingEntity.AccountId = Convert.ToString(values[1]);
                if(values[2] == "")
                {
                    MeterReadingEntity.Reading = 0;
                }
                else
                {
                    MeterReadingEntity.Reading = Convert.ToInt64(values[2]);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return MeterReadingEntity;
        }
    }

    public class MeterResponse : MeterRequest
    {
        public bool Success { get; set; }
    }
}
