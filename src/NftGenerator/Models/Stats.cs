using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NftGenerator.Models
{
    public class Stats
    {
        public string Name { get; set; }
        public ICollection<StatTrait> Traits { get; set; }
    }

    public class StatTrait
    {
        public string Name { get; set; }
        public Dictionary<string, int> Skills { get; set; } = new Dictionary<string, int>();
    }
}
