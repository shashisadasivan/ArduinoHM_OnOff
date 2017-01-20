using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;

namespace Xam.And.BL
{
    public class BluetoothListView2Adapter : BaseAdapter<BluetoothDevice>
    {
        private readonly List<BluetoothDevice> _blDevices;
        private readonly Activity _activity;

        public BluetoothListView2Adapter(Activity activity, IEnumerable<BluetoothDevice> blDevices)
        {
            _activity = activity;
            _blDevices = blDevices.ToList();   
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override BluetoothDevice this[int position]
        {
            get { return _blDevices[position]; }
        }

        public override int Count
        {
            get { return _blDevices.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if(view == null)
            {
                view = _activity.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemActivated2, null);
            }

            var blDevice = _blDevices[position];

            TextView text1 = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            text1.Text = blDevice.Name;

            TextView text2 = view.FindViewById<TextView>(Android.Resource.Id.Text2);
            text2.Text = blDevice.Address;

            return view;
        }
    }
}