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
        private Button synchronizacja_Button, wyslij_Button;
        private AplikacjaSerwisowa.kwronski.WebService kwronskiService;
        private ProgressDialog progressDialog, progressDialogWysylanie;
        private Thread wysylanieThread;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.synchronizacjaOkno);

            synchronizacja_Button = FindViewById<Button>(Resource.Id.synchronizacjaSynchronizacjaButton);
            synchronizacja_Button.Click += delegate { synchronizacja(); };

            wyslij_Button = FindViewById<Button>(Resource.Id.SynchronizacjaWyslijDaneButton);
            wyslij_Button.Click += delegate { synchronizacjaWysylanie(); };
            
            // Create your application here

            kwronskiService = new AplikacjaSerwisowa.kwronski.WebService();
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
            DBRepository dbr = new DBRepository();
            List<SrwZlcNag> srwZlcNagList = dbr.SrwZlcNagSynchronizacja(1);
            if(srwZlcNagList.Count>0)
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

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie 1/7..."));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie nag³ówków kontrahentów..."));
            String kntKartyString = kwronskiService.ZwrocListeKntKarty();

            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy nag³ówków kontrahentów..."));
            tworzenieBazyKntKarty(kntKartyString);

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie 2/7..."));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie adresów kontrahentów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            String kntAdresyString = kwronskiService.ZwrocListeKntAdresy();

            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy adresów kontrahentów..."));
            tworzenieBazyKntAdresy(kntAdresyString);

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie 3/7..."));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie nag³ówków zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            String serwisoweZlecenniaNaglowkiString = kwronskiService.ZwrocListeZlecenSerwisowychNaglowki();

            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy nag³ówków zleceñ serwisowych..."));
            tworzenieBazySerwisoweZleceniaNaglowki(serwisoweZlecenniaNaglowkiString);

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie 4/7..."));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie czynnosci zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            String srwZlcCzynnosciString = kwronskiService.ZwrocListeZlecenSerwisowychCzynnosci();

            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy czynnosci zleceñ serwisowych..."));
            tworzenieBazySrwZlcCzynnosci(srwZlcCzynnosciString);

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie 5/7..."));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie skladniki zleceñ serwisowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            String srwZlcSkladnikiString = kwronskiService.ZwrocListeZlecenSerwisowychSkladniki();

            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy skladniki zleceñ serwisowych..."));
            tworzenieBazySrwZlcSkladniki(srwZlcSkladnikiString);

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie 6/7..."));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie kart towarowych..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            String twrKartyString = kwronskiService.ZwrocListeTwrKarty();

            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy kart towarowych..."));
            tworzenieBazyTwrKarty(twrKartyString);

            RunOnUiThread(() => progressDialog.SetTitle("Pobieranie 7/7..."));
            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie podpisów..."));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);
            String podpisyString = kwronskiService.ZwrocListePodpisow();

            RunOnUiThread(() => progressDialog.SetMessage("Tworzenie bazy podpisów..."));
            tworzenieBazySrwZlcPodpisy(podpisyString);

            RunOnUiThread(() => messagebox("Pobieranie zakoñczone", "Ukoñczono"));
            progressDialog.Dismiss();
        }

        private void tworzenieBazyKntKarty(String kntKartyString)
        {
            List<KntKartyTable> records = JsonConvert.DeserializeObject<List<KntKartyTable>>(kntKartyString);

            DBRepository dbr = new DBRepository();
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

                KntKartyTable kntKarta = records[i];
                dbr.kntKarty_InsertRecord(kntKarta);
            }
        }

        private void tworzenieBazyKntAdresy(String kntAdresyString)
        {
            List<KntAdresyTable> records = JsonConvert.DeserializeObject<List<KntAdresyTable>>(kntAdresyString);

            DBRepository dbr = new DBRepository();
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

                KntAdresyTable kntAdres = records[i];
                dbr.kntAdresy_InsertRecord(kntAdres);
            }
        }

        private void tworzenieBazySrwZlcCzynnosci(String srwZlcCzynnosciString)
        {
            List<SrwZlcCzynnosci> records = JsonConvert.DeserializeObject<List<SrwZlcCzynnosci>>(srwZlcCzynnosciString);

            DBRepository dbr = new DBRepository();
            String result = dbr.stworzSrwZlcCzynnosci();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            if(records.Count > 0)
            {
                wprowadzWpisyDoTabeliSrwZlcCzynnosci(records, dbr);
            }
        }

        private void tworzenieBazySrwZlcPodpisy(string podpisyString)
        {
            List<SrwZlcPodpisTable> records = JsonConvert.DeserializeObject<List<SrwZlcPodpisTable>>(podpisyString);

            DBRepository dbr = new DBRepository();
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

                SrwZlcPodpisTable szp = records[i];
                dbr.SrwZlcPodpis_InsertRecord(szp);
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

                SrwZlcCzynnosci szc = records[i];
                dbr.SrwZlcCzynnosci_InsertRecord(szc);
            }
        }

        private void tworzenieBazySrwZlcSkladniki(String srwZlcCzynnosciString)
        {
            List<SrwZlcSkladniki> records = JsonConvert.DeserializeObject<List<SrwZlcSkladniki>>(srwZlcCzynnosciString);

            DBRepository dbr = new DBRepository();
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

                SrwZlcSkladniki szs = records[i];
                dbr.SrwZlcSkladniki_InsertRecord(szs);
            }
        }

        private void tworzenieBazySerwisoweZleceniaNaglowki(String serwisoweZlecenniaNaglowkiString)
        {
            List<SrwZlcNag> records = JsonConvert.DeserializeObject<List<SrwZlcNag>>(serwisoweZlecenniaNaglowkiString);

            DBRepository dbr = new DBRepository();
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

                SrwZlcNag szn = records[i];
                dbr.SrwZlcNag_InsertRecord(szn);
            }
        }
        private void tworzenieBazyTwrKarty(String twrKartyString)
        {
            List<TwrKartyTable> records = JsonConvert.DeserializeObject<List<TwrKartyTable>>(twrKartyString);

            DBRepository dbr = new DBRepository();
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

                TwrKartyTable twrKarta = records[i];
                dbr.TwrKartyTable_InsertRecord(twrKarta);
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
            DBRepository db = new DBRepository();

            List<SrwZlcNag> srwZlcNagList = db.SrwZlcNagSynchronizacja(1);

            if(srwZlcNagList.Count > 0)
            {
                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Wysy³anie danych zleceñ nag³ówkowych."));
                String jsonOut = JsonConvert.SerializeObject(srwZlcNagList);

                db.SrwZlcNag_OznaczWyslane(srwZlcNagList, 2);

                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Oczekiwanie na potwierdzenie odebrania danych przez serwer."));
                String result = kwronskiService.synchronizujSrwZlcNag(jsonOut);

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

            DBRepository db = new DBRepository();
            db.SrwZlcNag_OznaczWyslane(wyslaneNagList,3);
        }

        private List<int> wyslijSrwZlcCzynniki(List<int> wyslaneNagList)
        {
            RunOnUiThread(() => progressDialogWysylanie.SetMessage("Przygotowywanie czynnoœci zleceñ serwisowych."));
            
            DBRepository db = new DBRepository();
            List<int> wyslaneczynnosciList = new List<int>();

            List<SrwZlcCzynnosci> srwZlcCzynnosciList = db.SrwZlcCzynnosciSynchronizacja(1);

            if(srwZlcCzynnosciList.Count > 0)
            {
                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Wysy³anie czynnoœci zleceñ serwisowych."));
                String jsonOut = JsonConvert.SerializeObject(srwZlcCzynnosciList);

                db.SrwZlcCzynnosci_OznaczWyslane(srwZlcCzynnosciList, 2);

                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Oczekiwanie na potwierdzenie odebrania danych przez serwer."));
                String result = kwronskiService.synchronizujSrwZlcCzynnosci(jsonOut);

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

            DBRepository db = new DBRepository();
            db.SrwZlcCzynnosci_OznaczWyslane(wyslaneCzynnosciList, 3);
        }

        private List<int> wyslijSrwZlcSkladniki(List<int> wyslaneNagList)
        {
            RunOnUiThread(() => progressDialogWysylanie.SetMessage("Przygotowywanie sk³adników zleceñ serwisowych."));

            DBRepository db = new DBRepository();
            List<int> wyslaneSkladnikiList = new List<int>();

            List<SrwZlcSkladniki> srwZlcSkladnikiList = db.SrwZlcSkladnikiSynchronizacja(1);

            if(srwZlcSkladnikiList.Count > 0)
            {
                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Wysy³anie sk³adników zleceñ serwisowych."));
                String jsonOut = JsonConvert.SerializeObject(srwZlcSkladnikiList);

                db.SrwZlcSkladniki_OznaczWyslane(srwZlcSkladnikiList, 2);

                RunOnUiThread(() => progressDialogWysylanie.SetMessage("Oczekiwanie na potwierdzenie odebrania danych przez serwer."));
                String result = kwronskiService.synchronizujSrwZlcSkladniki(jsonOut);

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

            DBRepository db = new DBRepository();
            db.SrwZlcSkladniki_OznaczWyslane(wyslaneSkladnikiList, 3);
        }
    }
}



































//Mo¿e siê przydaæ ?
/*
 * private void synchronizacjaGalsoft()
        {
            AplikacjaSerwisowa.kwronski.WebService kwronskiService = new AplikacjaSerwisowa.kwronski.WebService();
            String teasfgsgsagsa = kwronskiService.HelloWorld("lama");


            //string url = @"http://91.196.8.98/AplikacjaSerwisowa/WebService.asmx/test";
            //string url = @"http://91.196.9.105/WebService.asmx?op=test";

            /* HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
             req.Host = "91.196.8.98"; 
             HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

             StreamReader reader = new StreamReader(resp.GetResponseStream());
             String test = reader.ReadToEnd();
             */

// HttpClient client = new HttpClient();
// client.MaxResponseContentBufferSize = 2500000;
//String test = client.GetStringAsync(url);

/* HttpWebRequest request;
 request = (HttpWebRequest)WebRequest.Create(url);
 request.ContentType = "text/xml; charset=utf-8";
 GetResponse(request);*/

//string url = @"http://91.196.9.105/WebService.asmx/test";
//string url = @"http://91.196.8.98/AplikacjaSerwisowa/WebService.asmx/test";

/*HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
XmlDocument xmlDoc = new XmlDocument();
string testsdgsdgsdg = "";

            using(HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                xmlDoc.Load(resp.GetResponseStream());
                testsdgsdgsdg = xmlDoc.InnerText;
            }

            //testeasdgsadgha(url);
        }
*/
/*

        private void zapisDanychDoPamieciUrzadzenia()
        {
            var prefs = Application.Context.GetSharedPreferences(ApplicationInfo.LoadLabel(PackageManager), FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString("adresSerwera", adresSerwera.Text);
            prefEditor.PutString("instancjaSerwera", instancjaSerwer.Text);
            prefEditor.PutString("loginSerwera", loginSerwer.Text);
            prefEditor.PutString("hasloSerwera", hasloSerwer.Text);
            prefEditor.Commit();

            Toast.MakeText(this, "Zapisano!", ToastLength.Short).Show();
        }
        private void Odczyt()
        {
            var prefs = Application.Context.GetSharedPreferences(ApplicationInfo.LoadLabel(PackageManager), FileCreationMode.Private);

            adresSerwera.Text = prefs.GetString("adresSerwera", "");
            instancjaSerwer.Text = prefs.GetString("instancjaSerwera", "");
            loginSerwer.Text = prefs.GetString("loginSerwera", "");
            hasloSerwer.Text = prefs.GetString("hasloSerwera", "");
        }
*/


/*
    String documentsPath = "/sdcard/Download";

    String nazwaPlikuKartyTowarow = "karty_towarowe.xml";
    String nazwaKntKarty = "knt_karty.xml";
    String nazwaKntAdresy= "kna_adresy.xml";

    private Boolean pobierzXML(String nazwaPlikuXML)
        {
            //Toast.MakeText(this, "Pobieranie pliku "+ nazwaPlikuXML, ToastLength.Short).Show();

            RunOnUiThread(() => progressDialog.SetMessage("Pobieranie pliku "+nazwaPlikuXML));
            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = 1);

            String server = adresSerwera.Text;
            String userName = loginSerwer.Text;
            String password = hasloSerwer.Text;

            var filePath = Path.GetFullPath(documentsPath + "/" + nazwaPlikuXML);

            System.IO.FileStream plikXML;
            try
            {
                System.IO.File.Delete(documentsPath + "/" + nazwaPlikuXML);
            }
            catch(Exception) { }

            try
            {
                plikXML = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite,FileShare.ReadWrite);
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new System.Uri(server + "/" + nazwaPlikuXML));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(userName, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                long cl = response.ContentLength;
                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];
                int readCount = ftpStream.Read(buffer, 0, bufferSize);

                while (readCount > 0)
                {
                    plikXML.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Flush();
                ftpStream.Close();
                response.Close();
                plikXML.Close();
                //Toast.MakeText(this, "Zamykanie pliku "+ nazwaPlikuXML, ToastLength.Short).Show();

                return true;
            }
            catch (Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d funkcji pobierzXML(" + nazwaPlikuXML + ") :" + exc.Message, "B³¹d", 0);
                System.IO.File.Delete(documentsPath + "/" + nazwaPlikuXML);
                return false;
            }

        }

 private void odczytajXML(String nazwaPlikuXML)
        {
            RunOnUiThread(() => progressDialog.SetMessage("Odczytywanie pliku " + nazwaPlikuXML));

            var filePath = Path.GetFullPath(documentsPath + "/" + nazwaPlikuXML);
            const Int32 BufferSize = 128; ;
            String plikXMLString = "";
            try
            {

                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String line;

                        while ((line = streamReader.ReadLine()) != null)
                        {
                            plikXMLString = plikXMLString + line;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d funkcji odczytajXML(" + nazwaPlikuXML + ") :" + exc.Message,"B³¹d",0);
            }

            if(nazwaPlikuXML == nazwaPlikuKartyTowarow)
            {
                wczytajPlikKartyTowarow(plikXMLString);
            }
            else if(nazwaPlikuXML == nazwaKntKarty)
            {
                wczytajPlikKntKarty(plikXMLString);
            }
            else if(nazwaPlikuXML == nazwaKntAdresy)
            {
                wczytajPlikKnaAdresy(plikXMLString);
            }
            else
            {
                messagebox("Nie uda³o siê odczytaæ pliku XML. Nie rozpoznana nazwa pliku", "B³¹d", 0);
            }            
        }

        private void wczytajPlikKartyTowarow(String plikXMLString)
        {
            List<String> twrKodList = new List<String>();
            List<String> twrGidNumerList = new List<String>();
            List<String> twrTypList = new List<String>();
            List<String> twrnazwaList = new List<String>();

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(plikXMLString);
                XmlNodeList xnList = xml.SelectNodes("/root/towary/towar");
                foreach(XmlNode xn in xnList)
                {
                    string twrKod = xn["twr_kod"].InnerText;
                    string twrGidNumer = xn["twr_gidnumer"].InnerText;
                    string twrTyp = xn["twr_typ"].InnerText;
                    string twrNawa = xn["twr_nazwa"].InnerText;

                    twrKodList.Add(twrKod);
                    twrGidNumerList.Add(twrGidNumer);
                    twrTypList.Add(twrTyp);
                    twrnazwaList.Add(twrNawa);
                }
            }
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d podczas odczytu xml: " + exc.Message);
            }            

            if(twrKodList.Count > 0 && twrGidNumerList.Count > 0 && twrTypList.Count > 0 && twrnazwaList.Count > 0)
            {
                zapiszKartyTowaroweWBazie(twrKodList, twrGidNumerList, twrTypList, twrnazwaList);
            }
        }

        private void zapiszKartyTowaroweWBazie(List<string> twrKodList, List<String> twrGidNumerList, List<String> twrTypList, List<String> twrnazwaList)
        {
            DBRepository dbr = new DBRepository();
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.stworzKartyTowarowTabele();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = twrGidNumerList.Count);

            for (int i = 0; i < twrKodList.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                kartyTowarowTable kartaTowarowa = new kartyTowarowTable();
                kartaTowarowa.TWR_GIDNumer = Convert.ToInt32(twrGidNumerList[i]);
                kartaTowarowa.TWR_Kod = twrKodList[i];
                kartaTowarowa.TWR_Nazwa = twrnazwaList[i];
                kartaTowarowa.TWR_Typ = Convert.ToInt32(twrTypList[i]);

                result = dbr.kartyTowarow_InsertRecord(kartaTowarowa);
               // Toast.MakeText(this, i + ": " + result, ToastLength.Short).Show();
            }
        }

        private void wczytajPlikKntKarty(String plikXMLString)
        {
            List<string> knt_gidnumer_List = new List<string>();
            List<string> knt_akronim_List = new List<string>();
            List<string> knt_nazwa1_List = new List<string>();
            List<string> knt_nazwa2_List = new List<string>();
            List<string> knt_nazwa3_List = new List<string>();
            List<string> knt_kodp_List = new List<string>();
            List<string> knt_miasto_List = new List<string>();
            List<string> knt_ulica_List = new List<string>();
            List<string> knt_adresy_List = new List<string>();
            List<string> knt_nip_List = new List<string>();
            List<string> knt_telefon1_List = new List<string>();
            List<string> knt_telefon2_List = new List<string>();
            List<string> knt_telex_List = new List<string>();
            List<string> knt_fax_List = new List<string>();
            List<string> knt_email_List = new List<string>();
            List<string> knt_url_List = new List<string>();
            
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(plikXMLString);
                XmlNodeList xnList = xml.SelectNodes("/root/kntkarty/karta");
                foreach(XmlNode xn in xnList)
                {
                    knt_gidnumer_List.Add(xn["knt_gidnumer"].InnerText);
                    knt_akronim_List.Add(xn["knt_akronim"].InnerText);
                    knt_nazwa1_List.Add(xn["knt_nazwa1"].InnerText);
                    knt_nazwa2_List.Add(xn["knt_nazwa2"].InnerText);
                    knt_nazwa3_List.Add(xn["knt_nazwa3"].InnerText);
                    knt_kodp_List.Add(xn["knt_kodp"].InnerText);
                    knt_miasto_List.Add(xn["knt_miasto"].InnerText);
                    knt_ulica_List.Add(xn["knt_ulica"].InnerText);
                    knt_adresy_List.Add(xn["knt_adresy"].InnerText);
                    knt_nip_List.Add(xn["knt_nip"].InnerText);
                    knt_telefon1_List.Add(xn["knt_telefon1"].InnerText);
                    knt_telefon2_List.Add(xn["knt_telefon2"].InnerText);
                    knt_telex_List.Add(xn["knt_telex"].InnerText);
                    knt_fax_List.Add(xn["knt_fax"].InnerText);
                    knt_email_List.Add(xn["knt_email"].InnerText);
                    knt_url_List.Add(xn["knt_url"].InnerText);                    
                }
            }

            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d funkcji synchronizacja_Activity.wczytajPlikKntKarty(): " + exc.Message,"B³¹d",0);
            }

            if(knt_gidnumer_List.Count > 0)
            {
                zapiszKntKartyWBazie(knt_gidnumer_List, knt_akronim_List, knt_nazwa1_List, knt_nazwa2_List, knt_nazwa3_List, knt_kodp_List, knt_miasto_List, knt_ulica_List, knt_adresy_List, knt_nip_List, knt_telefon1_List, knt_telefon2_List, knt_telex_List, knt_fax_List, knt_email_List, knt_url_List);
            }
        }
        private void zapiszKntKartyWBazie(List<string> knt_gidnumer_List, List<string> knt_akronim_List, List<string> knt_nazwa1_List, List<string> knt_nazwa2_List, List<string> knt_nazwa3_List, List<string> knt_kodp_List, List<string> knt_miasto_List, List<string> knt_ulica_List, List<string> knt_adresy_List, List<string> knt_nip_List, List<string> knt_telefon1_List, List<string> knt_telefon2_List, List<string> knt_telex_List, List<string> knt_fax_List, List<string> knt_email_List, List<string> knt_url_List)
        {
            DBRepository dbr = new DBRepository();
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.stworzKntKartyTabele();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = knt_gidnumer_List.Count);

            for(int i = 0; i < knt_gidnumer_List.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                KntKartyTable kntKarta = new KntKartyTable();
                kntKarta.Knt_GIDNumer = Convert.ToInt32(knt_gidnumer_List[i]);
                kntKarta.Knt_Akronim = knt_akronim_List[i];
                kntKarta.Knt_nazwa1 = knt_nazwa1_List[i];
                kntKarta.Knt_nazwa2 = knt_nazwa2_List[i];
                kntKarta.Knt_nazwa3 = knt_nazwa3_List[i];
                kntKarta.Knt_KodP = knt_kodp_List[i];
                kntKarta.Knt_miasto = knt_miasto_List[i];
                kntKarta.Knt_ulica = knt_ulica_List[i];
                kntKarta.Knt_Adres = knt_adresy_List[i];
                kntKarta.Knt_nip = knt_nip_List[i];
                kntKarta.Knt_telefon1 = knt_telefon1_List[i];
                kntKarta.Knt_telefon2 = knt_telefon2_List[i];
                kntKarta.Knt_telex = knt_telex_List[i];
                kntKarta.Knt_fax = knt_fax_List[i];
                kntKarta.Knt_email = knt_email_List[i];
                kntKarta.Knt_url = knt_url_List[i];

                result = dbr.kntKarty_InsertRecord(kntKarta);
                // Toast.MakeText(this, i + ": " + result, ToastLength.Short).Show();
            }
        }
        private void wczytajPlikKnaAdresy(String plikXMLString)
        {
            List<string> knt_gidnumer_List = new List<string>();
            List<string> knt_kntNumer_List = new List<string>();
            List<string> knt_akronim_List = new List<string>();
            List<string> knt_nazwa1_List = new List<string>();
            List<string> knt_nazwa2_List = new List<string>();
            List<string> knt_nazwa3_List = new List<string>();
            List<string> knt_kodp_List = new List<string>();
            List<string> knt_miasto_List = new List<string>();
            List<string> knt_ulica_List = new List<string>();
            List<string> knt_adresy_List = new List<string>();
            List<string> knt_nip_List = new List<string>();
            List<string> knt_telefon1_List = new List<string>();
            List<string> knt_telefon2_List = new List<string>();
            List<string> knt_telex_List = new List<string>();
            List<string> knt_fax_List = new List<string>();
            List<string> knt_email_List = new List<string>();

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(plikXMLString);
                XmlNodeList xnList = xml.SelectNodes("/root/knaadresy/karta");
                foreach(XmlNode xn in xnList)
                {
                    knt_gidnumer_List.Add(xn["kna_gidnumer"].InnerText);
                    knt_kntNumer_List.Add(xn["kna_kntnumer"].InnerText);
                    knt_akronim_List.Add(xn["kna_akronim"].InnerText);
                    knt_nazwa1_List.Add(xn["kna_nazwa1"].InnerText);
                    knt_nazwa2_List.Add(xn["kna_nazwa2"].InnerText);
                    knt_nazwa3_List.Add(xn["kna_nazwa3"].InnerText);
                    knt_kodp_List.Add(xn["kna_kodp"].InnerText);
                    knt_miasto_List.Add(xn["kna_miasto"].InnerText);
                    knt_ulica_List.Add(xn["kna_ulica"].InnerText);
                    knt_adresy_List.Add(xn["kna_adresy"].InnerText);
                    knt_nip_List.Add(xn["kna_nip"].InnerText);
                    knt_telefon1_List.Add(xn["kna_telefon1"].InnerText);
                    knt_telefon2_List.Add(xn["kna_telefon2"].InnerText);
                    knt_telex_List.Add(xn["kna_telex"].InnerText);
                    knt_fax_List.Add(xn["kna_fax"].InnerText);
                    knt_email_List.Add(xn["kna_email"].InnerText);
                }
            }

            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d funkcji synchronizacja_Activity.wczytajPlikKntKarty(): " + exc.Message, "B³¹d", 0);
            }

            if(knt_gidnumer_List.Count > 0)
            {
                zapiszKntAdresyWBazie(knt_gidnumer_List, knt_kntNumer_List, knt_akronim_List, knt_nazwa1_List, knt_nazwa2_List, knt_nazwa3_List, knt_kodp_List, knt_miasto_List, knt_ulica_List, knt_adresy_List, knt_nip_List, knt_telefon1_List, knt_telefon2_List, knt_telex_List, knt_fax_List, knt_email_List);
            }
        }

        private void zapiszKntAdresyWBazie(List<string> kna_gidnumer_List, List<string> kna_kntNumer_List, List<string> kna_akronim_List, List<string> kna_nazwa1_List, List<string> kna_nazwa2_List, List<string> kna_nazwa3_List, List<string> kna_kodp_List, List<string> kna_miasto_List, List<string> kna_ulica_List, List<string> kna_adresy_List, List<string> kna_nip_List, List<string> kna_telefon1_List, List<string> kna_telefon2_List, List<string> kna_telex_List, List<string> kna_fax_List, List<string> kna_email_List)
        {
            DBRepository dbr = new DBRepository();
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.stworzKntAdresyTabele();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            RunOnUiThread(() => progressDialog.Progress = 0);
            RunOnUiThread(() => progressDialog.Max = kna_gidnumer_List.Count);

            for(int i = 0; i < kna_gidnumer_List.Count; i++)
            {
                RunOnUiThread(() => progressDialog.Progress++);

                KntAdresyTable kntAdres = new KntAdresyTable();
                kntAdres.Kna_GIDNumer = Convert.ToInt32(kna_gidnumer_List[i]);
                kntAdres.Kna_KntNumer = Convert.ToInt32(kna_kntNumer_List[i]);                kntAdres.Kna_Akronim = kna_akronim_List[i];
                kntAdres.Kna_nazwa1 = kna_nazwa1_List[i];
                kntAdres.Kna_nazwa2 = kna_nazwa2_List[i];
                kntAdres.Kna_nazwa3 = kna_nazwa3_List[i];
                kntAdres.Kna_KodP = kna_kodp_List[i];
                kntAdres.Kna_miasto = kna_miasto_List[i];
                kntAdres.Kna_ulica = kna_ulica_List[i];
                kntAdres.Kna_Adres = kna_adresy_List[i];
                kntAdres.Kna_nip = kna_nip_List[i];
                kntAdres.Kna_telefon1 = kna_telefon1_List[i];
                kntAdres.Kna_telefon2 = kna_telefon2_List[i];
                kntAdres.Kna_telex = kna_telex_List[i];
                kntAdres.Kna_fax = kna_fax_List[i];
                kntAdres.Kna_email = kna_email_List[i];

                result = dbr.kntAdresy_InsertRecord(kntAdres);
                // Toast.MakeText(this, i + ": " + result, ToastLength.Short).Show();
            }
        }
     
     
     */
