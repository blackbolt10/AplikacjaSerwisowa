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

namespace AplikacjaSerwisowa
{
    [Activity(Label = "Synchronizacja", Icon = "@drawable/synchronizacja")]
    public class synchronizacja_Activity : Activity
    {
        EditText adresSerwera, instancjaSerwer, loginSerwer, hasloSerwer;
        Button zapiszButton, synchronizacja_Button, test2Button;

        String documentsPath = "/sdcard/Download";

        String nazwaPlikuKartyTowarow = "karty_towarowe.xml";
        String nazwaKntKarty = "knt_karty.xml";
        String nazwaKntAdresy= "kna_adresy.xml";

        ProgressDialog progrssDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.synchronizacjaOkno);

            adresSerwera = FindViewById<EditText>(Resource.Id.adresSerwerSynchronizacja_EditText);
            instancjaSerwer = FindViewById<EditText>(Resource.Id.instancjaSerwerSynchronizacja_EditText);
            loginSerwer = FindViewById<EditText>(Resource.Id.loginSerwerSynchronizacja_EditText);
            hasloSerwer = FindViewById<EditText>(Resource.Id.hasloSerwerSynchronizacja_EditText);

            zapiszButton = FindViewById<Button>(Resource.Id.zapiszSerwerSynchronizacja_Button);
            zapiszButton.Click += delegate { zapisDanychDoPamieciUrzadzenia(); };

            synchronizacja_Button = FindViewById<Button>(Resource.Id.synchronizacjaSynchronizacjaButton);
            synchronizacja_Button.Click += delegate { synchronizacja(); };

            test2Button = FindViewById<Button>(Resource.Id.test2Synchronizacja_button);
            test2Button.Click += delegate { test2(); };

            // Create your application here

            Odczyt();
        }
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

        private void PobieranieIParsowanie()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

            if(isOnline)
            {
                if(pobierzXML(nazwaPlikuKartyTowarow))
                {
                    odczytajXML(nazwaPlikuKartyTowarow);
                }

                if(pobierzXML(nazwaKntKarty))
                {
                    odczytajXML(nazwaKntKarty);
                }

                if(pobierzXML(nazwaKntAdresy))
                {
                    odczytajXML(nazwaKntAdresy);
                }
            }
            else
            {
                Toast.MakeText(this, "Brak dostêpu do internetu", ToastLength.Short).Show();
            }
            progrssDialog.Dismiss();
        }

        private void synchronizacja()
        {
            progrssDialog = new ProgressDialog(this);
            progrssDialog.SetTitle("Pobieranie");
            progrssDialog.SetMessage("Proszê czekaæ...");
            progrssDialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
            progrssDialog.SetCancelable(false);
            progrssDialog.Show();

            Thread th = new Thread(() => PobieranieIParsowanie());
            th.Start();   
        }
        private Boolean pobierzXML(String nazwaPlikuXML)
        {
            //Toast.MakeText(this, "Pobieranie pliku "+ nazwaPlikuXML, ToastLength.Short).Show();

            RunOnUiThread(() => progrssDialog.SetMessage("Pobieranie pliku "+nazwaPlikuXML));
            RunOnUiThread(() => progrssDialog.Progress = 0);
            RunOnUiThread(() => progrssDialog.Max = 1);

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
        private void odczytajXML(String nazwaPlikuXML)
        {
            RunOnUiThread(() => progrssDialog.SetMessage("Odczytywanie pliku " + nazwaPlikuXML));

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

            RunOnUiThread(() => progrssDialog.Progress = 0);
            RunOnUiThread(() => progrssDialog.Max = twrGidNumerList.Count);

            for (int i = 0; i < twrKodList.Count; i++)
            {
                RunOnUiThread(() => progrssDialog.Progress++);

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
            List<string> knt_telefon3_List = new List<string>();
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
                    knt_telefon3_List.Add(xn["knt_telefon3"].InnerText);
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
                zapiszKntKartyWBazie(knt_gidnumer_List, knt_akronim_List, knt_nazwa1_List, knt_nazwa2_List, knt_nazwa3_List, knt_kodp_List, knt_miasto_List, knt_ulica_List, knt_adresy_List, knt_nip_List, knt_telefon1_List, knt_telefon2_List, knt_telefon3_List, knt_telex_List, knt_fax_List, knt_email_List, knt_url_List);
            }
        }
        private void zapiszKntKartyWBazie(List<string> knt_gidnumer_List, List<string> knt_akronim_List, List<string> knt_nazwa1_List, List<string> knt_nazwa2_List, List<string> knt_nazwa3_List, List<string> knt_kodp_List, List<string> knt_miasto_List, List<string> knt_ulica_List, List<string> knt_adresy_List, List<string> knt_nip_List, List<string> knt_telefon1_List, List<string> knt_telefon2_List, List<string> knt_telefon3_List, List<string> knt_telex_List, List<string> knt_fax_List, List<string> knt_email_List, List<string> knt_url_List)
        {
            DBRepository dbr = new DBRepository();
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.stworzKntKartyTabele();
            //Toast.MakeText(this, result, ToastLength.Short).Show();

            RunOnUiThread(() => progrssDialog.Progress = 0);
            RunOnUiThread(() => progrssDialog.Max = knt_gidnumer_List.Count);

            for(int i = 0; i < knt_gidnumer_List.Count; i++)
            {
                RunOnUiThread(() => progrssDialog.Progress++);

                KntKartyTable kntKarta = new KntKartyTable();
                kntKarta.Knt_GIDNumer = Convert.ToInt32(knt_gidnumer_List[i]);
                kntKarta.Knt_Akrnonim = knt_akronim_List[i];
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
                kntKarta.Knt_telefon3 = knt_telefon3_List[i];
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

            RunOnUiThread(() => progrssDialog.Progress = 0);
            RunOnUiThread(() => progrssDialog.Max = kna_gidnumer_List.Count);

            for(int i = 0; i < kna_gidnumer_List.Count; i++)
            {
                RunOnUiThread(() => progrssDialog.Progress++);

                KntAdresyTable kntAdres = new KntAdresyTable();
                kntAdres.Kna_GIDNumer = Convert.ToInt32(kna_gidnumer_List[i]);
                kntAdres.Kna_KntNumer = Convert.ToInt32(kna_kntNumer_List[i]);                kntAdres.Kna_Akrnonim = kna_akronim_List[i];
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

    private void test2()
        {
            DBRepository dbr = new DBRepository();
            String result = dbr.kartyTowarow_GetAllRecords();
            Toast.MakeText(this, result, ToastLength.Long).Show();
        }
    }
}