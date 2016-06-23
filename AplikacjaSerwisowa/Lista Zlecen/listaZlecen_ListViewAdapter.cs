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
    class listaZlecen_ListViewAdapter : BaseAdapter<string>
    {
        private List<string> mKontrahenci;
        private List<string> mData;
        private List<string> mTelefon;
        private List<string> mAdres;
        private List<string> mGodzina;
        private List<string> mWykonanie;

        private Context mContext;

        public listaZlecen_ListViewAdapter(Context context, List<string> kontrahenci_List, List<string> data_List, List<string> adres_List, List<string> telefon_List, List<string> godzina_List, List<string> wykonanie_List)
        {
            mKontrahenci = kontrahenci_List;
            mData = data_List;
            mTelefon = telefon_List;
            mAdres = adres_List;
            mGodzina = godzina_List;
            mWykonanie = wykonanie_List;

            mContext = context;
        }

        public override int Count
        {
            get
            { return mKontrahenci.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if(row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.listview_row, null, false);
            }

            TextView kontrahent_TextView = row.FindViewById<TextView>(Resource.Id.kontrahentTextView);
            TextView data_TextView = row.FindViewById<TextView>(Resource.Id.dataTextView);
            TextView adres_TextView = row.FindViewById<TextView>(Resource.Id.adresTextView);
            TextView telefon_TextView = row.FindViewById<TextView>(Resource.Id.telefonTextView);
            TextView godzina_TextView = row.FindViewById<TextView>(Resource.Id.godzinaTextView);
            ImageView wykonanie_ImageView = row.FindViewById<ImageView>(Resource.Id.realizacjaImageView);

            kontrahent_TextView.Text = mKontrahenci[position];
            data_TextView.Text = mData[position];
            adres_TextView.Text = mAdres[position];
            telefon_TextView.Text = mTelefon[position];
            godzina_TextView.Text = mGodzina[position];

            if(mWykonanie[position] == "1")
            {
                wykonanie_ImageView.SetImageResource(Resource.Drawable.wykonane_ListaZlecen);
            }
            else
            {
                wykonanie_ImageView.SetImageResource(Resource.Drawable.wykonane_czesciowe_ListaZlecen);
            }

            return row;
        }

        public override string this[int position]
        {
            get{ return mKontrahenci[position]; }
        }
    }
}