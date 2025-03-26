using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.Ed;
using Titan.ViewModels;

namespace Titan.Models
{
    internal class TabModel
    {
        private TabViewModel viewModel;

        public async void BrowseUrl(string url)
        {
            viewModel.isLoading = true;
            var uri = new Uri(url);
            var response = await new GeminiPetition(uri).Fetch();
            viewModel.isLoading = false;

            if (viewModel.CurrentIndex != viewModel.browser.Count - 1)
            {
                viewModel.browser.RemoveRange(viewModel.CurrentIndex + 1, viewModel.browser.Count - 1);
            }

            viewModel.browser.Add(response);
            viewModel.CurrentIndex = viewModel.browser.Count - 1;
        }

        public void GoBack()
        {
            if (viewModel.CurrentIndex > 0)
                viewModel.CurrentIndex--;

            viewModel.CanGoForward = true;
        }

        public void GoForward()
        {
            if (viewModel.CurrentIndex < viewModel.browser.Count - 1)
                viewModel.CurrentIndex++;

            viewModel.CanGoBack = true;
        }
    }
}
