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
    class kontrahenciAdresy_ListViewAdapter : BaseAdapter<string>
    {
        private List<KntAdresyTable> mkntAdresyList = new List<KntAdresyTable>();
        private Int32 mukrywanie;

        private Context mContext;

        public kontrahenciAdresy_ListViewAdapter(Context context, List<KntAdresyTable> kntAdresyList, Int32 ukrywanie)
        {
            mkntAdresyList = kntAdresyList;
            mukrywanie = ukrywanie;
            mContext = context;
        }
        public override int Count
        {
            get
            {
                if(mkntAdresyList != null)
                {
                    return mkntAdresyList.Count;
                }
                else
                {
                    return 0;
                }
            }
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.kontrahenciAdresy_row, null, false);
            }

            TextView knt_akronimNazwa_TextView = row.FindViewById<TextView>(Resource.Id.akronimKontrahenciAdresyRowTextView);
            TextView knt_gidnumer_TextView = row.FindViewById<TextView>(Resource.Id.gidNumerKontrahenciAdresyRowTextView);
            TextView knt_kntNumer_TextView = row.FindViewById<TextView>(Resource.Id.kntNumerKontrahenciAdresyRowTextView);
            TextView knt_adres_TextView = row.FindViewById<TextView>(Resource.Id.adresKodPKontrahenciAdresyRowTextView);
            TextView knt_ulica_TextView = row.FindViewById<TextView>(Resource.Id.ulicaKontrahenciAdresyRowTextView);
            TextView knt_telefon_TextView = row.FindViewById<TextView>(Resource.Id.telefonKontrahenciAdresyRowTextView);
            TextView knt_email_TextView = row.FindViewById<TextView>(Resource.Id.emailKontrahenciAdresyRowsTextView);
            LinearLayout param_LinearLayout = row.FindViewById<LinearLayout>(Resource.Id.parametryKontrahenciAdresyLinearLayout);
            LinearLayout daneKontrahenta1_LinearLayout = row.FindViewById<LinearLayout>(Resource.Id.daneKontrahentaAdresy1inearLayout);
            LinearLayout daneKontrahenta2_LinearLayout = row.FindViewById<LinearLayout>(Resource.Id.daneKontrahentaAdresy2LinearLayout);

            knt_akronimNazwa_TextView.Text = "[" + mkntAdresyList[position].Kna_Akronim + "]\n" + mkntAdresyList[position].Kna_nazwa1;
            knt_gidnumer_TextView.Text = mkntAdresyList[position].Kna_GIDNumer.ToString();
            knt_kntNumer_TextView.Text = mkntAdresyList[position].Kna_KntNumer.ToString();

            if(mkntAdresyList[position].Kna_ulica == "" && mkntAdresyList[position].Kna_telefon1 == "")
            {
                daneKontrahenta1_LinearLayout.Visibility = ViewStates.Gone;
            }
            else
            {
                daneKontrahenta1_LinearLayout.Visibility = ViewStates.Visible;
                knt_ulica_TextView.Text = mkntAdresyList[position].Kna_ulica;
                knt_telefon_TextView.Text = mkntAdresyList[position].Kna_telefon1;
            }

            if(mkntAdresyList[position].Kna_email == "" && mkntAdresyList[position].Kna_KodP == "" && mkntAdresyList[position].Kna_miasto == "")
            {
                daneKontrahenta2_LinearLayout.Visibility = ViewStates.Gone;
            }
            else
            {
                daneKontrahenta2_LinearLayout.Visibility = ViewStates.Visible;
                knt_adres_TextView.Text = mkntAdresyList[position].Kna_KodP + " " + mkntAdresyList[position].Kna_miasto;
                knt_email_TextView.Text = mkntAdresyList[position].Kna_email;
            }

            if(mukrywanie == 1)
            {
                param_LinearLayout.Visibility = ViewStates.Visible;
            }
            else
            {
                param_LinearLayout.Visibility = ViewStates.Gone;
            }

            return row;
        }
        public override string this[int position]
        {
            get { return mkntAdresyList[position].Kna_GIDNumer.ToString(); }
        }
    }
}