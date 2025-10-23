using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsFleetIntel.Infrastructure.Configuration
{
    public class CacheSettings
    {
        public const string SectionName = "CacheSettings";
        public int DefaultExpirationMinutes { get; set; } = 60;
    }
}
