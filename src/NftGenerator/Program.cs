
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json;
using NftGenerator.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NftGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationFile = "appsettings.json";
            var content = File.ReadAllText(configurationFile);
            var configuration = JsonConvert.DeserializeObject<Configuration>(content);

        //    GetTraitsStats();

            GenerateNfts(configuration);
        }

        private static void GetTraitsStats()
        {
            var p =  Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + "\\";

            var files = new List<string>()
            {
                @"D:\Repository\NftGenerator\src\NftGenerator\bin\Debug\net5.0\database.json",
                @"D:\Repository\NftGenerator\src\NftGenerator\bin\Debug\net5.0\database_1.json",
                @"D:\Repository\NftGenerator\src\NftGenerator\bin\Debug\net5.0\database_2.json",
                @"D:\Repository\NftGenerator\src\NftGenerator\bin\Debug\net5.0\database_3.json",
                @"D:\Repository\NftGenerator\src\NftGenerator\bin\Debug\net5.0\database_4.json",
                @"D:\Repository\NftGenerator\src\NftGenerator\bin\Debug\net5.0\database_5.json",
                @"D:\Repository\NftGenerator\src\NftGenerator\bin\Debug\net5.0\database_6.json",
                @"D:\Repository\NftGenerator\src\NftGenerator\bin\Debug\net5.0\database_7.json",
                @"D:\Repository\NftGenerator\src\NftGenerator\bin\Debug\net5.0\database_8.json"
            };

            var nfts = files
                .Select(File.ReadAllText)
                .Select(x => JsonSerializer.Deserialize<ICollection<Nft>>(x))
                .SelectMany(x => x)
                .ToList();

            var grouped = nfts.SelectMany(x => x.Traits)
                .GroupBy(x => x.Value);

            var id = 1;
            var list = new List<TraitTypeStatistic>();

            foreach (var group in grouped)
            {
                var type = group.First();
                var t = group.Count();
                var percentage = ((double)group.Count() / ((double)nfts.Count) * 100);
                var stats = new TraitTypeStatistic(
                    id++,
                    group.Key,
                    type.TraitType,
                    percentage);

                list.Add(stats);
            }

            var orderer = list.OrderBy(x => x.Rarity);
            var json = JsonSerializer.Serialize(orderer, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText("stats.json", json);

        }

        private static void GenerateNfts(Configuration configuration)
        {
            var generator = new NftGenerator();

            var traitTypeOptions = configuration.Traits;
            var count = configuration.Count;
            var output = configuration.OutputDir;
            var baseProperties = configuration.BaseTraits;

            foreach (var traitTypeOption in traitTypeOptions)
            {
                var files = Directory.GetFiles(traitTypeOption.DirPath);
                foreach (var file in files)
                {
                    traitTypeOption.AddTrait(file);
                }
            }

            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }

            var nfts = generator.Generate(
                output,
                count,
                configuration.ImageUrl,
                configuration.ExternalUrl,
                configuration.Description,
                configuration.Name,
                configuration.Backgrounds,
                configuration.Height,
                configuration.Width,
                configuration.StartId,
                configuration.Stats,
                traitTypeOptions,
                baseProperties);

            var jsonFile = "database.json";
            var json = JsonSerializer.Serialize(nfts, new JsonSerializerOptions()
            {
                WriteIndented = true
            });

            File.WriteAllText(jsonFile, json);
        }
    }
}
