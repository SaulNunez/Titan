using System;
using Titan.Models;
using Titan.ViewModels;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private DataTransferManager dataTransferManager;
        private TabPageParameters pageParameters;

        public TabPage()
        {
            this.InitializeComponent();
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
    }
}
