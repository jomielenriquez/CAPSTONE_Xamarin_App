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
            lable_title.Text = "Tanauan City is home to scenic nature's beauty, attractive investment opportunities, rich culture and historical heritage, superb adventure and vigoruos industrialization. The Natural wealth of its land and the dynamism and resourcefulness of its people pave the way to competitiveness, bringing the city to the crest of progress and advancement, In the midst of developement, we envision to share the exquisite and enticing places being painstakingly preserved and nurtured i the city. We seek to share its rich history, culture heritage, natural attractions and th values, traits and traditions of a warm and friendly citizenry. Inspired by the industrious of Tanauenos, we preserve in moving the city towards attainment of its vision in sustainable agricultural, industrial, commercial and tourism development, people empowerment and well-being and good governance. We invite you to be part of a new experience, as we pursue tovether the continuous groeth and growth and progress of the city.";
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