﻿using System;
using System.Linq;
using Titan.Ed.Markup.Body;
using Titan.Models;
using Titan.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TextElement = Titan.Models.TextElement;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Titan
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class TabPage : Page
    {
        public TabViewModel viewModel = new TabViewModel();
        private DataTransferManager dataTransferManager;
        private TabPageParameters pageParameters;

        public TabPage()
        {
            this.InitializeComponent();
            RefreshButton.Click += RefreshButton_Click;
            GoButton.Click += GoButton_Click;
            ShareButton.Click += ShareButton_Click;
            OpenFavoriteButton.Click += OpenFavoriteButton_Click;

            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += OnDataRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is TabPageParameters)
            {
                pageParameters = e.Parameter as TabPageParameters;
                if (pageParameters.PageUrl != null)
                {
                    viewModel.LoadPage(pageParameters.PageUrl);
                }

                if (pageParameters.File != null)
                {
                    viewModel.LoadFile(pageParameters.File);
                }
            }
        }

        private void OpenFavoriteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.SetText("Check out this awesome UWP app!");
            request.Data.SetWebLink(new Uri(viewModel.Address));
            request.Data.Properties.Title = "Share this link";
            request.Data.Properties.Description = "This is an example of sharing in UWP.";
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e) => DataTransferManager.ShowShareUI();

        private void RefreshButton_Click(object sender, RoutedEventArgs e) => viewModel.ReloadPage();

        private void GoButton_Click(object sender, RoutedEventArgs e) => Navigate();


        private void Navigate()
        {
            if (!Uri.IsWellFormedUriString(Direction.Text, UriKind.Absolute))
            {
                return;
            }

            if (!Direction.Text.StartsWith("gemini://"))
            {
                Direction.Text = $"gemini://{Direction.Text}";
            }

            viewModel.LoadPage(Direction.Text);
        }

        private void Render(GemPage req)
        {

            Content.Blocks.Clear();
            if (req is OnlineGemPage)
            {
                var page = (OnlineGemPage)req;
                if (page.Response.Status.StartsWith("4") || page.Response.Status.StartsWith("5"))
                {
                    var paragraph = new Paragraph();

                    var run = new Run
                    {
                        Text = page.Response.ErrorMessage,
                        FontFamily = new FontFamily("Consolas")
                    };
                    paragraph.Inlines.Add(run);
                    Content.Blocks.Add(paragraph);
                }
                return;
            }

            pageParameters.Tab.Header = req.Title;

            foreach (var l in req.Layout)
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
                    Content.Blocks.Add(paragraph);
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
                    Content.Blocks.Add(paragraph);
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
                    Content.Blocks.Add(paragraph);
                }
            }
        }
    }
}
