using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;
using CAPSTONE.Droid.Services;
using CAPSTONE.Services;
using Java.Util;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothServiceRenderer))]
namespace CAPSTONE.Droid.Services
{
    class BluetoothServiceRenderer : IPrintService
    {
        public IList<string> GetDeviceList()
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                var btdevice = bluetoothAdapter?.BondedDevices.Select(i => i.Name).ToList();
                return btdevice;
            }
        }

        public async Task Print(string deviceName, string text)
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                          where bd.Name == deviceName
                                          select bd).FirstOrDefault();

                try
                {

                    using (BluetoothSocket bluetoothSocket = device?.
                        CreateRfcommSocketToServiceRecord(
                        UUID.FromString("00001101-0000-1000-8000-00805f9b34fb")))
                    {
                        bluetoothSocket?.Connect();
                        byte[] buffer = Encoding.UTF8.GetBytes(text);

                        bluetoothSocket?.OutputStream.Write(buffer, 0, buffer.Length);

                        bluetoothSocket.Close();
                    }
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
        public byte[] tobyte(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            return buffer;
        }
    }
}