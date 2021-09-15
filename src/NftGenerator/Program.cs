
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using NftGenerator.Models;
using NftGenerator.OpenSea;

namespace NftGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new NftGenerator();
            var background = new TraitTypeOption (@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Background", false, true);

            var traitTypeOptions = new List<TraitTypeOption>
            {
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Big equipment", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Emotion", false, false),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Tattoos", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Clothes", false, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Shirt", false, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Small equipment", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Beak", false, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Jewelry", false, false),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Eyes", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Head", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Eyes accessories", true, true),
                new(@"C:\Users\patry\GDrive\Crypto\nft\crypto-flamingo\Border", false, false),
            };
            //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\1\Body", false, true),
            //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\1\Tattoos", true, true),
            //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\1\Mouth", false, true),
            //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\1\Eye or glasses", false, true),
            //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\1\Head", true, true),
            //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\1\Outfit", true, true),
            // };

            //{
                //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\3\Hand", false, false),
                //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\3\Equipment", true, true),
                //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\3\Body", false, true),
                //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\3\Tattoos", true, true),
                //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\3\Outfit", true, true),
                //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\3\Mouth", false, true),
                //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\3\Eye or glasses", false, true),
                //     new(@"C:\Users\patry\GDrive\Crypto\nft\flamingo-punks\3\Head", true, true),
                // };

                foreach (var traitTypeOption in traitTypeOptions)
                {
                    var files = Directory.GetFiles(traitTypeOption.DirPath);
                    foreach (var file in files)
                    {
                        traitTypeOption.AddTrait(file);
                    }
                }

                if (background != null)
                {
                    var files = Directory.GetFiles(background.DirPath);
                    foreach (var file in files)
                    {
                        background.AddTrait(file);
                    }
                }

                var nfts = generator.Generate("outputs"
                    , traitTypeOptions
                     ,background
                );

                var jsonFile = "database.json";
                var json = JsonSerializer.Serialize(nfts, new JsonSerializerOptions()
                {
                    WriteIndented = true
                });

                File.WriteAllText(jsonFile, json);
        }
    }
}
