﻿using CAPSTONE.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CAPSTONE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Apprehension_Reports : ContentPage
    {
        string UID = string.Empty;
        LoginViewModel viewModel = new LoginViewModel();
        public Apprehension_Reports(string uid)
        {
            InitializeComponent();
            LoadingModalVisibility(true);
            UID = uid;

            BindingContext = viewModel;

            viewModel.IsLoading = false;

            _ = GetReport();

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
        public async Task<string> GetReport()
        {
            using (var client = new HttpClient())
            {
                SystemRepository sysRepo = new SystemRepository();
                var uri = sysRepo.GetReport();
                var result = await client.GetStringAsync(uri);

                //lblUsername.Text = result.ToString();
                if (result.ToString() == "Error")
                {
                    await DisplayAlert("Error", "Invalid username or password", "OK");
                    return "Error";
                }

                string strReport = result.ToString().Split(new[] { "<!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE-->" }, StringSplitOptions.None)[0];
                //lable_title.Text = result.ToString().Split(new[] { "<!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE-->" }, StringSplitOptions.None)[0];

                List<Report> List_report = JsonConvert.DeserializeObject<List<Report>>(strReport);

                int width = 100;

                foreach (Report r in List_report)
                {
                    ViewCell viewCell = new ViewCell()
                    {
                        View = new StackLayout()
                        {
                            Children = { 
                                new StackLayout()
                                {
                                    Children =
                                    {
                                        new Label { Text = "Full Name", WidthRequest = width }, 
                                        new Label { Text = ":"}, 
                                        new Label { Text = r.fname}, 
                                    },
                                    Orientation = StackOrientation.Horizontal,
                                },
                                new StackLayout()
                                {
                                    Children =
                                    {
                                        new Label { Text = "Address", WidthRequest = width },
                                        new Label { Text = ":"},
                                        new Label { Text = r.address},
                                    },
                                    Orientation = StackOrientation.Horizontal,
                                },
                                new StackLayout()
                                {
                                    Children =
                                    {
                                        new Label { Text = "License No.", WidthRequest = width },
                                        new Label { Text = ":"},
                                        new Label { Text = r.licenseno},
                                    },
                                    Orientation = StackOrientation.Horizontal,
                                },
                                new StackLayout()
                                {
                                    Children =
                                    {
                                        new Label { Text = "Vehicle Type", WidthRequest = width },
                                        new Label { Text = ":"},
                                        new Label { Text = r.vehicletype},
                                    },
                                    Orientation = StackOrientation.Horizontal,
                                },
                                new StackLayout()
                                {
                                    Children =
                                    {
                                        new Label { Text = "Place of Violation", WidthRequest = width },
                                        new Label { Text = ":"},
                                        new Label { Text = r.placeofviolation},
                                    },
                                    Orientation = StackOrientation.Horizontal,
                                },
                                new StackLayout()
                                {
                                    Children =
                                    {
                                        new Label { Text = "Violation", WidthRequest = width },
                                        new Label { Text = ":"},
                                        new Label { Text = r.vdesc},
                                    },
                                    Orientation = StackOrientation.Horizontal,
                                }
                            },
                            //Orientation = StackOrientation.Horizontal,
                            Padding = new Thickness(13, 0)
                            //HeightRequest=500
                        }

                    };
                    tblReport.Add(viewCell);
                }

                LoadingModalVisibility(false);
                return result.ToString();

            }
        }
    }
    public class Report
    {
        public int tid { get; set; }
        public string fname { get; set; }
        public string address { get; set; }
        public string licenseno { get; set; }
        public string vehicletype { get; set; }
        public string placeofviolation { get; set; }
        public string vdesc { get; set; }
    }
}