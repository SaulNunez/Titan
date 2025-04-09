using System;
using Titan.Models;
using Windows.Storage;
using Windows.UI.Popups;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;


namespace Titan.ViewModels
{
    public partial class TabViewModel : ObservableObject
    {
        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        public NavigatableList<GemPage> browser = new NavigatableList<GemPage>();

        private TaskNotifier<GemPage> requestPageTask;

        public Task<GemPage> PageTask
        {
            get => requestPageTask;
            set => SetPropertyAndNotifyOnCompletion(ref requestPageTask, value);
        }

        private bool _canGoBack = false;
        private bool _canGoForward = false;

        public TabViewModel()
        {
            GoForwardCommand = new RelayCommand(GoForward, () => browser.CanGoForward);
            GoBackCommand = new RelayCommand(GoBack, () => browser.CanGoBack);
            RefreshCommand = new RelayCommand(RefreshPage);
        }

        public Task<GemPage> RequestPageTask
        {
            get => requestPageTask;
            set => SetPropertyAndNotifyOnCompletion(ref requestPageTask, value);
        }

        public bool CanGoBack 
        { 
            get => _canGoBack; 
            set => SetProperty(ref _canGoBack, value); 
        }
        public bool CanGoForward 
        { 
            get => _canGoForward; 
            set => SetProperty(ref _canGoForward, value); 
        }
        public ICommand RefreshCommand { get; }

        public void RefreshPage()
        {
            //LoadPage(Address);
        }

        public async void LoadFile(StorageFile path)
        {
            try
            {
                PageTask =  FileGemPage.LoadAsync(path).ContinueWith(task => (GemPage)task.Result);

                browser.Push(await PageTask);
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
                PageTask = OnlineGemPage.LoadAsync(uri).ContinueWith(task => (GemPage)task.Result);
                browser.Push(await PageTask);
                Address = path;

                var messageDialog = new MessageDialog("Loaded");
                await messageDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog(ex.Message);
            await messageDialog.ShowAsync();
            }
        }

        public ICommand GoForwardCommand { get; }

        void GoForward() => browser.GoForward();

        public ICommand GoBackCommand { get; }

        void GoBack() => browser.GoBack();
    }
}
