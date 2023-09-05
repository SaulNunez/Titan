using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Titan.Ed;
using Titan.Models;
using Titan.ViewModels;
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
        public TabViewModel viewModel = new TabViewModel();

        char[] delimiters = { ' ', '\t' };

        public TabPage()
        {
            this.InitializeComponent();
            Direction.TextChanged += Direction_TextChanged;
            BackButton.Click += BackButton_Click;
            ForwardButton.Click += ForwardButton_Click;
            GoButton.Click += GoButton_Click;
            viewModel.pageContentChanged += Render;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate();
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.GoForward();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.GoBack();
        }

        private void Navigate()
        {
            if (!Uri.IsWellFormedUriString(Direction.Text, UriKind.Absolute))
            {
                return;
            }

            if (!Direction.Text.StartsWith("gemini://"))
            {
                Direction.TextChanged -= Direction_TextChanged;
                Direction.Text = $"gemini://{Direction.Text}";
                Direction.TextChanged += Direction_TextChanged;
            }
        }
        private void Direction_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.LoadPage(Direction.Text);
        }

        private void Render(GeminiResponse req)
        {
            var control = Content;

            control.Blocks.Clear();
            var items = req.Body.Split("\n");

            foreach (var l in items)
            {
                if (l.StartsWith("=>"))
                {
                    var sections = l.Substring(2).Split(new char[] { ' ', '\t' }, 2);
                    var hyperlink = new Hyperlink();
                    if (Uri.IsWellFormedUriString(sections[0], UriKind.Absolute))
                    {
                        hyperlink.NavigateUri = new Uri(sections[0]);
                    }
                    else if (Uri.IsWellFormedUriString(sections[0], UriKind.Relative))
                    {
                        //TODO: Link relative uri
                        //hyperlink.NavigateUri = new Uri(new Uri(string.Empty), sections[0]);
                    }
                    var run = new Run();
                    // Trim is used here, or there's a lot of empty space on links
                    if (sections.Length > 1)
                    {
                        run.Text = sections[1].Trim();
                    }
                    else
                    {
                        run.Text = sections[0].Trim();
                    }

                    hyperlink.Inlines.Add(run);

                    var paragraph = new Paragraph();
                    paragraph.Inlines.Add(hyperlink);
                    control.Blocks.Add(paragraph);
                }
                else if (l.StartsWith("###"))
                {
                    var paragraph = new Paragraph();
                    var run = new Run();
                    run.Text = l.Substring(3).TrimStart();
                    paragraph.Inlines.Add(run);
                    control.Blocks.Add(paragraph);
                }
                else if (l.StartsWith("##"))
                {
                    var paragraph = new Paragraph();
                    var run = new Run();
                    run.Text = l.Substring(2).TrimStart();
                    paragraph.Inlines.Add(run);
                    control.Blocks.Add(paragraph);
                }
                else if (l.StartsWith("#"))
                {
                    var paragraph = new Paragraph();
                    var run = new Run();
                    run.Text = l.Substring(1).TrimStart();
                    paragraph.Inlines.Add(run);
                    control.Blocks.Add(paragraph);
                }
                else if (l.StartsWith('>'))
                {
                    var paragraph = new Paragraph();
                    var span = new Span();
                    var run = new Run
                    {
                        Text = l
                    };
                    span.Inlines.Add(run);
                    paragraph.Inlines.Add(span);
                    control.Blocks.Add(paragraph);
                }
                else if (l.StartsWith('*'))
                {
                    var run = new Run
                    {
                        Text = $"\u2022 {l}"
                    };

                    var paragraph = new Paragraph();
                    paragraph.Inlines.Add(run);
                    control.Blocks.Add(paragraph);
                }
                else
                {
                    var paragraph = new Paragraph();
                    var run = new Run
                    {
                        Text = l
                    };
                    paragraph.Inlines.Add(run);
                    control.Blocks.Add(paragraph);
                }
            }
        }
    }
}
