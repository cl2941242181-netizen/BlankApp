using BlankApp.Models;
using BlankApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlankApp.Services.TranslateServices
{
    internal class BaiduTranslateService : ITranslateService
    {
        public Task<TranslateResult> TranslateAsync(string text, string formLanguage, string toLanguage)
        {
            string salt = new Random().Next(100000, 999999).ToString();

            return Task.FromResult(new TranslateResult());
        }
    }
}
