﻿using CAPSTONE.Model;
using CAPSTONE.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CAPSTONE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddApprehension : ContentPage
    {
        // Printer commads
        byte[] INIT = { 27, 64 };
        byte[] FEED_LINE = { 10 };

        byte[] SELECT_FONT_A = { 27, 33, 0 };
        string enableEmp = "\x1b\x45\x01";
        string disableEmp = "\x1b\x45\x00";

        byte[] FONT_B = { 0x1B, 0x4D, 0x01 };
        string ALLINEA_SX = "\x1B\x61\x00";
        string ALLINEA_CT = "\x1B\x61\x01";
        string ALLINEA_DX = "\x1B\x61\x02";
        byte[] GRASSETTO_ON = { 0x1B, 0x47, 0x11 };
        byte[] GRASSETTO_OFF = { 0x1B, 0x47, 0x00 };
        byte[] SET_6 = { 0x1B, 0x52, 0x06 };
        byte[] CODICI = { 0x1B, 0x74, 0x13 };
        byte[] EURO = { (byte)0xD5 };
        byte[] PALLINO = { (byte)0x5B };
        string FONT_3X = "\x1D\x21\x21";
        string FONT_2X = "\x1D\x21\x11";
        string FONT_1X = "\x1D\x21\x00";

        byte[] SET_BAR_CODE_HEIGHT = { 29, 104, 100 };
        byte[] PRINT_BAR_CODE_1 = { 29, 107, 2 };
        byte[] SEND_NULL_BYTE = { 0x00 };

        byte[] SELECT_PRINT_SHEET = { 0x1B, 0x63, 0x30, 0x02 };
        byte[] FEED_PAPER_AND_CUT = { 0x1D, 0x56, 66, 0x00 };

        byte[] SELECT_CYRILLIC_CHARACTER_CODE_TABLE = { 0x1B, 0x74, 0x11 };

        byte[] SELECT_BIT_IMAGE_MODE = { 0x1B, 0x2A, 33, 127, 3 };
        byte[] SET_LINE_SPACING_24 = { 0x1B, 0x33, 24 };
        byte[] SET_LINE_SPACING_30 = { 0x1B, 0x33, 30 };

        byte[] TRANSMIT_DLE_PRINTER_STATUS = { 0x10, 0x04, 0x01 };
        byte[] TRANSMIT_DLE_OFFLINE_PRINTER_STATUS = { 0x10, 0x04, 0x02 };
        byte[] TRANSMIT_DLE_ERROR_STATUS = { 0x10, 0x04, 0x03 };
        byte[] TRANSMIT_DLE_ROLL_PAPER_SENSOR_STATUS = { 0x10, 0x04, 0x04 };

        string UID = string.Empty;
        LoginViewModel viewModel = new LoginViewModel();
        List<Fines> fines = new List<Fines>();
        List<Compound> compound = new List<Compound>();
        string radclassification = "";
        string CurrentUserFullName = "";
        public AddApprehension(string uid)
        {
            InitializeComponent();
            LoadingModalVisibility(true);
            UID = uid;

            BindingContext = viewModel;

            viewModel.IsLoading = false;
            _ = GetFines();
            _ = GetPlaceOfViolation();
            _ = GetUserFullName(uid);

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
        public void classification_Changed(object sender, EventArgs args)
        {
            RadioButton radioButton = sender as RadioButton;

            radclassification = radioButton.Content.ToString();

            //DisplayAlert("Test", radioButton.Content.ToString(), "OK");
        }
        public async Task<string> GetFines()
        {
            using (var client = new HttpClient())
            {
                SystemRepository sysRepo = new SystemRepository();
                var uri = sysRepo.GetFinesLink();
                var result = await client.GetStringAsync(uri);
                
                if (result.ToString() == "Error")
                {
                    await DisplayAlert("Error", "Invalid username or password", "OK");
                    return "Error";
                }

                string Fines = result.ToString().Split(new[] { "<!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE-->" }, StringSplitOptions.None)[0];


                fines = JsonConvert.DeserializeObject<List<Fines>>(Fines);


                foreach (Fines f in fines)
                {
                    Picker_Fines.Items.Add(f.vdesc);
                }
                
                LoadingModalVisibility(false);

                return Fines;
            }
        }
        public async Task<string> GetPlaceOfViolation()
        {
            using (var client = new HttpClient())
            {
                SystemRepository sysRepo = new SystemRepository();
                var uri = sysRepo.prop_get_compound();
                var result = await client.GetStringAsync(uri);

                if (result.ToString() == "Error")
                {
                    await DisplayAlert("Error", "Invalid username or password", "OK");
                    return "Error";
                }

                string Coumpounds = result.ToString().Split(new[] { "<!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE-->" }, StringSplitOptions.None)[0];


                compound = JsonConvert.DeserializeObject<List<Compound>>(Coumpounds);


                foreach (Compound f in compound)
                {
                    Picker_Place.Items.Add(f.compound);
                }

                LoadingModalVisibility(false);

                return Coumpounds;
            }
        }
        public async Task<string> GetUserFullName(string uid)
        {
            using (var client = new HttpClient())
            {
                SystemRepository sysRepo = new SystemRepository();
                var uri = sysRepo.getCurrentUser(uid);
                var result = await client.GetStringAsync(uri);

                if (result.ToString() == "Error")
                {
                    await DisplayAlert("Error", "Invalid username or password", "OK");
                    return "Error";
                }

                string fname = result.ToString().Split(new[] { "<!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE-->" }, StringSplitOptions.None)[0];

                CurrentUserFullName = fname;

                return fname;
            }
        }

        public void btn_Submit(object sender, EventArgs args)
        {
            bool isFilled = fieldvalidataion();
            if (!isFilled)
            {
                DisplayAlert("Error", "Please verify that all required fields have been filled.", "OK");
                return;
            }

            string fname = (entry_fname.Text + " " + (entry_mi.Text == "" ? "" : entry_mi.Text[0] + "") + ". " + entry_lname.Text).ToUpper();
            string address = (entry_hn.Text + " " + entry_subd.Text + " " + entry_city.Text).ToUpper();
            string licneseno = entry_license.Text.ToUpper();
            string birthdate = datePicker_birthdate.Date.ToString("MMM d, yyyy");
            string dateofapprehension = datePicker_apprehensiodate.Date.ToString("MMM d, yyyy");
            //WEB#57 remove place of violation
            string placeofviolation = "";// Picker_Place.SelectedItem.ToString();//entry_placeofviolation.Text;
            string violationid = GetViolationId(Picker_Fines.SelectedItem.ToString()); // need to get
            string vehicletype = entry_vehicletype.Text; // radio button
            string classification = radclassification;
            string plateno = entry_plateno.Text;
            string vamount = GetViolationAmount(Picker_Fines.SelectedItem.ToString());

            LoadingModalVisibility(true);

            _ = insertCitation(
                fname,
                address,
                licneseno,
                birthdate,
                dateofapprehension,
                placeofviolation,
                violationid,
                vehicletype,
                classification,
                plateno,
                Picker_Fines.SelectedItem.ToString(),
                vamount
            );

            //DisplayAlert("Message", "Success", "OK");
            //PrintCitation(
            //    fname,
            //    address,
            //    licneseno,
            //    birthdate,
            //    dateofapprehension,
            //    placeofviolation,
            //    Picker_Fines.SelectedItem.ToString(),
            //    vehicletype,
            //    classification,
            //    plateno
            //);

        }
        public async Task<string> insertCitation(
            string fname,
            string address,
            string licneseno,
            string birthdate,
            string dateofapprehension,
            string placeofviolation,
            string violationid,
            string vehicletype,
            string classification,
            string plateno,
            string strviolation,
            string vamount
        )
        {
            using (var client = new HttpClient())
            {
                SystemRepository sysRepo = new SystemRepository();
                var uri = sysRepo.getinsertcitationlink(
                    fname,
                    address,
                    licneseno,
                    birthdate,
                    dateofapprehension,
                    placeofviolation,
                    violationid,
                    vehicletype,
                    classification,
                    plateno
                );
                var result = await client.GetStringAsync(uri);

                if (result.ToString() == "Error")
                {
                    await DisplayAlert("Error", "Invalid username or password", "OK");
                    return "Error";
                }

                string returnval = result.ToString().Split(new[] { "<!--SCRIPT GENERATED BY SERVER! PLEASE REMOVE-->" }, StringSplitOptions.None)[0];

                if(returnval == "SUCCESS")
                {
                    await DisplayAlert("Success", "The Ticket is now printing", "OK");
                    await Navigation.PushAsync(new Apprehension_Reports(UID));

                    PrintCitation(
                        fname,
                        address,
                        licneseno,
                        birthdate,
                        dateofapprehension,
                        placeofviolation,
                        strviolation,
                        vehicletype,
                        classification,
                        plateno,
                        vamount
                    );

                    entry_fname.Text = "";
                    entry_mi.Text = ""; 
                    entry_lname.Text="";
                    entry_hn.Text = "";
                    entry_subd.Text = "";
                    entry_city.Text="";
                    entry_license.Text="";
                    //entry_placeofviolation.Text="";
                    entry_vehicletype.Text = ""; 
                    entry_plateno.Text = "";

                    LoadingModalVisibility(false);

                }
                else if(returnval == "ERROR")
                {
                    await DisplayAlert("Error", "Error saving..", "OK");
                    LoadingModalVisibility(false);
                }


                return returnval;
            }
        }
        public string PrintCitation(
            string fname,
            string address,
            string licneseno,
            string birthdate,
            string dateofapprehension,
            string placeofviolation,
            string violation,
            string vehicletype,
            string classification,
            string plateno,
            string vamount
            )
        {
            IPrintService service = DependencyService.Get<IPrintService>();
            string To_Print = "";

            To_Print = To_Print + "\n" + ALLINEA_CT + enableEmp + "LUNGSOD NG TANAUAN";
            To_Print = To_Print + "\nLalawigan ng Batangas" + disableEmp;
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n" + ALLINEA_SX + "Full Name:" + ALLINEA_DX + enableEmp + fname + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Address:" + ALLINEA_DX + enableEmp + address + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Birth Date:" + ALLINEA_DX + enableEmp + birthdate + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_CT + "--------------------------------";
            To_Print = To_Print + "\n" + ALLINEA_SX + "License Number:" + ALLINEA_DX + enableEmp + licneseno + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Plate Number:" + ALLINEA_DX + enableEmp + plateno + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Vehicle Type:" + ALLINEA_DX + enableEmp + vehicletype + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Classification:" + ALLINEA_DX + enableEmp + classification + disableEmp;
            //WEB#57 remove all place of violation
            //To_Print = To_Print + "\n" + ALLINEA_SX + "Place of Violation:" + ALLINEA_DX + enableEmp + placeofviolation + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Date of Apprehension:" + ALLINEA_DX + enableEmp + dateofapprehension + disableEmp;
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n" + ALLINEA_CT + "You are hereby sited for committing the traffic and administrative violation here under";
            //To_Print = To_Print + "\n" + ALLINEA_SX + "Violation:" + ALLINEA_DX + enableEmp + violation + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_CT + "Violation:" + violation + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_CT + "P "+ vamount;
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n" + ALLINEA_CT + "You are hereby directed to report within 72 hours from date of apprehension to the City Fiscal's Office, City Treasure's Office, City Court, City Hall, Tanauan City";
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n" + ALLINEA_CT + CurrentUserFullName;
            To_Print = To_Print + "\n" + ALLINEA_CT + "APPREHENDING OFFICER";
            To_Print = To_Print + ALLINEA_CT;
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n" + ALLINEA_CT + enableEmp + "CAPSTONE";
            To_Print = To_Print + "\n\n\n\n\n\n\n\n";

            service.Print("IposPrinter", To_Print);

            return "Success";
        }

        public bool fieldvalidataion()
        {
            bool isFilled = true;
            if (entry_fname.Text == "" || entry_fname.Text == null)
            {
                isFilled = false;
            }
            if (entry_mi.Text == "" || entry_mi.Text == null)
            {
                isFilled = false;
            }
            if (entry_lname.Text == "" || entry_lname.Text == null)
            {
                isFilled = false;
            }
            if (entry_hn.Text == "" || entry_hn.Text == null)
            {
                isFilled = false;
            }
            if (entry_subd.Text == "" || entry_subd.Text == null)
            {
                isFilled = false;
            }
            if (entry_city.Text == "" || entry_city.Text == null)
            {
                isFilled = false;
            }
            if (entry_license.Text == "" || entry_license.Text == null)
            {
                isFilled = false;
            }
            if (entry_vehicletype.Text == "" || entry_vehicletype.Text == null)
            {
                isFilled = false;
            }
            if (radclassification == "" || radclassification == null)
            {
                isFilled = false;
            }
            try
            {
                string pp = Picker_Place.SelectedItem.ToString();
            }
            catch (Exception e)
            {
                //WEB#57
                //isFilled = false;
            }
            try
            {
                string dd = Picker_Fines.SelectedItem.ToString();
            }
            catch (Exception e)
            {
                isFilled = false;
            }

            return isFilled;
        }

        public void entry_Changed(object sender, EventArgs args)
        {
            Entry thisentry = sender as Entry;
            thisentry.TextColor = Color.Gray;

            DisplayAlert("Error", "nagbago", "OK");
        }

        public string GetViolationId(string str_violation)
        {
            foreach (Fines f in fines)
            {
                if (f.vdesc == str_violation) return f.vid.ToString();
            }

            return "0"; // means violation is not in database.
        }

        public string GetViolationAmount(string str_violation)
        {
            foreach (Fines f in fines)
            {
                if (f.vdesc == str_violation) return f.vamount.ToString();
            }

            return "0.00"; // means violation is not in database.
        }

        public void Printest(object sender, EventArgs args)
        {
            IPrintService service = DependencyService.Get<IPrintService>();
            string To_Print = "";

            To_Print = To_Print + "\n" + ALLINEA_CT + enableEmp + "Lungsod ng Tanauan";
            To_Print = To_Print + "\nLalawigan ng Batangas" + disableEmp;
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n" + ALLINEA_SX + "Full Name:" + ALLINEA_DX + enableEmp + "This is just a test" + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Address:" + ALLINEA_DX + enableEmp + "This is just a test" + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Birth Date:" + ALLINEA_DX + enableEmp + "This is just a test" + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_CT + "--------------------------------";
            To_Print = To_Print + "\n" + ALLINEA_SX + "License Number:" + ALLINEA_DX + enableEmp + "This is just a test" + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Vehicle Type:" + ALLINEA_DX + enableEmp + "This is just a test" + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Classification:" + ALLINEA_DX + enableEmp + "This is just a test" + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Place of Violation:" + ALLINEA_DX + enableEmp + "This is just a test" + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Date of Apprehension:" + ALLINEA_DX + enableEmp + "This is just a test" + disableEmp;
            To_Print = To_Print + "\n" + ALLINEA_SX + "Violation:" + ALLINEA_DX + enableEmp + "This is just a test" + disableEmp;
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n" + ALLINEA_CT + CurrentUserFullName;
            To_Print = To_Print + "\n" + ALLINEA_CT + "APPREHENDING OFFICER";
            To_Print = To_Print + ALLINEA_CT;
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n--------------------------------";
            To_Print = To_Print + "\n" + ALLINEA_CT + enableEmp + "CAPSTONE";
            To_Print = To_Print + "\n\n\n\n\n\n\n\n";

            //To_Print = To_Print + "\n" + ALLINEA_CT + "Center";
            //To_Print = To_Print + "\n" + ALLINEA_SX + "Left";
            //To_Print = To_Print + ALLINEA_DX + "right";
            //To_Print = To_Print + "\n\x1b\x21\x01 Select Font B";
            //To_Print = To_Print + "\n\x1b\x21\x11 Select double height mode";
            //To_Print = To_Print + "\n\x1b\x21\x00 Select Font A";
            //To_Print = To_Print + "\n\x1b\x2d\x01 Enable Underline";
            //To_Print = To_Print + "\n\x1b\x2d\x00 Disable Underline";
            //To_Print = To_Print + "\n\x1b\x34\x01 Enable Italics";
            //To_Print = To_Print + "\n\x1b\x34\x00 Disable Italics";
            //To_Print = To_Print + "\n\x1b\x45\x01 Enable Emphasis";
            //To_Print = To_Print + "\n\x1b\x45\x00 Disable Emphasis";
            //To_Print = To_Print + "\n\x1B\x21\x10 this is x10";
            //To_Print = To_Print + "\n\x1B\x21\x20 this is x20";
            //To_Print = To_Print + "\n\x1B\x21\x30 this is x30";
            //To_Print = To_Print + "\n\x1B\x21\x40 this is x40";
            //To_Print = To_Print + "\n" + FONT_1X + " This is 1x font";
            //To_Print = To_Print + "\n" + FONT_2X + " This is 2x font";
            //To_Print = To_Print + "\n" + FONT_3X + " This is 3x font";
            //To_Print = To_Print + "\n\n\x1B\x21\x30Test\n\n\n\n\n\n";

            service.Print("IposPrinter", To_Print);
        }
    }
}