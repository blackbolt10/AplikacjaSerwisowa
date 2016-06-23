using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

        String documentsPath = "/sdcard/Download/";
        String fileName = "karty_towarowe.xml";

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
        private void synchronizacja()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

            if (isOnline)
            {
                if (pobierzXML())
                {
                    odczytajXML();
                }
            }
            else
            {
                Toast.MakeText(this, "Brak dostêpu do internetu", ToastLength.Short).Show();
            }

        }
        private Boolean pobierzXML()
        {
            Toast.MakeText(this, "Pobieranie danych", ToastLength.Short).Show();

            String server = adresSerwera.Text;
            String userName = loginSerwer.Text;
            String password = hasloSerwer.Text;

            var filePath = Path.Combine(documentsPath, fileName);
            System.IO.FileStream plikXML = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new System.Uri(server + "/" + fileName));
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

                plikXML.Close();
                ftpStream.Close();
                response.Close();
                Toast.MakeText(this, "koniec odczytu!", ToastLength.Short).Show();

                return true;
            }
            catch (Exception exc)
            {
                messagebox(exc.Message, "B³¹d", 0);
                DeleteFile(filePath);
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
        private void odczytajXML()
        {
            var filePath = Path.Combine(documentsPath, fileName);
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
                messagebox("Wyst¹pi³ b³¹d podczas odczytu pliku: " + exc.Message);
            }
            List<String> twrKodList = new List<String>();
            List<String> twrGidNumerList = new List<String>();
            List<String> twrTypList = new List<String>();
            List<String> twrnazwaList = new List<String>();

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(plikXMLString);
                XmlNodeList xnList = xml.SelectNodes("/root/towary/towar");
                foreach (XmlNode xn in xnList)
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
            catch (Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d podczas odczytu xml: " + exc.Message);
            }

            if (twrKodList.Count > 0 && twrGidNumerList.Count > 0 && twrTypList.Count > 0 && twrnazwaList.Count > 0)
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

            for (int i = 0; i < twrKodList.Count; i++)
            {
                kartyTowarowTable kartaTowarowa = new kartyTowarowTable();
                kartaTowarowa.TWR_GIDNumer = Convert.ToInt32(twrGidNumerList[i]);
                kartaTowarowa.TWR_Kod = twrKodList[i];
                kartaTowarowa.TWR_Nazwa = twrnazwaList[i];
                kartaTowarowa.TWR_Typ = Convert.ToInt32(twrTypList[i]);

                result = dbr.kartyTowarow_InsertRecord(kartaTowarowa);
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