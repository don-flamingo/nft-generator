using System.Collections.Generic;

namespace NftGenerator.Models
{
    public class Nft
    {
        public Nft(
            int id,
            string image,
            string externalLink,
            string description,
            ICollection<NftTrait> traits)
        {
            Id = id;
            Traits = traits;
            Description = description;
            Name = $"#{id}";
            ExternalLink = externalLink;
            Image = image;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string ExternalLink { get; private set; }
        public string Image { get; private set; }
        public string Description { get; private set; }
        public ICollection<NftTrait> Traits { get; private set; }
    }

    public class NftTrait
    {
        public NftTrait(string traitType, string value)
        {
            TraitType = traitType;
            Value = value;
        }

        public string TraitType { get; private set; }
        public string Value { get; private set; }
    }
}
