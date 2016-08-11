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

        public Boolean PolaczZBaza(int baza)
        {
            Hasla haslo = new Hasla(2);

            try
            {
                String loginBD = haslo.GetInstanceUserName();
                String hasloBD = haslo.GetInstancePassword();
                String instancja = haslo.GetInstanceName();
                String bazaDanych = "";

                if(baza == 0)
                {
                    bazaDanych = haslo.GetDataBaseNameXL();
                }
                else
                {
                    bazaDanych = haslo.GetDataBaseNameSerwis();
                }
                
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

        public DataTable pobierzNoweSrwZlcNag()
        {
            DataTable noweSrwZlecDT = new DataTable();

            try
            {
                String zapytanieString = "SELECT * FROM [GAL].[SrwZlcNag] where GZN_Synchronizacja = 0";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(noweSrwZlecDT);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Błąd funkcji DataBase.pobierzNoweSrwZlcNag():\n" + exc.Message, EventLogEntryType.Error);
            }

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

        public void oznaczZlcSrwNagZapisany(int id, int oznaczenie)
        {
            /* switch(oznaczenie)
             * 1 - dodano poprawnie do XL
             * 2 - błąd dodawania cznnosci
             * 3 - błąd dodawania składników
            */

            try
            {
                String zapytanieString = "UPDATE [GAL].[SrwZlcNag] SET [GZN_Synchronizacja] = " + oznaczenie + " WHERE [GZN_Id] = " +id.ToString();
                zapiszDB(zapytanieString);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Błąd funkcji DataBase.oznaczZlcSrwNagZapisany("+id.ToString()+"):\n" + exc.Message, EventLogEntryType.Error);
            }
        }

        public DataTable pobierzSrwZlcCzynnosci(int GZN_Id)
        {
            DataTable SrwZlcCzynnosciDT = new DataTable();

            try
            {
                String zapytanieString = "SELECT * FROM [GAL].[SrwZlcCzynnosci] where GZC_GZNId = " + GZN_Id.ToString();
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(SrwZlcCzynnosciDT);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Błąd funkcji DataBase.pobierzSrwZlcCzynnosci(" + GZN_Id + "):\n" + exc.Message, EventLogEntryType.Error);
            }

            return SrwZlcCzynnosciDT;
        }

        public DataTable pobierzSrwZlcSkladniki(int GZN_Id)
        {
            DataTable SrwZlcSkladnikiDT = new DataTable();

            try
            {
                String zapytanieString = "SELECT * FROM [GAL].[SrwZlcSkladniki] where GZS_GZNId = " + GZN_Id.ToString();
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(SrwZlcSkladnikiDT);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Błąd funkcji DataBase.pobierzSrwZlcSkladniki(" + GZN_Id + "):\n" + exc.Message, EventLogEntryType.Error);
            }

            return SrwZlcSkladnikiDT;
        }

        public Boolean SrwZlcNag_InsertRecord(SrwZlcNag SZN)
        {
            String zapytanieString = "";
            try
            {
                zapytanieString = @"
                    INSERT INTO [GAL].[SrwZlcNag] VALUES(" +
                    SZN.SZN_Synchronizacja.ToString() + ", " +
                    SZN.SZN_KntTyp.ToString() + ", " + SZN.SZN_KntNumer.ToString() + ", " +
                    SZN.SZN_KnATyp.ToString() + ", " + SZN.SZN_KnANumer.ToString() + ", " +
                    "'" + SZN.SZN_Dokument + "', " +
                    "'" + SZN.SZN_DataWystawienia + "', " +
                    "'" + SZN.SZN_DataRozpoczecia + "', " +
                    "'" + SZN.SZN_Stan + "', " +
                    "'" + SZN.SZN_Status + "', " +
                    "'" + SZN.SZN_Opis + "')";
                SqlDataAdapter da = zapytanie(zapytanieString);
                DataTable pomDataTable = new DataTable();
                da.Fill(pomDataTable);

                return true;
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Błąd funkcji DataBase.SrwZlcNag_InsertRecord(" + SZN.SZN_Id + "):\n" + exc.Message+"\nZapytanie: "+zapytanieString, EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean SrwZlcCzynnosci_InsertRecord(SrwZlcCzynnosci SZC)
        {
            String zapytanieString = "";
            try
            {
                zapytanieString = @"
                    INSERT INTO [GALXL_Serwis].[GAL].[SrwZlcCzynnosci] VALUES(" +
                    SZC.SZC_Id + ", " +
                    SZC.SZC_SZNId.ToString() + ", " +
                    SZC.SZC_SZUId.ToString() + ", " +
                    SZC.SZC_Synchronizacja.ToString() + ", " +
                    SZC.SZC_Pozycja.ToString() + ", " +
                    SZC.SZC_TwrTyp.ToString() + ", " +
                    SZC.SZC_TwrNumer.ToString() + ", " +
                    "'" + SZC.SZC_TwrNazwa + "', " +
                    "'" + SZC.SZC_Ilosc + "', " +
                    "'" + SZC.SZC_Opis + "')";
                SqlDataAdapter da = zapytanie(zapytanieString);
                DataTable pomDataTable = new DataTable();
                da.Fill(pomDataTable);

                eventLog.WriteEntry("dodano: "+ SZC.SZC_Id.ToString());
                return true;
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Błąd funkcji DataBase.SrwZlcCzynnosci_InsertRecord(" + SZC.SZC_SZNId + "):\n" + exc.Message+" "+ zapytanieString, EventLogEntryType.Error);
                return false;
            }
        }

        public Boolean SrwZlcSkladniki_InsertRecord(SrwZlcSkladniki SZS)
        {
            String zapytanieString = "";
            try
            {
                zapytanieString = @"
                    INSERT INTO [GALXL_Serwis].[GAL].[SrwZlcSkladniki] VALUES(" +
                    SZS.SZS_Id.ToString() + ", " +
                    SZS.SZS_SZNId.ToString() + ", " +
                    SZS.SZS_Synchronizacja.ToString() + ", " +
                    SZS.SZS_Pozycja.ToString() + ", " +
                    SZS.SZS_TwrTyp.ToString() + ", " +
                    SZS.SZS_TwrNumer.ToString() + ", " +
                    "'" + SZS.SZS_TwrNazwa + "', " +
                    "'" + SZS.SZS_Ilosc + "', " +
                    "'" + SZS.SZS_Opis + "')";
                SqlDataAdapter da = zapytanie(zapytanieString);
                DataTable pomDataTable = new DataTable();
                da.Fill(pomDataTable);

                eventLog.WriteEntry("dodano: " + SZS.SZS_Id.ToString());
                return true;
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Błąd funkcji DataBase.SrwZlcSkladniki_InsertRecord(" + SZS.SZS_SZNId + "):\n" + exc.Message + " " + zapytanieString, EventLogEntryType.Error);
                return false;
            }
        }
    }
}
