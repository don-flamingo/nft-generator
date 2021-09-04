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
        public ICollection<Nft> Generate(string outputDir, ICollection<TraitTypeOption> traitTypeDirs)
        {
            var nfts = new List<Nft>();
            var id = 1;

            var dict = traitTypeDirs
                .ToDictionary(x => x,
                v => v.TraitOptions);

            var files = dict.Values
                .Select(x => x)
                .ToList();

            var alls = files.CartesianProduct()
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            foreach (var traits in alls)
            {
                var nft = new Nft(id, new Dictionary<string, string>());
                var img = null as Image;
                var g = null as Graphics;

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
                nfts.Add(nft);
            }

            return nfts;
        }
    }
}
