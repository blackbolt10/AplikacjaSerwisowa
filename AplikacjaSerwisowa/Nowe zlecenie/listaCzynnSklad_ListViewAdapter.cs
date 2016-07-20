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
    class listaCzynnSklad_ListViewAdapter : BaseAdapter<string>
    {
        private List<TwrKartyTable> twrKartyList;
        private Context mContext;
        private Boolean full;
        private Boolean czynniki;

        public listaCzynnSklad_ListViewAdapter(Context context, List<TwrKartyTable> _twrKartyList, Boolean _full, Boolean _czynniki)
        {
            twrKartyList = _twrKartyList;
            czynniki = _czynniki;
            full = _full;

            mContext = context;
        }

        public override int Count
        {
            get
            { return twrKartyList.Count; }
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.noweZlecenieZakladkaCzynnSklad_row, null, false);
            }

            TextView akronim_TextView = row.FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladAkronimTextView);
            TextView nazwa_TextView = row.FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladNazwaTextView);
            TextView nazwaFull_TextView = row.FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladNazwaFullTextView);
            TextView jm_TextView = row.FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladJmTextView);
            TextView ilosc_TextView = row.FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladIloscTextView);
            CheckBox checkBox = row.FindViewById<CheckBox>(Resource.Id.noweZlecenieZakCzynnSkladCheckBox);

            LinearLayout nazwaFullLinearLayout = row.FindViewById<LinearLayout>(Resource.Id.noweZlecenieZakCzynnSkladNazwaFullLinearLayout);
            LinearLayout iloscJmNazwaLinearLayout = row.FindViewById<LinearLayout>(Resource.Id.noweZlecenieZakCzynnSkladIloscJmNazwaLinearLayout);
            LinearLayout naglowek = row.FindViewById<LinearLayout>(Resource.Id.noweZlecenieZakCzynnSkladNaglowekLinearLayout);

            if(full)
            {
                nazwaFullLinearLayout.Visibility = ViewStates.Visible;
                iloscJmNazwaLinearLayout.Visibility = ViewStates.Gone;
                checkBox.Visibility = ViewStates.Gone;

                nazwaFull_TextView.Text = twrKartyList[position].Twr_Nazwa;
            }
            else
            {
                nazwaFullLinearLayout.Visibility = ViewStates.Gone;
                iloscJmNazwaLinearLayout.Visibility = ViewStates.Visible;
                checkBox.Visibility = ViewStates.Visible;

                ilosc_TextView.Text = twrKartyList[position].Ilosc.ToString();
                jm_TextView.Text = twrKartyList[position].Twr_Jm;
                nazwa_TextView.Text = twrKartyList[position].Twr_Nazwa;
            }

            akronim_TextView.Text = "["+twrKartyList[position].Twr_Kod+"]";

            checkBox.Checked = twrKartyList[position].zaznaczone;
            if(czynniki)
            {
                checkBox.Click += delegate (object sender, EventArgs e)
                {
                    zakladkaCzynnosciNoweZlecenie.aktualizujChecBox(position);
                };
            }
            else
            {
                checkBox.Click += delegate (object sender, EventArgs e)
                {
                    zakladkaSkladnikiNoweZlecenie.aktualizujChecBox(position);
                };
            }

            return row;
        }

        public override string this[int position]
        {
            get{ return twrKartyList[position].Twr_GIDNumer.ToString(); }
        }
    }
}