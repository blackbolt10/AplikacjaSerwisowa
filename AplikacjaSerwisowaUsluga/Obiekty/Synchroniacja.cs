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
                dodajSrwZlcNag();



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

        private void dodajSrwZlcNag()
        {
            DataTable noweZleceniaDT = dbSERWIS.pobierzNoweSrwZlcNag();

            if(noweZleceniaDT.Rows.Count > 0)
            {
                for(int i = 0; i < noweZleceniaDT.Rows.Count; i++)
                {
                    wprowadzWierszSrwZlcNagDoXL(noweZleceniaDT.Rows[i]);
                }
            }
        }

        private void wprowadzWierszSrwZlcNagDoXL(DataRow dataRow)
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

            Int32 wynikWgenerujZlcSrwNag = wygenerujZlcSrwNag(srwZlcNag);

            if(wynikWgenerujZlcSrwNag == 0)
            {
                Boolean wynikDodawaniaCzynnosci = dodajSrwZlcCzynnosci(srwZlcNag.Id);
                Boolean wynikDodawaniaSkladnikow = dodajSrwZlcSkladniki(srwZlcNag.Id);

                if(wynikDodawaniaCzynnosci && wynikDodawaniaSkladnikow)
                {
                    Int32 wynikZmknijZlcSrwNag = zmknijZlcSrwNag(srwZlcNag.Id);

                    if(wynikZmknijZlcSrwNag == 0)
                    {
                        dbSERWIS.oznaczZlcSrwNagZapisany(srwZlcNag.Id, 1);
                    }
                }
                else
                {
                    if(!wynikDodawaniaCzynnosci)
                    {
                        dbSERWIS.oznaczZlcSrwNagZapisany(srwZlcNag.Id, 2);
                    }

                    if(!wynikDodawaniaSkladnikow)
                    {
                        dbSERWIS.oznaczZlcSrwNagZapisany(srwZlcNag.Id, 3);
                    }
                }
            }
        }

        private Boolean dodajSrwZlcCzynnosci(int GZN_Id)
        {
            DataTable noweCzynnosciZleceniaDT = dbSERWIS.pobierzSrwZlcCzynnosci(GZN_Id);
            Boolean wynikDodawaniaCzynnosci = true;

            if(noweCzynnosciZleceniaDT.Rows.Count > 0)
            {
                for(int i = 0; i < noweCzynnosciZleceniaDT.Rows.Count; i++)
                {
                    if(!wprowadzWierszSrwZlcCzynnosci(noweCzynnosciZleceniaDT.Rows[i]))
                    {
                        wynikDodawaniaCzynnosci = false;
                        break;
                    }
                }
            }
            return wynikDodawaniaCzynnosci;
        }

        private Boolean wprowadzWierszSrwZlcCzynnosci(DataRow dataRow)
        {
            SrwZlcCzynnosciStruct srwZlcCzynnosc = new SrwZlcCzynnosciStruct();
            try
            {
                Int32 Id = Convert.ToInt32(dataRow["GZC_Id"].ToString());
                Int32 Sync = Convert.ToInt32(dataRow["GZC_Sync"].ToString());
                Int32 GZNId = Convert.ToInt32(dataRow["GZC_GZNId"].ToString());
                Int32 Pozycja = Convert.ToInt32(dataRow["GZC_Pozycja"].ToString());
                Int32 TwrTyp = Convert.ToInt32(dataRow["GZC_TwrTyp"].ToString());
                Int32 TwrNumer = Convert.ToInt32(dataRow["GZC_TwrNumer"].ToString());

                String Ilosc = dataRow["GZC_Ilosc"].ToString();
                String Opis = dataRow["GZC_Opis"].ToString();

                srwZlcCzynnosc = new SrwZlcCzynnosciStruct(Id, Sync, GZNId, Pozycja, TwrTyp, TwrNumer, Ilosc, Opis);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd podczas tworzenia struktury w funkcji Synchroniacja.wprowadzWierszSrwZlcCzynnosci():\n" + exc.Message, EventLogEntryType.Error);
                return false;
            }

            Int32 wynikWgenerujZlcSrwCznnosc = wygenerujZlcSrwCzynnosc(srwZlcCzynnosc);

            if(wynikWgenerujZlcSrwCznnosc == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean dodajSrwZlcSkladniki(int GZN_Id)
        {
            DataTable noweSkladnikiZleceniaDT = dbSERWIS.pobierzSrwZlcSkladniki(GZN_Id);
            Boolean wynikDodawaniaSkladnikow = true;

            if(noweSkladnikiZleceniaDT.Rows.Count > 0)
            {
                for(int i = 0; i < noweSkladnikiZleceniaDT.Rows.Count; i++)
                {
                    if(!wprowadzWierszSrwZlcSkladniki(noweSkladnikiZleceniaDT.Rows[i]))
                    {
                        wynikDodawaniaSkladnikow = false;
                        break;
                    }
                }
            }
            return wynikDodawaniaSkladnikow;
        }

        private bool wprowadzWierszSrwZlcSkladniki(DataRow dataRow)
        {
            SrwZlcSkladnikiStruct srwZlcSkladnik = new SrwZlcSkladnikiStruct();
            try
            {
                Int32 Id = Convert.ToInt32(dataRow["GZS_Id"].ToString());
                Int32 Sync = Convert.ToInt32(dataRow["GZS_Sync"].ToString());
                Int32 GZNId = Convert.ToInt32(dataRow["GZS_GZNId"].ToString());
                Int32 Pozycja = Convert.ToInt32(dataRow["GZS_Pozycja"].ToString());
                Int32 TwrTyp = Convert.ToInt32(dataRow["GZS_TwrTyp"].ToString());
                Int32 TwrNumer = Convert.ToInt32(dataRow["GZS_TwrNumer"].ToString());

                String Ilosc = dataRow["GZS_Ilosc"].ToString();
                String Opis = dataRow["GZS_Opis"].ToString();

                srwZlcSkladnik = new SrwZlcSkladnikiStruct(Id, Sync, GZNId, Pozycja, TwrTyp, TwrNumer, Ilosc, Opis);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd podczas tworzenia struktury w funkcji Synchroniacja.wprowadzWierszSrwZlcSkladniki():\n" + exc.Message, EventLogEntryType.Error);
                return false;
            }

            Int32 wynikWgenerujZlcSrwSkladnik = wygenerujZlcSrwSkladnik(srwZlcSkladnik);

            if(wynikWgenerujZlcSrwSkladnik == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Int32 wygenerujZlcSrwNag(SrwZlcNagStruct srwZlcNag)
        {
            int wynik = -100;
            try
            {
                cdn_api.XLSerwisNagInfo_20162 DokumentXLSerwisNagInfo = new XLSerwisNagInfo_20162();
                DokumentXLSerwisNagInfo.Wersja = 20162;
                DokumentXLSerwisNagInfo.Tryb = 2;

                /*
                DokumentXLSerwisNagInfo.DataRozpoczecia = zwrocDateClarion(srwZlcNag.DataRozpoczecia);
                DokumentXLSerwisNagInfo.DataWystawienia = zwrocDateClarion(srwZlcNag.DataWystawienia);
                DokumentXLSerwisNagInfo.Rok = srwZlcNag.DataWystawienia.Year;
                DokumentXLSerwisNagInfo.Miesiac = srwZlcNag.DataWystawienia.Month;
                */

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
            else
            {
                eventLog.WriteEntry("wygenerujZlcSrwNag(" + srwZlcNag.Id + ") iddokumentu = " + IDDokSrwZlcNag);
            }

            return wynik;
        }
        
        private DateTime wygenerujDate(String data)
        {
            String[] dataArray = data.Split('-', ':', ' ');
            DateTime date = new DateTime(Convert.ToInt32(dataArray[0]), Convert.ToInt32(dataArray[1]), Convert.ToInt32(dataArray[2]));
            return date;
        }

        private int wygenerujZlcSrwCzynnosc(SrwZlcCzynnosciStruct srwZlcCzynnosc)
        {
            int wynik = -100;
            try
            {
                XLSerwisCzynnoscInfo_20162 DokumentXLSerwisCzynnoscInfo = new XLSerwisCzynnoscInfo_20162();
                DokumentXLSerwisCzynnoscInfo.Wersja = 20162;
                DokumentXLSerwisCzynnoscInfo.TwrTyp = srwZlcCzynnosc.GZC_TwrTyp;
                DokumentXLSerwisCzynnoscInfo.TwrNumer = srwZlcCzynnosc.GZC_TwrNumer;
                DokumentXLSerwisCzynnoscInfo.Ilosc = srwZlcCzynnosc.GZC_Ilosc;
                DokumentXLSerwisCzynnoscInfo.Opis = srwZlcCzynnosc.GZC_Opis;


                wynik = cdn_api.cdn_api.XLDodajCzynnoscSerwis(ref IDDokSrwZlcNag, DokumentXLSerwisCzynnoscInfo);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji wygenerujZlcSrwCzynnosc(" + srwZlcCzynnosc.GZC_Id + "):\n" + exc.Message, EventLogEntryType.Error);
            }

            if(wynik != 0)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji wygenerujZlcSrwCzynnosc(" + srwZlcCzynnosc.GZC_Id + ") result = " + wynik, EventLogEntryType.Error);
            }
            else
            {
                eventLog.WriteEntry("wygenerujZlcSrwCzynnosc(" + srwZlcCzynnosc.GZC_Id + ") iddokumentu = " + IDDokSrwZlcNag);
            }
            return wynik;
        }

        private int wygenerujZlcSrwSkladnik(SrwZlcSkladnikiStruct srwZlcSkladnik)
        {
            int wynik = -100;
            try
            {
                XLSerwisSkladnikInfo_20162 XlSerwisSkladnikInfo = new XLSerwisSkladnikInfo_20162();
                XlSerwisSkladnikInfo.Wersja = 20162;
                XlSerwisSkladnikInfo.TwrTyp = srwZlcSkladnik.GZS_TwrTyp;
                XlSerwisSkladnikInfo.TwrNumer = srwZlcSkladnik.GZS_TwrNumer;
                XlSerwisSkladnikInfo.Ilosc = srwZlcSkladnik.GZS_Ilosc;
                XlSerwisSkladnikInfo.Opis = srwZlcSkladnik.GZS_Opis;


                wynik = cdn_api.cdn_api.XLDodajSkladnikSerwis(ref IDDokSrwZlcNag, XlSerwisSkladnikInfo);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji wygenerujZlcSrwSkladnik(" + srwZlcSkladnik.GZS_Id + "):\n" + exc.Message, EventLogEntryType.Error);
            }

            if(wynik != 0)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji wygenerujZlcSrwSkladnik(" + srwZlcSkladnik.GZS_Id + ") result = " + wynik, EventLogEntryType.Error);
            }
            else
            {
                eventLog.WriteEntry("wygenerujZlcSrwSkladnik(" + srwZlcSkladnik.GZS_Id + ") iddokumentu = " + IDDokSrwZlcNag);
            }
            return wynik;
        }


        private int zwrocDateClarion(DateTime dataRozpoczecia)
        {
            DateTime date = new DateTime(1800, 12, 28, 0, 0, 0);
            int daysSince = (DateTime.Now - date).Days;
            return daysSince;
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
                eventLog.WriteEntry("Wystąpił błąd funkcji zmknijZlcSrwNag(" + srwZlcNagId + ") = " + wynik+", ID zamkanego dokument ="+IDDokSrwZlcNag, EventLogEntryType.Error);
            }

            return wynik;
        }














    }
}
