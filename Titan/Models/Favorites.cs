using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.Models
{
    public class Favorites
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime Added { get; set; }
        public string Notes { get; set; }
    }
}
