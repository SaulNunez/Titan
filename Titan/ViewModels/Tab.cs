using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.Ed;
using Titan.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Titan.ViewModels
{
    public class Tab
    {
        public string Name { get; set; }
        public List<GeminiResponse> browsedPages = new List<GeminiResponse>();
        public int currentIndex = 0;
        public bool isLoading = true;
        public bool canGoBack = false;
        public bool canGoForward = false;

        public GeminiResponse CurrentPage
        {
            get { return browsedPages[currentIndex]; }
        }

        public void Browse(string url)
        {
            isLoading = true;
            var response = GeminiPetition.Fetch(url);
            isLoading = false;

            if(currentIndex != browsedPages.Count - 1)
            {
                browsedPages.RemoveRange(currentIndex + 1, browsedPages.Count - 1);
            }

            browsedPages.Append(response);
            currentIndex = browsedPages.Count - 1;
        }

        public void GoBack()
        {
            if(currentIndex > 0)
                currentIndex--;

            canGoForward = true;
        }

        public void GoForward()
        {
            if(currentIndex < browsedPages.Count - 1)
                currentIndex++;

            canGoBack = true;
        }

        public static readonly DependencyProperty Body =
            DependencyProperty.RegisterAttached("BodyText", typeof(string),
                typeof(Tab), new PropertyMetadata(String.Empty, OnBodyChanged));

        private static void OnBodyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RichTextBlock;
        }
    }
}
