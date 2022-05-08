using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.Models
{
    internal class GeminiResponse
    {
        public int Status { get; set; }
        /// <summary>
        /// Is a UTF-8 encoded string of maximum length 1024 bytes, whose meaning is <STATUS> dependent
        /// </summary>
        public string Meta { get; set; }

        public string MimeType { get
            {
                if (Meta.Length == 0) return "text / gemini; charset = utf - 8";
                else return Meta;
            }
        }

        public string Body { get; set; }
    }
}
