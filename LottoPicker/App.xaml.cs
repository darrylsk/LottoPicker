using LottoPicker.Common;
using LottoPicker.Services;
using Xamarin.Forms;

namespace LottoPicker
{
    /// <summary>
    /// References:
    /// MVVM Pattern: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm#ui-interaction-using-commands-and-behaviors
    /// DI: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/dependency-injection
    /// ViewModel creation: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm#automatically-creating-a-view-model-with-a-view-model-locator
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var sorter = ContainerBuilder.Resolve<IArraySort<int>>();
            var picker = ContainerBuilder.Resolve<INumberPicker>();

            MainPage = new NavigationPage(new MainPage(picker));
            //MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
