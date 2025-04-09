using System;
using System.Collections.Generic;
using Titan.Ed.Markup.Body;
using Titan.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using TextElement = Titan.Models.TextElement;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Titan.CustomControls
{
    public sealed partial class GemTextBox : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(GemPage), new PropertyMetadata(null, OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue == null)
            {
                return;
            }

            var control = d as GemTextBox;
            control?.UpdateContent();
        }

        private void UpdateContent()
        {
            var content = new List<Block>();
            foreach (var l in GemText.Layout)
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
                    TextContent.Blocks.Add(paragraph);
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
                    TextContent.Blocks.Add(paragraph);
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
                    TextContent.Blocks.Add(paragraph);
                }
            }

        }

        public GemPage GemText
        {
            get => (GemPage)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public GemTextBox()
        {
            this.InitializeComponent();
        }
    }
}
