using System.Text;

namespace Titan.Models
{
    public class GeminiResponse
    {
        public GeminiResponse(string bodyContent)
        {
            var content = bodyContent.Split("\r\n", 2);

            //Get first line of the response
            var statusRegion = content[0];
            var statusInformation = statusRegion.Split(' ');

            if (statusInformation.Length >= 2)
            {
                if(statusInformation[0].Length == 2)
                {
                    Status = statusInformation[0];
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

        public string Status { get; set; }
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

        public string Body { get; set; } = string.Empty;

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

        public override string ToString()
        {
            return $"Status: {Status}, Meta: {Meta}, Body = {Body}";
        }
    }
}
