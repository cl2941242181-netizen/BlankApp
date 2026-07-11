using BlankApp.Models;
using BlankApp.Services.Interfaces;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlankApp.Services
{
    public class TranslateProxyService:ITranslateService
    {
        private readonly IContainerProvider _containerProvider;

        public APITypes CurrentType { get; set; } = APITypes.Baidu;

        public TranslateProxyService(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
        }

        public async Task<string> TranslateAsync(string text, string formLanguage, string toLanguage)
        {
            var service = _containerProvider.Resolve<ITranslateService>(CurrentType.ToString());
            return await service.TranslateAsync(text, formLanguage, toLanguage);
        }

        public string AppendTranslateResStr(string res)
        {
            var service = _containerProvider.Resolve<ITranslateService>(CurrentType.ToString());
            return service.AppendTranslateResStr(res);
        }
    }
}
