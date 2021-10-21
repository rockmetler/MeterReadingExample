
using MeterAPI.DAL.Entities;

using MeterContracts;

using System.Collections.Generic;
using System.Threading.Tasks;

using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace MeterAPI.DAL.Interface
{
    [TransientService]
    public interface IMeter
    {
        Task<List<MeterResponse>> CreateMeterReadings(EntityContainer Request);
        List<MeterResponse> GetByAccountId(string AccountId);
        MeterResponse GetByMeterReadingId(string MeterReadingId);
        Task<MeterResponse> UpsertMeterReading(MeterReadingEntity Entity);
        Task<bool> DeleteMeterReading(string MeterReadingId);
    }
}
