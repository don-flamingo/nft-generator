using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.Json;
using NftGenerator.Extensions;
using NftGenerator.Models;
using NftGenerator.Serializator;

namespace NftGenerator
{
    public class NftGenerator
    {
        public ICollection<Nft> Generate(string outputDir,
            int size,
            string imageUrlTemplate,
            string externalUrlTemplate,
            string description,
            int height,
            int width,
            int id,
            ICollection<TraitTypeOption> traitTypeDirs,
            ICollection<NftTrait> baseProperties)
        {
            var nfts = new List<Nft>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                WriteIndented = true,
                IgnoreNullValues = true
            };

            var dict = traitTypeDirs
                .ToDictionary(x => x,
                    v => v.TraitOptions);

            var traitOptionsLists = dict.Values.AsEnumerable()
                .Select(x => x.AsEnumerable());

            var alls = traitOptionsLists
                .Select(x => x.AsEnumerable())
                .CartesianProduct(size);

            var traitsCount = traitTypeDirs.Sum(x => x.TraitOptions.Count);

            Console.WriteLine($"Traits count {traitsCount}");

            foreach (var traits in alls)
            {
                Console.WriteLine(id);

                var img = null as Image;
                var g = null as Graphics;
                var nft = new Nft(id,
                    imageUrlTemplate.Replace("{id}", id.ToString()),
                    externalUrlTemplate.Replace("{id}", id.ToString()),
                    description,
                    new List<NftTrait>(baseProperties));

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
                        nft.Traits.Add(new NftTrait(trait.TraitTypeOption.Name, trait.Name));
                    }
                }

                if (img == null)
                {
                    continue;
                }

                var jsonPath = Path.Combine(outputDir, $"{id}.json");
                var json = JsonSerializer.Serialize(nft, jsonOptions);
                var imgPath = Path.Combine(outputDir, $"{id++}.png");

                File.WriteAllText(jsonPath, json);

                // var resizedImg = img.GetThumbnailImage(width, height, null,IntPtr.Zero);
                img.Save(imgPath, ImageFormat.Png);

              //  resizedImg.Dispose();
                img.Dispose();

                nfts.Add(nft);
            }

            return nfts;
        }
    }
}
