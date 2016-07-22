using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using cdn_api;

namespace AplikacjaSerwisowaKomp
{
    public partial class MainWindow : Form
    {
        private static MainWindow MainWindowForm;
        private static SqlConnection uchwytBD;
        private static SqlCommand polecenieSQL;
        private static Boolean pobieranieDanych = false;

        public MainWindow()
        {
            InitializeComponent();
            MainWindowForm = this;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if(!podlaczDoBazyDanych())
            {
                this.Close();
            }
            APIConnect();
        }

        private Boolean podlaczDoBazyDanych()
        {
            Boolean wynikLogowaniaDoBD = false;
            hasla haslo = new hasla(2);

            try
            {
                String loginBD = haslo.GetInstanceUserName();
                String hasloBD = haslo.GetInstancePassword();
                String instancja = haslo.GetInstanceName();
                String bazaDanych = haslo.GetDataBaseName();

                //MessageBox.Show(loginBD + "|" + hasloBD + "|" + instancja + "|" + bazaDanych);
                uchwytBD = new SqlConnection(@"user id=" + loginBD + "; password=" + hasloBD + "; Data Source=" + instancja + "; Initial Catalog=" + bazaDanych + ";");
                uchwytBD.Open();
                wynikLogowaniaDoBD = true;
            }
            catch(Exception exc)
            {
                MessageBox.Show("Błąd podczas wykonywania funkcji Logowanie.podlaczDoBazyDanych():" + Environment.NewLine + exc.Message);
            }
            return wynikLogowaniaDoBD;
        }
        public static SqlDataAdapter zapytanie(string zapytanie1)
        {
            SqlDataAdapter wynik = new SqlDataAdapter();
            polecenieSQL = new SqlCommand(zapytanie1);
            polecenieSQL.CommandTimeout = 240;
            polecenieSQL.Connection = uchwytBD;
            wynik = new SqlDataAdapter(polecenieSQL);

            return wynik;
        }

        public static void zapiszDB(string zapytanie1)
        {
            polecenieSQL = new SqlCommand(zapytanie1);
            polecenieSQL.Connection = uchwytBD;
            polecenieSQL.ExecuteNonQuery();
        }

        private void operatorzyButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(pobierzOperatorow);
            t.Start();
        }

        private void pobierzOperatorow()
        {
            AktualizujMainWindow(operatorzyLabel, "Odczyt z bazy danych...", operatorzyPictureBox, true, operatorzyButton, false);

            while(pobieranieDanych)
            {
                Thread.Sleep(1000);
            }

            DataTable pomDataTable = new DataTable();
            try
            {
                pobieranieDanych = true;
                String zapytanieString = "";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania funkcji MainWindow.pobierzOperatorow() :\n" + exc.Message);

                AktualizujMainWindow(operatorzyLabel, "Błąd...", operatorzyPictureBox, false, operatorzyButton, false);
            }

            pobieranieDanych = false;

            if(pomDataTable.Rows.Count > 0)
            {
                AktualizujMainWindow(operatorzyLabel, "Tworzenie pliku...", operatorzyPictureBox, true);

                wygenerujPlikXML("operatorzy", "operator", pomDataTable);

                AktualizujMainWindow(operatorzyLabel, "Wysyłanie na serwer...", operatorzyPictureBox, true);

                sendToFtp("operatorzy.xml");

                AktualizujMainWindow(operatorzyLabel, "Wykonano...", operatorzyPictureBox, false, operatorzyButton, true);
            }
            else
            {
                AktualizujMainWindow(operatorzyLabel, "Brak wierszy do zapisania...", operatorzyPictureBox, false, operatorzyButton, true);
            }
        }

        private void wygenerujPlikXML(string glownaNazwa, string pomocniczaNazwa, DataTable pomDataTable)
        {
            DataSet ds = new DataSet(glownaNazwa);
            ds.Tables.Add(pomDataTable);
            
            ds.Tables[0].TableName = pomocniczaNazwa;
            ds.WriteXml(glownaNazwa+".xml");
            // MessageBox.Show("Zobione");
        }

        private void kntKartyButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(pobierzKntKarty);
            t.Start();
        }

        private void pobierzKntKarty()
        {
            AktualizujMainWindow(kntKartyLabel, "Odczyt z bazy danych...", kntKartyPictureBox, true,kntKartyButton,false);

            while(pobieranieDanych)
            {
                Thread.Sleep(1000);
            }

            DataTable pomDataTable = new DataTable();
            try
            {
                pobieranieDanych = true;

                String zapytanieString = "select knt.knt_GIDNumer, knt.knt_Akronim, knt.knt_nazwa1, knt.knt_nazwa2, knt.knt_nazwa3, knt.knt_KodP, knt.knt_miasto, knt.knt_ulica, knt.knt_Adres, knt.knt_nip, knt.knt_telefon1, knt.knt_telefon2, knt.knt_telex, knt.knt_fax, knt.knt_email, knt.knt_url from cdn.kntkarty knt";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania funkcji MainWindow.pobierzKntKarty() :\n" + exc.Message);

                AktualizujMainWindow(kntKartyLabel, "Błąd...", kntKartyPictureBox, false, kntKartyButton, true);
            }

            pobieranieDanych = false;

            if(pomDataTable.Rows.Count > 0)
            {
                AktualizujMainWindow(kntKartyLabel, "Tworzenie pliku...", kntKartyPictureBox, true);

                wygenerujPlikXML("KntKarty", "KntKarta", pomDataTable);

                AktualizujMainWindow(kntKartyLabel, "Wysyłanie na serwer...", kntKartyPictureBox, true);

                sendToFtp("KntKarty.xml");

                AktualizujMainWindow(kntKartyLabel, "Wykonano...", kntKartyPictureBox, false, kntKartyButton, true);

            }
            else
            {
                AktualizujMainWindow(kntKartyLabel, "Brak wierszy do zapisania...", kntKartyPictureBox, false, kntKartyButton, true);
            }
        }

        private void kntAdresyButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(pobierzKntAdresy);
            t.Start();
        }

        private void pobierzKntAdresy()
        {
            AktualizujMainWindow(kntAdresyLabel, "Odczyt z bazy danych...", kntAdresyPictureBox, true, kntAdresyButton, false);

            while(pobieranieDanych)
            {
                Thread.Sleep(1000);
            }

            DataTable pomDataTable = new DataTable();
            try
            {
                pobieranieDanych = true;

                String zapytanieString = "select kna_GIDNumer, kna_kntnumer, kna_Akronim, kna_nazwa1, kna_nazwa2, kna_nazwa3, kna_KodP, kna_miasto, kna_ulica, kna_Adres, kna_nip, kna_telefon1, kna_telefon2, kna_telex, kna_fax, kna_email from cdn.kntadresy";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania funkcji MainWindow.pobierzKntAdresy() :\n" + exc.Message);

                AktualizujMainWindow(kntAdresyLabel, "Błąd...", kntAdresyPictureBox, false, kntAdresyButton, true);
            }

            pobieranieDanych = false;

            if(pomDataTable.Rows.Count > 0)
            {
                AktualizujMainWindow(kntAdresyLabel, "Tworzenie pliku...", kntAdresyPictureBox, true);

                wygenerujPlikXML("KntAdresy", "KntAdres", pomDataTable);

                AktualizujMainWindow(kntAdresyLabel, "Wysyłanie na serwer...", kntAdresyPictureBox, true);

                sendToFtp("KntAdresy.xml");

                AktualizujMainWindow(kntAdresyLabel, "Wykonano...", kntAdresyPictureBox, false, kntAdresyButton, true);
            }
            else
            {
                AktualizujMainWindow(kntAdresyLabel, "Brak wierszy do zapisania...", kntAdresyPictureBox, false, kntAdresyButton, true);
            }
        }

        private void twrKartyCzynnosciButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(pobierzTwrKartyCzynnosci);
            t.Start();
        }

        private void pobierzTwrKartyCzynnosci()
        {
            AktualizujMainWindow(twrKartyCzynnosciLabel, "Odczyt z bazy danych...", twrKartyCzynnosciPictureBox, true, twrKartyCzynnosciButton, false);

            while(pobieranieDanych)
            {
                Thread.Sleep(1000);
            }

            DataTable pomDataTable = new DataTable();
            try
            {
                pobieranieDanych = true;

                String zapytanieString = "select twr_gidnumer, twr_kod, twr_typ, twr_nazwa, twr_nazwa1 from cdn.twrkarty where twr_typ = 4 order by twr_kod";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania funkcji MainWindow.pobierzTwrKartyCzynnosci() :\n" + exc.Message);

                AktualizujMainWindow(twrKartyCzynnosciLabel, "Błąd...", twrKartyCzynnosciPictureBox, false, twrKartyCzynnosciButton, true);
            }

            pobieranieDanych = false;

            if(pomDataTable.Rows.Count > 0)
            {
                AktualizujMainWindow(twrKartyCzynnosciLabel, "Tworzenie pliku...", twrKartyCzynnosciPictureBox, true);

                wygenerujPlikXML("TwrKartyCzynnosci", "TwrKartyCzynnosc", pomDataTable);

                AktualizujMainWindow(twrKartyCzynnosciLabel, "Wysyłanie na serwer...", twrKartyCzynnosciPictureBox, true);

                sendToFtp("TwrKartyCzynnosci.xml");

                AktualizujMainWindow(twrKartyCzynnosciLabel, "Wykonano...", twrKartyCzynnosciPictureBox, false, twrKartyCzynnosciButton, true);
            }
            else
            {
                AktualizujMainWindow(twrKartyCzynnosciLabel, "Brak wierszy do zapisania...", twrKartyCzynnosciPictureBox, false, twrKartyCzynnosciButton, true);
            }
        }

        private void twrKartySkladnikiButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(pobierzTwrKartySkladniki);
            t.Start();
        }

        private void pobierzTwrKartySkladniki()
        {
            AktualizujMainWindow(twrKartySkladnikiLabel, "Odczyt z bazy danych...", twrKartySkladnikiPictureBox, true, twrKartySkladnkiButton, false);

            while(pobieranieDanych)
            {
                Thread.Sleep(1000);
            }

            DataTable pomDataTable = new DataTable();
            try
            {
                pobieranieDanych = true;

                String zapytanieString = "select twr_gidnumer, twr_kod, twr_typ, twr_nazwa, twr_nazwa1  from cdn.twrkarty where twr_typ in (1,2) order by twr_kod";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania funkcji MainWindow.pobierzTwrKartyCzynnosci() :\n" + exc.Message);

                AktualizujMainWindow(twrKartySkladnikiLabel, "Błąd...", twrKartySkladnikiPictureBox, false, twrKartySkladnkiButton, true);
            }

            pobieranieDanych = false;

            if(pomDataTable.Rows.Count > 0)
            {
                AktualizujMainWindow(twrKartySkladnikiLabel, "Tworzenie pliku...", twrKartySkladnikiPictureBox, true);

                wygenerujPlikXML("TwrKartySkladniki", "TwrKartySkladnik", pomDataTable);

                AktualizujMainWindow(twrKartySkladnikiLabel, "Wysyłanie na serwer...", twrKartySkladnikiPictureBox, true);

                sendToFtp("TwrKartySkladniki.xml");

                AktualizujMainWindow(twrKartySkladnikiLabel, "Wykonano...", twrKartySkladnikiPictureBox, false, twrKartySkladnkiButton, true);
            }
            else
            {
                AktualizujMainWindow(twrKartySkladnikiLabel, "Brak wierszy do zapisania...", twrKartySkladnikiPictureBox, false, twrKartySkladnkiButton, true);
            }
        }

        private static void AktualizujMainWindow(Label pomLabel, String tekst, PictureBox pomPictureBox, Boolean status, Button pomButton = null, Boolean buttonStatus = true)
        {
            MainWindowForm.BeginInvoke(new EventHandler(delegate
            {
                if(status)
                {
                    pomPictureBox.Enabled = status;
                    pomPictureBox.Image = AplikacjaSerwisowaKomp.Properties.Resources.animatedCircle;
                }
                else
                {
                    pomPictureBox.Enabled = status;
                    pomPictureBox.Image = null;
                }

            if(pomButton != null)
            {
                pomButton.Enabled = buttonStatus;
            }

                pomLabel.Text = tekst;
            }));
        }
        private void sendToFtp(String nazwaPliku)
        {
            hasla haslo = new hasla();

            using(WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(haslo.GetFtpUserName(), haslo.GetFtpPassword());
                client.UploadFile(haslo.GetFtp()+"\\"+nazwaPliku, "STOR", nazwaPliku);
            }

        }

        private void noweZlecenie(object sender, EventArgs e)
        {
            APIConnect();
        }












        public Int32 Sesja;

        private void APIConnect()
        {
            hasla haslo = new hasla();

            Sesja = 0;
            XLLoginInfo_20162 LogINFO_20162 = new XLLoginInfo_20162();

            LogINFO_20162.Wersja = 20162;
            LogINFO_20162.ProgramID = "VisualCSharpHandlowcy";
            LogINFO_20162.OpeIdent = haslo.GetoperatorXL();
            LogINFO_20162.Baza = haslo.GetBazaXl();
            LogINFO_20162.OpeHaslo = haslo.GetHasloXL();
            LogINFO_20162.PlikLog = "";
            LogINFO_20162.SerwerKlucza = "";
            LogINFO_20162.UtworzWlasnaSesje = 0;

            int WynikLogowania = cdn_api.cdn_api.XLLogin(LogINFO_20162, ref Sesja);

            if(WynikLogowania != 0)
            {
                MessageBox.Show("Błąd logowania " + Convert.ToString(WynikLogowania));
                stworzNaglowekButton.Enabled = false;
                zamknijNaglowekButton.Enabled = false;
                dodajPozycjeButton.Enabled = false;
            }
            else
            {
                //MessageBox.Show("Połączono z bazą " + LogINFO_20162.Baza);
                stworzNaglowekButton.Enabled = true;
                zamknijNaglowekButton.Enabled = true;
                dodajPozycjeButton.Enabled = true;
            }

        }


        private void Logout()
        {
            int WynikWylogowania = cdn_api.cdn_api.XLLogout(Sesja);
            /* if (WynikWylogowania == 0)
             {
                 MessageBox.Show("Wylogowano poprawnie");
             }
             else
             {
                 MessageBox.Show("Błąd wylogowania" + Convert.ToString(WynikWylogowania));
             }*/
        }



        private int IDDok = 0;

        private void stworzNaglowekButton_Click(object sender, EventArgs e)
        {
            String gidnumerOld = "";
            
            cdn_api.XLSerwisNagInfo_20162 DokumentZlcRemNagInfo = new XLSerwisNagInfo_20162();
            DokumentZlcRemNagInfo.Wersja = 20162;

            DokumentZlcRemNagInfo.KntNumer = 805;
            DokumentZlcRemNagInfo.KntTyp = 32;

            DokumentZlcRemNagInfo.KnANumer = 1041;
            DokumentZlcRemNagInfo.KnATyp = 864;

            DokumentZlcRemNagInfo.AdWNumer = 1069;
            DokumentZlcRemNagInfo.AdWTyp = 896;

            DokumentZlcRemNagInfo.Tryb = 2;





            DokumentZlcRemNagInfo.Opis = "lama lama lama";



            int wynik = cdn_api.cdn_api.XLNoweZlecenieSerwis(Sesja,ref IDDok, DokumentZlcRemNagInfo);
            MessageBox.Show("Wynik = " + wynik.ToString()+"\nIDD0k = "+IDDok.ToString());
        }

        private void zamknijNaglowekButton_Click(object sender, EventArgs e)
        {
            cdn_api.XLZamkniecieSerwisNagInfo_20162 test = new XLZamkniecieSerwisNagInfo_20162();
            test.Wersja = 20162;
            test.TrybZamkniecia = 6;
            test.Akcja = 3;

            int wynik = cdn_api.cdn_api.XLZamknijZlecenieSerwis(ref IDDok, test);

            MessageBox.Show("Wynik = " + wynik.ToString() + "\niddok = " + IDDok.ToString());
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logout();
        }
    }
}
