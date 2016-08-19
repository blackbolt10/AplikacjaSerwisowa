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
    public class zakladkaOgolneNoweZlecenie : Android.Support.V4.App.Fragment
    {
        private Button dataWystButton, dataRealizButton;
        private static EditText opisEditText, dataWystawEditText, dataRealizEditText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.noweZlecenieZakladkaOgolneLayout, container, false);
            
            opisEditText = view.FindViewById<EditText>(Resource.Id.noweZakOpisNagEditText);
            opisEditText.TextChanged += OpisEditText_TextChanged;
            opisEditText.Text = noweZlecenie_Activity.opisSrwZlcNag;
            dataWystawEditText = view.FindViewById<EditText>(Resource.Id.noweZakDataWystawieniaEditText);
            dataWystawEditText.TextChanged += DataWystawEditText_TextChanged;
            dataWystawEditText.Text = noweZlecenie_Activity.DataWystawienia;
            dataRealizEditText = view.FindViewById<EditText>(Resource.Id.noweZakDataRealizacjiEditText);
            dataRealizEditText.TextChanged += DataRealizEditText_TextChanged;
            dataRealizEditText.Text = noweZlecenie_Activity.DataRealizacji;

            dataWystButton = view.FindViewById<Button>(Resource.Id.noweZakDataWystButton);
            dataWystButton.Click += DataWystButton_Click;
            dataRealizButton = view.FindViewById<Button>(Resource.Id.noweZakDataRealizButton);
            dataRealizButton.Click += DataRealizButton_Click;  

            return view;
        }

        private void DataRealizEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            noweZlecenie_Activity.DataRealizacji = dataRealizEditText.Text;
        }

        private void DataWystawEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            noweZlecenie_Activity.DataWystawienia = dataWystawEditText.Text;
        }

        private void OpisEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            noweZlecenie_Activity.opisSrwZlcNag = opisEditText.Text;
        }

        private void DataRealizButton_Click(object sender, EventArgs e)
        {
            String dataWystawienia = dataWystawEditText.Text;

            Intent noweZlcenieDataIntent = new Intent(this.Activity, typeof(noweZlecenieDatePickerActivity));
            noweZlcenieDataIntent.PutExtra("czyWystawienia", "0");
            noweZlcenieDataIntent.PutExtra("dataWystawienia", dataWystawienia);
            StartActivity(noweZlcenieDataIntent);
        }

        private void DataWystButton_Click(object sender, EventArgs e)
        {
            String dataWystawienia = dataWystawEditText.Text;

            Intent noweZlcenieDataIntent = new Intent(this.Activity, typeof(noweZlecenieDatePickerActivity));
            noweZlcenieDataIntent.PutExtra("czyWystawienia", "1");
            noweZlcenieDataIntent.PutExtra("dataWystawienia", dataWystawienia);
            StartActivity(noweZlcenieDataIntent);
        }

        public static SrwZlcNag pobierzNaglowek()
        {
            SrwZlcNag srwZlcNag = new SrwZlcNag();
            srwZlcNag.SZN_DataRozpoczecia = dataRealizEditText.Text;
            srwZlcNag.SZN_DataWystawienia = dataWystawEditText.Text;
            srwZlcNag.SZN_Opis = opisEditText.Text;

            return srwZlcNag;
        }

        public static void aktualizujDate(Int32 dataParam, string data)
        {
            if(dataParam == 1)
            {
                dataWystawEditText.Text = data;
                dataRealizEditText.Text = data;
            }
            else
            {
                dataRealizEditText.Text = data;
            }
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Ogólne";
        }
    }
}