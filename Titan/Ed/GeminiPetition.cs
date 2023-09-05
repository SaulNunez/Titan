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
        public async static Task<GeminiResponse> Fetch(string url)
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

                            return new GeminiResponse(responseContent.ToString());
                        }

                        throw new Exception("Connection had a problem");
                    }
                }
            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog(ex.Message);
                await messageDialog.ShowAsync();
                throw ex;
            }
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
