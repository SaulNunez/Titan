using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Titan.Models;
using Windows.UI.Popups;

namespace Titan.Ed
{
    public static class GeminiPetition
    {
        public static readonly int DEFAULT_GEMINI_PORT = 1965;
        public static string Fetch(string url)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes($"{url}\r\n");

            try
            {
                var uri = new Uri(url);
                using (var client = new TcpClient(uri.Authority, DEFAULT_GEMINI_PORT))
                {
                    using (var stream = new SslStream(client.GetStream(), false,
                    new RemoteCertificateValidationCallback(ValidateServerCertificate), null))
                    {
                        stream.AuthenticateAsClient(url);
                        stream.Write(sendBuffer, 0, sendBuffer.Length);

                        var responseObj = new GeminiResponse();

                        if (stream.CanRead)
                        {
                            byte[] recvBuffer = new byte[256];
                            int bytesRead = 0;
                            var responseContent = new StringBuilder();

                            do
                            {
                                bytesRead = stream.Read(recvBuffer, 0, recvBuffer.Length);
                                var msg = Encoding.UTF8.GetString(recvBuffer);
                                responseContent.Append(msg);
                                if (msg.IndexOf("<EOF>") != -1)
                                {
                                    break;
                                }
                            } while (bytesRead != 0);

                            return responseContent.ToString();
                        }

                        return string.Empty;
                    }
                }
            } catch (Exception ex)
            {
                var messageDialog = new MessageDialog(ex.Message);
                messageDialog.ShowAsync();
                return string.Empty;
            }
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
