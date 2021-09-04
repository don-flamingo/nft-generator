using System.Collections.Generic;
using System.IO;

namespace NftGenerator.Models
{
    public class TraitTypeOption
    {
        public TraitTypeOption(
            string dirPath,
            bool nullable,
            bool notable,
            string name = null)
        {
            DirPath = dirPath;
            Nullable = nullable;
            Name = name;
            Notable = notable;

            if (nullable)
            {
                TraitOptions.Add(null);
            }

            if (string.IsNullOrEmpty(name))
            {
                Name = Path.GetFileNameWithoutExtension(DirPath);
            }
        }

        public string DirPath { get; private set; }
        public string Name { get; private set; }
        public bool Nullable { get; private set; }
        public bool Notable { get; private set; }
        public ICollection<TraitOption> TraitOptions { get; private set; } = new List<TraitOption>();

        public void AddTrait(string file, TraitTypeOption traitTypeOption)
        {
            TraitOptions.Add(new TraitOption(file, traitTypeOption));
        }
    }
}
