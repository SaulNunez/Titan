using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Titan.Models
{
    internal class TabPageParameters
    {
        public string PageUrl { get; set; }
        public TabViewItem Tab { get; internal set; }
        public StorageFile File { get; internal set; }
    }
}
