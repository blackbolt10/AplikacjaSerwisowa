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
    class listaKontrahentow_ListViewAdapter : BaseAdapter<string>
    {
        struct kntStruktura
        {
            public int Id { get; set; }
            public Int32 GIDNumer { get; set; }
            public String Akronim { get; set; }
            public String Nazwa { get; set; }
        }

        private List<kntStruktura> kntLista;
        private Context mContext;

        public listaKontrahentow_ListViewAdapter(Context context, List<KntKartyTable> _kntKartyList, List<KntAdresyTable> _kntAdresyList)
        {
            kntLista = new List<kntStruktura>();

            if(_kntKartyList != null)
            {
                aktualizujStrukture(_kntKartyList);
            }
            else
            {
                aktualizujStrukture(_kntAdresyList);
            }

            mContext = context;
        }

        private void aktualizujStrukture(List<KntKartyTable> kntKartyList)
        {
            if(kntKartyList.Count > 0)
            {
                for(int i = 0;i < kntKartyList.Count;i++)
                {
                    kntStruktura strukt = new kntStruktura();
                    strukt.Id = kntKartyList[i].Id;
                    strukt.Nazwa = kntKartyList[i].Knt_nazwa1;
                    strukt.Akronim = kntKartyList[i].Knt_Akronim;
                    strukt.GIDNumer = kntKartyList[i].Knt_GIDNumer;

                    kntLista.Add(strukt);
                }
            }
        }

        private void aktualizujStrukture(List<KntAdresyTable> kntAdresyList)
        {
            if(kntAdresyList.Count > 0)
            {
                for(int i = 0; i < kntAdresyList.Count; i++)
                {
                    kntStruktura strukt = new kntStruktura();
                    strukt.Id = kntAdresyList[i].Id;
                    strukt.Nazwa = kntAdresyList[i].Kna_nazwa1;
                    strukt.Akronim = kntAdresyList[i].Kna_Akronim;
                    strukt.GIDNumer = kntAdresyList[i].Kna_GIDNumer;

                    kntLista.Add(strukt);
                }
            }
        }

        public override int Count
        {
            get
            { return kntLista.Count; }
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

            TextView akronim_TextView = row.FindViewById<TextView>(Resource.Id.akronimKontrahenciRowTextView);
            TextView email_TextView = row.FindViewById<TextView>(Resource.Id.emailKontrahenciRowsTextView);
            TextView telefon1_TextView = row.FindViewById<TextView>(Resource.Id.telefonKontrahenciRowTextView);
            TextView ulica_TextView = row.FindViewById<TextView>(Resource.Id.ulicaKontrahenciRowTextView);
            TextView adres_TextView = row.FindViewById<TextView>(Resource.Id.adresKodPKontrahenciRowTextView);
            TextView gidnumer_TextView = row.FindViewById<TextView>(Resource.Id.gidNumerKontrahenciRowTextView);
            LinearLayout linearlayout = row.FindViewById<LinearLayout>(Resource.Id.linearLayout2);
            LinearLayout linearlayout1 = row.FindViewById<LinearLayout>(Resource.Id.daneKontrahenta2LinearLayout);
            LinearLayout parametryKontrahenciLinearLayout = row.FindViewById<LinearLayout>(Resource.Id.parametryKontrahenciLinearLayout);


            
            akronim_TextView.Visibility = ViewStates.Gone;
            email_TextView.Visibility = ViewStates.Gone;
            telefon1_TextView.Visibility = ViewStates.Gone;
            gidnumer_TextView.Visibility = ViewStates.Gone;
            parametryKontrahenciLinearLayout.Visibility = ViewStates.Gone;
            

            LinearLayout.LayoutParams param = new LinearLayout.LayoutParams(
                             ViewGroup.LayoutParams.MatchParent,
                             ViewGroup.LayoutParams.MatchParent, 1.0f);

            adres_TextView.LayoutParameters = param;
            ulica_TextView.LayoutParameters = param;

            ulica_TextView.Text = "["+kntLista[position].Akronim + "]";
            adres_TextView.Text = kntLista[position].Nazwa;

            if(adres_TextView.Text == "")
            {
                linearlayout1.Visibility = ViewStates.Gone;
            }

            return row;
        }

        public override string this[int position]
        {
            get{ return kntLista[position].GIDNumer.ToString(); }
        }
    }
}