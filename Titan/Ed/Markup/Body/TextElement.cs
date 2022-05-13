using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.Models
{
    internal class TextElement
    {
        public enum TextType
        {
            Text,
            Heading1,
            Heading2,
            Heading3,
            UnorderedList,
            Quote
        }
        public string Text { get; set; }
        public TextType Type { get; set; } = TextType.Text;
    }
}
