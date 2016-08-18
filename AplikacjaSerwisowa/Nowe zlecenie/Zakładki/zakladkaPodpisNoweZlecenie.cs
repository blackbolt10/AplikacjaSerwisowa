using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using SQLite;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AplikacjaSerwisowa
{
    class zakladkaPodpisNoweZlecenie : Android.Support.V4.App.Fragment
    {
        private Button zapiszButton;
        private Context kontekst;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.noweZlecenieZakladkaPodpisLayout, container, false);

            LinearLayout linear = view.FindViewById<LinearLayout>(Resource.Id.noweZlecenieZakladkaPodpisLinearLayout);
            linear.Visibility = ViewStates.Gone;

            kontekst = noweZlecenie_Activity.GetContext();

            zapiszButton = view.FindViewById<Button>(Resource.Id.noweZlecenieZakladkaPodpisZapiszButton);
            zapiszButton.Click += ZapiszButton_Click;

            return view;
        }

        private void ZapiszButton_Click(object sender, EventArgs e)
        {
            Boolean result = pobierzKontrahentow();

            if(result)
            {
                Intent noweZlecenieZapiszActivity = new Intent(kontekst, typeof(noweZlecenieZapisz));
                StartActivityForResult(noweZlecenieZapiszActivity, 0);
            }
        }

        private Boolean pobierzKontrahentow()
        {
            if(noweZlecenie_Activity.Knt_GIDNumer != -1 && noweZlecenie_Activity.Kna_GIDNumer != -1)
            {
                return true;
            }
            else if(noweZlecenie_Activity.Knt_GIDNumer == -1 && noweZlecenie_Activity.Kna_GIDNumer == -1)
            {
                Toast.MakeText(kontekst, "Kontrahent główny i docelowy nie zostali ustawieni! \nOperacja zapisu jest niemożliwa.", ToastLength.Short).Show();
            }
            else if(noweZlecenie_Activity.Knt_GIDNumer == -1 && noweZlecenie_Activity.Kna_GIDNumer != -1)
            {
                Toast.MakeText(kontekst, "Kontrahent główny nie został ustawieny! \nOperacja zapisu jest niemożliwa.", ToastLength.Short).Show();
            }
            else if(noweZlecenie_Activity.Knt_GIDNumer != -1 && noweZlecenie_Activity.Kna_GIDNumer == -1)
            {
                Toast.MakeText(kontekst, "Kontrahent docelowy nie został ustawieny! \nOperacja zapisu jest niemożliwa.", ToastLength.Short).Show();
            }
            return false;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            switch(requestCode)
            {
                case 0:
                    Boolean result = data.GetBooleanExtra("koniec", false);

                    if(result)
                    {
                        this.Activity.Finish();
                    }
                break;
            }
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Podpis";
        }
    }
}