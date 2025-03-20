using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.Ed;
using Titan.Models;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace Titan.ViewModels
{
    public class TabViewModel
    {
        public string Name 
        { 
            get => CurrentPage.Title; 
        }
        public string Address { get; set; }
        public List<GemPage> browsedPages = new List<GemPage>();
        public int currentIndex = 0;
        public bool isLoading = true;
        public bool canGoBack = false;
        public bool canGoForward = false;

        public GemPage CurrentPage
        {
            get { return browsedPages[currentIndex]; }
        }

        public void ReloadPage()
        {
            LoadPage(Address);
        }
        public async void LoadFile(StorageFile path)
        {
            try
            {
                var newPage = await FileGemPage.LoadAsync(path);
                Address = path.Path;
            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
            }
        }

        public async void LoadPage(string path)
        {
            try
            {
                var uri = new Uri(path);
                var newPage = await OnlineGemPage.LoadAsync(uri);
                Address = path;
            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
            }
        }

        public delegate void PageContentChanged(GemPage pageContent);
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
