namespace Titan.Ed.Markup
{
    public class GeminiElement
    {
        public string RawText { get; private set; }

        public GeminiElement(string text) 
        {
            RawText = text;
        }
    }
}
