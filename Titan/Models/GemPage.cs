using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titan.Ed.Markup;

namespace Titan.Models
{
    public class GemPage
    {
        public string Title { get; set; }

        public List<GeminiElement> Layout { get; set; }
    }
}
