using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using Org.BouncyCastle.Asn1.Crmf;
using System.Data;
using System.Drawing;
using System.Net;
using System.IO;
using Google.Protobuf;
using System.Net.Http;

namespace ScholarCounter
{
    class HTTPService
    {
        WebClient wc = new WebClient();

        public HTTPService()
        {
            
        }

        public async Task<string> Get(string url, string options) 
        {
            String getResponse = "";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(options));
                HttpResponseMessage res = await client.GetAsync(client.BaseAddress);
                HttpContent content = res.Content;
                getResponse = await content.ReadAsStringAsync();
                //MessageBox.Show("Status Code: \n\n" + res.StatusCode.ToString());
                //MessageBox.Show("Headers: \n\n" + res.Content.Headers);
                if (res.StatusCode.ToString() == "OK")
                {
                    return getResponse;
                }
                else
                {
                    return "Error";
                }
            }
            catch(Exception ex)
            {
                return "Error";
            }
        }    
    }
}
