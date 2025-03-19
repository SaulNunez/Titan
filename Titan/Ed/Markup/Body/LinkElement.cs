using System;
using System.Text.RegularExpressions;
using Titan.Ed.Markup;

namespace Titan.Models
{
    public class LinkElement: GeminiElement
    {
        private readonly string pattern = @"^=>\s*(\S+)\s*(.*)?$";
        public LinkElement(string input) : base(input)
        {
            var match = Regex.Match(input, pattern);
            if (match.Success)
            {
                Url = match.Groups[1].Value; // Captures the URL
                UserFriendlyLinkName = match.Groups[2].Value.Trim(); // Captures the optional link name
                if (UserFriendlyLinkName.Length > 0)
                {
                    UserFriendlyLinkName = Url;
                }
            }
        }

        // In case the url has a user friendly name, like the child of an <a> in HTML, if none given will return url.
        public string UserFriendlyLinkName { get; private set; }

        // String version of the url, can be relative or absolute
        public string Url { get; private set; }

        public Uri UriFromUrl => new Uri(Url, UriKind.RelativeOrAbsolute);
    }
}
