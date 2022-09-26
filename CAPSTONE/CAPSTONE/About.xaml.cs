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
    public partial class About : ContentPage
    {
        string UID = string.Empty;
        LoginViewModel viewModel = new LoginViewModel();
        public About(string uid)
        {
            InitializeComponent();
            LoadingModalVisibility(true);
            UID = uid;

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
    }
}