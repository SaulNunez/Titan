
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Titan.Ed.Markup.Body;
using Titan.Ed.Parsing;
using Titan.Models;

namespace Titan.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            string gemString = "gmi-web makes writing HTML documents as simple as learning the handful of Gemini line-types:";

            var parseResults = await gemString.ParseGeminiElements();

            if (parseResults.Count > 0)
            {
                var element = parseResults.First();
                if (element is LinkElement)
                {
                    var textElement = element as TextElement;
                    Assert.AreEqual("gmi-web makes writing HTML documents as simple as learning the handful of Gemini line-types:", textElement.Text);
                    Assert.AreEqual(TextElement.TextType.Text, textElement.Type);
                }
            }
        }

        [TestMethod]
        public async Task TestHyperLink()
        {
            string gemString = "=> https://www.youtube.com/watch?v=DoEI6VzybDk\tOr, if you'd prefer, here's a video overview";

            var parseResults = await gemString.ParseGeminiElements();
            if (parseResults.Count > 0)
            {
                var element = parseResults.First();
                if (element is LinkElement)
                {
                    var textElement = element as LinkElement;
                    Assert.AreEqual("Or, if you'd prefer, here's a video overview", textElement.UserFriendlyLinkName);
                    Assert.AreEqual("https://www.youtube.com/watch?v=DoEI6VzybDk", textElement.Url);
                }
            }
        }

        [TestMethod]
        public async Task TestHyperLinkSimple()
        {
            string gemString = "=> https://codeberg.org/talon/gmi-web";

            var parseResults = await gemString.ParseGeminiElements();

            if (parseResults.Count > 0)
            {
                var element = parseResults.First();
                if (element is LinkElement)
                {
                    var textElement = element as LinkElement;
                    Assert.AreEqual("https://codeberg.org/talon/gmi-web", textElement.Url);
                }
            }
        }

        [TestMethod]
        public async Task TestHyperLinkSimpleFriendlyUri()
        {
            string gemString = "=> https://codeberg.org/talon/gmi-web";

            var parseResults = await gemString.ParseGeminiElements();

            if (parseResults.Count > 0)
            {
                var element = parseResults.First();
                if (element is LinkElement)
                {
                    var textElement = element as LinkElement;
                    Assert.AreEqual("https://codeberg.org/talon/gmi-web", textElement.UserFriendlyLinkName);
                    Assert.AreEqual("https://codeberg.org/talon/gmi-web", textElement.Url);
                }
            }
        }

        [TestMethod]
        public async Task TestQuote()
        {
            string gemString = "> a bridge between HTML and Gemini";

            var parseResults = await gemString.ParseGeminiElements();

            if (parseResults.Count > 0)
            {
                var element = parseResults.First();
                if (element is LinkElement)
                {
                    var textElement = element as TextElement;
                    Assert.AreEqual("a bridge between HTML and Gemini", textElement.Text);
                    Assert.AreEqual(TextElement.TextType.Quote, textElement.Type);
                }
            }
        }

        [TestMethod]
        public async Task TestHeader1()
        {
            string gemString = "# Project Gemini";

            var parseResults = await gemString.ParseGeminiElements();
            if (parseResults.Count > 0)
            {
                var element = parseResults.First();
                if (element is LinkElement)
                {
                    var textElement = element as TextElement;
                    Assert.AreEqual("Project Gemini", textElement.Text);
                    Assert.AreEqual(TextElement.TextType.Heading1, textElement.Type);
                }
            }
        }

        [TestMethod]
        public async Task TestHeader2()
        {
            string gemString = "## Official resources";

            var parseResults = await gemString.ParseGeminiElements();

            if (parseResults.Count > 0)
            {
                var element = parseResults.First();
                if (element is LinkElement)
                {
                    var textElement = element as TextElement;
                    Assert.AreEqual("Official resources", textElement.Text);
                    Assert.AreEqual(TextElement.TextType.Heading2, textElement.Type);
                }
            }
        }

        [TestMethod]
        public async Task TestHeader3()
        {
            string gemString = "### inline media";

            var parseResults = await gemString.ParseGeminiElements();

            if (parseResults.Count > 0)
            {
                var element = parseResults.First();
                if (element is LinkElement)
                {
                    var textElement = element as TextElement;
                    Assert.AreEqual("inline media", textElement.Text);
                    Assert.AreEqual(TextElement.TextType.Heading3, textElement.Type);
                }
            }
        }

        [TestMethod]
        public async Task TestPreformatted()
        {
            string gemString = @"```if you are familiar with markdown it's kinda like that but even simpler
if you are
   familiar with markdown
it's        kinda like that


but even simpler
```";

            var parseResults = await gemString.ParseGeminiElements();
            if (parseResults.Count > 0)
            {
                var element = parseResults.First();
                if(element is PreformatedElement)
                {
                    var textElement = element as PreformatedElement;
                        Assert.AreEqual("", textElement.RawText);
                }
            }
        }
    }
}
