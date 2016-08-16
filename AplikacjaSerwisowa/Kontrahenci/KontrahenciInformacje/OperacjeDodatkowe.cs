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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.kontrahenciOperacjeDodatkowe, container, false);

            mNoweZleceniButton = view.FindViewById<Button>(Resource.Id.kontrahenciOperacjeNoweZlecenieButton);
            mNoweZleceniButton.Click += mNoweZleceniButton_Click;

            return view;
        }

        private void mNoweZleceniButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Operacje";
        }
    }
}