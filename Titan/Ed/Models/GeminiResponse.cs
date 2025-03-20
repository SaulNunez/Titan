using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Titan.Ed.Markup;
using Titan.Ed.Markup.Body;

namespace Titan.Models
{
    public enum ResponseStatus
    {

    }

    public class GeminiResponse
    {
        private enum ProcessingStatus
        {
            NORMAL,
            PREPROCESSED
        }
        public GeminiResponse(string bodyContent)
        {
            var content = bodyContent.Split("\r\n", 2);

            //Get first line of the response
            var statusRegion = content[0];
            var statusInformation = statusRegion.Split(' ', 2);

            if (statusInformation.Length >= 2)
            {
                if(statusInformation[0].Length == 2)
                {
                    Status = statusInformation[0];
                }

                if (Status.StartsWith("4") || Status.StartsWith("5"))
                {
                    ErrorMessage = statusInformation[1];
                }

                if(Encoding.UTF8.GetByteCount(statusInformation[1]) <= 1024)
                {
                    Meta = statusInformation[1];
                }

                if (IsSuccess)
                {
                    Body = content[1];
                }
            }
        }

        public string Status { get; private set; }
        /// <summary>
        /// Is a UTF-8 encoded string of maximum length 1024 bytes, whose meaning is <STATUS> dependent
        /// </summary>
        public string Meta { get; private set; }

        public string MimeType { get
            {
                if (Meta.Length == 0) return "text / gemini; charset = utf - 8";
                else return Meta;
            }
        }

        public string Body { get; private set; } = string.Empty;

        /// <summary>
        /// Returns true if status code starts with 3
        /// </summary>
        public bool IsRedirect
        {
            get => Status[0] == '3';
        }

        /// <summary>
        /// Returns true if status code starts with 2
        /// </summary>
        public bool IsSuccess
        {
            get => Status[0] == '2';
        }
        public string ErrorMessage { get; internal set; }

        public List<GeminiElement> BodyElements()
        {
            var elements = new List<GeminiElement>();

            var processingStatus = ProcessingStatus.NORMAL;
            var preprocessedBuffer = new StringBuilder();

            foreach (var element in Body.Split("\n"))
            {
                Console.WriteLine(element);
                switch (processingStatus)
                {
                    case ProcessingStatus.NORMAL:
                        if (element.StartsWith("'''"))
                        {
                            preprocessedBuffer.Clear();
                            processingStatus = ProcessingStatus.PREPROCESSED;
                        }
                        else if (element.StartsWith("=>"))
                        {
                            elements.Add(new LinkElement(element));
                        }
                        else
                        {
                            elements.Add(new TextElement(element));
                        }
                        break;
                    case ProcessingStatus.PREPROCESSED:
                        preprocessedBuffer.AppendLine(element);

                        if (element.StartsWith("'''"))
                        {
                            elements.Add(new PreformatedElement(preprocessedBuffer.ToString()));
                            processingStatus = ProcessingStatus.NORMAL;
                        }
                        break;
                }
            }

            return elements;
        }

        public override string ToString()
        {
            return $"Status: {Status}, Meta: {Meta}, Body = {Body}";
        }
    }
}
