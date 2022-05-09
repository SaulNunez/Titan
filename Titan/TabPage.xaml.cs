using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Titan.Ed;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
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
        public TabPage()
        {
            this.InitializeComponent();
            Direction.TextChanged += Direction_TextChanged;
        }

        private void Direction_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!Uri.IsWellFormedUriString(Direction.Text, UriKind.Absolute))
            {
                return;
            }

            if (!Direction.Text.StartsWith("gemini://"))
            {
                Direction.TextChanged -= Direction_TextChanged;
                Direction.Text = $"gemini://{Direction.Text}";
                Direction.TextChanged += Direction_TextChanged;
            }

            var response = GeminiPetition.Fetch(Direction.Text);
            if (response.IsSuccess)
            {
                Content.Text = response.Body;
            }

            if (response.IsRedirect)
            {
                Direction.Text = response.Meta;
            }
        }
    }
}
