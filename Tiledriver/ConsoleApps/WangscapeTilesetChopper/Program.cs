using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using WangscapeTilesetChopper.Model;

namespace WangscapeTilesetChopper
{
    class Program
    {
        static void Main(string[] args)
        {
            var serializedDefinitions = File.ReadAllText(args[0]);
            
            var definitions = JsonSerializer.Deserialize<List<TileDefinition>>(
                serializedDefinitions, 
                new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
            
            foreach (var definition in definitions)
            {
                Console.Out.WriteLine(definition);
            }
        }
    }
}
