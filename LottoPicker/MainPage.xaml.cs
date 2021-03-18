using System;
using System.Diagnostics;
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

        //private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (e == null)
        //        Ticket.Text = "xxx";
        //    else
        //        Ticket.Text = e.ToString();
        //}
    }
}
