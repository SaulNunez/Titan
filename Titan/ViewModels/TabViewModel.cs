using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.Ed;
using Titan.Models;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace Titan.ViewModels
{
    public class TabViewModel
    {
        public string Name { get; set; }
        public string Direction { get; set; }
        public List<GeminiResponse> browsedPages = new List<GeminiResponse>();
        public int currentIndex = 0;
        public bool isLoading = true;
        public bool canGoBack = false;
        public bool canGoForward = false;
        public string currentPageContent = string.Empty;

        public GeminiResponse CurrentPage
        {
            get { return browsedPages[currentIndex]; }
        }

        public void ReloadPage()
        {
            LoadPage(Direction);
        }

        public async void LoadPage(string uri)
        {
            try
            {
                var response = await Task.Run(() => GeminiPetition.Fetch(uri));
                Direction = uri;
                pageContentChanged?.Invoke(response);
            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
            }
        }

        public delegate void PageContentChanged(GeminiResponse pageContent);
        public event PageContentChanged pageContentChanged;


        internal void GoForward()
        {
            throw new NotImplementedException();
        }

        internal void GoBack()
        {
            throw new NotImplementedException();
        }
    }
}
