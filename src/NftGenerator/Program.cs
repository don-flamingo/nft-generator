
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NftGenerator.Models;
using NftGenerator.OpenSea;

namespace NftGenerator
{
    class Program
    {
        private static ICollection<string> traitTypeDirs = new[]
        {
            @"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Background",
            @"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Base",
            @"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Suit",
            @"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Gun",
            @"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Watch",
        };

        static void Main(string[] args)
        {
            // var cfg = new WebScrapperConfiguration
            // {
            //     CollectionAddress = "https://opensea.io/collection/flamingo-mafia-family",
            //     Browser = "Firefox"
            // };
            //
            // var uploader = new WebScrapperUploader(cfg);

            var generator = new NftGenerator();
            var traitTypeOptions = new List<TraitTypeOption>
            {
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Background", true, false),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Emotion", false, false),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Clothes", false, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Shirt", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Small equipment", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Hand", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Neck", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Mask", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Eyes", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Head", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Eyes accessory", true, true),
            };

            foreach (var traitTypeOption in traitTypeOptions)
            {
                var files = Directory.GetFiles(traitTypeOption.DirPath);
                foreach (var file in files)
                {
                    traitTypeOption.AddTrait(file, traitTypeOption);
                }
            }

            var nfts = generator.Generate("outputs", traitTypeOptions);

            var jsonFile = "database.json";
            var json = JsonSerializer.Serialize(nfts, new JsonSerializerOptions()
            {
                WriteIndented = true
            });

            File.WriteAllText(jsonFile, json);


        }
    }
}
