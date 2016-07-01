using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace AplikacjaSerwisowaKomp
{
    public partial class MainWindow : Form
    {
        private static SqlConnection uchwytBD;
        private static SqlCommand polecenieSQL;




        public MainWindow()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            pobierzOperatorow();
        }

        private void pobierzOperatorow()
        {
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
            }

            if(pomDataTable.Rows.Count > 0)
            {
                wygenerujPlikXML("operatorzy", "operator", pomDataTable);
            }
        }

        private void wygenerujPlikXML(string glownaNazwa, string pomocniczaNazwa, DataTable pomDataTable)
        {
            DataSet ds = new DataSet(glownaNazwa);
            ds.Tables.Add(pomDataTable);
            
            ds.Tables[0].TableName = pomocniczaNazwa;
            ds.WriteXml(glownaNazwa+".xml");
            MessageBox.Show("Zobione");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pobierzKntKarty();
        }

        private void pobierzKntKarty()
        {
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
            }

            if(pomDataTable.Rows.Count > 0)
            {
                wygenerujPlikXML("KntKarty", "KntKarta", pomDataTable);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pobierzKntAdresy();
        }

        private void pobierzKntAdresy()
        {
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
            }

            if(pomDataTable.Rows.Count > 0)
            {
                wygenerujPlikXML("KntAdresy", "KntAdres", pomDataTable);
            }
        }
    }
}
