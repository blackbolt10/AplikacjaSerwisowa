using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using SQLite;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

using Java.IO;

using SignaturePad;

namespace AplikacjaSerwisowa
{
    class zakladkaPodpisNoweZlecenie : Android.Support.V4.App.Fragment
    {
        private Button zapiszButton;
        private List<SrwZlcCzynnosci> SrwZlcCzynnosciList;
        private List<SrwZlcSkladniki> SrwZlcSkladnikiList;
        private List<TwrKartyTable> skladnikiList;
        private SrwZlcNag srwZlcNag;
        private SignaturePadView signature;
        private Context kontekst;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.noweZlecenieZakladkaPodpisLayout, container, false);

            LinearLayout linear = view.FindViewById<LinearLayout>(Resource.Id.noweZlecenieZakladkaPodpisLinearLayout);

            signature = new SignaturePadView(this.Activity)
            {
              //  LineWidth = 3f
            };

            linear.AddView(signature, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 450));

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
            DBRepository dbr = new DBRepository();

            srwZlcNag = zakladkaOgolneNoweZlecenie.pobierzNaglowek();
            srwZlcNag.SZN_Id = dbr.SrwZlcNagGenerujNoweID(this.Activity);
            srwZlcNag.SZN_Synchronizacja = 1;

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
            SrwZlcCzynnosciList = new List<SrwZlcCzynnosci>();

            if(czynnosciList != null)
            {
                for(int i = 0; i < czynnosciList.Count; i++)
                {
                    SrwZlcCzynnosci srwZlcCzynnosc = new SrwZlcCzynnosci();
                    srwZlcCzynnosc.SZC_SZNId = srwZlcNag.SZN_Id;
                    srwZlcCzynnosc.SZC_Pozycja = i + 1;
                    srwZlcCzynnosc.SZC_TwrNumer = czynnosciList[i].Twr_GIDNumer;
                    srwZlcCzynnosc.SZC_TwrTyp = czynnosciList[i].Twr_GIDTyp;
                    srwZlcCzynnosc.SZC_TwrNazwa = czynnosciList[i].Twr_Nazwa;
                    srwZlcCzynnosc.SZC_Ilosc = czynnosciList[i].Ilosc;
                    srwZlcCzynnosc.SZC_Synchronizacja = 1;

                    SrwZlcCzynnosciList.Add(srwZlcCzynnosc);
                }
            }

            return true;
        }

        private Boolean pobierzSkladniki()
        {
            skladnikiList = zakladkaSkladnikiNoweZlecenie.pobierzListSkladnikow();
            SrwZlcSkladnikiList = new List<SrwZlcSkladniki>();

            if(skladnikiList != null)
            {
                for(int i = 0; i < skladnikiList.Count; i++)
                {
                    SrwZlcSkladniki srwZlcSkladnik = new SrwZlcSkladniki();
                    srwZlcSkladnik.SZS_SZNId = srwZlcNag.SZN_Id;
                    srwZlcSkladnik.SZS_Pozycja = i + 1;
                    srwZlcSkladnik.SZS_TwrNumer = skladnikiList[i].Twr_GIDNumer;
                    srwZlcSkladnik.SZS_TwrTyp = skladnikiList[i].Twr_GIDTyp;
                    srwZlcSkladnik.SZS_TwrNazwa = skladnikiList[i].Twr_Nazwa;
                    srwZlcSkladnik.SZS_Ilosc = skladnikiList[i].Ilosc;
                    srwZlcSkladnik.SZS_Synchronizacja = 1;

                    SrwZlcSkladnikiList.Add(srwZlcSkladnik);
                }
            }

            return true;
        }

        private void wygenerujZlecenie()
        {
            try
            {
                DBRepository db = new DBRepository();

                if(srwZlcNag != null)
                {
                    uzupelnijDaneKontrahenta();

                    db.SrwZlcNag_InsertRecord(srwZlcNag);

                    zapiszPodpis();

                    if(SrwZlcCzynnosciList != null)
                    {
                        DBRepository dbr = new DBRepository();
                        for(int i = 0; i < SrwZlcCzynnosciList.Count; i++)
                        {
                            dbr.SrwZlcCzynnosci_InsertRecord(SrwZlcCzynnosciList[i]);
                        }
                    }

                    if(SrwZlcSkladnikiList != null)
                    {
                        DBRepository dbr = new DBRepository();
                        for(int i = 0; i < SrwZlcSkladnikiList.Count; i++)
                        {
                            dbr.SrwZlcSkladniki_InsertRecord(SrwZlcSkladnikiList[i]);
                        }
                    }
                }

                this.Activity.RunOnUiThread(() => Toast.MakeText(kontekst, "Zlecenie zostało stworzone.", ToastLength.Short));
                //Thread.Sleep(2000);
                this.Activity.Finish();
            }
            catch(Exception exc)
            {
                messagebox("Wystąpił błąd tworzenia nowego zlecenia:\n" + exc.Message, "Błąd", 0);
            }
        }

        private void zapiszPodpis()
        {
            Bitmap podpisBitmap = signature.GetImage();

            SrwZlcPodpisTable szp = new SrwZlcPodpisTable();
            szp.SZN_Id = srwZlcNag.SZN_Id;
            szp.Podpis = BitMapToString(podpisBitmap);
            szp.SZP_Synchronizacja = 1;

            DBRepository db = new DBRepository();
            db.SrwZlcPodpis_InsertRecord(szp);
        }

        public String BitMapToString(Android.Graphics.Bitmap bitmap)
        {
            string str = "";
            try
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byte[] byteArray = stream.ToArray();

                str = byteArray[0].ToString();
                for(int i = 1; i < byteArray.Length; i++)
                {
                    str += "," + byteArray[i].ToString();
                }
            }
            catch(Exception) {}

            return str;
        }

        private void messagebox(String tekst, String tytul = "", Int32 icon = 1)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(kontekst);

            if(icon == 0)
            {
                alert.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
            }

            alert.SetTitle(tytul);
            alert.SetMessage(tekst);
            alert.SetPositiveButton("OK", (senderAlert, args) => { });

            Dialog dialog = alert.Create();
            dialog.Show();
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
        }

        public override string ToString() //Called on line 156 in SlidingTabScrollView
        {
            return "Podpis";
        }
    }
}