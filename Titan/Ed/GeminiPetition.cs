using System;
using System.Collections.Generic;
using System.IO;
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
    public class GeminiPetition
    {
        public static readonly int DEFAULT_GEMINI_PORT = 1965;

        public Uri URL { get; private set; }
        public GeminiPetition(Uri uri) 
        { 
            URL = uri;
        }

        public string Body() => $"{URL.AbsoluteUri}\r\n";

        public async Task<GeminiResponse> Fetch()
        {
            try
            {
                string host = URL.Host;
                int port = URL.Port > 0 ? URL.Port : 1965; // Default Gemini port

                // Connect to the Gemini server
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(host, port);

                    // Secure the connection with TLS
                    using (SslStream sslStream = new SslStream(client.GetStream(), false,
                        new RemoteCertificateValidationCallback(ValidateServerCertificate)))
                    { 
                        await sslStream.AuthenticateAsClientAsync(host);

                        byte[] requestBytes = Encoding.UTF8.GetBytes(Body());
                        await sslStream.WriteAsync(requestBytes, 0, requestBytes.Length);
                        await sslStream.FlushAsync();

                        // Read the response
                        using (StreamReader reader = new StreamReader(sslStream, Encoding.UTF8))
                        {
                            var result = await reader.ReadToEndAsync();
                            return new GeminiResponse(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValidateServerCertificate(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
        {
            //// If there are no SSL policy errors, the certificate is valid.
            //if (sslPolicyErrors == SslPolicyErrors.None)
            //{
            //    return true;
            //}

            //Console.WriteLine($"Certificate error: {sslPolicyErrors}");

            //// Optional: Manually check the chain's validity
            //foreach (X509ChainStatus chainStatus in chain.ChainStatus)
            //{
            //    Console.WriteLine($"Chain status: {chainStatus.Status} - {chainStatus.StatusInformation}");
            //    if (chainStatus.Status != X509ChainStatusFlags.NoError)
            //    {
            //        return false;
            //    }
            //}

            //// In production, do NOT accept an invalid certificate.
            //return false;
            return true;
        }
    }
}
