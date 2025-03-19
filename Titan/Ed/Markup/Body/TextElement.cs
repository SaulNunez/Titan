using Titan.Ed.Markup;

namespace Titan.Models
{
    public class TextElement: GeminiElement
    {
        public TextElement(string text) : base(text)
        {
            if (text.StartsWith("*"))
            {
                Type = TextType.ListItem;
                Text = text.Substring(1).Trim();
            } else if(text.StartsWith(">"))
            { 
                Type = TextType.Quote;
                Text = text.Substring(1).Trim();
            }
            else if (text.StartsWith("###"))
            {
                Type = TextType.Heading3;
                Text = text.Substring(3).Trim();
            }
            else if (text.StartsWith("##"))
            {
                Type = TextType.Heading2;
                Text = text.Substring(2).Trim();
            }
            else if (text.StartsWith("#"))
            {
                Type = TextType.Heading2;
                Text = text.Substring(1).Trim();
            }
            else
            {
                Type = TextType.Text;
                Text = text;
            }
        }

        public enum TextType
        {
            Text,
            Heading1,
            Heading2,
            Heading3,
            ListItem,
            Quote
        }
        public string Text { get; set; }
        public TextType Type { get; set; } = TextType.Text;
    }
}
