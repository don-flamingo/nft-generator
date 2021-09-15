using System.IO;

namespace NftGenerator.Models
{
    public class TraitOption
    {
        public TraitOption(string filePath,
            TraitTypeOption traitTypeOption,
            string? name = null)
        {
            FilePath = filePath;
            TraitTypeOption = traitTypeOption;
            Name = name;

            if (string.IsNullOrEmpty(name))
            {
                Name = Path.GetFileNameWithoutExtension(FilePath);
            }
        }

        public string FilePath { get; private set; }
        public string Name { get; private set; }
        public TraitTypeOption TraitTypeOption { get; private set; }

        public override bool Equals(object? obj)
        {
            if (obj is TraitOption traitOption)
            {
                return traitOption.FilePath == FilePath;
            }

            return base.Equals(obj);
        }
    }
}
