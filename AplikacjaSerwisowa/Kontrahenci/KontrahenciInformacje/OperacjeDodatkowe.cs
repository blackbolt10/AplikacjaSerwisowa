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

namespace AplikacjaSerwisowa
{
    public class OperacjeDodatkowe : Android.Support.V4.App.Fragment
    {
        private Button mNoweZleceniButton;
        private string mKNT_GIDNumer;
        private Context mContext;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.kontrahenciOperacjeDodatkowe, container, false);

            mNoweZleceniButton = view.FindViewById<Button>(Resource.Id.kontrahenciOperacjeNoweZlecenieButton);
            mNoweZleceniButton.Click += mNoweZleceniButton_Click;

            mKNT_GIDNumer = kontrahenciInformacje.GetKnt_GidNumer();
            mContext = kontrahenciInformacje.GetContext();

            return view;
        }

        private void mNoweZleceniButton_Click(object sender, EventArgs e)
        {
            Intent noweZlecenieActivity = new Intent(mContext, typeof(noweZlecenie_Activity));
            noweZlecenieActivity.PutExtra("KNT_GIDNumer", mKNT_GIDNumer);
            StartActivity(noweZlecenieActivity);
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Operacje";
        }
    }
}