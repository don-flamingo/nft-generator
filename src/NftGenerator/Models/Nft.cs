using System.Collections.Generic;

namespace NftGenerator.Models
{
    public class Nft
    {
        public Nft(
            int id,
            IDictionary<string, string> properties)
        {
            Id = id;
            Properties = properties;
        }

        public int Id { get; private set; }
        public IDictionary<string, string> Properties { get; private set; }
    }
}
