using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Net;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

namespace AplikacjaSerwisowa
{
    [Activity(Label = "Ustawienia", Icon = "@drawable/ustawienia")]
    public class ustawienia_Activity : Activity
    {
        private Button synchronizacja_Button, wyslij_Button, synch2_Button;
        private AplikacjaSerwisowa.kwronski.WebService kwronskiService;
        private ProgressDialog progressDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.synchronizacjaOkno);

            // Create variable object

            synchronizacja_Button = FindViewById<Button>(Resource.Id.synchronizacjaSynchronizacjaButton);
            synchronizacja_Button.Click += delegate { synchronizacja(); };

            wyslij_Button = FindViewById<Button>(Resource.Id.SynchronizacjaWyslijDaneButton);
            wyslij_Button.Visibility = ViewStates.Gone;

            synch2_Button = FindViewById<Button>(Resource.Id.SynchronizacjaSynch2Button);
            synch2_Button.Visibility = ViewStates.Gone;

            // Create your application here

            kwronskiService = new AplikacjaSerwisowa.kwronski.WebService();
        }

        private void synchronizacja()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

            if(isOnline)
            {
                progressDialog = new ProgressDialog(this);
                progressDialog.SetTitle("Synchronizacja");
                progressDialog.SetMessage("Proszê czekaæ...");
                progressDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
                progressDialog.SetCancelable(false);
                progressDialog.Max = 1;
                progressDialog.Show();

                Thread th = new Thread(() => pobieranieOperatorowWebService());
                th.Start();
            }
            else
            {
                Toast.MakeText(this, "Brak dostêpu do internetu", ToastLength.Short).Show();
            }
        }

        private void pobieranieOperatorowWebService()
        {
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie operatorów..."));
            String OperatorzyString = kwronskiService.ZwrocListeOperatorow();

            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy operatorów..."));
            tworzenieBazyOperatorow(OperatorzyString);

            progressDialog.Dismiss();
        }

        private void tworzenieBazyOperatorow(string kntKartyString)
        {
            List<OperatorzyTable> records = JsonConvert.DeserializeObject<List<OperatorzyTable>>(kntKartyString);

            DBRepository dbr = new DBRepository();
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.OperatorzyTable_CreateTable();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                zapiszOperatorowWBazie(records, dbr);
            }
        }

        private void zapiszOperatorowWBazie(List<OperatorzyTable> operatorzyList, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie operatorów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = operatorzyList.Count);

            for(int i = 0; i < operatorzyList.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                OperatorzyTable uzytkownik = operatorzyList[i];
                dbr.OperatorzyTable_InsertRecord(uzytkownik);
            }
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
    }
}