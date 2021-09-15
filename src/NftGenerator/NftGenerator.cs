using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using NftGenerator.Extensions;
using NftGenerator.Models;

namespace NftGenerator
{
    public class NftGenerator
    {
        public ICollection<Nft> Generate(string outputDir, ICollection<TraitTypeOption> traitTypeDirs, TraitTypeOption background = null)
        {
            var nfts = new List<Nft>();
            var id = 1;
            var size = 2500;

            var dict = traitTypeDirs
                .ToDictionary(x => x,
                    v => v.TraitOptions);

            var traitOptionsLists = dict.Values.AsEnumerable()
                .Select(x => x.AsEnumerable());

            var alls = traitOptionsLists
                .Select(x => x.AsEnumerable())
                .CartesianProduct(size)
                .OrderByDescending(x => Guid.NewGuid())
                .Take(size)
                .ToList();

            var traitsCount = traitTypeDirs.Sum(x => x.TraitOptions.Count()) + background?.TraitOptions.Count() ?? 0;

            Console.WriteLine($"Traits count {traitsCount}");

            foreach (var traits in alls)
            {
                Console.WriteLine(id);

                var nft = new Nft(id, new Dictionary<string, string>());
                var img = null as Image;
                var g = null as Graphics;

                if (background != null)
                {
                    var backgroundTrait = background.TraitOptions
                        .OrderBy(x => Guid.NewGuid())
                        .First();

                    img = Image.FromFile(backgroundTrait.FilePath);
                    g = Graphics.FromImage(img);

                    if (backgroundTrait.TraitTypeOption.Notable)
                    {
                        nft.Properties.Add(backgroundTrait.TraitTypeOption.Name, backgroundTrait.Name);
                    }
                }

                foreach (var trait in traits.Where(x => x != null))
                {
                    if (img == null)
                    {
                        img = Image.FromFile(trait.FilePath);
                        g = Graphics.FromImage(img);
                    }
                    else
                    {
                        g.DrawImage(Image.FromFile(trait.FilePath), new Point(0, 0));
                    }

                    if (trait.TraitTypeOption.Notable)
                    {
                        nft.Properties.Add(trait.TraitTypeOption.Name, trait.Name);
                    }
                }

                if (img == null)
                {
                    continue;
                }

                img.Save(Path.Combine(outputDir, $"{id++}.png"), ImageFormat.Png);
                img.Dispose();

                nfts.Add(nft);
            }

            return nfts;
        }

    }
}
