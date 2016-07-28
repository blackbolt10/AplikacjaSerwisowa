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
    class zakladkaPodpisNoweZlecenie : Android.Support.V4.App.Fragment
    {
        private Button zapiszButton;
        List<SrwZlcCzynnosciTable> SrwZlcCzynnosciTableList;
        List<SrwZlcSkladnikiTable> SrwZlcSkladnikiTableList;
        List<TwrKartyTable> skladnikiList;
        SrwZlcNagTable srwZlcNag;
        private Context kontekst;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.noweZlecenieZakladkaPodpisLayout, container, false);

            kontekst = noweZlecenie_Activity.GetContext();

            zapiszButton = view.FindViewById<Button>(Resource.Id.noweZlecenieZakladkaPodpisZapiszButton);
            zapiszButton.Click += ZapiszButton_Click;
            
            skladnikiList = new List<TwrKartyTable>();
            srwZlcNag = null;

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
            srwZlcNag = zakladkaOgolneNoweZlecenie.pobierzNaglowek();

            return true;
        }

        private Boolean pobierzKontrahentow()
        {
            if(noweZlecenie_Activity.Knt_GIDNumer != -1 && noweZlecenie_Activity.Kna_GIDNumer != -1)
            {
                return true;
            }
            else if(noweZlecenie_Activity.Knt_GIDNumer == -1 && noweZlecenie_Activity.Kna_GIDNumer == -1)
            {
                Toast.MakeText(kontekst, "Kontrahent główny i docelowy nie zostali ustawieni! \nOperacja zapisu jest niemożliwa.", ToastLength.Short).Show();
            }
            else if(noweZlecenie_Activity.Knt_GIDNumer == -1 && noweZlecenie_Activity.Kna_GIDNumer != -1)
            {
                Toast.MakeText(kontekst, "Kontrahent główny nie został ustawieny! \nOperacja zapisu jest niemożliwa.", ToastLength.Short).Show();
            }
            else if(noweZlecenie_Activity.Knt_GIDNumer != -1 && noweZlecenie_Activity.Kna_GIDNumer == -1)
            {
                Toast.MakeText(kontekst, "Kontrahent docelowy nie został ustawieny! \nOperacja zapisu jest niemożliwa.", ToastLength.Short).Show();
            }
            return false;
        }

        private Boolean pobierzCzynnosci()
        {
            List<TwrKartyTable> czynnosciList = zakladkaCzynnosciNoweZlecenie.pobierzListeCzynnosci();
            SrwZlcCzynnosciTableList = new List<SrwZlcCzynnosciTable>();

            if(czynnosciList != null)
            {
                for(int i = 0; i < czynnosciList.Count; i++)
                {
                    SrwZlcCzynnosciTable srwZlcCzynnosc = new SrwZlcCzynnosciTable();
                    srwZlcCzynnosc.szc_sznId = srwZlcNag.SZN_Id;
                    srwZlcCzynnosc.szc_Pozycja = i;
                    srwZlcCzynnosc.szc_TwrNumer = czynnosciList[i].Twr_GIDNumer;
                    srwZlcCzynnosc.szc_TwrTyp = czynnosciList[i].Twr_GIDTyp;
                    srwZlcCzynnosc.szc_TwrNazwa = czynnosciList[i].Twr_Nazwa;
                    srwZlcCzynnosc.szc_Ilosc = czynnosciList[i].Ilosc;
                    srwZlcCzynnosc.Twr_Kod = czynnosciList[i].Twr_Kod;

                    SrwZlcCzynnosciTableList.Add(srwZlcCzynnosc);
                }
            }

            return true;
        }

        private Boolean pobierzSkladniki()
        {
            skladnikiList = zakladkaSkladnikiNoweZlecenie.pobierzListSkladnikow();
            SrwZlcSkladnikiTableList = new List<SrwZlcSkladnikiTable>();

            if(skladnikiList != null)
            {
                for(int i = 0; i < skladnikiList.Count; i++)
                {
                    SrwZlcSkladnikiTable srwZlcSkladnik = new SrwZlcSkladnikiTable();
                    srwZlcSkladnik.szs_sznId = srwZlcNag.SZN_Id;
                    srwZlcSkladnik.szs_Pozycja = i;
                    srwZlcSkladnik.szs_TwrNumer = skladnikiList[i].Twr_GIDNumer;
                    srwZlcSkladnik.szs_TwrTyp = skladnikiList[i].Twr_GIDTyp;
                    srwZlcSkladnik.szs_TwrNazwa = skladnikiList[i].Twr_Nazwa;
                    srwZlcSkladnik.szs_Ilosc = skladnikiList[i].Ilosc;
                    srwZlcSkladnik.Twr_Kod = skladnikiList[i].Twr_Kod;

                    SrwZlcSkladnikiTableList.Add(srwZlcSkladnik);
                }
            }

            return true;
        }

        private void wygenerujZlecenie()
        {
            DBRepository db = new DBRepository();

            if(srwZlcNag != null)
            {
                uzupelnijDaneKontrahenta();

                db.SrwZlcNag_InsertRecord(srwZlcNag);
            }

            if(SrwZlcCzynnosciTableList != null)
            {
                for(int i = 0; i < SrwZlcCzynnosciTableList.Count; i++)
                {
                    db.SrwZlcCzynnosci_InsertRecord(SrwZlcCzynnosciTableList[i]);
                }
            }

            if(SrwZlcSkladnikiTableList != null)
            {
                for(int i = 0; i < SrwZlcSkladnikiTableList.Count; i++)
                {
                    db.SrwZlcSkladniki_InsertRecord(SrwZlcSkladnikiTableList[i]);
                }
            }
        }

        private void uzupelnijDaneKontrahenta()
        {
            DBRepository db = new DBRepository();

            KntKartyTable kntKarty = new KntKartyTable();
            KntAdresyTable knaKarty= new KntAdresyTable();

            kntKarty = db.kntKarty_GetRecord(noweZlecenie_Activity.Knt_GIDNumer.ToString());
            knaKarty = db.kntAdresy_GetRecord(noweZlecenie_Activity.Kna_GIDNumer.ToString());

            srwZlcNag.SZN_KnANumer = knaKarty.Kna_GIDNumer;
            srwZlcNag.SZN_KnATyp = knaKarty.Kna_GIDTyp;
            
            srwZlcNag.SZN_KntNumer = knaKarty.Kna_KntNumer;
            srwZlcNag.SZN_KntTyp = knaKarty.Kna_GIDTyp;

            srwZlcNag.SZN_KnDNumer = knaKarty.Kna_KntNumer;
            srwZlcNag.SZN_KnDTyp = knaKarty.Kna_GIDTyp;

            srwZlcNag.SZN_AdWNumer = knaKarty.Kna_GIDNumer;
            srwZlcNag.SZN_AdWTyp = knaKarty.Kna_GIDTyp;
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Podpis";
        }
    }
}