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
        public DashBoard(string uid)
        {
            InitializeComponent();
            UID = uid;
            lable_title.Text += UID;
        }
    }
}