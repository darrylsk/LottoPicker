using LottoPicker.Common;
using LottoPicker.ViewModels;
using Xamarin.Forms;

namespace LottoPicker
{
    public partial class MainPage : ContentPage
    {
        public MainPage(INumberPicker numberPicker)
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(numberPicker);
        }
    }
}
