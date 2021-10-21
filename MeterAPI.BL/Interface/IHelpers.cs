using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace MeterAPI.Helpers.Interfaces
{
    [TransientService]
    public interface IHelpers
    {
        List<string> ReadAllLines(TextReader textReader);
    }
}
