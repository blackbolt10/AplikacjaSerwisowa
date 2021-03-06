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
    class listaZlecenSzczegolySkladniki_ListViewAdapter : BaseAdapter<string>
    {
        List<SrwZlcSkladniki> szsList;

        Context mContext;


        public listaZlecenSzczegolySkladniki_ListViewAdapter(Context context, List<SrwZlcSkladniki> _szsList)
        {
            szsList = _szsList;
            mContext = context;
        }

        public override int Count
        {
            get
            { return szsList.Count; }
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.skladnikiListaZlecenSzczegoly_row, null, false);
            }

            TextView akronim_TextView = row.FindViewById<TextView>(Resource.Id.akronimSkladnikiListaZlecenSzczegolyTextView);
            TextView pozycja_TextView = row.FindViewById<TextView>(Resource.Id.pozycjaSkladnikiListaZlecenSzczegolyTextView);
            TextView ilosc_TextView = row.FindViewById<TextView>(Resource.Id.iloscSkladnikiListaZlecenSzczegolyTextView);
            TextView nazwa_TextView = row.FindViewById<TextView>(Resource.Id.nazwaSkladnikiListaZlecenSzczegolyTextView);
            TextView jm_TextView = row.FindViewById<TextView>(Resource.Id.jmSkladnikiListaZlecenSzczegolyTextView);

            DBRepository dbr = new DBRepository();
            TwrKartyTable twrKarta = dbr.TwrKartyTable_GetRecord(szsList[position].SZS_TwrNumer);

            akronim_TextView.Text = twrKarta.Twr_Kod;
            pozycja_TextView.Text = szsList[position].SZS_Pozycja.ToString();
            ilosc_TextView.Text = szsList[position].SZS_Ilosc.ToString();
            nazwa_TextView.Text = twrKarta.Twr_Nazwa;
            jm_TextView.Text = twrKarta.Twr_Jm;

            return row;
        }

        public override string this[int position]
        {
            get { return szsList[position].SZS_Id.ToString(); }
        }
    }
}