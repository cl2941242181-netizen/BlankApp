using BlankApp.Models;
using BlankApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlankApp.Services.TranslateServices
{
    internal class BaiduTranslateService : ITranslateService
    {
        // ====== 填入你在百度开放平台申请的凭证 ======
        private const string AppId = "你的APP_ID";
        private const string SecretKey = "你的密钥";
        private const string ApiUrl = "https://fanyi-api.baidu.com/api/trans/vip/translate";


        public static async Task<string> TranslateAsync(string text, string formLanguage, string toLanguage)
        {
            string salt = new Random().Next(100000, 999999).ToString();

            string signStr = AppId + text + salt + SecretKey;
            string sign = GetMd5Hash(signStr);

            var postData = new Dictionary<string, string>
        {
            { "q", text },
            { "from", formLanguage },
            { "to", toLanguage },
            { "appid", AppId },
            { "salt", salt },
            { "sign", sign }
        };

            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(postData);

                // 4. 发送请求并获取结果
                HttpResponseMessage response = await client.PostAsync(ApiUrl, content);
                response.EnsureSuccessStatusCode();

               var res = await response.Content.ReadAsStringAsync();


            }
        }

        /// <summary>
        /// 计算 32 位小写 MD5
        /// </summary>
        private static string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // "x2" 表示 16 进制小写
                }
                return sb.ToString();
            }
        }



    }
}
