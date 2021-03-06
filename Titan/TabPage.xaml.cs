using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Titan.Ed;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Titan
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class TabPage : Page
    {
        char[] delimiters = { ' ', '\t' };

        public TabPage()
        {
            this.InitializeComponent();
            Direction.TextChanged += Direction_TextChanged;
        }

        private void Direction_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!Uri.IsWellFormedUriString(Direction.Text, UriKind.Absolute))
            {
                return;
            }

            if (!Direction.Text.StartsWith("gemini://"))
            {
                Direction.TextChanged -= Direction_TextChanged;
                Direction.Text = $"gemini://{Direction.Text}";
                Direction.TextChanged += Direction_TextChanged;
            }

            var response = GeminiPetition.Fetch(Direction.Text);
            if (response.IsSuccess)
            {
                Content.Blocks.Clear();
                var items = response.Body.Split("\n");
                //var msgBox = new MessageDialog($"Size: {items.Count()}");
                //msgBox.ShowAsync();

                foreach (var l in items)
                {
                    if (l.StartsWith("=>"))
                    {
                        var sections = l.Split(delimiters, 3);
                        var hyperlink = new Hyperlink();
                        if (Uri.IsWellFormedUriString(sections[1], UriKind.Absolute))
                        {
                            hyperlink.NavigateUri = new Uri(sections[1]);
                        }
                        else if (Uri.IsWellFormedUriString(sections[1], UriKind.Relative))
                        {
                            hyperlink.NavigateUri = new Uri(new Uri(Direction.Text), sections[1]);
                        }
                        var run = new Run();
                        if (sections.Length > 2)
                        {
                            run.Text = sections[2];
                        }
                        else
                        {
                            run.Text = sections[1];
                        }

                        hyperlink.Inlines.Add(run);

                        var paragraph = new Paragraph();
                        paragraph.Inlines.Add(hyperlink);
                        Content.Blocks.Add(paragraph);
                    }
                    else if (l.StartsWith("###"))
                    {
                        var paragraph = new Paragraph();
                        var run = new Run();
                        run.Text = l.Substring(3).TrimStart();
                        paragraph.Inlines.Add(run);
                        Content.Blocks.Add(paragraph);
                    }
                    else if (l.StartsWith("##"))
                    {
                        var paragraph = new Paragraph();
                        var run = new Run();
                        run.Text = l.Substring(2).TrimStart();
                        paragraph.Inlines.Add(run);
                        Content.Blocks.Add(paragraph);
                    }
                    else if (l.StartsWith("#"))
                    {
                        var paragraph = new Paragraph();
                        var run = new Run();
                        run.Text = l.Substring(1).TrimStart();
                        paragraph.Inlines.Add(run);
                        Content.Blocks.Add(paragraph);
                    }
                    else if (l.StartsWith('>'))
                    {
                        var paragraph = new Paragraph();
                        var span = new Span();
                        var run = new Run();
                        run.Text = l;
                        span.Inlines.Add(run);
                        paragraph.Inlines.Add(span);
                        Content.Blocks.Add(paragraph);
                    }
                    else if (l.StartsWith('*'))
                    {
                        var run = new Run();
                        run.Text = $"\u2022 {l}";

                        var paragraph = new Paragraph();
                        paragraph.Inlines.Add(run);
                        Content.Blocks.Add(paragraph);
                    }
                    else
                    {
                        var paragraph = new Paragraph();
                        var run = new Run();
                        run.Text = l;
                        paragraph.Inlines.Add(run);
                        Content.Blocks.Add(paragraph);
                    }
                }
            }

            if (response.IsRedirect)
            {
                Direction.Text = response.Meta;
            }
        }
    }
}
