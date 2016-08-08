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
    class listaZlecenSzczegolyCzynnosci_ListViewAdapter : BaseAdapter<string>
    {
        List<SrwZlcCzynnosci> szcList;

        Context mContext;


        public listaZlecenSzczegolyCzynnosci_ListViewAdapter(Context context, List<SrwZlcCzynnosci> _szcList)
        {
            szcList = _szcList;
            mContext = context;
        }

        public override int Count
        {
            get
            { return szcList.Count; }
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.czynnosciListaZlecenSzczegoly_row, null, false);
            }

            TextView akronim_TextView = row.FindViewById<TextView>(Resource.Id.akronimCzynnosciListaZlecenSzczegolyTextView);
            TextView pozycja_TextView = row.FindViewById<TextView>(Resource.Id.pozycjaCzynnosciListaZlecenSzczegolyTextView);
            TextView ilosc_TextView = row.FindViewById<TextView>(Resource.Id.iloscCzynnosciListaZlecenSzczegolyTextView);
            TextView nazwa_TextView = row.FindViewById<TextView>(Resource.Id.nazwaCzynnosciListaZlecenSzczegolyTextView);
            TextView jm_TextView = row.FindViewById<TextView>(Resource.Id.jmCzynnosciListaZlecenSzczegolyTextView);

            DBRepository dbr = new DBRepository();
            TwrKartyTable twr = dbr.TwrKartyTable_GetRecord(szcList[position].SZC_TwrNumer);

            akronim_TextView.Text = twr.Twr_Kod;
            pozycja_TextView.Text = szcList[position].SZC_Pozycja.ToString();
            ilosc_TextView.Text = szcList[position].SZC_Ilosc.ToString();
            nazwa_TextView.Text = szcList[position].SZC_TwrNazwa;
            jm_TextView.Text = twr.Twr_Jm;

            return row;
        }

        public override string this[int position]
        {
            get { return szcList[position].SZC_Id.ToString(); }
        }
    }
}