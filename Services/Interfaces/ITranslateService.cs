using BlankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace BlankApp.Services.Interfaces
{
    internal interface ITranslateService
    {
        Task<TranslateResult> TranslateAsync(string text,string formLanguage,string toLanguage);
    }
}
