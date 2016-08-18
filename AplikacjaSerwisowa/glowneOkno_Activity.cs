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
    [Activity(Label = "Aplikacja Serwisowa", Icon = "@drawable/icon")]
    public class glowneOkno_Activity : Activity
    {
        Button noweZlecenie_Button, listaZlecen_Button, kontrahenci_Button, magazyn_Button;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.glowneOkno);

            // Get our button from the layout resource and attach an event to it

            noweZlecenie_Button = FindViewById<Button>(Resource.Id.noweZlecenie_Button);
            noweZlecenie_Button.Click += delegate { noweZlecenie_Button_Click(); };

            listaZlecen_Button = FindViewById<Button>(Resource.Id.listaZlecen_Button);
            listaZlecen_Button.Click += delegate { listaZlecen_Button_Click(); };

            kontrahenci_Button = FindViewById<Button>(Resource.Id.kontrahenci_Button);
            kontrahenci_Button.Click += delegate { kontrahenci_Button_Click(); };

            magazyn_Button = FindViewById<Button>(Resource.Id.magazyn_Button);
            magazyn_Button.Click += delegate { magazyn_Button_Click(); };
        }
        private void messagebox(String tekst, String tytul = "", Int32 icon = 1)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            if (icon == 0)
            {
                alert.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);
            }

            alert.SetTitle(tytul);
            alert.SetMessage(tekst);
            alert.SetPositiveButton("OK", (senderAlert, args) => { });

            Dialog dialog = alert.Create();
            dialog.Show();
        }
        public override void OnBackPressed()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Potwierdzenie zamykania");
            alert.SetMessage("Czy zamkn¹æ aplikacjê?");
            alert.SetPositiveButton("Tak", (senderAlert, args) => { this.FinishAffinity(); });
            alert.SetNegativeButton("Nie", (senderAlert, args) => { });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void noweZlecenie_Button_Click()
        {
            StartActivity(typeof(noweZlecenie_Activity));
        }
        private void listaZlecen_Button_Click()
        {
            StartActivity(typeof(listaZlecen_Activity));
        }
        private void kontrahenci_Button_Click()
        {
            StartActivity(typeof(kontrahenci_Activity));
        }
        private void magazyn_Button_Click()
        {
            StartActivity(typeof(magazyn_Activity));
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.glowneOkno_Menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.synchronizacja_glowneOknoMenu:
                    StartActivity(typeof(synchronizacja_Activity));
                    return true;
                case Resource.Id.wyloguj_glowneOknoMenu:
                    Finish();
                    return true;
                case Resource.Id.zamknij_glowneOknoMenu:
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    return true;
                default:
                    return false;
            }

        }
    }
}