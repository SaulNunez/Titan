using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.Ed.Markup;
using Titan.Ed.Markup.Body;
using Titan.Models;

namespace Titan.Ed.Parsing
{
    public static class ParseGemString
    {
        private enum ProcessingStatus
        {
            NORMAL,
            PREPROCESSED
        }

        public static Task<List<GeminiElement>> ParseGeminiElements(this string input)
        {
            return Task.Run(() =>
            {
                var elements = new List<GeminiElement>();

                var processingStatus = ProcessingStatus.NORMAL;
                var preprocessedBuffer = new StringBuilder();

                foreach (var element in input.Split("\n"))
                {
                    Console.WriteLine(element);
                    switch (processingStatus)
                    {
                        case ProcessingStatus.NORMAL:
                            if (element.StartsWith("'''"))
                            {
                                preprocessedBuffer.Clear();
                                processingStatus = ProcessingStatus.PREPROCESSED;
                            }
                            else if (element.StartsWith("=>"))
                            {
                                elements.Add(new LinkElement(element));
                            }
                            else
                            {
                                elements.Add(new TextElement(element));
                            }
                            break;
                        case ProcessingStatus.PREPROCESSED:
                            preprocessedBuffer.AppendLine(element);

                            if (element.StartsWith("'''"))
                            {
                                elements.Add(new PreformatedElement(preprocessedBuffer.ToString()));
                                processingStatus = ProcessingStatus.NORMAL;
                            }
                            break;
                    }
                }

                return elements;
            });
        }
    }
}
