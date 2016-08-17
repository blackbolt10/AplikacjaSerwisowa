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
    public class zakladkaKontrahentListaZlecenSerwisowychSzczegoly : Android.Support.V4.App.Fragment
    {
        private string szn_ID;
        private Context kontekst;

        private TextView akronimGlownyTextView, nazwa1GlownyTextView, nazwa2GlownyTextView, nazwa3GlownyTextView,
            ulicaGlownyTextView, kodPMiastoGlownyTextView, telefon1GlownyTextView, telefon2GlownyTextView,
            nipGlownyTextView, emailGlownyTextView;
        private TextView nipNaglowekGlownyTextView, telefonNaglowekGlownyTextView, emailNaglowekGlownyTextView;

        private LinearLayout telefonGlownyLinearLayout;

        private TextView akronimDocelowyTextView, nazwa1DocelowyTextView, nazwa2DocelowyTextView, nazwa3DocelowyTextView,
            ulicaDocelowyTextView, kodPMiastoDocelowyTextView, telefon1DocelowyTextView, telefon2DocelowyTextView,
            nipDocelowyTextView, emailDocelowyTextView;
        private TextView nipNaglowekDocelowyTextView, telefonNaglowekDocelowyTextView, emailNaglowekDocelowyTextView;

        private LinearLayout telefonDocelowyLinearLayout;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.kontrahentListaZlecenSzczegolyLayout, container, false);

            akronimGlownyTextView = view.FindViewById<TextView>(Resource.Id.akronimGlownyKontrahentListaZlecenSczegolyTextView);
            nazwa1GlownyTextView = view.FindViewById<TextView>(Resource.Id.nazwa1GlownyKontrahentListaZlecenSczegolyTextView);
            nazwa2GlownyTextView = view.FindViewById<TextView>(Resource.Id.nazwa2GlownyKontrahentListaZlecenSczegolyTextView);
            nazwa3GlownyTextView = view.FindViewById<TextView>(Resource.Id.nazwa3GlownyKontrahentListaZlecenSczegolyTextView);
            ulicaGlownyTextView = view.FindViewById<TextView>(Resource.Id.ulicaGlownyKontrahentListaZlecenSczegolyTextView);
            kodPMiastoGlownyTextView = view.FindViewById<TextView>(Resource.Id.kodpMiastoGlownyKontrahentListaZlecenSczegolyTextView);
            telefon1GlownyTextView = view.FindViewById<TextView>(Resource.Id.telefon2GlownyKontrahentListaZlecenSczegolyTextView);
            telefon2GlownyTextView = view.FindViewById<TextView>(Resource.Id.telefon1GlownyKontrahentListaZlecenSczegolyTextView);
            nipGlownyTextView = view.FindViewById<TextView>(Resource.Id.nipGlownyKontrahentListaZlecenSczegolyTextView);
            emailGlownyTextView = view.FindViewById<TextView>(Resource.Id.emailGlownyKontrahentListaZlecenSczegolyTextView);
            telefonGlownyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.telefonGlownyKontrahentListaZlecenSczegolyLinearLayout);

            nipNaglowekGlownyTextView = view.FindViewById<TextView>(Resource.Id.nipNaglowekGlownyKontrahentListaZlecenSczegolyTextView);
            telefonNaglowekGlownyTextView = view.FindViewById<TextView>(Resource.Id.telefonNaglowekGlownyKontrahentListaZlecenSczegolyTextView);
            emailNaglowekGlownyTextView = view.FindViewById<TextView>(Resource.Id.emailNaglowekGlownyKontrahentListaZlecenSczegolyTextView);

            akronimDocelowyTextView = view.FindViewById<TextView>(Resource.Id.akronimDocelowyKontrahentListaZlecenSczegolyTextView);
            nazwa1DocelowyTextView = view.FindViewById<TextView>(Resource.Id.nazwa1DocelowyKontrahentListaZlecenSczegolyTextView);
            nazwa2DocelowyTextView = view.FindViewById<TextView>(Resource.Id.nazwa2DocelowyKontrahentListaZlecenSczegolyTextView);
            nazwa3DocelowyTextView = view.FindViewById<TextView>(Resource.Id.nazwa3DocelowyKontrahentListaZlecenSczegolyTextView);
            ulicaDocelowyTextView = view.FindViewById<TextView>(Resource.Id.ulicaDocelowyKontrahentListaZlecenSczegolyTextView);
            kodPMiastoDocelowyTextView = view.FindViewById<TextView>(Resource.Id.kodpMiastoDocelowyKontrahentListaZlecenSczegolyTextView);
            telefon1DocelowyTextView = view.FindViewById<TextView>(Resource.Id.telefon2DocelowyKontrahentListaZlecenSczegolyTextView);
            telefon2DocelowyTextView = view.FindViewById<TextView>(Resource.Id.telefon1DocelowyKontrahentListaZlecenSczegolyTextView);
            nipDocelowyTextView = view.FindViewById<TextView>(Resource.Id.nipDocelowyKontrahentListaZlecenSczegolyTextView);
            emailDocelowyTextView = view.FindViewById<TextView>(Resource.Id.emailDocelowyKontrahentListaZlecenSczegolyTextView);
            telefonDocelowyLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.telefonDocelowyKontrahentListaZlecenSczegolyLinearLayout);

            nipNaglowekDocelowyTextView = view.FindViewById<TextView>(Resource.Id.nipNaglowekDocelowyKontrahentListaZlecenSczegolyTextView);
            telefonNaglowekDocelowyTextView = view.FindViewById<TextView>(Resource.Id.telefonNaglowekDocelowyKontrahentListaZlecenSczegolyTextView);
            emailNaglowekDocelowyTextView = view.FindViewById<TextView>(Resource.Id.emailNaglowekDocelowyKontrahentListaZlecenSczegolyTextView);

            szn_ID = listaZlecenSzczegoly_Activity.szn_ID;
            kontekst = listaZlecenSzczegoly_Activity.GetContext();

            ustawKontrahentaGlownego();
            ustawKontrahentaDocelowego();

            return view;
        }

        private void ustawKontrahentaGlownego()
        {
            KntKartyTable kntKarta = null;
            DBRepository dbr = new DBRepository();

            try
            {
                kntKarta = dbr.kntKarty_GetRecord(listaZlecenSzczegoly_Activity.knt_GidNumer);
            }
            catch(Exception exc)
            {
                Toast.MakeText(kontekst, "B³¹d zakladkaKontrahentListaZlecenSerwisowychSzczegoly.ustawKontrahentaGlownego():\n" + exc.Message, ToastLength.Short);
            }

            if(kntKarta != null)
            {
                akronimGlownyTextView.Text = "[" + kntKarta.Knt_Akronim + "]";
                if(akronimGlownyTextView.Text == "[]")
                {
                    akronimGlownyTextView.Visibility = ViewStates.Gone;
                }

                nazwa1GlownyTextView.Text = kntKarta.Knt_nazwa1;
                if(nazwa1GlownyTextView.Text == "")
                {
                    nazwa1GlownyTextView.Visibility = ViewStates.Gone;
                }

                nazwa2GlownyTextView.Text = kntKarta.Knt_nazwa2;
                if(nazwa2GlownyTextView.Text == "")
                {
                    nazwa2GlownyTextView.Visibility = ViewStates.Gone;
                }

                nazwa3GlownyTextView.Text = kntKarta.Knt_nazwa3;
                if(nazwa3GlownyTextView.Text == "")
                {
                    nazwa3GlownyTextView.Visibility = ViewStates.Gone;
                }

                ulicaGlownyTextView.Text = kntKarta.Knt_ulica;
                if(ulicaGlownyTextView.Text == "")
                {
                    ulicaGlownyTextView.Visibility = ViewStates.Gone;
                }

                kodPMiastoGlownyTextView.Text = kntKarta.Knt_KodP + " " + kntKarta.Knt_miasto;
                if(kodPMiastoGlownyTextView.Text == "")
                {
                    kodPMiastoGlownyTextView.Visibility = ViewStates.Gone;
                }

                telefon1GlownyTextView.Text = kntKarta.Knt_telefon1;
                if(telefon1GlownyTextView.Text == "")
                {
                    telefon1GlownyTextView.Visibility = ViewStates.Gone;
                }

                telefon2GlownyTextView.Text = kntKarta.Knt_telefon2;
                if(telefon2GlownyTextView.Text == "")
                {
                    telefon2GlownyTextView.Visibility = ViewStates.Gone;
                }

                if(telefon1GlownyTextView.Text == "" && telefon2GlownyTextView.Text == "")
                {
                    telefonNaglowekGlownyTextView.Visibility = ViewStates.Gone;
                }

                nipGlownyTextView.Text = kntKarta.Knt_nip;
                if(nipGlownyTextView.Text == "")
                {
                    nipGlownyTextView.Visibility = ViewStates.Gone;
                    nipNaglowekGlownyTextView.Visibility = ViewStates.Gone;
                }

                emailGlownyTextView.Text = kntKarta.Knt_email;
                if(emailGlownyTextView.Text == "")
                {
                    emailGlownyTextView.Visibility = ViewStates.Gone;
                    emailNaglowekGlownyTextView.Visibility = ViewStates.Gone;
                }

            }
        }
        private void ustawKontrahentaDocelowego()
        {
            KntAdresyTable kntAdres = null;
            DBRepository dbr = new DBRepository();

            try
            {
                kntAdres = dbr.kntAdresy_GetRecord(listaZlecenSzczegoly_Activity.szn_AdWNumer);
            }
            catch(Exception exc)
            {
                Toast.MakeText(kontekst, "B³¹d zakladkaKontrahentListaZlecenSerwisowychSzczegoly.ustawKontrahentaGlownego():\n" + exc.Message, ToastLength.Short);
            }

            if(kntAdres != null)
            {
                akronimDocelowyTextView.Text = "[" + kntAdres.Kna_Akronim + "]";
                if(akronimDocelowyTextView.Text == "[]")
                {
                    akronimDocelowyTextView.Visibility = ViewStates.Gone;
                }

                nazwa1DocelowyTextView.Text = kntAdres.Kna_nazwa1;
                if(nazwa1DocelowyTextView.Text == "")
                {
                    nazwa1DocelowyTextView.Visibility = ViewStates.Gone;
                }

                nazwa2DocelowyTextView.Text = kntAdres.Kna_nazwa2;
                if(nazwa2DocelowyTextView.Text == "")
                {
                    nazwa2DocelowyTextView.Visibility = ViewStates.Gone;
                }

                nazwa3DocelowyTextView.Text = kntAdres.Kna_nazwa3;
                if(nazwa3DocelowyTextView.Text == "")
                {
                    nazwa3DocelowyTextView.Visibility = ViewStates.Gone;
                }

                ulicaDocelowyTextView.Text = kntAdres.Kna_ulica;
                if(ulicaDocelowyTextView.Text == "")
                {
                    ulicaDocelowyTextView.Visibility = ViewStates.Gone;
                }

                kodPMiastoDocelowyTextView.Text = kntAdres.Kna_KodP + " " + kntAdres.Kna_miasto;
                if(kodPMiastoDocelowyTextView.Text == "")
                {
                    kodPMiastoDocelowyTextView.Visibility = ViewStates.Gone;
                }

                telefon1DocelowyTextView.Text = kntAdres.Kna_telefon1;
                if(telefon1DocelowyTextView.Text == "")
                {
                    telefon1DocelowyTextView.Visibility = ViewStates.Gone;
                }

                telefon2DocelowyTextView.Text = kntAdres.Kna_telefon2;
                if(telefon2DocelowyTextView.Text == "")
                {
                    telefon2DocelowyTextView.Visibility = ViewStates.Gone;
                }

                if(telefon1DocelowyTextView.Text == "" && telefon2DocelowyTextView.Text == "")
                {
                    telefonNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
                }

                nipDocelowyTextView.Text = kntAdres.Kna_nip;
                if(nipDocelowyTextView.Text == "")
                {
                    nipDocelowyTextView.Visibility = ViewStates.Gone;
                    nipNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
                }

                emailDocelowyTextView.Text = kntAdres.Kna_email;
                if(emailDocelowyTextView.Text == "")
                {
                    emailDocelowyTextView.Visibility = ViewStates.Gone;
                    emailNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
                }

            }
            else
            {
                ukryjDocelowego();
            }
        }

        private void ukryjDocelowego()
        {
            akronimDocelowyTextView.Visibility = ViewStates.Gone;
            nazwa1DocelowyTextView.Text = "{Brak kontrahenta docelowego}";
            nazwa2DocelowyTextView.Visibility = ViewStates.Gone;
            nazwa3DocelowyTextView.Visibility = ViewStates.Gone;
            ulicaDocelowyTextView.Text = "{Brak adresu kontrahenta docelowego}";
            kodPMiastoDocelowyTextView.Visibility = ViewStates.Gone;
            telefon1DocelowyTextView.Visibility = ViewStates.Gone;
            telefon2DocelowyTextView.Visibility = ViewStates.Gone;
            nipDocelowyTextView.Visibility = ViewStates.Gone;
            emailDocelowyTextView.Visibility = ViewStates.Gone;
            telefonDocelowyLinearLayout.Visibility = ViewStates.Gone;
            nipNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
            telefonNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
            emailNaglowekDocelowyTextView.Visibility = ViewStates.Gone;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Kontrahent";
        }
    }
}