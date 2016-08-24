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
    class listaZlecenSzczegolyUrzadzenia_ListViewAdapter : BaseAdapter<string>
    {
        List<SrwZlcUrz> SZUList;

        Context mContext;


        public listaZlecenSzczegolyUrzadzenia_ListViewAdapter(Context context, List<SrwZlcUrz> _SZUList)
        {
            SZUList = _SZUList;
            mContext = context;
        }

        public override int Count
        {
            get
            { return SZUList.Count; }
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
            SrwUrzadzenia urzadzenie = dbr.SrwUrzadzenia_GetRecord(SZUList[position].SZU_SrUId);

            pozycja_TextView.Text = SZUList[position].SZU_Pozycja.ToString();
            akronim_TextView.Text = urzadzenie.Sru_Kod;
            nazwa_TextView.Text = urzadzenie.Sru_Nazwa;

            ilosc_TextView.Visibility = ViewStates.Gone;
            jm_TextView.Visibility = ViewStates.Gone;

            return row;
        }

        public override string this[int position]
        {
            get { return SZUList[position].SZU_Id.ToString(); }
        }
    }
}