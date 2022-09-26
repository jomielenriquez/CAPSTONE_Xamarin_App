using CAPSTONE.Model;
using CAPSTONE.Services;
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
    public partial class AddApprehension : ContentPage
    {
        byte[] INIT = { 27, 64 };
        byte[] FEED_LINE = { 10 };

        byte[] SELECT_FONT_A = { 27, 33, 0 };

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
        public AddApprehension(string uid)
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
        public void classification_Changed(object sender, EventArgs args)
        {
            RadioButton radioButton = sender as RadioButton;
            DisplayAlert("Test", radioButton.Content.ToString(), "OK");
        }
        public void Printest(object sender, EventArgs args)
        {
            IPrintService service = DependencyService.Get<IPrintService>();
            string To_Print = "";
            To_Print = To_Print + "\n" + ALLINEA_CT + "Center";
            To_Print = To_Print + "\n" + ALLINEA_SX + "Left";
            To_Print = To_Print + ALLINEA_DX + "right";
            To_Print = To_Print + "\n\x1b\x21\x01 Select Font B";
            To_Print = To_Print + "\n\x1b\x21\x11 Select double height mode";
            To_Print = To_Print + "\n\x1b\x21\x00 Select Font A";
            To_Print = To_Print + "\n\x1b\x2d\x01 Enable Underline";
            To_Print = To_Print + "\n\x1b\x2d\x00 Disable Underline";
            To_Print = To_Print + "\n\x1b\x34\x01 Enable Italics";
            To_Print = To_Print + "\n\x1b\x34\x00 Disable Italics";
            To_Print = To_Print + "\n\x1b\x45\x01 Enable Emphasis";
            To_Print = To_Print + "\n\x1b\x45\x00 Disable Emphasis";
            To_Print = To_Print + "\n\x1B\x21\x10 this is x10";
            To_Print = To_Print + "\n\x1B\x21\x20 this is x20";
            To_Print = To_Print + "\n\x1B\x21\x30 this is x30";
            To_Print = To_Print + "\n\x1B\x21\x40 this is x40";
            To_Print = To_Print + "\n" + FONT_1X + " This is 1x font";
            To_Print = To_Print + "\n" + FONT_2X + " This is 2x font";
            To_Print = To_Print + "\n" + FONT_3X + " This is 3x font";
            To_Print = To_Print + "\n\n\x1B\x21\x30Test\n\n\n\n\n\n";

            service.Print("IposPrinter", To_Print);
        }
    }
}