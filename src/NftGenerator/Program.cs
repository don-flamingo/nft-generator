
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using NftGenerator.Models;
using NftGenerator.OpenSea;
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
                configuration.Height,
                configuration.Width,
                configuration.StartId,
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
