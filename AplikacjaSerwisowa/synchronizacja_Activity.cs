using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    [Activity(Label = "Synchronizacja", Icon = "@drawable/synchronizacja")]
    public class synchronizacja_Activity : Activity
    {
        private Button synchronizacja_Button, wyslij_Button, synch2_Button, format_Button;
        private ProgressDialog progressDialog, progressDialogWysylanie;
        private Dialog dialog;
        private Thread wysylanieThread;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.synchronizacjaOkno);

            synchronizacja_Button = FindViewById<Button>(Resource.Id.synchronizacjaSynchronizacjaButton);
            synchronizacja_Button.Click += delegate
            { synchronizacja(); };

            wyslij_Button = FindViewById<Button>(Resource.Id.SynchronizacjaWyslijDaneButton);
            wyslij_Button.Click += delegate
            { synchronizacjaWysylanie(); };

            synch2_Button = FindViewById<Button>(Resource.Id.SynchronizacjaSynch2Button);
            synch2_Button.Click += Synch2_Button_Click;

            format_Button = FindViewById<Button>(Resource.Id.FormatSynch2Button);
            format_Button.Click += Format_Button_Click;
            // Create your application here
        }

        private void Format_Button_Click(object sender, EventArgs e)
        {
            DBRepository dbr = new DBRepository(this);
            dbr.dropAllTables();
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

            RunOnUiThread(() => dialog = alert.Create());
            RunOnUiThread(() => dialog.Show());
        }

        private void synchronizacja()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

            if(isOnline)
            {
                sprawdzNoweZlecenia();
            }
            else
            {
                Toast.MakeText(this, "Brak dostêpu do internetu", ToastLength.Short).Show();
            }
        }

        private void sprawdzNoweZlecenia()
        {
            DBRepository dbr = new DBRepository(this);
            List<SrwZlcNag> srwZlcNagList = dbr.SrwZlcNagSynchronizacja(1);
            if(srwZlcNagList.Count > 0)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);

                alert.SetTitle("Informacja");
                alert.SetMessage("W pamiêci urz¹dzenia znajduj¹ siê nowe nie zsynchronizowane zlecenia serwisowe. Synchronizacja spowoduje ich usuniêcie. Czy chcesz wczeœniej wys³aæ dane?");
                alert.SetPositiveButton("Tak", (senderAlert, args) =>
                    {
                        wyslijDane();
                        rozpocznijPobieranie();
                    });
                alert.SetNegativeButton("Nie", (senderAlert, args) =>
                    {
                        rozpocznijPobieranie();
                    });

                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
            {
                rozpocznijPobieranie();
            }
        }

        private void rozpocznijPobieranie()
        {
            progressDialog = new ProgressDialog(this);
            progressDialog.SetTitle("Pobieranie");
            progressDialog.SetMessage("Proszê czekaæ...");
            progressDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
            progressDialog.SetCancelable(false);
            progressDialog.Max = 1;
            progressDialog.Show();

            Thread th = new Thread(() => pobieranieDanychWebService());
            th.Start();
        }

        private void pobieranieDanychWebService()
        {
            if(wysylanieThread != null)
            {
                if(wysylanieThread.IsAlive)
                {
                    wysylanieThread.Join();
                }
            }

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie nag³ówków kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 11);
            String kntKartyString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeKntKarty();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie adresów kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress++);
            String kntAdresyString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeKntAdresy();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie nag³ówków zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress++);
            String serwisoweZlecenniaNaglowkiString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeZlecenSerwisowychNaglowki();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie czynnosci zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress++);
            String srwZlcCzynnosciString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeZlecenSerwisowychCzynnosci();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie skladniki zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress++);
            String srwZlcSkladnikiString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeZlecenSerwisowychSkladniki();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie kart towarowych..."));
            RunOnUiThread(() => progressDialog.Progress++);
            String twrKartyString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeTwrKarty();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie podpisów..."));
            RunOnUiThread(() => progressDialog.Progress++);
            String podpisyString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListePodpisow();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress++);
            String urzadeniaString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeSrwUrzadzenia();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie definicji parametrów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress++);
            String SrwUrzParDefString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeSrwUrzParDef();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie definicji rodzajów parametrów..."));
            RunOnUiThread(() => progressDialog.Progress++);
            String SrwUrzRodzParString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeSrwUrzRodzPar();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie definicji rodzajów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress++);
            String SrwUrzRodzajeString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeSrwUrzRodzaje();

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie urz¹dzeñ zleceñ serwisowycj."));
            RunOnUiThread(() => progressDialog.Progress++);
            String SrwZlcUrzString = new AplikacjaSerwisowa.kwronski.WebService().ZwrocListeSrwZlcUrz();




            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 1/10"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy nag³ówków kontrahentów..."));
            tworzenieBazyKntKarty(kntKartyString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 2/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy adresów kontrahentów..."));
            tworzenieBazyKntAdresy(kntAdresyString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 3/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy nag³ówków zleceñ serwisowych..."));
            tworzenieBazySerwisoweZleceniaNaglowki(serwisoweZlecenniaNaglowkiString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 4/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy czynnosci zleceñ serwisowych..."));
            tworzenieBazySrwZlcCzynnosci(srwZlcCzynnosciString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 5/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy skladniki zleceñ serwisowych..."));
            tworzenieBazySrwZlcSkladniki(srwZlcSkladnikiString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 6/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy kart towarowych..."));
            tworzenieBazyTwrKarty(twrKartyString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 7/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy podpisów..."));
            tworzenieBazySrwZlcPodpisy(podpisyString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 8/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy urz¹dzeñ..."));
            tworzenieBazySrwUrzadzenia(urzadeniaString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 9/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy definicji parametrów urz¹dzeñ..."));
            tworzenieBazySrwUrzParDef(SrwUrzParDefString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 10/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy definicji rodzajów parametrów..."));
            tworzenieBazySrwUrzRodzPar(SrwUrzRodzParString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 11/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy definicji rodzajów urz¹dzeñ..."));
            tworzenieBazySrwUrzRodzaje(SrwUrzRodzajeString);

            RunOnUiThread(() => progressDialog.SetTitle("Zapisywanie 11/12"));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy urz¹dzeñ zleceñ serwisowych..."));
            tworzenieBazySrwZlcUrz(SrwZlcUrzString);

            RunOnUiThread(() => messagebox("Pobieranie zakoñczone", "Ukoñczono"));
            progressDialog.Dismiss();
        }

        private void tworzenieBazySrwZlcUrz(string srwZlcUrzString)
        {
            List<SrwZlcUrz> records = JsonConvert.DeserializeObject<List<SrwZlcUrz>>(srwZlcUrzString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.stworzSrwZlcUrz();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliySrwZlcUrz(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliySrwZlcUrz(List<SrwZlcUrz> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie urz¹dzeñ zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);
                dbr.SrwZlcUrz_InsertRecord(records[i]);
            }
        }

        private void tworzenieBazySrwUrzRodzPar(string SrwUrzRodzParString)
        {
            List<SrwUrzRodzPar> records = JsonConvert.DeserializeObject<List<SrwUrzRodzPar>>(SrwUrzRodzParString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.stworzSrwUrzRodzPar();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliySrwUrzRodzPar(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliySrwUrzRodzPar(List<SrwUrzRodzPar> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie definicji rodzajów parametrów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.SrwUrzRodzPar_InsertRecord(records[i]);
            }
        }

        private void tworzenieBazySrwUrzRodzaje(string SrwUrzRodzajeString)
        {
            List<SrwUrzRodzaje> records = JsonConvert.DeserializeObject<List<SrwUrzRodzaje>>(SrwUrzRodzajeString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.stworzSrwUrzRodzaje();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliySrwUrzRodzaje(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliySrwUrzRodzaje(List<SrwUrzRodzaje> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie definicji rodzajów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.SrwUrzRodzaje_InsertRecord(records[i]);
            }
        }

        private void tworzenieBazySrwUrzParDef(string srwUrzParDefString)
        {
            List<SrwUrzParDef> records = JsonConvert.DeserializeObject<List<SrwUrzParDef>>(srwUrzParDefString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.stworzSrwUrzParDef();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliySrwUrzParDef(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliySrwUrzParDef(List<SrwUrzParDef> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie definicji parametrów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.SrwUrzParDef_InsertRecord(records[i]);
            }
        }

        private void tworzenieBazySrwUrzadzenia(string urzadeniaString)
        {
            List<SrwUrzadzenia> records = JsonConvert.DeserializeObject<List<SrwUrzadzenia>>(urzadeniaString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.stworzSrwUrzadzenia();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliySrwUrzadzenia(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliySrwUrzadzenia(List<SrwUrzadzenia> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.SrwUrzadzenia_InsertRecord(records[i]);
            }
        }

        private void tworzenieBazyKntKarty(String kntKartyString)
        {
            List<KntKartyTable> records = JsonConvert.DeserializeObject<List<KntKartyTable>>(kntKartyString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.stworzKntKartyTabele();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliyKntKarty(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliyKntKarty(List<KntKartyTable> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie nag³ówków kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.kntKarty_InsertRecord(records[i]);
            }
        }

        private void tworzenieBazyKntAdresy(String kntAdresyString)
        {
            List<KntAdresyTable> records = JsonConvert.DeserializeObject<List<KntAdresyTable>>(kntAdresyString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.stworzKntAdresyTabele();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliyKnAdresy(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliyKnAdresy(List<KntAdresyTable> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie adresów kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.kntAdresy_InsertRecord(records[i]);
            }
        }

        private void tworzenieBazySrwZlcCzynnosci(String srwZlcCzynnosciString)
        {
            List<SrwZlcCzynnosci> records = JsonConvert.DeserializeObject<List<SrwZlcCzynnosci>>(srwZlcCzynnosciString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.stworzSrwZlcCzynnosci();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliSrwZlcCzynnosci(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliSrwZlcCzynnosci(List<SrwZlcCzynnosci> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie czynnoœci zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.SrwZlcCzynnosci_InsertRecord(records[i]);
            }
        }

        private void tworzenieBazySrwZlcPodpisy(string podpisyString)
        {
            List<SrwZlcPodpisTable> records = JsonConvert.DeserializeObject<List<SrwZlcPodpisTable>>(podpisyString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.stworzSrwZlcPodpisTable();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliSrwZlcPodpisy(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliSrwZlcPodpisy(List<SrwZlcPodpisTable> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie podpisów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.SrwZlcPodpis_InsertRecord(records[i]);
            }
        }

        private void tworzenieBazySrwZlcSkladniki(String srwZlcCzynnosciString)
        {
            List<SrwZlcSkladniki> records = JsonConvert.DeserializeObject<List<SrwZlcSkladniki>>(srwZlcCzynnosciString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.stworzSrwZlcSkladniki();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliSrwZlcSkladniki(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliSrwZlcSkladniki(List<SrwZlcSkladniki> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie sk³adniki zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.SrwZlcSkladniki_InsertRecord(records[i]);
            }
        }

        private void tworzenieBazySerwisoweZleceniaNaglowki(String serwisoweZlecenniaNaglowkiString)
        {
            List<SrwZlcNag> records = JsonConvert.DeserializeObject<List<SrwZlcNag>>(serwisoweZlecenniaNaglowkiString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.stworzSrwZlcNag();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliSerwisoweZleceniaNaglowki(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliSerwisoweZleceniaNaglowki(List<SrwZlcNag> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie nag³ówków zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.SrwZlcNag_InsertRecord(records[i]);
            }
        }
        private void tworzenieBazyTwrKarty(String twrKartyString)
        {
            List<TwrKartyTable> records = JsonConvert.DeserializeObject<List<TwrKartyTable>>(twrKartyString);

            DBRepository dbr = new DBRepository(this);
            String result = dbr.stworzTwrKartyTable();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliTwrKarty(records, dbr);
            }
        }

        private void wprowadzWpisyDoTabeliTwrKarty(List<TwrKartyTable> records, DBRepository dbr)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie kart towarowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = records.Count);

            for(int i = 0; i < records.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                dbr.TwrKartyTable_InsertRecord(records[i]);
            }
        }

        private void synchronizacjaWysylanie()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

            if(isOnline)
            {
                wyslijDane();
            }
            else
            {
                Toast.MakeText(this, "Brak dostêpu do internetu", ToastLength.Short).Show();
            }
        }


        private void wyslijDane()
        {
            progressDialogWysylanie = new ProgressDialog(this);
            progressDialogWysylanie.SetTitle("Wysy³anie");
            progressDialogWysylanie.SetMessage("Proszê czekaæ...");
            progressDialogWysylanie.SetProgressStyle(ProgressDialogStyle.Horizontal);
            progressDialogWysylanie.SetCancelable(false);
            progressDialogWysylanie.Max = 1;
            progressDialogWysylanie.Show();

            wysylanieThread = new Thread(() => wysylanieDanychWebService());
            wysylanieThread.Start();
        }

        private void wysylanieDanychWebService()
        {
            List<int> wyslaneNagList = wyslijSrwZlcNag();
            oznaczWyslaneSrwZlcNag(wyslaneNagList);

            List<int> wyslaneCzynnosciList = wyslijSrwZlcCzynniki(wyslaneNagList);
            oznaczWyslaneSrwZlcCzynnosci(wyslaneCzynnosciList);

            List<int> wyslaneSkladnikiList = wyslijSrwZlcSkladniki(wyslaneNagList);
            oznaczWyslaneSrwZlcSkladniki(wyslaneSkladnikiList);

            progressDialogWysylanie.Dismiss();
        }

        private List<int> wyslijSrwZlcNag()
        {
            RunOnUiThread(() => progressDialogWysylanie.SetMessage("Przygotowywanie danych zleceñ nag³ówkowych."));

            List<int> wyslaneNagList = new List<int>();
            DBRepository db = new DBRepository(this);

            List<SrwZlcNag> srwZlcNagList = db.SrwZlcNagSynchronizacja(1);

            if(srwZlcNagList.Count > 0)
            {
                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Wysy³anie danych zleceñ nag³ówkowych."));
                String jsonOut = JsonConvert.SerializeObject(srwZlcNagList);

                db.SrwZlcNag_OznaczWyslane(srwZlcNagList, 2);

                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Oczekiwanie na potwierdzenie odebrania danych przez serwer."));
                String result = new AplikacjaSerwisowa.kwronski.WebService().synchronizujSrwZlcNag(jsonOut);

                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Przetwarzanie odwpowiedz serwera."));
                if(result != "[]")
                {
                    result = result.Replace('[', ' ');
                    result = result.Replace(']', ' ');

                    String[] resultArray = result.Split(',');
                    for(int i = 0; i < resultArray.Length; i++)
                    {
                        wyslaneNagList.Add(Convert.ToInt32(resultArray[i]));
                    }
                }
            }
            return wyslaneNagList;
        }

        private void oznaczWyslaneSrwZlcNag(List<int> wyslaneNagList)
        {
            RunOnUiThread(() => progressDialogWysylanie.SetMessage("Oznaczanie wys³anych nag³ówków"));

            DBRepository db = new DBRepository(this);
            db.SrwZlcNag_OznaczWyslane(wyslaneNagList, 3);
        }

        private List<int> wyslijSrwZlcCzynniki(List<int> wyslaneNagList)
        {
            RunOnUiThread(() => progressDialogWysylanie.SetMessage("Przygotowywanie czynnoœci zleceñ serwisowych."));

            DBRepository db = new DBRepository(this);
            List<int> wyslaneczynnosciList = new List<int>();

            List<SrwZlcCzynnosci> srwZlcCzynnosciList = db.SrwZlcCzynnosciSynchronizacja(1);

            if(srwZlcCzynnosciList.Count > 0)
            {
                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Wysy³anie czynnoœci zleceñ serwisowych."));
                String jsonOut = JsonConvert.SerializeObject(srwZlcCzynnosciList);

                db.SrwZlcCzynnosci_OznaczWyslane(srwZlcCzynnosciList, 2);

                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Oczekiwanie na potwierdzenie odebrania danych przez serwer."));
                String result = new AplikacjaSerwisowa.kwronski.WebService().synchronizujSrwZlcCzynnosci(jsonOut);

                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Przetwarzanie odwpowiedz serwera."));
                if(result != "[]")
                {
                    result = result.Replace('[', ' ');
                    result = result.Replace(']', ' ');

                    String[] resultArray = result.Split(',');
                    for(int i = 0; i < resultArray.Length; i++)
                    {
                        wyslaneczynnosciList.Add(Convert.ToInt32(resultArray[i]));
                    }
                }
            }
            return wyslaneczynnosciList;
        }

        private void oznaczWyslaneSrwZlcCzynnosci(List<int> wyslaneCzynnosciList)
        {
            RunOnUiThread(() => progressDialogWysylanie.SetMessage("Oznaczanie wys³anych czynnoœci"));

            DBRepository db = new DBRepository(this);
            db.SrwZlcCzynnosci_OznaczWyslane(wyslaneCzynnosciList, 3);
        }

        private List<int> wyslijSrwZlcSkladniki(List<int> wyslaneNagList)
        {
            RunOnUiThread(() => progressDialogWysylanie.SetMessage("Przygotowywanie sk³adników zleceñ serwisowych."));

            DBRepository db = new DBRepository(this);
            List<int> wyslaneSkladnikiList = new List<int>();

            List<SrwZlcSkladniki> srwZlcSkladnikiList = db.SrwZlcSkladnikiSynchronizacja(1);

            if(srwZlcSkladnikiList.Count > 0)
            {
                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Wysy³anie sk³adników zleceñ serwisowych."));
                String jsonOut = JsonConvert.SerializeObject(srwZlcSkladnikiList);

                db.SrwZlcSkladniki_OznaczWyslane(srwZlcSkladnikiList, 2);

                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Oczekiwanie na potwierdzenie odebrania danych przez serwer."));
                String result = new AplikacjaSerwisowa.kwronski.WebService().synchronizujSrwZlcSkladniki(jsonOut);

                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Przetwarzanie odwpowiedz serwera."));
                if(result != "[]")
                {
                    result = result.Replace('[', ' ');
                    result = result.Replace(']', ' ');

                    String[] resultArray = result.Split(',');
                    for(int i = 0; i < resultArray.Length; i++)
                    {
                        wyslaneSkladnikiList.Add(Convert.ToInt32(resultArray[i]));
                    }
                }
            }
            return wyslaneSkladnikiList;
        }
        private void oznaczWyslaneSrwZlcSkladniki(List<int> wyslaneSkladnikiList)
        {
            RunOnUiThread(() => progressDialogWysylanie.SetMessage("Oznaczanie wys³anych sk³adników"));

            DBRepository db = new DBRepository(this);
            db.SrwZlcSkladniki_OznaczWyslane(wyslaneSkladnikiList, 3);
        }

















        private void Synch2_Button_Click(object sender, EventArgs e)
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

            if(isOnline)
            {
                startSynch2();
            }
            else
            {
                Toast.MakeText(this, "Brak dostêpu do internetu", ToastLength.Short).Show();
            }
        }

        private void startSynch2()
        {
            progressDialog = new ProgressDialog(this);
            progressDialog.SetTitle("Pobieranie");
            progressDialog.SetMessage("Proszê czekaæ...");
            progressDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
            progressDialog.SetCancelable(false);
            progressDialog.Max = 1;
            progressDialog.Show();

            Thread th = new Thread(() => pobierzDaneWebSerwice());
            th.Start();
        }

        private void pobierzDaneWebSerwice()
        {
            int idOperatora = 0;
            int max = 12;

            tworzenieTabel(idOperatora, max);

            wysylanieDanych(idOperatora, max);

            pobierz_Kontrahentow(idOperatora);

            pobierz_Towary(idOperatora);

            pobierz_Zlecenia(idOperatora);

            pobierz_Urzadzenia(idOperatora);            

            progressDialog.Dismiss();
        }

        private void pobierz_Urzadzenia(int idOperatora)
        {
            int licznik = 1;
            int max = 10;

            RunOnUiThread(() => progressDialog.SetTitle("Urzadzenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie kart urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultSrwUrzadzeniaNew = "";
            try
            { 
            resultSrwUrzadzeniaNew = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwUrzadzeniaLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Urzadzenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie kart urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            SrwUrzadzenia_Synchronizacja(idOperatora, resultSrwUrzadzeniaNew);

            RunOnUiThread(() => progressDialog.SetTitle("Urzadzenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie w³aœcicieli urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultSrwUrzWlascNew = "";
            try
            {
                resultSrwUrzWlascNew = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwUrzWlascLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Urzadzenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie w³aœcicieli urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            SrwUrzWlasc_Synchronizacja(idOperatora, resultSrwUrzWlascNew);

            RunOnUiThread(() => progressDialog.SetTitle("Urzadzenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie definicji parametrów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultSrwUrzParDefNew = "";
            try
            {
                resultSrwUrzParDefNew = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwUrzParDefLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Urzadzenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie definicji parametrów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            SrwUrzParDef_Synchronizacja(idOperatora, resultSrwUrzParDefNew);

            RunOnUiThread(() => progressDialog.SetTitle("Urzadzenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie rodzajów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultSrwUrzRodzajeNew = "";
            try
            {
                resultSrwUrzRodzajeNew = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwUrzRodzajeLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Urzadzenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie rodzajów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            SrwUrzRodzaje_Synchronizacja(idOperatora, resultSrwUrzRodzajeNew);

            RunOnUiThread(() => progressDialog.SetTitle("Urzadzenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie rodzajów parametrów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultSrwUrzRodzParNew = "";
            try
            {
                resultSrwUrzRodzParNew = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwUrzRodzParLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Urzadzenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie rodzajów parametrów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            SrwUrzRodzPar_Synchronizacja(idOperatora, resultSrwUrzRodzParNew);
        }

        private void pobierz_Zlecenia(int idOperatora)
        {
            int licznik = 1;
            int max = 4;

            RunOnUiThread(() => progressDialog.SetTitle("Zlecenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie nag³ówków zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultSrwZlcNagNew = "";
            try
            {
                resultSrwZlcNagNew = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwZlcNagLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Zlecenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie nag³ówków zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            SrwZlcNag_Synchronizacja(idOperatora, resultSrwZlcNagNew);

            RunOnUiThread(() => progressDialog.SetTitle("Zlecenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie czynnoœci zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultSrwZlcCzynnosciNew = "";
            try
            {
                resultSrwZlcCzynnosciNew = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwZlcCzynnosciLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Zlecenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie czynnoœci zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            SrwZlcCzynnosci_Synchronizacja(idOperatora, resultSrwZlcCzynnosciNew);

            RunOnUiThread(() => progressDialog.SetTitle("Zlecenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie sk³adników zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultSrwZlcSkladnikiNew = "";
            try
            {
                resultSrwZlcSkladnikiNew = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwZlcSkladnikiLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Zlecenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie sk³adników zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            SrwZlcSkladniki_Synchronizacja(idOperatora, resultSrwZlcSkladnikiNew);

            RunOnUiThread(() => progressDialog.SetTitle("Zlecenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie urz¹dzeñ zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultSrwZlcUrzNew = "";
            try
            {
                resultSrwZlcUrzNew = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwZlcUrzLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Zlecenia " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie urz¹dzeñ zleceñ serwisowycj..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            SrwZlcUrz_Synchronizacja(idOperatora, resultSrwZlcUrzNew);
        }

        private void pobierz_Towary(int idOperatora)
        {
            RunOnUiThread(() => progressDialog.SetTitle("Towary 1/2"));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie kart towarowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultTwrKartyNew = "";
            try
            {
                resultTwrKartyNew = new AplikacjaSerwisowa.kwronski.WebService().WS_TwrKartyLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Towary 2/2"));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie kart towarowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            TwrKarty_Synchronizacja(idOperatora, resultTwrKartyNew);
        }

        private void pobierz_Kontrahentow(int idOperatora)
        {
            int licznik = 1;
            int max = 4;

            RunOnUiThread(() => progressDialog.SetTitle("Kontrahenci " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie kart kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultKntKartyNew = "";
            try
            {
                resultKntKartyNew = new AplikacjaSerwisowa.kwronski.WebService().WS_KntKartyLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Kontrahenci " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie kart kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            KntKarty_Synchronizacja(idOperatora, resultKntKartyNew);

            RunOnUiThread(() => progressDialog.SetTitle("Kontrahenci " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie adresów kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resultKntAdresyNew = "";
            try
            {
                resultKntAdresyNew = new AplikacjaSerwisowa.kwronski.WebService().WS_KntAdresyLista(idOperatora);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d pobierania danych z serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Kontrahenci " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Zapisywanie adresów kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            KntAdresy_Synchronizacja(idOperatora, resultKntAdresyNew);
        }

        private void tworzenieTabel(int idOperatora, int max)
        {
            int licznik = 1;

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy kart kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            KntKarty_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy adresów kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            KntAdresy_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy kart towarowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            TwrKarty_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy nag³ówków zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            SrwZlcNag_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy czynnoœci zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            SrwZlcCzynnosci_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy skladników zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            SrwZlcSkladniki_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy urz¹dzeñ zleceñ skladników..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            SrwZlcUrz_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy kart urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            SrwUrzadzenia_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy w³aœcicieli urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            SrwUrzWlasc_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy definicji parametrów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            SrwUrzParDef_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy rodzajów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            SrwUrzRodzaje_StworzBaze();

            RunOnUiThread(() => progressDialog.SetTitle("Tworzenie " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy rodzajów parametrów urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            SrwUrzRodzPar_StworzBaze();
        }

        private void wysylanieDanych(Int32 idOperatora, Int32 max)
        {
            Int32 licznik = 1;

            RunOnUiThread(() => progressDialog.SetTitle("Przygotowywanie danych " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Przygotowywanie danych o kontrahentach..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            List<int> listaKntKarty = wygenerujListeKontrahentow();
            String jsonOutKntKarty = JsonConvert.SerializeObject(listaKntKarty);

            List<int> listaKntAdresy = wygenerujListeAdresow();
            String jsonOutKntAdresy = JsonConvert.SerializeObject(listaKntAdresy);

            RunOnUiThread(() => progressDialog.SetTitle("Przygotowywanie danych " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Przygotowywanie danych o towarach..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            List<int> listaTwrKarty = wygenerujListeTwrKarty();
            String jsonOutTwrKarty = JsonConvert.SerializeObject(listaTwrKarty);

            RunOnUiThread(() => progressDialog.SetTitle("Przygotowywanie danych " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Przygotowywanie danych o zleceniach..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            List<int> listaSrwZlcNag = wygenerujListeSrwZlcNag();
            String jsonOutSrwZlcNag = JsonConvert.SerializeObject(listaSrwZlcNag);

            List<int> listaSrwZlcCzynnosci = wygenerujListeSrwZlcCzynnosci();
            String jsonOutSrwZlcCzynnosci = JsonConvert.SerializeObject(listaSrwZlcCzynnosci);

            List<int> listaSrwZlcSkladniki = wygenerujListeSrwZlcSkladniki();
            String jsonOutSrwZlcSkladniki = JsonConvert.SerializeObject(listaSrwZlcSkladniki);

            List<int> listaSrwZlcUrz = wygenerujListeSrwZlcUrz();
            String jsonOutSrwZlcUrz = JsonConvert.SerializeObject(listaSrwZlcUrz);

            RunOnUiThread(() => progressDialog.SetTitle("Przygotowywanie danych " + licznik++ + "/" + max));
            RunOnUiThread(() => progressDialog.SetMessage("Przygotowywanie danych o urz¹dzeniach..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            List<int> listaSrwUrzadzenia = wygenerujListeSrwUrzadzenia();
            String jsonOutSrwUrzadzenia = JsonConvert.SerializeObject(listaSrwUrzadzenia);

            List<int> listaSrwUrzWlasc = wygenerujListeSrwUrzWlasc();
            String jsonOutSrwUrzWlasc = JsonConvert.SerializeObject(listaSrwUrzWlasc);

            List<int> listaSrwUrzParDef = wygenerujListeSrwUrzParDef();
            String jsonOutSrwUrzParDef = JsonConvert.SerializeObject(listaSrwUrzParDef);

            List<int> listaSrwUrzRodzaje = wygenerujListeSrwUrzRodzaje();
            String jsonOutSrwUrzRodzaje = JsonConvert.SerializeObject(listaSrwUrzRodzaje);

            List<int> listaSrwUrzRodzPar = wygenerujListeSrwUrzRodzPar();
            String jsonOutSrwUrzRodzPar = JsonConvert.SerializeObject(listaSrwUrzRodzPar);

            licznik = 1;

            RunOnUiThread(() => progressDialog.SetTitle("Wysy³anie danych " + licznik++ + "/4"));
            RunOnUiThread(() => progressDialog.SetMessage("Wysy³anie danych kontrahentów z urz¹dzenia..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resulWysylaniaKontrahentow = "";
            try
            {
                resulWysylaniaKontrahentow = new AplikacjaSerwisowa.kwronski.WebService().Aplikacja_Zapisz_Kontrahentow(idOperatora, jsonOutKntKarty, jsonOutKntAdresy);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d wysy³ania danych do serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Wysy³anie danych " + licznik++ + "/4"));
            RunOnUiThread(() => progressDialog.SetMessage("Wysy³anie danych towarów z urz¹dzenia..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resulWysylaniaTwrKarty = "";
            try
            {
                resulWysylaniaTwrKarty = new AplikacjaSerwisowa.kwronski.WebService().Aplikacja_Zapisz_Towary(idOperatora, jsonOutTwrKarty);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d wysy³ania danych do serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Wysy³anie danych " + licznik++ + "/4"));
            RunOnUiThread(() => progressDialog.SetMessage("Wysy³anie danych zleceñ z urz¹dzenia..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resulWysylaniaSrwNagSkladCzynn = "";
            try
            {
                resulWysylaniaSrwNagSkladCzynn = new AplikacjaSerwisowa.kwronski.WebService().Aplikacja_Zapisz_Zlecenia(idOperatora, jsonOutSrwZlcNag, jsonOutSrwZlcCzynnosci, jsonOutSrwZlcSkladniki, jsonOutSrwZlcUrz);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d wysy³ania danych do serwisu.\n" + exc.Message, "B³¹d", 0);
            }

            RunOnUiThread(() => progressDialog.SetTitle("Wysy³anie danych " + licznik++ + "/4"));
            RunOnUiThread(() => progressDialog.SetMessage("Wysy³anie danych urz¹dzeñ..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String resulWysylaniaUrzadzen = "";
            try
            {
                resulWysylaniaUrzadzen = new AplikacjaSerwisowa.kwronski.WebService().Aplikacja_Zapisz_Urzadzenia(idOperatora, jsonOutSrwUrzadzenia, jsonOutSrwUrzWlasc, jsonOutSrwUrzParDef, jsonOutSrwUrzRodzaje, jsonOutSrwUrzRodzPar);
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d wysy³ania danych do serwisu.\n" + exc.Message, "B³¹d", 0);
            }
        }

        private void KntKarty_Synchronizacja(Int32 idOperatora, string resulKntKartyNew)
        {
            List<KntKartyTable> records = JsonConvert.DeserializeObject<List<KntKartyTable>>(resulKntKartyNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String kntKartyListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].Knt_ToDo)
                            {
                                case 1:
                                if(dbr.kntKarty_InsertRecord(records[licznik]))
                                {
                                    kntKartyListZapisanych += ", " + records[licznik].Knt_GIDNumer;
                                }
                                break;

                                case 2:
                                if(dbr.kntKarty_UpdateRecord(records[licznik]))
                                {
                                    kntKartyListZapisanych += ", " + records[licznik].Knt_GIDNumer;
                                }
                                break;

                                case 3:
                                if(dbr.kntKarty_DeleteRecord(records[licznik].Knt_GIDNumer))
                                {
                                    kntKartyListZapisanych += ", " + records[licznik].Knt_GIDNumer;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        KntKarty_RaportZapisanych(idOperatora, kntKartyListZapisanych);
                    }
                }
            }
        }

        private void KntKarty_RaportZapisanych(Int32 idOperatora, String kntKartyListZapisanych)
        {
            String kntKartyDoUsuniecia = new AplikacjaSerwisowa.kwronski.WebService().WS_KntKartyPotwierdz(idOperatora, kntKartyListZapisanych);
        }

        private void KntAdresy_Synchronizacja(int idOperatora, string resulKntAdresyNew)
        {
            List<KntAdresyTable> records = JsonConvert.DeserializeObject<List<KntAdresyTable>>(resulKntAdresyNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String kntAdresyListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].Kna_ToDo)
                            {
                                case 1:
                                if(dbr.kntAdresy_InsertRecord(records[licznik]))
                                {
                                    kntAdresyListZapisanych += ", " + records[licznik].Kna_GIDNumer;
                                }
                                break;

                                case 2:
                                if(dbr.kntAdresy_UpdateRecord(records[licznik]))
                                {
                                    kntAdresyListZapisanych += ", " + records[licznik].Kna_GIDNumer;
                                }
                                break;

                                case 3:
                                if(dbr.kntAdresy_DeleteRecord(records[licznik].Kna_GIDNumer))
                                {
                                    kntAdresyListZapisanych += ", " + records[licznik].Kna_GIDNumer;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        KntAdresy_RaportZapisanych(idOperatora, kntAdresyListZapisanych);
                    }
                }
            }
        }

        private void KntAdresy_RaportZapisanych(Int32 idOperatora, String kntAdresyListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_KntAdresyPotwierdz(idOperatora, kntAdresyListZapisanych);
        }

        private void TwrKarty_Synchronizacja(int idOperatora, string resulTwrKartyNew)
        {
            List<TwrKartyTable> records = JsonConvert.DeserializeObject<List<TwrKartyTable>>(resulTwrKartyNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String TwrKartyListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].Twr_ToDo)
                            {
                                case 1:
                                if(dbr.TwrKartyTable_InsertRecord(records[licznik]))
                                {
                                    TwrKartyListZapisanych += ", " + records[licznik].Twr_GIDNumer;
                                }
                                break;

                                case 2:
                                if(dbr.TwrKartyTable_UpdateRecord(records[licznik]))
                                {
                                    TwrKartyListZapisanych += ", " + records[licznik].Twr_GIDNumer;
                                }
                                break;

                                case 3:
                                if(dbr.TwrKartyTable_DeleteRecord(records[licznik].Twr_GIDNumer))
                                {
                                    TwrKartyListZapisanych += ", " + records[licznik].Twr_GIDNumer;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        TwrKarty_RaportZapisanych(idOperatora, TwrKartyListZapisanych);
                    }
                }
            }
        }

        private void TwrKarty_RaportZapisanych(int idOperatora, string twrKartyListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_TwrKartyPotwierdz(idOperatora, twrKartyListZapisanych);
        }

        private void SrwZlcNag_Synchronizacja(int idOperatora, string resultSrwZlcNagNew)
        {
            List<SrwZlcNag> records = JsonConvert.DeserializeObject<List<SrwZlcNag>>(resultSrwZlcNagNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String SrwZlcNagListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].SZN_ToDo)
                            {
                                case 1:
                                if(dbr.SrwZlcNag_InsertRecord(records[licznik]))
                                {
                                    SrwZlcNagListZapisanych += ", " + records[licznik].SZN_Id;
                                }
                                break;

                                case 2:
                                if(dbr.SrwZlcNag_UpdateRecord(records[licznik]))
                                {
                                    SrwZlcNagListZapisanych += ", " + records[licznik].SZN_Id;
                                }
                                break;

                                case 3:
                                if(dbr.SrwZlcNag_DeleteRecord(records[licznik].SZN_Id))
                                {
                                    SrwZlcNagListZapisanych += ", " + records[licznik].SZN_Id;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        SrwZlcNag_RaportZapisanych(idOperatora, SrwZlcNagListZapisanych);
                    }
                }
            }
        }

        private void SrwZlcNag_RaportZapisanych(int idOperatora, string srwZlcNagListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwZlcNagPotwierdz(idOperatora, srwZlcNagListZapisanych);
        }

        private void SrwZlcCzynnosci_Synchronizacja(int idOperatora, string resultSrwZlcCzynnosciNew)
        {
            List<SrwZlcCzynnosci> records = JsonConvert.DeserializeObject<List<SrwZlcCzynnosci>>(resultSrwZlcCzynnosciNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String SrwZlcCzynnosciListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].SZC_ToDo)
                            {
                                case 1:
                                if(dbr.SrwZlcCzynnosci_InsertRecord(records[licznik]))
                                {
                                    SrwZlcCzynnosciListZapisanych += ", " + records[licznik].SZC_Id;
                                }
                                break;

                                case 2:
                                if(dbr.SrwZlcCzynnosci_UpdateRecord(records[licznik]))
                                {
                                    SrwZlcCzynnosciListZapisanych += ", " + records[licznik].SZC_Id;
                                }
                                break;

                                case 3:
                                if(dbr.SrwZlcCzynnosci_DeleteRecord(records[licznik].SZC_Id))
                                {
                                    SrwZlcCzynnosciListZapisanych += ", " + records[licznik].SZC_Id;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        SrwZlcCzynnosci_RaportZapisanych(idOperatora, SrwZlcCzynnosciListZapisanych);
                    }
                }
            }
        }

        private void SrwZlcCzynnosci_RaportZapisanych(int idOperatora, string srwZlcCzynnosciListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwZlcCzynnosciPotwierdz(idOperatora, srwZlcCzynnosciListZapisanych);
        }

        private void SrwZlcSkladniki_Synchronizacja(int idOperatora, string resultSrwZlcSkladnikiNew)
        {
            List<SrwZlcSkladniki> records = JsonConvert.DeserializeObject<List<SrwZlcSkladniki>>(resultSrwZlcSkladnikiNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String SrwZlcSkladnikiListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].SZS_ToDo)
                            {
                                case 1:
                                if(dbr.SrwZlcSkladniki_InsertRecord(records[licznik]))
                                {
                                    SrwZlcSkladnikiListZapisanych += ", " + records[licznik].SZS_Id;
                                }
                                break;

                                case 2:
                                if(dbr.SrwZlcSkladniki_UpdateRecord(records[licznik]))
                                {
                                    SrwZlcSkladnikiListZapisanych += ", " + records[licznik].SZS_Id;
                                }
                                break;

                                case 3:
                                if(dbr.SrwZlcSkladniki_DeleteRecord(records[licznik].SZS_Id))
                                {
                                    SrwZlcSkladnikiListZapisanych += ", " + records[licznik].SZS_Id;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        SrwZlcSkladniki_RaportZapisanych(idOperatora, SrwZlcSkladnikiListZapisanych);
                    }
                }
            }
        }

        private void SrwZlcSkladniki_RaportZapisanych(int idOperatora, string srwZlcSkladnikiListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwZlcSkladnikiPotwierdz(idOperatora, srwZlcSkladnikiListZapisanych);
        }

        private void SrwZlcUrz_Synchronizacja(int idOperatora, string resultSrwZlcUrzNew)
        {
            List<SrwZlcUrz> records = JsonConvert.DeserializeObject<List<SrwZlcUrz>>(resultSrwZlcUrzNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String SrwZlcUrzListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].SZU_ToDo)
                            {
                                case 1:
                                if(dbr.SrwZlcUrz_InsertRecord(records[licznik]))
                                {
                                    SrwZlcUrzListZapisanych += ", " + records[licznik].SZU_Id;
                                }
                                break;

                                case 2:
                                if(dbr.SrwZlcUrz_UpdateRecord(records[licznik]))
                                {
                                    SrwZlcUrzListZapisanych += ", " + records[licznik].SZU_Id;
                                }
                                break;

                                case 3:
                                if(dbr.SrwZlcUrz_DeleteRecord(records[licznik].SZU_Id))
                                {
                                    SrwZlcUrzListZapisanych += ", " + records[licznik].SZU_Id;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        SrwZlcUrz_RaportZapisanych(idOperatora, SrwZlcUrzListZapisanych);
                    }
                }
            }
        }

        private void SrwZlcUrz_RaportZapisanych(int idOperatora, string srwZlcUrzListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwZlcUrzPotwierdz(idOperatora, srwZlcUrzListZapisanych);
        }

        private void SrwUrzadzenia_Synchronizacja(int idOperatora, string resultSrwUrzadzeniaNew)
        {
            List<SrwUrzadzenia> records = JsonConvert.DeserializeObject<List<SrwUrzadzenia>>(resultSrwUrzadzeniaNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String SrwUrzadzeniaListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].SrU_ToDo)
                            {
                                case 1:
                                if(dbr.SrwUrzadzenia_InsertRecord(records[licznik]))
                                {
                                    SrwUrzadzeniaListZapisanych += ", " + records[licznik].SrU_Id;
                                }
                                break;

                                case 2:
                                if(dbr.SrwUrzadzenia_UpdateRecord(records[licznik]))
                                {
                                    SrwUrzadzeniaListZapisanych += ", " + records[licznik].SrU_Id;
                                }
                                break;

                                case 3:
                                if(dbr.SrwUrzadzenia_DeleteRecord(records[licznik].SrU_Id))
                                {
                                    SrwUrzadzeniaListZapisanych += ", " + records[licznik].SrU_Id;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        SrwUrzadzenia_RaportZapisanych(idOperatora, SrwUrzadzeniaListZapisanych);
                    }
                }
            }
        }

        private void SrwUrzadzenia_RaportZapisanych(int idOperatora, string SrwUrzadzeniaListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwUrzadzeniaPotwierdz(idOperatora, SrwUrzadzeniaListZapisanych);
        }

        private void SrwUrzWlasc_Synchronizacja(int idOperatora, string resultSrwUrzWlascNew)
        {
            List<SrwUrzWlasc> records = JsonConvert.DeserializeObject<List<SrwUrzWlasc>>(resultSrwUrzWlascNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String SrwUrzWlascListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].SUW_ToDo)
                            {
                                case 1:
                                if(dbr.SrwUrzWlasc_InsertRecord(records[licznik]))
                                {
                                    SrwUrzWlascListZapisanych += ", " + records[licznik].SUW_SrUId;
                                }
                                break;

                                case 2:
                                if(dbr.SrwUrzWlasc_UpdateRecord(records[licznik]))
                                {
                                    SrwUrzWlascListZapisanych += ", " + records[licznik].SUW_SrUId;
                                }
                                break;

                                case 3:
                                if(dbr.SrwUrzWlasc_DeleteRecord(records[licznik].ID))
                                {
                                    SrwUrzWlascListZapisanych += ", " + records[licznik].SUW_SrUId;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        SrwUrzWlasc_RaportZapisanych(idOperatora, SrwUrzWlascListZapisanych);
                    }
                }
            }
        }

        private void SrwUrzWlasc_RaportZapisanych(int idOperatora, string SrwUrzWlascListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwUrzWlascPotwierdz(idOperatora, SrwUrzWlascListZapisanych);
        }

        private void SrwUrzParDef_Synchronizacja(int idOperatora, string resultSrwUrzParDefNew)
        {
            List<SrwUrzParDef> records = JsonConvert.DeserializeObject<List<SrwUrzParDef>>(resultSrwUrzParDefNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String SrwUrzParDefListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].SUD_ToDo)
                            {
                                case 1:
                                if(dbr.SrwUrzParDef_InsertRecord(records[licznik]))
                                {
                                    SrwUrzParDefListZapisanych += ", " + records[licznik].SUD_Id;
                                }
                                break;

                                case 2:
                                if(dbr.SrwUrzParDef_UpdateRecord(records[licznik]))
                                {
                                    SrwUrzParDefListZapisanych += ", " + records[licznik].SUD_Id;
                                }
                                break;

                                case 3:
                                if(dbr.SrwUrzParDef_DeleteRecord(records[licznik].SUD_Id))
                                {
                                    SrwUrzParDefListZapisanych += ", " + records[licznik].SUD_Id;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        SrwUrzParDef_RaportZapisanych(idOperatora, SrwUrzParDefListZapisanych);
                    }
                }
            }
        }

        private void SrwUrzParDef_RaportZapisanych(int idOperatora, string SrwUrzParDefListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwUrzParDefPotwierdz(idOperatora, SrwUrzParDefListZapisanych);
        }
        
        private void SrwUrzRodzaje_Synchronizacja(int idOperatora, string resultSrwUrzRodzajeNew)
        {
            List<SrwUrzRodzaje> records = JsonConvert.DeserializeObject<List<SrwUrzRodzaje>>(resultSrwUrzRodzajeNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String SrwUrzRodzajeListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].SUR_ToDo)
                            {
                                case 1:
                                if(dbr.SrwUrzRodzaje_InsertRecord(records[licznik]))
                                {
                                    SrwUrzRodzajeListZapisanych += ", " + records[licznik].SUR_Id;
                                }
                                break;

                                case 2:
                                if(dbr.SrwUrzRodzaje_UpdateRecord(records[licznik]))
                                {
                                    SrwUrzRodzajeListZapisanych += ", " + records[licznik].SUR_Id;
                                }
                                break;

                                case 3:
                                if(dbr.SrwUrzRodzaje_DeleteRecord(records[licznik].SUR_Id))
                                {
                                    SrwUrzRodzajeListZapisanych += ", " + records[licznik].SUR_Id;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        SrwUrzRodzaje_RaportZapisanych(idOperatora, SrwUrzRodzajeListZapisanych);
                    }
                }
            }
        }

        private void SrwUrzRodzaje_RaportZapisanych(int idOperatora, string SrwUrzRodzajeListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwUrzRodzajePotwierdz(idOperatora, SrwUrzRodzajeListZapisanych);
        }
        
        private void SrwUrzRodzPar_Synchronizacja(int idOperatora, string resultSrwUrzRodzParNew)
        {
            List<SrwUrzRodzPar> records = JsonConvert.DeserializeObject<List<SrwUrzRodzPar>>(resultSrwUrzRodzParNew);

            DBRepository dbr = new DBRepository(this);

            if(records != null)
            {
                if(records.Count > 0)
                {
                    RunOnUiThread(() => progressDialog.Max = records.Count);

                    Int32 coIleRaport = 500;
                    Int32 liczbaRaportow = ((int)records.Count / coIleRaport) + 1;
                    Int32 licznik = 0;

                    for(int i = 1; i <= liczbaRaportow; i++)
                    {
                        Int32 max = 500 * i;
                        String SrwUrzRodzParListZapisanych = "-1";

                        while(licznik < records.Count && licznik <= max)
                        {
                            switch(records[licznik].SRP_ToDo)
                            {
                                case 1:
                                if(dbr.SrwUrzRodzPar_InsertRecord(records[licznik]))
                                {
                                    SrwUrzRodzParListZapisanych += ", " + records[licznik].SRP_Id;
                                }
                                break;

                                case 2:
                                if(dbr.SrwUrzRodzPar_UpdateRecord(records[licznik]))
                                {
                                    SrwUrzRodzParListZapisanych += ", " + records[licznik].SRP_Id;
                                }
                                break;

                                case 3:
                                if(dbr.SrwUrzRodzPar_DeleteRecord(records[licznik].SRP_Id))
                                {
                                    SrwUrzRodzParListZapisanych += ", " + records[licznik].SRP_Id;
                                }
                                break;
                            }
                            licznik++;
                            RunOnUiThread(() => progressDialog.Progress++);
                        }

                        SrwUrzRodzPar_RaportZapisanych(idOperatora, SrwUrzRodzParListZapisanych);
                    }
                }
            }
        }

        private void SrwUrzRodzPar_RaportZapisanych(int idOperatora, string SrwUrzRodzParListZapisanych)
        {
            String result = new AplikacjaSerwisowa.kwronski.WebService().WS_SrwUrzRodzParPotwierdz(idOperatora, SrwUrzRodzParListZapisanych);
        }

        private void KntKarty_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.createDB();
            dbr.stworzKntKartyTabele();
        }

        private List<int> wygenerujListeKontrahentow()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.kntKarty_generujListeZapisanch();
        }

        private void KntAdresy_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzKntAdresyTabele();
        }
        private List<int> wygenerujListeAdresow()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.KntAdresy_generujListeZapisanch();
        }

        private void TwrKarty_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzTwrKartyTable();
        }
        private List<int> wygenerujListeTwrKarty()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.TwrKarty_generujListeZapisanch();
        }

        private void SrwZlcNag_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzSrwZlcNag();
        }

        private List<int> wygenerujListeSrwZlcNag()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.SrwZlcNag_generujListeZapisanch();
        }

        private void SrwZlcCzynnosci_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzSrwZlcCzynnosci();
        }
        private List<int> wygenerujListeSrwZlcCzynnosci()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.SrwZlcCzynnosci_generujListeZapisanch();
        }

        private void SrwZlcSkladniki_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzSrwZlcSkladniki();
        }
        private List<int> wygenerujListeSrwZlcSkladniki()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.SrwZlcSkladniki_generujListeZapisanch();
        }

        private void SrwZlcUrz_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzSrwZlcUrz();
        }
        private List<int> wygenerujListeSrwZlcUrz()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.SrwZlcSUrz_generujListeZapisanch();
        }

        private void SrwUrzadzenia_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzSrwUrzadzenia();
        }
        private List<int> wygenerujListeSrwUrzadzenia()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.SrwUrzadzenia_generujListeZapisanch();
        }

        private void SrwUrzWlasc_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzSrwUrzWlasc();
        }
        private List<int> wygenerujListeSrwUrzWlasc()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.SrwUrzWlasc_generujListeZapisanch();
        }

        private void SrwUrzParDef_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzSrwUrzParDef();
        }
        private List<int> wygenerujListeSrwUrzParDef()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.SrwUrzParDef_generujListeZapisanch();
        }

        private void SrwUrzRodzaje_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzSrwUrzRodzaje();
        }
        private List<int> wygenerujListeSrwUrzRodzaje()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.SrwUrzRodzaje_generujListeZapisanch();
        }

        private void SrwUrzRodzPar_StworzBaze()
        {
            DBRepository dbr = new DBRepository(this);
            dbr.stworzSrwUrzRodzPar();
        }
        private List<int> wygenerujListeSrwUrzRodzPar()
        {
            DBRepository dbr = new DBRepository(this);
            return dbr.SrwUrzRodzPar_generujListeZapisanch();
        }
    }
}