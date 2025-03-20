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

        public async void Browse(string url)
        {
            viewModel.isLoading = true;
            var uri = new Uri(url);
            var response = await new GeminiPetition(uri).Fetch();
            viewModel.isLoading = false;

            if (viewModel.currentIndex != viewModel.browsedPages.Count - 1)
            {
                viewModel.browsedPages.RemoveRange(viewModel.currentIndex + 1, viewModel.browsedPages.Count - 1);
            }

            viewModel.browsedPages.Add(response);
            viewModel.currentIndex = viewModel.browsedPages.Count - 1;
        }

        public void GoBack()
        {
            if (viewModel.currentIndex > 0)
                viewModel.currentIndex--;

            viewModel.canGoForward = true;
        }

        public void GoForward()
        {
            if (viewModel.currentIndex < viewModel.browsedPages.Count - 1)
                viewModel.currentIndex++;

            viewModel.canGoBack = true;
        }
    }
}
