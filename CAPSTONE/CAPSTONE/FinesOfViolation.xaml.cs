﻿using CAPSTONE.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CAPSTONE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FinesOfViolation : ContentPage
    {
        public string UID = string.Empty;
        LoginViewModel viewModel = new LoginViewModel();
        public FinesOfViolation(string uid)
        {
            InitializeComponent();
            LoadingModalVisibility(true);
            UID = uid;

            BindingContext = viewModel;

            viewModel.IsLoading = false;
            
            _ = GetFines();

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
        //public void GenerateFines()
        //{
        //    lable_title.Text = GetFines().ToString();
        //}
        public async Task<string> GetFines()
        {
            using (var client = new HttpClient())
            {
                SystemRepository sysRepo = new SystemRepository();
                var uri = sysRepo.GetViolation();
                var result = await client.GetStringAsync(uri);

                if (result.ToString() == "Error")
                {
                    await DisplayAlert("Error", "Invalid username or password", "OK");
                    return "Error";
                }

                string Fines = result.ToString().Split(new[] { "<!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE-->" }, StringSplitOptions.None)[0];
                
                List<Fines> fines = JsonConvert.DeserializeObject<List<Fines>>(Fines);


                foreach(Fines f in fines)
                {
                    ViewCell test = new ViewCell()
                    {
                        View = new StackLayout()
                        {
                            Children = { 
                                new Label { Text = f.vdesc, WidthRequest=250 }, 
                                new Label { Text = "P " + f.vamount } 
                            },
                            Orientation = StackOrientation.Horizontal,
                            Padding = new Thickness(13, 0)
                        }

                    };
                    tblFines.Add(test);
                }

                LoadingModalVisibility(false);
                return result.ToString();
            }
        }
    }
    public class Fines
    {
        public int vid { get; set; }
        public string vdesc { get; set; }
        public string vamount { get; set; }
    }
}