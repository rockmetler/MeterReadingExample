using MeterAPI.DAL.Entities;
using MeterAPI.DAL.Interface;

using MeterContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterAPI.DAL
{
    public class Meter : IMeter
    {
        public async Task<List<MeterResponse>> CreateMeterReadings(EntityContainer request)
        {
            using (var db = new MeterDBContext())
            {
                var accountNumbers = db.TestAccounts.Select(y => y.AccountId);
                await db.AddRangeAsync(request.ValidList.Where(x => accountNumbers.Contains(x.AccountId)).ToList());

                int alteredRows = await db.SaveChangesAsync();
                var insertedRecs = db.MeterReadings.Where(x => request.ValidList.Contains(x)).ToList();
                List<MeterResponse> response = new List<MeterResponse>();
                foreach(var rec in insertedRecs)
                {
                    response.Add(new MeterResponse()
                    {
                        AccountId = rec.AccountId,
                        MeterReadingId = rec.MeterReadingId,
                        Reading = long.Parse(rec.Reading.Value.ToString("0000")),
                        Success = true
                    });
                }
                foreach(var rec in request.InvalidList)
                {
                    response.Add(new MeterResponse()
                    {
                        AccountId = rec.AccountId,
                        MeterReadingId = rec.MeterReadingId,
                        Reading = rec.Reading,
                        Success = false
                    });
                }
                return response;
            }
        }

        public async Task<bool> DeleteMeterReading(string MeterReadingId)
        {
            using (var db = new MeterDBContext())
            {
                if (MeterReadingId != null)
                {
                    var Item = db.MeterReadings.Where(x => x.MeterReadingId == MeterReadingId).First();
                    var result = db.Remove(Item);
                    if (await db.SaveChangesAsync() > 0)
                    {
                        return true;
                    };
                }
            }
            return false;
        }

        public List<MeterResponse> GetByAccountId(string AccountId)
        {
            using (var db = new MeterDBContext())
            {
                if (AccountId != null)
                {
                    var result = db.MeterReadings.Where(x => x.AccountId == AccountId).ToList();
                    var accountList = new List<MeterResponse>();
                    foreach (var i in result)
                    {
                        accountList.Add(new MeterResponse()
                        {
                            AccountId = i.AccountId,
                            MeterReadingId = i.MeterReadingId,
                            Reading = i.Reading,
                            Success = true
                        });
                    }
                    return accountList;
                }
                else
                {
                    return new List<MeterResponse>();
                }
            }
        }

        public MeterResponse GetByMeterReadingId(string MeterReadingId)
        {
            using (var db = new MeterDBContext())
            {
                if (MeterReadingId != null)
                {
                    var result = db.MeterReadings.Where(x => x.MeterReadingId == MeterReadingId).First();
                    return new MeterResponse()
                    {
                        AccountId = result.AccountId,
                        MeterReadingId = result.MeterReadingId,
                        Reading = result.Reading,
                        Success = result != null
                    };
                }
                else
                {
                    return new MeterResponse();
                }
            }
        }

        public async Task<MeterResponse> UpsertMeterReading(MeterReadingEntity Entity)
        {
            using (var db = new MeterDBContext())
            {
                if (Entity.MeterReadingId != null)
                {
                    if (db.MeterReadings.Where(x => x.MeterReadingId == Entity.MeterReadingId).FirstOrDefault() != null)
                    {
                        db.Update(Entity);
                    }
                    else
                    {
                        await db.AddAsync(Entity);
                    }
                }
                try
                {
                    int alteredRows = await db.SaveChangesAsync();
                    var insertedRec = db.MeterReadings.Last();
                    MeterResponse response = new MeterResponse()
                    {
                        AccountId = insertedRec.AccountId,
                        MeterReadingId = insertedRec.MeterReadingId,
                        Reading = insertedRec.Reading,
                        Success = alteredRows > 0
                    };
                    return response;
                }
                catch (Exception ex)
                {
                    return new MeterResponse()
                    {
                        Success = false
                    };
                }
            }
        }
    }
}
