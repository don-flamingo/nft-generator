using System.Collections.Generic;

namespace NftGenerator.Models
{
    public class Configuration
    {
        public int StartId { get; set; } = 1;
        public ICollection<NftTrait> BaseTraits { get; set; } = new List<NftTrait>();
        public int Count { get; set; }
        public string OutputDir { get; set; }
        public string ExternalUrl { get; set; }
        public string ImageUrl { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ICollection<TraitTypeOption> Traits { get; set; } = new List<TraitTypeOption>();
        public string Description { get; set; }
    }
}
