using BlankApp.Models;
using BlankApp.Services;
using BlankApp.Services.Interfaces;
using BlankApp.Services.TranslateServices;
using BlankApp.ViewModels;
using BlankApp.Views;
using Prism;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using System.Windows;

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

            containerRegistry.RegisterForNavigation<TranslateHistoryView, TranslateHistoryViewModel>();

            containerRegistry.RegisterDialog<APISettingView,APISettingViewModel>("APISettingDialog");
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            //区域管理器
            var regionManager = Container.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion("HistoryRegion", typeof(TranslateHistoryView));
        }
    }
}
