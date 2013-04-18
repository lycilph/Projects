using System;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace WpfLocalizationTest
{
    public class AuthenticationProvider
    {
        public static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private const int token_renewal_interval = 9; // in minutes (token expires after 10 min, so get a new one before that)
        private readonly string client_id;
        private readonly string client_secret;
        private readonly string request;
        private AccessToken token;
        private DateTime token_timestamp;

        public AuthenticationProvider(string id, string secret)
        {
            client_id = id;
            client_secret = secret;

            request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",HttpUtility.UrlEncode(client_id), HttpUtility.UrlEncode(client_secret));
            token = HttpPost();
            token_timestamp = DateTime.Now;
        }

        public AccessToken GetAccessToken()
        {
            if (DateTime.Now > token_timestamp.AddMinutes(token_renewal_interval))
            {
                token = HttpPost();
                token_timestamp = DateTime.Now;
            }

            return token;
        }

        private AccessToken HttpPost()
        {
            //Prepare OAuth request 
            var web_request = WebRequest.Create(DatamarketAccessUri);
            web_request.ContentType = "application/x-www-form-urlencoded";
            web_request.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(request);
            web_request.ContentLength = bytes.Length;

            using (var output_stream = web_request.GetRequestStream())
            {
                output_stream.Write(bytes, 0, bytes.Length);
            }
            using (var web_response = web_request.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AccessToken));
                //Get deserialized object from JSON stream
// ReSharper disable AssignNullToNotNullAttribute
                return (AccessToken)serializer.ReadObject(web_response.GetResponseStream());
// ReSharper restore AssignNullToNotNullAttribute
            }
        }
    }
}
