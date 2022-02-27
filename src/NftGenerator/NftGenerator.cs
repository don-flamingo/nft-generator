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
            string name,
            string backgrounds,
            int height,
            int width,
            int id,
            ICollection<string> stats,
            ICollection<TraitTypeOption> traitTypeDirs,
            ICollection<NftTrait> baseProperties)
        {
            var statsJson = File.ReadAllText("stats.json");
            var nfts = new List<Nft>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                WriteIndented = true,
                IgnoreNullValues = true
            };

            var statsValues = JsonSerializer.Deserialize<ICollection<Stats>>(statsJson, jsonOptions);

            var dict = traitTypeDirs
                .ToDictionary(x => x,
                    v => v.TraitOptions);

            var traitOptionsLists = dict.Values.AsEnumerable()
                .Select(x => x.AsEnumerable());

            var alls = traitOptionsLists
                .Select(x => x.AsEnumerable())
                .CartesianProduct(size);

            var backgroundsOpts =  new TraitTypeOption(backgrounds, false, true);
            foreach (var file in Directory.GetFiles(backgrounds))
            {
                backgroundsOpts.AddTrait(file);
            }

            var traitsCount = traitTypeDirs.Sum(x => x.TraitOptions.Count);

            Console.WriteLine($"Traits count {traitsCount}");

            foreach (var traits in alls)
            {
                Console.WriteLine(id);

                var img = null as Image;
                var g = null as Graphics;
                var nft = new Nft(id,
                    $"{name} #{id}",
                    imageUrlTemplate.Replace("{id}", id.ToString()),
                    externalUrlTemplate.Replace("{id}", id.ToString()),
                    description,
                    new List<NftTrait>(baseProperties));

                var backgroundTrait = backgroundsOpts.TraitOptions
                    .OrderByDescending(x => Guid.NewGuid())
                    .First();
                var nftTraits = traits.Where(x => x != null).ToList();
                var backgroundTraits = new List<TraitOption>
                {
                    backgroundTrait
                };
                backgroundTraits.AddRange(nftTraits);

                GenerateNftWithoutBackground(nft,
                    nftTraits,
                    jsonOptions,
                    id,
                    outputDir);

                foreach (var trait in backgroundTraits.Where(x => x != null))
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

                FillStats(stats, statsValues, nft);

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

        private void FillStats(ICollection<string> stats, ICollection<Stats> statsValues, Nft nft)
        {
            var statsValuesTransformed = statsValues.SelectMany(x => x.Traits).ToList();
            var traits = new List<NftTrait>();
            var sum = new List<IDictionary<string, int>>();

            var currents = nft.Traits.Select(x => x.Value.ToString());
            foreach (var current in currents)
            {
                var statValue = statsValuesTransformed.FirstOrDefault(x => x.Name == current);
                if (statValue == null)
                {
                    continue;
                }
                sum.Add(statValue.Skills);
            }


            var filled = new[] { stats.ToDictionary(x => x, v => 0) }
                .Union(sum)
                .ToList();

            foreach (var fill in filled)
            {
                foreach (var dict in fill)
                {
                    var trait = traits.FirstOrDefault(x => x.TraitType == dict.Key);
                    if (trait == null)
                    {
                        traits.Add(new NftTrait(dict.Key, dict.Value));
                    } 
                    else
                    {
                        traits.Remove(trait);
                        var value = ((int)trait.Value) + dict.Value;
                        if (value > 10)
                        {
                            value = 10;
                        }

                        traits.Add(new NftTrait(dict.Key, value));
                    }
                }
            }

            var s = traits.Sum(x => (int)x.Value);
            var lvl = s != 0
                ? traits.Sum(x => (int)x.Value) / traits.Count
                : 0;

            traits.Add(new NftTrait("Level", lvl, "number"));
                
            foreach (var trait in traits)
            {
                nft.Traits.Add(trait);
            }
                
        }

        private void GenerateNftWithoutBackground(Nft nft,
            ICollection<TraitOption> traits,
            JsonSerializerOptions jsonOptions,
            int id,
            string outputDir)
        {
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
            }

            if (img == null)
            {
                return;
            }

            var json = JsonSerializer.Serialize(nft, jsonOptions);
            var imgPath = Path.Combine(outputDir, $"{id++}_no_bg.png");

            // var resizedImg = img.GetThumbnailImage(width, height, null,IntPtr.Zero);
            img.Save(imgPath, ImageFormat.Png);

            //  resizedImg.Dispose();
            img.Dispose();
        }
    }
}

public class Test : Specification
{
    public void Test2()
    {
        int returnValue = 0;
        Given = 
            "first number" > As > (() => { var c = 0; }) >
            "second number" > As > (() => { var d = 9; });
        When = "action happening" > As > (() => returnValue = 8 );
        It = "should be 8" > As > (() => returnValue.Should().Be(8));
    }
}
