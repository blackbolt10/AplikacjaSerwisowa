using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using cdn_api;

namespace AplikacjaSerwisowaUsluga
{
    class Synchroniacja
    {
        private DataBase dbXL;
        private DataBase dbSERWIS;
        private EventLog eventLog;

        private int IDDokSrwZlcNag = 0;
        private static Int32 Sesja = 0;

        [DllImport("ClaRUN.dll")]
        public static extern void AttachThreadToClarion(bool x);

        public Synchroniacja(DataBase _dbXL, DataBase _dbSERWIS, EventLog _eventLog)
        {
            dbXL = _dbXL;
            dbSERWIS = _dbSERWIS;
            eventLog = _eventLog;
        }

        public void Start()
        {
            int wynik = APIConnect();
            if(wynik == 0)
            {
                SrwZlcSynchronizacja();



                ApiLogout();
            }
        }
        public Int32 APIConnect()
        {
            Hasla haslo = new Hasla(2);
            Sesja = 0;

            XLLoginInfo_20162 LogINFO_20162 = new XLLoginInfo_20162();

            LogINFO_20162.Wersja = 20162;
            LogINFO_20162.ProgramID = "ASUsługa";
            LogINFO_20162.OpeIdent = haslo.GetOperatorXL();
            LogINFO_20162.Baza = haslo.GetBazaXL();
            LogINFO_20162.OpeHaslo = haslo.GetHasloXL();
            LogINFO_20162.PlikLog = "";
            LogINFO_20162.SerwerKlucza = "";
            LogINFO_20162.UtworzWlasnaSesje = 0;
            LogINFO_20162.TrybWsadowy = 1;
            
            AttachThreadToClarion(true);
            int WynikLogowania = cdn_api.cdn_api.XLLogin(LogINFO_20162, ref Sesja);

            if(WynikLogowania != 0)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji ApiXl.ApiConnect() result = " + WynikLogowania, EventLogEntryType.Error);
            }

            return WynikLogowania;
        }

        public Int32 ApiLogout()
        {
            int WynikWylogowania = cdn_api.cdn_api.XLLogout(Sesja);


            if(WynikWylogowania != 0 && WynikWylogowania != -1)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji ApiXl.ApiLogout() result = " + WynikWylogowania, EventLogEntryType.Error);
            }

            return WynikWylogowania;
        }


        private void SrwZlcSynchronizacja()
        {
            odczytajSrwZkcNagZBazy();
        }

        private void odczytajSrwZkcNagZBazy()
        {
            DataTable noweZleceniaDT = dbSERWIS.pobierzNoweSrwZlcNag();

            if(noweZleceniaDT.Rows.Count>0)
            {
                for(int i=0;i<noweZleceniaDT.Rows.Count;i++)
                {
                    wprowadzWierszSrwZkcNagDoXL(noweZleceniaDT.Rows[i]);
                }
            }
        }

        private void wprowadzWierszSrwZkcNagDoXL(DataRow dataRow)
        {
            SrwZlcNagStruct srwZlcNag = new SrwZlcNagStruct();
            try
            {
                Int32 Id = Convert.ToInt32(dataRow["GZN_Id"].ToString());
                Int32 KntNumer = Convert.ToInt32(dataRow["GZN_KntNumer"].ToString());
                Int32 KntTyp = Convert.ToInt32(dataRow["GZN_KntTyp"].ToString());
                Int32 KnANumer = Convert.ToInt32(dataRow["GZN_KnANumer"].ToString());
                Int32 KnATyp = Convert.ToInt32(dataRow["GZN_KnATyp"].ToString());
                Int32 KndNumer = Convert.ToInt32(dataRow["GZN_KndNumer"].ToString());
                Int32 KndTyp = Convert.ToInt32(dataRow["GZN_KndTyp"].ToString());
                Int32 KnPNumer = Convert.ToInt32(dataRow["GZN_KnPNumer"].ToString());
                Int32 KnPTyp = Convert.ToInt32(dataRow["GZN_KnPTyp"].ToString());
                
                DateTime DataWystawienia = wygenerujDate(dataRow["GZN_DataWystawienia"].ToString());
                DateTime DataRozpoczecia = wygenerujDate(dataRow["GZN_DataRozpoczecia"].ToString());
                
                String Opis = dataRow["GZN_Opis"].ToString();

                srwZlcNag = new SrwZlcNagStruct(Id, KntTyp, KntNumer, KnATyp, KnANumer, KndTyp, KndNumer, KnPTyp, KnPNumer, DataWystawienia, DataRozpoczecia, Opis);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd podczas tworzenia struktury w funkcji Synchroniacja.wprowadzWierszSrwZkcNagDoXL():\n" + exc.Message, EventLogEntryType.Error);
            }

            Int32 result = wygenerujZlcSrwNag(srwZlcNag);

            if(result == 0)
            {
                dodajCzynnosci();
                dodajSkladniki();

                result = zmknijZlcSrwNag(srwZlcNag.Id);
                
                if(result == 0)
                {
                    dbSERWIS.oznaczZlcSrwNagZapisany(srwZlcNag.Id);
                }
            }
        }

        private DateTime wygenerujDate(String data)
        {
            String[] dataArray = data.Split('-', ':', ' ');
            DateTime date = new DateTime(Convert.ToInt32(dataArray[0]), Convert.ToInt32(dataArray[1]), Convert.ToInt32(dataArray[2]));
            return date;
        }

        private void dodajCzynnosci()
        {
        }

        private void dodajSkladniki()
        {
        }




        public Int32 wygenerujZlcSrwNag(SrwZlcNagStruct srwZlcNag)
        {
            int wynik = -100;
            try
            {
                cdn_api.XLSerwisNagInfo_20162 DokumentXLSerwisNagInfo = new XLSerwisNagInfo_20162();
                DokumentXLSerwisNagInfo.Wersja = 20162;
                DokumentXLSerwisNagInfo.Tryb = 2;

                DokumentXLSerwisNagInfo.Opis = srwZlcNag.Opis;

                DokumentXLSerwisNagInfo.KntTyp = srwZlcNag.KntTyp;
                DokumentXLSerwisNagInfo.KntNumer = srwZlcNag.KntNumer;

                DokumentXLSerwisNagInfo.KnATyp = srwZlcNag.KnATyp;
                DokumentXLSerwisNagInfo.KnANumer = srwZlcNag.KnANumer;

                DokumentXLSerwisNagInfo.KnDTyp = srwZlcNag.KndTyp;
                DokumentXLSerwisNagInfo.KnDNumer = srwZlcNag.KndNumer;

                DokumentXLSerwisNagInfo.KnPTyp = srwZlcNag.KnPTyp;
                DokumentXLSerwisNagInfo.KnPNumer = srwZlcNag.KnPNumer;

                wynik = cdn_api.cdn_api.XLNoweZlecenieSerwis(Sesja, ref IDDokSrwZlcNag, DokumentXLSerwisNagInfo);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji wygenerujZlcSrwNag(" + srwZlcNag.Id + "):\n" + exc.Message, EventLogEntryType.Error);
            }

            if(wynik != 0)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji wygenerujZlcSrwNag(" + srwZlcNag.Id + ") result = " + wynik, EventLogEntryType.Error);
            }
            return wynik;
        }

        public Int32 zmknijZlcSrwNag(Int32 srwZlcNagId)
        {
            cdn_api.XLZamkniecieSerwisNagInfo_20162 XLZamkniecieSerwisNagInfo = new XLZamkniecieSerwisNagInfo_20162();
            XLZamkniecieSerwisNagInfo.Wersja = 20162;
            XLZamkniecieSerwisNagInfo.TrybZamkniecia = 6;
            XLZamkniecieSerwisNagInfo.Akcja = 3;

            int wynik = cdn_api.cdn_api.XLZamknijZlecenieSerwis(ref IDDokSrwZlcNag, XLZamkniecieSerwisNagInfo);

            if(wynik != 0)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji zmknijZlcSrwNag(" + srwZlcNagId + ") result = " + wynik, EventLogEntryType.Error);
            }

            return wynik;
        }














    }
}
