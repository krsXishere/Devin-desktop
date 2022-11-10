using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Security.Policy;

namespace Devin.Models
{
    public static class SpesificationModel
    {
        private static readonly string baseURL = "http://192.100.103.14:8000/api/";
        public static bool isNull;
        
        public static async Task<string> PostDataSpesification(String pcName, String os, String processor, String ram, String kartu_grafik, String penyimpanan, String lab, String nomorMeja)
        {
            var data = new Dictionary<String, String>
            {
                {"pc_name", pcName },
                {"os", os},
                {"processor", processor},
                {"ram", ram},
                {"kartu_grafik", kartu_grafik},
                {"penyimpanan", penyimpanan},
                {"lab", lab},
                {"nomor_meja", nomorMeja},
            };

            var inputData = new FormUrlEncodedContent(data);

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage responseMessage = await client.PostAsync(baseURL + "store", inputData))
                {
                    using (HttpContent content = responseMessage.Content)
                    {
                        string returnData = await content.ReadAsStringAsync();

                        if(returnData != null)
                        {
                            isNull = false;
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}
