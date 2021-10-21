using MeterAPI.Helpers.Interfaces;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MeterAPI.Helpers
{
    public class Helpers : IHelpers
    {
        public List<string> ReadAllLines(TextReader textReader)
        {
            string line;
            List<string> lines = new List<string>();
            while ((line = textReader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            return lines;
        }
    }
}
