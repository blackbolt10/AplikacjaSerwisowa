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
    [Activity(Label = "Ustawienia", Icon = "@drawable/ustawienia")]
    public class ustawienia_Activity : Activity
    {
        EditText adresSerwera, instancjaSerwer, loginSerwer, hasloSerwer;
        Button zapiszButton, synchronizacja_Button, test2Button;
        
        String documentsPath = "/sdcard/Download";
        String fileName = "operatorzy.xml";


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ustawienia);

            // Create variable object

            adresSerwera = FindViewById<EditText>(Resource.Id.adresSerwer_EditText);
            instancjaSerwer = FindViewById<EditText>(Resource.Id.instancjaSerwer_EditText);
            loginSerwer = FindViewById<EditText>(Resource.Id.loginSerwer_EditText);
            hasloSerwer = FindViewById<EditText>(Resource.Id.hasloSerwer_EditText);

            zapiszButton = FindViewById<Button>(Resource.Id.zapiszSerwer_Button);
            zapiszButton.Click += delegate { zapisDanychDoPamieciUrzadzenia(); };

            synchronizacja_Button = FindViewById<Button>(Resource.Id.synchronizacjaButton);
            synchronizacja_Button.Click += delegate { synchronizacja(); };

            test2Button = FindViewById<Button>(Resource.Id.test2_button);
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
        public static byte[] ToByteArray(string value)
        {
            char[] charArr = value.ToCharArray();
            byte[] bytes = new byte[charArr.Length];
            for (int i = 0; i < charArr.Length; i++)
            {
                byte current = Convert.ToByte(charArr[i]);
                bytes[i] = current;
            }

            return bytes;
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

            var filePath = Path.GetFullPath(documentsPath+"/"+fileName);

            System.IO.FileStream plikXML;
            try
            {
                System.IO.File.Delete(documentsPath + "/" + fileName);
            }
            catch (Exception) { }

            try
            {
                plikXML = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new System.Uri(server + "//" + fileName));
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
                ftpStream.Flush(); //czyszczenie strumienia
                ftpStream.Close();
                response.Close();

                Toast.MakeText(this, "koniec odczytu!", ToastLength.Short).Show();

                return true;
            }
            catch (Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d funkcji ustawienia_Activity.pobierzXML(): " + exc.Message,"B³¹d",0);
                System.IO.File.Delete(documentsPath + "/" + fileName);
                return false;
            }       
        }
        private void odczytajXML()
        {
            var filePath = Path.GetFullPath(documentsPath + "/" + fileName);
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
            catch(Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d podczas odczytu pliku: "+exc.Message);
            }
            List<String> akronimyList = new List<String>();
            List<String> haslaList = new List<String>();
            List<String> imionaList = new List<String>();
            List<String> nazwiskaList = new List<String>();

            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(plikXMLString);
                XmlNodeList xnList = xml.SelectNodes("/operatorzy/operator");
                foreach (XmlNode xn in xnList)
                {
                    string akronim = xn["akronim"].InnerText;
                    string haslo = xn["haslo"].InnerText;
                    string imie = xn["imie"].InnerText;
                    string nazwisko = xn["nazwisko"].InnerText;

                    akronimyList.Add(akronim);
                    haslaList.Add(haslo);
                    imionaList.Add(imie);
                    nazwiskaList.Add(nazwisko);
                }
            }
            catch (Exception exc)
            {
                messagebox("Wyst¹pi³ b³¹d podczas odczytu xml: " + exc.Message);
            }

            if (akronimyList.Count > 0 && haslaList.Count > 0 && imionaList.Count > 0 && nazwiskaList.Count > 0)
            {
                zapiszOperatorowWBazie(akronimyList, haslaList, imionaList, nazwiskaList);
            }
        }

        private void zapiszOperatorowWBazie(List<String> akronimyList, List<String> haslaList, List<String> imionaList, List<String> nazwiskaList)
        {
            DBRepository dbr = new DBRepository();
            String result = dbr.createDB();
            //Toast.MakeText(this, result, ToastLength.Short).Show();            
            result = dbr.CreateTable();
            //Toast.MakeText(this, result, ToastLength.Short).Show();
            
            for (int i = 0; i < akronimyList.Count; i++)
            {
                OperatorzyTable uzytkownik = new OperatorzyTable();
                uzytkownik.Haslo = haslaList[i];
                uzytkownik.Akronim = akronimyList[i];
                uzytkownik.Imie= imionaList[i];
                uzytkownik.Nazwisko = nazwiskaList[i];

                result = dbr.InsertRecord(uzytkownik);
                Toast.MakeText(this, i+": "+result, ToastLength.Short).Show();                
            }             
        }
        private void test2()
        {
            odczytajXML();
        }
    }
}