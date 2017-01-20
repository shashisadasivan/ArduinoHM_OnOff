using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Android.Bluetooth;
using System.Linq;

namespace Xam.And.BL
{
    [Activity(Label = "My bluetooth test thingy", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private ListView _BlListView;
        private BluetoothSocket _socket;
        private Button _LEDOnButton;
        private Button _LEDOffButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our UI controls from the loaded layout:
            this._BlListView = FindViewById<ListView>(Resource.Id.DevicesListView);
            _LEDOnButton = FindViewById<Button>(Resource.Id.LEDButtonOn);
            _LEDOffButton = FindViewById<Button>(Resource.Id.LEDButtonOff);

            #region call region
            /*
            EditText phoneNumberText = FindViewById<EditText>(Resource.Id.phNumberText);
            Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            Button callButton = FindViewById<Button>(Resource.Id.CallButton);

            // Disable the "Call" button
            callButton.Enabled = false;

            // Add code to translate number
            string translatedNumber = string.Empty;

            translateButton.Click += (object sender, EventArgs e) =>
            {
                // Translate user's alphanumeric phone number to numeric
                translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
                if (String.IsNullOrWhiteSpace(translatedNumber))
                {
                    callButton.Text = "Call";
                    callButton.Enabled = false;
                }
                else
                {
                    callButton.Text = "Call " + translatedNumber;
                    callButton.Enabled = true;
                }
            };

            callButton.Click += (object sender, EventArgs e) =>
            {
                // On "Call" button click, try to dial phone number.
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call " + translatedNumber + "?");
                callDialog.SetNeutralButton("Call", delegate
                {
                    // Create intent to dial phone
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
                    StartActivity(callIntent);
                });
                callDialog.SetNegativeButton("Cancel", delegate { });

                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            };
            */
            #endregion

            this.AddBluetooth();
            _LEDOnButton.Click += LEDOnButton_Click;
            _LEDOffButton.Click += LEDOffButton_Click;
        }

        private async void LEDOffButton_Click(object sender, EventArgs e)
        {
            if(_socket != null)
            {
                if (!_socket.IsConnected)
                    await _socket.ConnectAsync();
                string value = "0";
                var bytes = System.Text.Encoding.UTF8.GetBytes(value);
                await _socket.OutputStream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        private async void LEDOnButton_Click(object sender, EventArgs e)
        {
            if (_socket != null)
            {
                if (!_socket.IsConnected)
                    await _socket.ConnectAsync();
                string value = "1";
                try
                {
                    // var bytes = Convert.FromBase64String(value);
                    var bytes = System.Text.Encoding.UTF8.GetBytes(value);
                    await _socket.OutputStream.WriteAsync(bytes, 0, bytes.Length);
                }
                catch(Exception ex)
                {
                    //TODO: Show an error
                }
            }
        }

        private void AddBluetooth()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter == null)
                throw new Exception("No bluetooth found");
            if (!adapter.IsEnabled)
            {
                throw new Exception("Bluetooth Not enabled");
            }

            else if (adapter.IsEnabled)
            {
                var blDevicesArray = adapter.BondedDevices.ToArray();

                // BlListView.Adapter = new ArrayAdapter<BluetoothDevice>(this, Android.Resource.Layout.SimpleListItem2, blDevicesArray);
                _BlListView.Adapter = new BluetoothListView2Adapter(this, blDevicesArray.ToList());


                _BlListView.ChoiceMode = ChoiceMode.Single;
                _BlListView.ItemClick += BlListView_ItemClick;
            }
        }

        private async void BlListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var adapter = _BlListView.Adapter as BluetoothListView2Adapter;
            var blDevice = adapter[e.Position];
            _socket = blDevice.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
            await _socket.ConnectAsync();

            _LEDOnButton.Enabled = _socket.IsConnected;
            _LEDOffButton.Enabled = _socket.IsConnected;

            if(_socket.IsConnected)
            {
                
            }
        }
    }
}

