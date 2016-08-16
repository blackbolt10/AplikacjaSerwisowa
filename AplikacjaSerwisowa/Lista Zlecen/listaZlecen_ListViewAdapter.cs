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
        private List<SrwZlcNag> mSrwZlcNagList;

        private Context mContext;

        public listaZlecen_ListViewAdapter(Context context, List<SrwZlcNag> SrwZlcNagList)
        {
            if(SrwZlcNagList != null)
            {
                mSrwZlcNagList = SrwZlcNagList;
            }
            else
            {
                mSrwZlcNagList = new List<SrwZlcNag>();
            }

            mContext = context;
        }

        public override int Count
        {
            get
            { return mSrwZlcNagList.Count; }
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


            kontrahent_TextView.Text = pobierzKntKartyNazwa(mSrwZlcNagList[position].SZN_KntNumer, mSrwZlcNagList[position].SZN_KnANumer);
            stan_TextView.Text = mSrwZlcNagList[position].SZN_Stan;
            naglowek_TextView.Text = mSrwZlcNagList[position].SZN_DataWystawienia.Split(' ')[0]+" - "+ mSrwZlcNagList[position].SZN_Dokument;

            Random test = new Random();
            test.Next(0, 1);
            if(test.Next(0, 1).ToString() == "1")
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
            get{ return mSrwZlcNagList[position].SZN_Id.ToString(); }
        }


        private String pobierzKntKartyNazwa(Int32 KNT_GuidNumer, Int32 KNA_GuidNumer)
        {

            string wynik = "";
            string kntKartaNazwa = pobierzInformacjeOKntKarta(KNT_GuidNumer);
            string kntAdresNazwa = pobierzInformacjeOKntAdres(KNA_GuidNumer);

            if(kntKartaNazwa != "" && kntAdresNazwa != "")
            {
                if(kntKartaNazwa == kntAdresNazwa)
                {
                    wynik = kntKartaNazwa;
                }
                else
                {
                    wynik = kntKartaNazwa + "\n" + kntAdresNazwa;
                }
            }
            else if(kntKartaNazwa != "" && kntAdresNazwa == "")
            {
                wynik = kntKartaNazwa;
            }
            else if(kntKartaNazwa == "" && kntAdresNazwa != "")
            {
                wynik = kntAdresNazwa;
            }
            else
            {
                wynik = "{brak nazwy}";
            }

            return wynik;
        }
        private String pobierzInformacjeOKntAdres(int SZN_KnaNumer)
        {
            KntAdresyTable kntAdres = new KntAdresyTable();

            try
            {
                DBRepository dbr = new DBRepository();
                kntAdres = dbr.kntAdresy_GetRecord(SZN_KnaNumer.ToString());
            }
            catch(Exception exc)
            {
                Toast.MakeText(mContext, "B³¹d listaZlecen_Activity.pobierzInformacjeOKntAdres():\n" + exc.Message, ToastLength.Short);
            }

            if(kntAdres != null)
            {
                if(kntAdres.Kna_Akronim != "")
                {
                    return "{" + kntAdres.Kna_Akronim + "}";
                }
                else
                {
                    return "{Brak akronimu}";
                }
            }
            else
            {
                return "{}";
            }
        }
        private String pobierzInformacjeOKntKarta(int sZN_KnANumer)
        {
            KntKartyTable kntKarta = new KntKartyTable();

            try
            {
                DBRepository dbr = new DBRepository();
                kntKarta = dbr.kntKarty_GetRecord(sZN_KnANumer.ToString());
            }
            catch(Exception exc)
            {
                Toast.MakeText(mContext, "B³¹d listaZlecen_Activity.pobierzInformacjeOKntKarta():\n" + exc.Message, ToastLength.Short);
            }

            if(kntKarta != null)
            {
                if(kntKarta.Knt_Akronim != "")
                {
                    return "{" + kntKarta.Knt_Akronim + "}";
                }
                else
                {
                    return "{Brak akronimu}";
                }
            }
            else
            {
                return "{}";
            }
        }
    }
}