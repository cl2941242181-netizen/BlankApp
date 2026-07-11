using System.Windows;
using BlankApp.Views;
using Prism.Ioc;
using Prism;
using Prism.Unity;
using BlankApp.Services.Interfaces;
using BlankApp.Services.TranslateServices;
using BlankApp.ViewModels;
using BlankApp.Models;
using BlankApp.Services;

namespace BlankApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App :PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ITranslateService, BaiduTranslateService>(APITypes.Baidu.ToString());

            containerRegistry.RegisterSingleton<ITranslateService, TranslateProxyService>();

            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();

        }
    }
}
