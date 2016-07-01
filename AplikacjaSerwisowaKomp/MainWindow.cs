using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace AplikacjaSerwisowaKomp
{
    public partial class MainWindow : Form
    {
        private static MainWindow MainWindowForm;
        private static SqlConnection uchwytBD;
        private static SqlCommand polecenieSQL;




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
            AktualizujMainWindow(operatorzyLabel, "Odczyt z bazy danych...", operatorzyPictureBox, true);
            
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania funkcji MainWindow.pobierzOperatorow() :\n" + exc.Message);

                AktualizujMainWindow(operatorzyLabel, "Błąd...", operatorzyPictureBox, false);
            }

            if(pomDataTable.Rows.Count > 0)
            {
                AktualizujMainWindow(operatorzyLabel, "Odczyt z bazy danych...", operatorzyPictureBox, true);

                wygenerujPlikXML("operatorzy", "operator", pomDataTable);

                AktualizujMainWindow(operatorzyLabel, "Wykonano...", operatorzyPictureBox, false);
            }
            else
            {
                AktualizujMainWindow(operatorzyLabel, "Brak wierszy do zapisania...", operatorzyPictureBox, false);
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
            AktualizujMainWindow(kntKartyLabel, "Odczyt z bazy danych...", kntKartyPictureBox, true);

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "select knt.knt_GIDNumer, knt.knt_Akronim, knt.knt_nazwa1, knt.knt_nazwa2, knt.knt_nazwa3, knt.knt_KodP, knt.knt_miasto, knt.knt_ulica, knt.knt_Adres, knt.knt_nip, knt.knt_telefon1, knt.knt_telefon2, knt.knt_telex, knt.knt_fax, knt.knt_email, knt.knt_url from cdn.kntkarty knt";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania funkcji MainWindow.pobierzKntKarty() :\n" + exc.Message);

                AktualizujMainWindow(kntKartyLabel, "Błąd...", kntKartyPictureBox, false);
            }

            if(pomDataTable.Rows.Count > 0)
            {
                AktualizujMainWindow(kntKartyLabel, "Tworzenie pliku...", kntKartyPictureBox, true);

                wygenerujPlikXML("KntKarty", "KntKarta", pomDataTable);

                AktualizujMainWindow(kntKartyLabel, "Wykonano...", kntKartyPictureBox, false);
            }
            else
            {
                AktualizujMainWindow(kntKartyLabel, "Brak wierszy do zapisania...", kntKartyPictureBox, false);
            }
        }

        private void kntAdresyButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(pobierzKntAdresy);
            t.Start();
        }

        private void pobierzKntAdresy()
        {
            AktualizujMainWindow(kntAdresyLabel, "Odczyt z bazy danych...", kntAdresyPictureBox, true);
            
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "select kna_GIDNumer, kna_kntnumer, kna_Akronim, kna_nazwa1, kna_nazwa2, kna_nazwa3, kna_KodP, kna_miasto, kna_ulica, kna_Adres, kna_nip, kna_telefon1, kna_telefon2, kna_telex, kna_fax, kna_email from cdn.kntadresy";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania funkcji MainWindow.pobierzKntAdresy() :\n" + exc.Message);

                AktualizujMainWindow(kntAdresyLabel, "Błąd...", kntAdresyPictureBox, false);
            }

            if(pomDataTable.Rows.Count > 0)
            {
                AktualizujMainWindow(kntAdresyLabel, "Tworzenie pliku...", kntAdresyPictureBox, true);

                wygenerujPlikXML("KntAdresy", "KntAdres", pomDataTable);

                AktualizujMainWindow(kntAdresyLabel, "Wykonano...", kntAdresyPictureBox, false);
            }
            else
            {
                AktualizujMainWindow(kntAdresyLabel, "Brak wierszy do zapisania...", kntAdresyPictureBox, false);
            }
        }

        private void twrKartyCzynnosciButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(pobierzTwrKartyCzynnosci);
            t.Start();
        }

        private void pobierzTwrKartyCzynnosci()
        {
            AktualizujMainWindow(twrKartyCzynnosciLabel, "Odczyt z bazy danych...", twrKartyCzynnosciPictureBox, true);

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "select twr_gidnumer, twr_kod, twr_typ, twr_nazwa, twr_nazwa1 from cdn.twrkarty where twr_typ = 4 order by twr_kod";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania funkcji MainWindow.pobierzTwrKartyCzynnosci() :\n" + exc.Message);

                AktualizujMainWindow(twrKartyCzynnosciLabel, "Błąd...", twrKartyCzynnosciPictureBox, false);
            }

            if(pomDataTable.Rows.Count > 0)
            {
                AktualizujMainWindow(twrKartyCzynnosciLabel, "Tworzenie pliku...", twrKartyCzynnosciPictureBox, true);

                wygenerujPlikXML("TwrKartyCzynnosci", "TwrKartyCzynnosc", pomDataTable);

                AktualizujMainWindow(twrKartyCzynnosciLabel, "Wykonano...", twrKartyCzynnosciPictureBox, false);
            }
            else
            {
                AktualizujMainWindow(twrKartyCzynnosciLabel, "Brak wierszy do zapisania...", twrKartyCzynnosciPictureBox, false);
            }
        }

        private void twrKartySkladnikiButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(pobierzTwrKartySkladniki);
            t.Start();
        }

        private void pobierzTwrKartySkladniki()
        {
            AktualizujMainWindow(twrKartySkladnikiLabel, "Odczyt z bazy danych...", twrKartySkladnikiPictureBox, true);

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "select twr_gidnumer, twr_kod, twr_typ, twr_nazwa, twr_nazwa1  from cdn.twrkarty where twr_typ in (1,2) order by twr_kod";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Wystąpił błąd podczas wykonywania funkcji MainWindow.pobierzTwrKartyCzynnosci() :\n" + exc.Message);

                AktualizujMainWindow(twrKartySkladnikiLabel, "Błąd...", twrKartySkladnikiPictureBox, false);
            }

            if(pomDataTable.Rows.Count > 0)
            {
                AktualizujMainWindow(twrKartySkladnikiLabel, "Tworzenie pliku...", twrKartySkladnikiPictureBox, true);

                wygenerujPlikXML("TwrKartySkladniki", "TwrKartySkladnik", pomDataTable);

                AktualizujMainWindow(twrKartySkladnikiLabel, "Wykonano...", twrKartySkladnikiPictureBox, false);
            }
            else
            {
                AktualizujMainWindow(twrKartySkladnikiLabel, "Brak wierszy do zapisania...", twrKartySkladnikiPictureBox, false);
            }
        }

        private static void AktualizujMainWindow(Label pomLabel, String tekst, PictureBox pomPictureBox, Boolean status)
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
                pomLabel.Text = tekst;
            }));
        }
    }
}
