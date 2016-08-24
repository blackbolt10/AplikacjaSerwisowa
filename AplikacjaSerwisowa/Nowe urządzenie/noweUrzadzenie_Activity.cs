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
    [Activity(Label = "Nowe urz¹dzenie")]
    public class noweUrzadzenie_Activity : Activity
    {
        private EditText kodEditText, nazwaEditText, opisEditText;
        private static EditText nazwKontrahentaEditText, akronimKontrahentaEditText;
        private Button dodajButton, zapiszButton;
        private static int KNT_GIDNumer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.noweUrzadzenieLayout);

            dodajButton = FindViewById<Button>(Resource.Id.NoweUrzadzenieDodajButton);
            dodajButton.Click += DodajButton_Click;

            zapiszButton = FindViewById<Button>(Resource.Id.NoweUrzadzenieZapiszButton);
            zapiszButton.Click += ZapiszButton_Click;

            kodEditText = FindViewById<EditText>(Resource.Id.NoweUrzadzenieKodEditText);
            nazwaEditText = FindViewById<EditText>(Resource.Id.NoweUrzadzenieNazwaEditText);
            opisEditText = FindViewById<EditText>(Resource.Id.NoweUrzadzenieOpisEditText);
            nazwKontrahentaEditText = FindViewById<EditText>(Resource.Id.NoweUrzadzenieNazwaKontrahentaEditText);
            akronimKontrahentaEditText = FindViewById<EditText>(Resource.Id.NoweUrzadzenieAkronimKontrahentaEditText);


            // Create your application here

            KNT_GIDNumer = -1;
            KNT_GIDNumer = noweZlecenie_Activity.Knt_GIDNumer;

            if(KNT_GIDNumer!=-1)
            {
                pobierzKontrahenta();
            }
        }

        private void pobierzKontrahenta()
        {
            DBRepository dbr = new DBRepository();
            KntKartyTable kntKarta =  dbr.kntKarty_GetRecord(KNT_GIDNumer.ToString());

            aktualizujKontrahenta(kntKarta.Knt_nazwa1, kntKarta.Knt_Akronim, kntKarta.Knt_GIDNumer);
            dodajButton.Text = "Zmieñ";
        }

        private void ZapiszButton_Click(object sender, EventArgs e)
        {
            if(czyWypenionePola())
            {
                DBRepository dbr = new DBRepository();

                SrwUrzadzenia urzadzenie = new SrwUrzadzenia();
                urzadzenie.SrU_Id = dbr.SrwUrzadzenia_GenerujNoweID(this);
                urzadzenie.Sru_Kod = kodEditText.Text;
                urzadzenie.Sru_Nazwa = nazwaEditText.Text;
                urzadzenie.SrU_Opis = opisEditText.Text;
                urzadzenie.SUW_WlaNumer = KNT_GIDNumer;

                dbr.SrwUrzadzenia_InsertRecord(urzadzenie);

                noweZlecenie_Activity.urzadzeniaList.Add(urzadzenie);
                zakladkaUrzadzeniaNoweZlecenie.aktualizujListeUrzadzen();

                Intent resultIntent = new Intent();
                resultIntent.PutExtra("koniec", true);
                this.SetResult(Result.Ok, resultIntent);
                this.Finish();
            }
        }

        private bool czyWypenionePola()
        {
            if(kodEditText.Text == "")
            {
                Toast.MakeText(this, "Pole \"Kod\" nie mo¿e byæ puste", ToastLength.Short).Show();
                return false;
            }

            if(nazwaEditText.Text == "")
            {
                Toast.MakeText(this, "Pole \"Nazwa\" nie mo¿e byæ puste", ToastLength.Short).Show();
                return false;
            }

            if(KNT_GIDNumer == -1)
            {
                Toast.MakeText(this, "Nie wybrano kontrahenta", ToastLength.Short).Show();
                return false;
            }

            return true;
        }

        private void DodajButton_Click(object sender, EventArgs e)
        {
            Intent noweZlecenieDodajKontrahentGlowny = new Intent(this, typeof(noweZlecenieDodajKontrahenta_Activity));
            noweZlecenieDodajKontrahentGlowny.PutExtra("glowny", "3");
            StartActivity(noweZlecenieDodajKontrahentGlowny);
        }

        public static void aktualizujKontrahenta(String nazwa, String akronim, int KnT_GidNumer)
        {
            KNT_GIDNumer = KnT_GidNumer;
            nazwKontrahentaEditText.Text = nazwa;
            akronimKontrahentaEditText.Text = akronim;
        }
    }
}