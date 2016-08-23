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
    class listaUrzadzenia_ListViewAdapter : BaseAdapter<string>
    {
        private List<SrwUrzadzenia> urzadzeniaList;
        private Boolean full;
        private Context mContext;

        public listaUrzadzenia_ListViewAdapter(Context context, List<SrwUrzadzenia> _urzadzeniaList, Boolean _full)
        {
            urzadzeniaList = _urzadzeniaList;
            full = _full;
            mContext = context;
        }

        public override int Count
        {
            get
            { return urzadzeniaList.Count; }
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
            TextView nazwaFull_TextView = row.FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladNazwaFullTextView);
            TextView jm_TextView = row.FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladJmTextView);
            TextView ilosc_TextView = row.FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladIloscTextView);
            CheckBox checkBox = row.FindViewById<CheckBox>(Resource.Id.noweZlecenieZakCzynnSkladCheckBox);

            LinearLayout nazwaFullLinearLayout = row.FindViewById<LinearLayout>(Resource.Id.noweZlecenieZakCzynnSkladNazwaFullLinearLayout);
            LinearLayout iloscJmNazwaLinearLayout = row.FindViewById<LinearLayout>(Resource.Id.noweZlecenieZakCzynnSkladIloscJmNazwaLinearLayout);
            LinearLayout naglowek = row.FindViewById<LinearLayout>(Resource.Id.noweZlecenieZakCzynnSkladNaglowekLinearLayout);

            ilosc_TextView.Visibility = ViewStates.Gone;
            jm_TextView.Visibility = ViewStates.Gone;

            nazwaFull_TextView.Text = urzadzeniaList[position].Sru_Nazwa;

            if(nazwaFull_TextView.Text == "")
            {
                nazwaFull_TextView.Visibility = ViewStates.Gone;
            }

            akronim_TextView.Text = "["+ urzadzeniaList[position].Sru_Kod+"]";
            if(akronim_TextView.Text == "")
            {
                akronim_TextView.Text = "[brak kodu]";
            }

            if(full)
            {
                checkBox.Visibility = ViewStates.Visible;
                checkBox.Checked = urzadzeniaList[position].zaznaczone;

                checkBox.Click += delegate (object sender, EventArgs e)
                {
                    zakladkaUrzadzeniaNoweZlecenie.aktualizujChecBox(position);
                };
            }
            else
            {
                checkBox.Visibility = ViewStates.Gone;
            }

            return row;
        }

        public override string this[int position]
        {
            get{ return urzadzeniaList[position].SrU_Id.ToString(); }
        }
    }
}