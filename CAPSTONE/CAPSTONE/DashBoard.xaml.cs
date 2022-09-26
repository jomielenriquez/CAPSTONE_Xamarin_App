using CAPSTONE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CAPSTONE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashBoard : ContentPage
    {
        string UID = string.Empty;
        LoginViewModel viewModel = new LoginViewModel();
        public DashBoard(string uid)
        {
            InitializeComponent();
            LoadingModalVisibility(true);
            UID = uid;
            //lable_title.Text += UID;

            BindingContext = viewModel;

            viewModel.IsLoading = false;
            LoadingModalVisibility(false);
        }

        public void OnButtonCancel(object sender, EventArgs args)
        {
            LoadingModalVisibility(false);
        }

        public void LoadingModalVisibility(bool status)
        {
            boxView.IsVisible = status;
            activityIndicator.IsRunning = status;
            activityIndicator.IsVisible = status;
            stackLayout.IsVisible = status;

        }
        private async void OnButton_AddApprehension(object sender, EventArgs args)
        {
            LoadingModalVisibility(true);
            await Navigation.PushAsync(new AddApprehension(UID));
            LoadingModalVisibility(false);
        }
        private async void OnButton_FinesOfApprehension(object sender, EventArgs args)
        {
            LoadingModalVisibility(true);
            await Navigation.PushAsync(new FinesOfViolation(UID));
            LoadingModalVisibility(false);
        }
        private async void OnButton_About(object sender, EventArgs args)
        {
            LoadingModalVisibility(true);
            await Navigation.PushAsync(new About(UID));
            LoadingModalVisibility(false);
        }
        private async void OnButton_ApprehensionReports(object sender, EventArgs args)
        {
            LoadingModalVisibility(true);
            await Navigation.PushAsync(new Apprehension_Reports(UID));
            LoadingModalVisibility(false);
        }
    }
}