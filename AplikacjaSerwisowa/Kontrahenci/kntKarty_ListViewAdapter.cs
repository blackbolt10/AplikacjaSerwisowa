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
    class kntKarty_ListViewAdapter : BaseAdapter<string>
    {
        private List<KntKartyTable> mKntKartyList = new List<KntKartyTable>();
        private Int32 mukrywanie;

        private Context mContext;

        public kntKarty_ListViewAdapter(Context context, List<KntKartyTable> kntKartyList, Int32 ukrywanie)
        {
            mKntKartyList = kntKartyList;
            mukrywanie = ukrywanie;
            mContext = context;
        }
        public override int Count
        {
            get
            {
                if(mKntKartyList != null)
                {
                    return mKntKartyList.Count;
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.kontrahenci_row, null, false);
            }

            TextView knt_akronimNazwa_TextView = row.FindViewById<TextView>(Resource.Id.akronimKontrahenciRowTextView);
            TextView knt_gidnumer_TextView = row.FindViewById<TextView>(Resource.Id.gidNumerKontrahenciRowTextView);
            TextView knt_adres_TextView = row.FindViewById<TextView>(Resource.Id.adresKodPKontrahenciRowTextView);
            TextView knt_ulica_TextView = row.FindViewById<TextView>(Resource.Id.ulicaKontrahenciRowTextView);
            TextView knt_telefon_TextView = row.FindViewById<TextView>(Resource.Id.telefonKontrahenciRowTextView);
            TextView knt_email_TextView = row.FindViewById<TextView>(Resource.Id.emailKontrahenciRowsTextView);
            LinearLayout param_LinearLayout = row.FindViewById<LinearLayout>(Resource.Id.parametryKontrahenciLinearLayout);
            LinearLayout daneKontrahenta1_LinearLayout = row.FindViewById<LinearLayout>(Resource.Id.daneKontrahenta1inearLayout);
            LinearLayout daneKontrahenta2_LinearLayout = row.FindViewById<LinearLayout>(Resource.Id.daneKontrahenta2LinearLayout);
            
            knt_akronimNazwa_TextView.Text = "["+ mKntKartyList[position].Knt_Akronim+"]\n"+ mKntKartyList[position].Knt_nazwa1;
            knt_gidnumer_TextView.Text = mKntKartyList[position].Knt_GIDNumer.ToString();

            if(mKntKartyList[position].Knt_ulica == "" && mKntKartyList[position].Knt_ulica == "")
            {
                daneKontrahenta1_LinearLayout.Visibility = ViewStates.Gone;
            }
            else
            {
                daneKontrahenta1_LinearLayout.Visibility = ViewStates.Visible;
                knt_ulica_TextView.Text = mKntKartyList[position].Knt_ulica;
                knt_telefon_TextView.Text = mKntKartyList[position].Knt_telefon1;
            }

            if(mKntKartyList[position].Knt_email == "" && mKntKartyList[position].Knt_KodP == "" && mKntKartyList[position].Knt_miasto == "")
            {
                daneKontrahenta2_LinearLayout.Visibility = ViewStates.Gone;
            }
            else
            {
                daneKontrahenta2_LinearLayout.Visibility = ViewStates.Visible;
                knt_adres_TextView.Text = mKntKartyList[position].Knt_KodP + "  " + mKntKartyList[position].Knt_miasto;
                knt_email_TextView.Text = mKntKartyList[position].Knt_email;
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
            get { return mKntKartyList[position].Knt_GIDNumer.ToString(); }
        }
    }
}