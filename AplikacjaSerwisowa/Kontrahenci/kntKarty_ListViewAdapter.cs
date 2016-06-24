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
        private List<string> mkna_gidnumer_List = new List<string>();
        private List<string> mkna_akronim_List = new List<string>();
        private List<string> mkna_nazwa1_List = new List<string>();
        private List<string> mkna_nazwa2_List = new List<string>();
        private List<string> mkna_nazwa3_List = new List<string>();
        private List<string> mkna_kodp_List = new List<string>();
        private List<string> mkna_miasto_List = new List<string>();
        private List<string> mkna_ulica_List = new List<string>();
        private List<string> mkna_adresy_List = new List<string>();
        private List<string> mkna_nip_List = new List<string>();
        private List<string> mkna_telefon1_List = new List<string>();
        private List<string> mkna_telefon2_List = new List<string>();
        private List<string> mkna_telefon3_List = new List<string>();
        private List<string> mkna_telex_List = new List<string>();
        private List<string> mkna_fax_List = new List<string>();
        private List<string> mkna_email_List = new List<string>();
        private Int32 mukrywanie;

        private Context mContext;

        public kntKarty_ListViewAdapter(Context context, List<string> kna_gidnumer_List, List<string> kna_akronim_List, List<string> kna_nazwa1_List, List<string> kna_nazwa2_List, List<string> kna_nazwa3_List, List<string> kna_kodp_List, List<string> kna_miasto_List, List<string> kna_ulica_List, List<string> kna_adresy_List, List<string> kna_nip_List, List<string> kna_telefon1_List, List<string> kna_telefon2_List, List<string> kna_telefon3_List, List<string> kna_telex_List, List<string> kna_fax_List, List<string> kna_email_List, Int32 ukrywanie)
        {
            mkna_gidnumer_List = kna_gidnumer_List;
            mkna_akronim_List = kna_akronim_List;
            mkna_nazwa1_List = kna_nazwa1_List;
            mkna_nazwa2_List = kna_nazwa2_List;
            mkna_nazwa3_List = kna_nazwa3_List;
            mkna_kodp_List = kna_kodp_List;
            mkna_miasto_List = kna_miasto_List;
            mkna_ulica_List = kna_ulica_List;
            mkna_adresy_List = kna_adresy_List;
            mkna_nip_List = kna_nip_List;
            mkna_telefon1_List = kna_telefon1_List;
            mkna_telefon2_List = kna_telefon2_List;
            mkna_telefon3_List = kna_telefon3_List;
            mkna_telex_List = kna_telex_List;
            mkna_fax_List = kna_fax_List;
            mkna_email_List = kna_email_List;

            mukrywanie = ukrywanie;

            mContext = context;
        }
        public override int Count
        {
            get
            {
                if(mkna_akronim_List != null)
                {
                    return mkna_akronim_List.Count;
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

            int test = position;
            knt_akronimNazwa_TextView.Text = "["+mkna_akronim_List[position]+"]"+mkna_nazwa1_List[position];
            knt_gidnumer_TextView.Text = mkna_gidnumer_List[position];
            knt_adres_TextView.Text = mkna_kodp_List[position]+" "+mkna_miasto_List[position];
            knt_ulica_TextView.Text = mkna_adresy_List[position];
            knt_telefon_TextView.Text = mkna_telefon1_List[position];
            knt_email_TextView.Text = mkna_email_List[position];

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
            get { return mkna_akronim_List[position]; }
        }
    }
}