using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Titan.Ed.Markup;
using Titan.Ed.Parsing;
using Windows.Storage;

namespace Titan.Models
{
    public class FileGemPage: GemPage
    {
        public static async Task<FileGemPage> LoadAsync(StorageFile file)
        {
            string text = await FileIO.ReadTextAsync(file);
            var parsedResponse = await text.ParseGeminiElements();

            return new FileGemPage
            {
                Layout = parsedResponse,
                Title = file.Name
            };
        }
    }
}
