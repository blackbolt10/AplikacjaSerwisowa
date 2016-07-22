using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaSerwisowaUsluga
{

    class DataBase
    {
        private static SqlConnection uchwytBD;
        private static SqlCommand polecenieSQL;
        private static EventLog eventLog;

        public DataBase(EventLog _eventLog)
        {
            eventLog = _eventLog;
        }

        public Boolean PolaczZBaza()
        {
            Hasla haslo = new Hasla(2);

            try
            {
                String loginBD = haslo.GetInstanceUserName();
                String hasloBD = haslo.GetInstancePassword();
                String instancja = haslo.GetInstanceName();
                String bazaDanych = haslo.GetDataBaseName();

                //MessageBox.Show(loginBD + "|" + hasloBD + "|" + instancja + "|" + bazaDanych);
                uchwytBD = new SqlConnection(@"user id=" + loginBD + "; password=" + hasloBD + "; Data Source=" + instancja + "; Initial Catalog=" + bazaDanych + ";");
                uchwytBD.Open();
                return true;
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Błąd funkcji DataBase.PolaczZBaza():\n" + exc.Message, EventLogEntryType.Error);
                return false;
            }
        }

        public DataTable pobierzNoweSrwZlc()
        {
            DataTable noweSrwZlecDT = new DataTable();

            try
            {
                String zapytanieString = "";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(noweSrwZlecDT);
            }
            catch(Exception) {}

            return noweSrwZlecDT;
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
    }
}
