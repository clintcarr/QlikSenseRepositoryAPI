using System.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace QlikSenseRepository
{
    class ConnectQlik
    {
        private X509Certificate2 Certificate_ { get; set; }

        public ConnectQlik()
        {
            // First locate the Qlik Sense client certificate
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            Certificate_ = store.Certificates.Cast<X509Certificate2>().FirstOrDefault(c => c.FriendlyName == "QlikClient");
            store.Close();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public string GetRequest(string server, string endpoint)
        {
            string url = "https://" + server + ":4242" + endpoint;
            string Xrfkey = "0123456789abcdef";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?Xrfkey=" + Xrfkey);
            request.ClientCertificates.Add(Certificate_);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers.Add("X-Qlik-Xrfkey", Xrfkey);
            request.Headers.Add("X-Qlik-User", "UserDirectory=INTERNAL; UserID=sa_repository");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            return stream != null ? new StreamReader(stream).ReadToEnd() : string.Empty;
        }

    }
}