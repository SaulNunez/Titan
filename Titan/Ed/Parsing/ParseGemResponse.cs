using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.Ed.Markup;
using Titan.Ed.Parsing;
using Titan.Models;

namespace Titan.Parsing
{
    public static class ParseGemResponse
    {
        public static readonly string Gemini_IME = "text/gemini";
        public static async Task<List<GeminiElement>> BodyElements(this GeminiResponse response)
        {
            if (!response.IsSuccess || response.Meta != Gemini_IME)
            {
                return new List<GeminiElement>();
            }

            return await response.Body.ParseGeminiElements();
        }
    }
}
