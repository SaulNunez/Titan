using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Titan.Ed;
using Titan.Ed.Markup;
using Titan.Ed.Parsing;
using Titan.Parsing;

namespace Titan.Models
{
    public class OnlineGemPage: GemPage
    {
        public GeminiResponse Response { get; set; }

        public static async Task<OnlineGemPage> LoadAsync(Uri uri)
        {
            var response = await new GeminiPetition(uri).Fetch();
            var parsedResponse = await response.BodyElements();

            var firstTitle = parsedResponse.FirstOrDefault(item => (item is TextElement) ? (item as TextElement).Type == TextElement.TextType.Heading1 : false);
            var title = firstTitle != null ? (firstTitle as TextElement).Text : "";

            return new OnlineGemPage
            {
                Layout = parsedResponse,
                Title = title,
                Response = response
            };
        }
    }
}
