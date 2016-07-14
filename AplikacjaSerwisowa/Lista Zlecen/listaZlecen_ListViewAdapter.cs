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
        private List<string> mStan;
        private List<string> mNaglowki;
        private List<string> mWykonanie;
        private List<string> mData;

        private Context mContext;

        public listaZlecen_ListViewAdapter(Context context, List<string> kontrahenci_List, List<string> stan_List, List<string> naglowki_List, List<string> data_List, List<string> wykonanie_List)
        {
            mKontrahenci = kontrahenci_List;
            mStan = stan_List;
            mNaglowki = naglowki_List;
            mWykonanie = wykonanie_List;
            mData = data_List;

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

            TextView naglowek_TextView = row.FindViewById<TextView>(Resource.Id.naglowekTextViewListaZlecenNaglowek);
            TextView kontrahent_TextView = row.FindViewById<TextView>(Resource.Id.kontrahentTextViewListaZlecenNaglowek);
            TextView stan_TextView = row.FindViewById<TextView>(Resource.Id.stanTextViewListaZlecenNaglowek);
            ImageView wykonanie_ImageView = row.FindViewById<ImageView>(Resource.Id.realizacjaImageView);

            kontrahent_TextView.Text = mKontrahenci[position];
            stan_TextView.Text = mStan[position];
            naglowek_TextView.Text = mData[position]+" - "+ mNaglowki[position];

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