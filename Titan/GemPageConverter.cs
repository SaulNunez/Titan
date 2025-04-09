using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.Ed.Markup.Body;
using Titan.Models;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using TextElement = Titan.Models.TextElement;

namespace Titan
{
    internal class GemPageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is OnlineGemPage onlineGemPage)
            {
                if (onlineGemPage.Response.Status.StartsWith("4") || onlineGemPage.Response.Status.StartsWith("5"))
                {
                    var paragraph = new Paragraph();

                    var run = new Run
                    {
                        Text = onlineGemPage.Response.ErrorMessage,
                        FontFamily = new FontFamily("Consolas")
                    };
                    paragraph.Inlines.Add(run);
                    return paragraph;
                }
                
            }
            if (value is GemPage page)
            {
                var content = new List<Block>();
                foreach (var l in page.Layout)
                {
                    if (l is LinkElement)
                    {
                        var hyperlink = new Hyperlink();
                        if (Uri.IsWellFormedUriString((l as LinkElement).Url, UriKind.Absolute))
                        {
                            hyperlink.NavigateUri = new Uri((l as LinkElement).Url);
                        }

                        var run = new Run();
                        run.Text = (l as LinkElement).UserFriendlyLinkName;

                        hyperlink.Inlines.Add(run);

                        var paragraph = new Paragraph();
                        paragraph.Inlines.Add(hyperlink);
                        content.Add(paragraph);
                    }
                    else if (l is PreformatedElement)
                    {
                        var paragraph = new Paragraph();

                        var run = new Run
                        {
                            Text = l.RawText,
                            FontFamily = new FontFamily("Consolas")
                        };
                        paragraph.Inlines.Add(run);
                        content.Add(paragraph);
                    }
                    else if (l is TextElement)
                    {
                        var paragraph = new Paragraph();
                        var run = new Run
                        {
                            Text = (l as TextElement).Type == TextElement.TextType.ListItem ? $"\u2022 {(l as TextElement).Text}" : (l as TextElement).Text
                        };
                        switch ((l as TextElement).Type)
                        {
                            case TextElement.TextType.Heading3:
                                break;
                            case TextElement.TextType.Heading2:
                                break;
                            case TextElement.TextType.Heading1:
                                break;
                        }
                        paragraph.Inlines.Add(run);
                        content.Add(paragraph);
                    }
                }
                return content.AsEnumerable();
            }

            return new Paragraph();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
