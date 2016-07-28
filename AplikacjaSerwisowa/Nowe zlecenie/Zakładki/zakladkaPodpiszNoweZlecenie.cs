using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace AplikacjaSerwisowa
{
    class zakladkaPodpiszNoweZlecenie : Android.Support.V4.App.Fragment
    {
        private Button zapiszButton;
        private Int32 KNT_GIDNumer, KNA_GIDNumer;
        List<TwrKartyTable> czynnosciList;
        List<TwrKartyTable> skladnikiList;
        private Context kontekst;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.noweZlecenieZakladkaPodpisLayout, container, false);

            kontekst = noweZlecenie_Activity.GetContext();

            zapiszButton = view.FindViewById<Button>(Resource.Id.noweZlecenieZakladkaPodpisZapiszButton);
            zapiszButton.Click += ZapiszButton_Click;

            KNT_GIDNumer = -1;
            KNA_GIDNumer = -1;
            czynnosciList = new List<TwrKartyTable>();
            skladnikiList = new List<TwrKartyTable>();

            return view;
        }

        private void ZapiszButton_Click(object sender, EventArgs e)
        {
            Boolean kontrahenci = false;
            Boolean czynnosci = false;
            Boolean skladniki = false;
            Boolean naglowki =  pobierzInformacjeNaglowkowe();

            if(naglowki)
            {
                kontrahenci = pobierzKontrahentow();

                if(kontrahenci)
                {
                    czynnosci = pobierzCzynnosci();
                    if(czynnosci)
                    {
                        skladniki = pobierzSkladniki();

                        if(skladniki)
                        {
                            wygenerujZlecenie();
                        }
                    }
                }
            }
        }

        private Boolean pobierzInformacjeNaglowkowe()
        {
            return true;
        }

        private Boolean pobierzKontrahentow()
        {
            List<Int32> listaKontrahentow =  zakladkaKontrahentNoweZlecenie.pobierzKontrahentow();

            if(listaKontrahentow.Count>1)
            {
                KNT_GIDNumer = listaKontrahentow[0];
                KNA_GIDNumer = listaKontrahentow[1];

                return true;
            }
            else if(listaKontrahentow.Count == 1)
            {
                switch(listaKontrahentow[0])
                {
                    case 0:
                        Toast.MakeText(kontekst, "Kontrahent główny i docelowy nie zostali ustawieni! \nOperacja zapisu jest niemożliwa.", ToastLength.Short).Show();
                    break;

                    case 1:
                        Toast.MakeText(kontekst, "Kontrahent główny nie został ustawieny! \nOperacja zapisu jest niemożliwa.", ToastLength.Short).Show();
                    break;

                    case 2:
                        Toast.MakeText(kontekst, "Kontrahent docelowy nie został ustawieny! \nOperacja zapisu jest niemożliwa.", ToastLength.Short).Show();
                    break;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        private Boolean pobierzCzynnosci()
        {
            czynnosciList = zakladkaCzynnosciNoweZlecenie.pobierzListeCzynnosci();
            /*
            if(czynnosciList.Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
            */

            return true;
        }

        private Boolean pobierzSkladniki()
        {
            skladnikiList = zakladkaSkladnikiNoweZlecenie.pobierzListSkladnikow();
            /*
            if(skladnikiList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            */

            return true;
        }

        private void wygenerujZlecenie()
        {
            throw new NotImplementedException();
        }
    }
}