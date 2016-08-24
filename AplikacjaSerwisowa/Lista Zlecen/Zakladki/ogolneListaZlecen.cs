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
    public class zakladkaOgolneListaZlecenSerwisowychSzczegoly : Android.Support.V4.App.Fragment
    {
        private string szn_ID;
        private Context kontekst;
        private TextView dataNumerTextView, kontrahentGlownyTextView, kontrahentDocelowyTextView, stanTextView;
        private LinearLayout linearLayout;
        private ImageView imageView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ogolneListaZlecenSzczegoly, container, false);

            dataNumerTextView = view.FindViewById<TextView>(Resource.Id.dataNumerDokListaZlecenSzczegolyTextView);
            kontrahentGlownyTextView = view.FindViewById<TextView>(Resource.Id.nazwaGlownegoKontrahentaListaZlecenSzczegolyTextView);
            kontrahentDocelowyTextView = view.FindViewById<TextView>(Resource.Id.nazwaDocelowegoKontrahentaListaZlecenSzczegolyTextView);
            stanTextView = view.FindViewById<TextView>(Resource.Id.stanListaZlecenSzczegolyTextView);

            linearLayout = view.FindViewById<LinearLayout>(Resource.Id.ListaZlecenSzczegolyLinearLayout);
            imageView = view.FindViewById<ImageView>(Resource.Id.ListaZlecenSzczegolyLimageView);

            szn_ID = listaZlecenSzczegoly_Activity.szn_ID;
            kontekst = listaZlecenSzczegoly_Activity.GetContext();

            ustawDaneZlecenia();

            return view;
        }

        private void ustawDaneZlecenia()
        {
            SrwZlcNag szn = null;
            DBRepository dbr = new DBRepository();

            try
            {
                szn = dbr.SrwZlcNag_GetRecordGetRecord(szn_ID);
            }
            catch(Exception exc)
            {
                Toast.MakeText(kontekst, "B³¹d listaZlecenSzczegoly_Activity.ustawDaneZlecenia():\n" + exc.Message, ToastLength.Short);
            }

            if(szn != null)
            {
                ustawObraz(szn.SZN_Stan);
                dataNumerTextView.Text = szn.SZN_DataWystawienia.Split(' ')[0] + " - " + szn.SZN_Dokument;
                stanTextView.Text = szn.SZN_Stan;
                if(szn.SZN_KnANumer != -1)
                {
                    KntAdresyTable kntAdres = dbr.kntAdresy_GetRecord(szn.SZN_KnANumer.ToString());
                    if(kntAdres != null)
                    {
                        if(kntAdres.Kna_nazwa1 != "")
                        {
                            kontrahentDocelowyTextView.Text = kntAdres.Kna_nazwa1;
                        }
                        else if(kntAdres.Kna_Akronim != "")
                        {
                            kontrahentDocelowyTextView.Text = kntAdres.Kna_Akronim;
                        }
                        else
                        {
                            kontrahentDocelowyTextView.Text = "{brak kontrahenta docelowego}";
                        }
                    }
                }
                else
                {
                    kontrahentDocelowyTextView.Text = "{brak kontrahenta docelowego}";
                }

                if(szn.SZN_KntNumer != -1)
                {
                    KntKartyTable kntKarta = dbr.kntKarty_GetRecord(szn.SZN_KntNumer.ToString());
                    if(kntKarta != null)
                    {
                        if(kntKarta.Knt_nazwa1 != "")
                        {
                            kontrahentGlownyTextView.Text = kntKarta.Knt_nazwa1;
                        }
                        else if(kntKarta.Knt_Akronim != "")
                        {
                            kontrahentGlownyTextView.Text = kntKarta.Knt_Akronim;
                        }
                        else
                        {
                            kontrahentGlownyTextView.Text = "{brak kontrahenta g³ównego}";
                        }
                    }
                }
                else
                {
                    kontrahentGlownyTextView.Text = "{brak kontrahenta g³ównego}";
                }
                listaZlecenSzczegoly_Activity.knt_GidNumer = szn.SZN_KntNumer.ToString();
                listaZlecenSzczegoly_Activity.szn_AdWNumer = szn.SZN_KnANumer.ToString();
            }
        }

        private void ustawObraz(string sZN_Stan)
        {
            switch(sZN_Stan)
            {
                case "W realizacji":
                imageView.SetImageResource(Resource.Drawable.ListaZlecen_do_realizacji);
                break;

                case "Do realizacji":
                imageView.SetImageResource(Resource.Drawable.ListaZlecen_do_realizacji);
                break;

                case "Niezatwierdzone":
                imageView.SetImageResource(Resource.Drawable.ListaZlecen_do_realizacji);
                break;

                case "Zatwierdzone":
                imageView.SetImageResource(Resource.Drawable.ListaZlecen_zamkniete);
                break;

                case "Odrzucone":
                imageView.SetImageResource(Resource.Drawable.ListaZlecen_odrzucone);
                break;

                case "Anulowane":
                imageView.SetImageResource(Resource.Drawable.ListaZlecen_anulowane);
                break;
            }
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Ogólne";
        }
    }
}