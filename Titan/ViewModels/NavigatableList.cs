using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Titan.ViewModels
{
    public class NavigatableList<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly List<T> browsedPages = new List<T>();
        private int _currentIndex = 0;
        private bool canGoBack = false;
        private bool canGoForward = false;
        private T currentItem = default;

        public bool CanGoBack
        {
            get => canGoBack;
            set
            {
                if(canGoBack == value) return;
                canGoBack = value;
                OnPropertyChanged(nameof(CanGoBack));
            }
        }
        public bool CanGoForward
        {
            get => canGoForward;
            set
            {
                if(canGoForward == value) return;
                canGoForward = value;
                OnPropertyChanged(nameof(CanGoForward));
            }
        }
        public T CurrentItem
        {
            get => currentItem;
            set
            {
                currentItem = value;
                OnPropertyChanged(nameof(CurrentItem));
            }
        }

        public void Push(T item)
        {
            if(_currentIndex == browsedPages.Count - 1 || browsedPages.Count == 0)
            {
                browsedPages.Add(item);
                _currentIndex++;
                CanGoForward = false;
            }
            else
            {
                // If we have backed out of a page while browsing
                // And we click to a different one
                // We should pop all the pages the user can forward
                // And replace them with this new page
                browsedPages.RemoveRange(_currentIndex + 1, (browsedPages.Count - 1) - _currentIndex );
                browsedPages.Add(item);
                _currentIndex = browsedPages.Count - 1;
                CanGoForward = false;
            }

            CurrentItem = browsedPages[_currentIndex];
        }

        public void GoBack()
        {
            _currentIndex--;
            CanGoForward = true;
            if (_currentIndex < 0)
            {
                _currentIndex = 0;
                CanGoBack = false;
            }
            CurrentItem = browsedPages[_currentIndex];
        }

        public void GoForward()
        {
            _currentIndex++;
            CanGoBack = true;
            if (_currentIndex >= browsedPages.Count)
            {
                _currentIndex = browsedPages.Count - 1;
                CanGoForward = false;
            }
            CurrentItem = browsedPages[ _currentIndex];
        }
    }
}
