using MeterAPI.DAL.Entities;

using MeterContracts;

using Microsoft.AspNetCore.Http;

using System.Collections.Generic;
using System.Threading.Tasks;

using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace MeterAPI.BL.Interface
{
    [TransientService]
    public interface IMeterService
    {
        Task<MeterRequestContainer> ProcessMeterCSV(IFormFile file);
        Task<List<MeterResponse>> CreateMeterReadings(MeterRequestContainer request);
        Task<MeterResponse> UpsertMeterReading(MeterRequest request);
        MeterResponse GetMeterReading(string MeterId);
        Task<bool> DeleteMeterReading(string MeterId);
    }
}
