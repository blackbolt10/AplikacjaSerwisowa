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
    [Activity(Label = "noweZlecenieDodajCzynnSkladIlosc_Activit")]
    public class noweZlecenieDodajCzynnSkladIlosc_Activit : Activity
    {
        private EditText iloscEditText;
        private TextView akronimTextView, nazwaTextView, jmTextView;
        private Button akceptujButton;
        private string czynnosc;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.noweZlecenieZakladkaCzynnSkladIlosc);

            // Create your application here

            iloscEditText = FindViewById<EditText>(Resource.Id.noweZlecenieZakCzynnSkladIloscIloscEditText);
            akronimTextView = FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladIloscAkronimTextView);
            nazwaTextView = FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladIloscNazwaTextView);
            jmTextView = FindViewById<TextView>(Resource.Id.noweZlecenieZakCzynnSkladIloscJmTextView);
            akceptujButton = FindViewById<Button>(Resource.Id.noweZlecenieZakCzynnSkladIloscAkceptujButton);
            akceptujButton.Click += AkceptujButton_Click;

            akronimTextView.Text = Intent.GetStringExtra("akronim") ?? "błąd wczytywania";
            nazwaTextView.Text = Intent.GetStringExtra("nazwa") ?? "błąd wczytywania";
            jmTextView.Text = Intent.GetStringExtra("jm") ?? "B.W.";
            czynnosc = Intent.GetStringExtra("czynnosc") ?? "1";

            iloscEditText.Text = "";
        }

        private void AkceptujButton_Click(object sender, EventArgs e)
        {
            if(sprawdzPoprawnosc())
            {
                double wartosc = Convert.ToDouble(iloscEditText.Text);

                if(czynnosc == "1")
                {
                    zakladkaCzynnosciNoweZlecenie.ustawIlosc(wartosc);
                }
                else
                {
                    zakladkaSkladnikiNoweZlecenie.ustawIlosc(wartosc);
                }
                this.Finish();
            }
            else
            {
                messagebox("Wprowadzona wartość jest nie poprawna.", "Błąd", 0);
            }
        }

        private bool sprawdzPoprawnosc()
        {
            Boolean poprawny = true;
            int liczbaKropek = 0;
            String tekst = "";

            if(iloscEditText.Text != "")
            {
                for(int i = 0; i < iloscEditText.Text.Length; i++)
                {
                    if(iloscEditText.Text[i] == '.')
                    {
                        tekst += ',';
                    }
                    else
                    {
                        tekst += iloscEditText.Text[i];
                    }

                    if(iloscEditText.Text[i] == ',')
                    {
                        liczbaKropek++;
                    }

                    if(liczbaKropek > 1)
                    {
                        poprawny = false;
                        break;
                    }
                }
                iloscEditText.Text = tekst;
            }
            else
            {
                iloscEditText.Text = "0";
            }

            return poprawny;
        }
        private void messagebox(String tekst, String tytul = "", Int32 icon = 1)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

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
    }
}