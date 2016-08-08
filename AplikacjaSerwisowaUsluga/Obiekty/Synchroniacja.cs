using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using cdn_api;

using Newtonsoft.Json;

namespace AplikacjaSerwisowaUsluga
{
    class Synchronizacja
    {
        private DataBase dbXL;
        private DataBase dbSERWIS;
        private EventLog eventLog;

        private int IDDokSrwZlcNag = 0;
        private static Int32 Sesja = 0;

        private AplikacjaSerwisowaUsluga.kwronski.WebService kwronskiService;

        [DllImport("ClaRUN.dll")]
        public static extern void AttachThreadToClarion(bool x);

        public Synchronizacja(DataBase _dbXL, DataBase _dbSERWIS, EventLog _eventLog)
        {
            dbXL = _dbXL;
            dbSERWIS = _dbSERWIS;
            eventLog = _eventLog;
            
            kwronskiService = new AplikacjaSerwisowaUsluga.kwronski.WebService();
        }

        public void Start()
        {
            synchronizacjaWebService();

            int wynik = APIConnect();
            if(wynik == 0)
            {
                dodajSrwZlcNag();



                ApiLogout();
            }
        }

        private void synchronizacjaWebService()
        {
            pobierzSrwZlcNag();
            pobierzSrwZlcCzynnosci();
            pobierzSrwZlcSkladniki();
        }

        private void pobierzSrwZlcNag()
        {
            List<Int32> listaZapisanych = new List<Int32>();
            try
            {
                eventLog.WriteEntry("przed naglowkami");
                String SrwZlcNagString = kwronskiService.GalSrv_SrwZlcNag();
                List<SrwZlcNag> records = JsonConvert.DeserializeObject<List<SrwZlcNag>>(SrwZlcNagString);
                eventLog.WriteEntry("Pobrane naglowki:" + records.Count.ToString());

                if(records!=null)
                {
                    for(int i = 0; i < records.Count; i++)
                    {
                        Boolean wynik = dbSERWIS.SrwZlcNag_InsertRecord(records[i]);

                        if(wynik)
                        {
                            listaZapisanych.Add(records[i].SZN_Id);
                        }
                    }
                    if(listaZapisanych.Count > 0)
                    {
                        potwierdzZapisanieSrwZlcNag(listaZapisanych);
                    }
                }
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji pobierzSrwZlcNag():\n"+exc.Message, EventLogEntryType.Error);
            }
        }

        private void potwierdzZapisanieSrwZlcNag(List<int> listaZapisanych)
        {
            try
            {
                String listaSrwZlcNagWprowadzone = JsonConvert.SerializeObject(listaZapisanych);
                eventLog.WriteEntry("Wysyłane zaptanie:" + listaSrwZlcNagWprowadzone);
                kwronskiService.GalSrv_SrwZlcNagPotwierdzenie(listaSrwZlcNagWprowadzone);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji potwierdzZapisanieSrwZlcNag():\n" + exc.Message, EventLogEntryType.Error);
            }
        }

        private void potwierdzZapisanieSrwZlcCzynnosci(List<int> listaZapisanych)
        {
            try
            {
                String listaSrwZlcCzynnosciWprowadzone = JsonConvert.SerializeObject(listaZapisanych);
                eventLog.WriteEntry("Wysyłane zaptanie:" + listaSrwZlcCzynnosciWprowadzone);
                kwronskiService.GalSrv_SrwZlcCzynnosciPotwierdzenie(listaSrwZlcCzynnosciWprowadzone);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji potwierdzZapisanieSrwZlcCzynnosci():\n" + exc.Message, EventLogEntryType.Error);
            }
        }

        private void potwierdzZapisanieSrwZlcSkladniki(List<int> listaZapisanych)
        {
            try
            {
                String listaSrwZlcSkladnikiWprowadzone = JsonConvert.SerializeObject(listaZapisanych);
                eventLog.WriteEntry("Wysyłane zaptanie:" + listaSrwZlcSkladnikiWprowadzone);
                kwronskiService.GalSrv_SrwZlcSkladnikiPotwierdzenie(listaSrwZlcSkladnikiWprowadzone);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji potwierdzZapisanieSrwZlcSkladniki():\n" + exc.Message, EventLogEntryType.Error);
            }
        }

        private void pobierzSrwZlcCzynnosci()
        {
            List<Int32> listaZapisanych = new List<Int32>();
            try
            {
                eventLog.WriteEntry("przed pobieraniem skladnikow");
                String SrwZlcCzynnosciString = kwronskiService.GalSrv_SrwZlcCzynnosci();
                eventLog.WriteEntry("skladnikow pobrane:" + SrwZlcCzynnosciString);
                List<SrwZlcCzynnosci> records = JsonConvert.DeserializeObject<List<SrwZlcCzynnosci>>(SrwZlcCzynnosciString);
                eventLog.WriteEntry("skladnikow pobrane rekordy:" + records.Count.ToString());

                if(records != null)
                {
                    for(int i = 0; i < records.Count; i++)
                    {
                        Boolean wynik = dbSERWIS.SrwZlcCzynnosci_InsertRecord(records[i]);

                        if(wynik)
                        {
                            listaZapisanych.Add(records[i].SZC_Id);
                        }
                    }
                    if(listaZapisanych.Count > 0)
                    {
                        potwierdzZapisanieSrwZlcCzynnosci(listaZapisanych);
                    }
                }
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji pobierzSrwZlcCzynnosci():\n" + exc.Message, EventLogEntryType.Error);
            }
        }

        private void pobierzSrwZlcSkladniki()
        {
            List<Int32> listaZapisanych = new List<Int32>();
            try
            {
                eventLog.WriteEntry("przed pobieraniem skladnikow");
                String SrwZlcSkladnikiString = kwronskiService.GalSrv_SrwZlcSkladniki();
                eventLog.WriteEntry("skladniki pobrane:" + SrwZlcSkladnikiString);
                List<SrwZlcSkladniki> records = JsonConvert.DeserializeObject<List<SrwZlcSkladniki>>(SrwZlcSkladnikiString);

                if(records != null)
                {
                    for(int i = 0; i < records.Count; i++)
                    {
                        Boolean wynik = dbSERWIS.SrwZlcSkladniki_InsertRecord(records[i]);

                        if(wynik)
                        {
                            listaZapisanych.Add(records[i].SZS_Id);
                        }
                    }
                    if(listaZapisanych.Count > 0)
                    {
                        potwierdzZapisanieSrwZlcSkladniki(listaZapisanych);
                    }
                }
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji pobierzSrwZlcSkladniki():\n" + exc.Message, EventLogEntryType.Error);
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
            SrwZlcNag srwZlcNag = new SrwZlcNag();
            try
            {
                Int32 GZN_Id = Convert.ToInt32(dataRow["GZN_Id"].ToString());
                Int32 GZN_Synchronizacja = Convert.ToInt32(dataRow["GZN_Synchronizacja"].ToString());
                Int32 GZN_KntNumer = Convert.ToInt32(dataRow["GZN_KntNumer"].ToString());
                Int32 GZN_KntTyp = Convert.ToInt32(dataRow["GZN_KntTyp"].ToString());
                Int32 GZN_KnANumer = Convert.ToInt32(dataRow["GZN_KnANumer"].ToString());
                Int32 GZN_KnATyp = Convert.ToInt32(dataRow["GZN_KnATyp"].ToString());
                String GZN_Dokument = dataRow["GZN_Dokument"].ToString();
                String GZN_DataWystawienia = dataRow["GZN_DataWystawienia"].ToString();
                String GZN_DataRozpoczecia = dataRow["GZN_DataRozpoczecia"].ToString();
                String GZN_Stan = dataRow["GZN_Stan"].ToString();
                String GZN_Status = dataRow["GZN_Status"].ToString();
                String GZN_Opis = dataRow["GZN_Opis"].ToString();

                srwZlcNag = new SrwZlcNag(GZN_Id, GZN_Synchronizacja, GZN_KntTyp, GZN_KntNumer, GZN_KnATyp, GZN_KnANumer, GZN_Dokument, GZN_DataWystawienia, GZN_DataRozpoczecia, GZN_Stan, GZN_Status, GZN_Opis);
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
            SrwZlcCzynnosci srwZlcCzynnosc = new SrwZlcCzynnosci();
            try
            {
                Int32 GZC_GZCId = Convert.ToInt32(dataRow["GZC_GZCId"].ToString());
                Int32 GZC_GZNId = Convert.ToInt32(dataRow["GZC_GZNId"].ToString());
                Int32 GZC_GZUId = Convert.ToInt32(dataRow["GZC_GZUId"].ToString());
                Int32 GZC_Synchronizacja = Convert.ToInt32(dataRow["GZC_Synchronizacja"].ToString());
                Int32 GZC_Pozycja = Convert.ToInt32(dataRow["GZC_Pozycja"].ToString());
                Int32 GZC_TwrTyp = Convert.ToInt32(dataRow["GZC_TwrTyp"].ToString());
                Int32 GZC_TwrNumer = Convert.ToInt32(dataRow["GZC_TwrNumer"].ToString());
                String GZC_TwrNazwa = dataRow["GZC_TwrNazwa"].ToString();
                Double GZC_Ilosc = Convert.ToDouble(dataRow["GZC_Ilosc"].ToString());
                String GZC_Opis = dataRow["GZC_Opis"].ToString();

                srwZlcCzynnosc = new SrwZlcCzynnosci(GZC_GZCId, GZC_GZNId, GZC_GZUId, GZC_Synchronizacja, GZC_Pozycja, GZC_TwrTyp, GZC_TwrNumer, GZC_TwrNazwa, GZC_Ilosc, GZC_Opis);
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
            SrwZlcSkladniki srwZlcSkladnik = new SrwZlcSkladniki();
            try
            {
                Int32 GZS_GZSId = Convert.ToInt32(dataRow["GZS_GZSId"].ToString());
                Int32 GZS_GZNId = Convert.ToInt32(dataRow["GZS_GZNId"].ToString());
                Int32 GZS_Synchronizacja = Convert.ToInt32(dataRow["GZS_Synchronizacja"].ToString());
                Int32 GZS_Pozycja = Convert.ToInt32(dataRow["GZS_Pozycja"].ToString());
                Int32 GZS_TwrTyp = Convert.ToInt32(dataRow["GZS_TwrTyp"].ToString());
                Int32 GZS_TwrNumer = Convert.ToInt32(dataRow["GZS_TwrNumer"].ToString());
                String GZS_TwrNazwa= dataRow["GZS_TwrNazwa"].ToString();
                Double GZS_Ilosc = Convert.ToDouble(dataRow["GZS_Ilosc"].ToString());
                String GZS_Opis = dataRow["GZS_Opis"].ToString();

                srwZlcSkladnik = new SrwZlcSkladniki(GZS_GZSId, GZS_GZNId, GZS_Synchronizacja, GZS_Pozycja, GZS_Ilosc, GZS_TwrNumer, GZS_TwrTyp, GZS_TwrNazwa, GZS_Opis);
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

        public Int32 wygenerujZlcSrwNag(SrwZlcNag srwZlcNag)
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

                DokumentXLSerwisNagInfo.Opis = srwZlcNag.SZN_Opis;

                DokumentXLSerwisNagInfo.KntTyp = srwZlcNag.SZN_KntTyp;
                DokumentXLSerwisNagInfo.KntNumer = srwZlcNag.SZN_KntNumer;

                DokumentXLSerwisNagInfo.KnATyp = srwZlcNag.SZN_KnATyp;
                DokumentXLSerwisNagInfo.KnANumer = srwZlcNag.SZN_KnANumer;

                DokumentXLSerwisNagInfo.KnDTyp = srwZlcNag.SZN_KntTyp;
                DokumentXLSerwisNagInfo.KnDNumer = srwZlcNag.SZN_KntTyp;

                DokumentXLSerwisNagInfo.KnPTyp = srwZlcNag.SZN_KntTyp;
                DokumentXLSerwisNagInfo.KnPNumer = srwZlcNag.SZN_KntNumer;

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

        private int wygenerujZlcSrwCzynnosc(SrwZlcCzynnosci srwZlcCzynnosc)
        {
            int wynik = -100;
            try
            {
                XLSerwisCzynnoscInfo_20162 DokumentXLSerwisCzynnoscInfo = new XLSerwisCzynnoscInfo_20162();
                DokumentXLSerwisCzynnoscInfo.Wersja = 20162;
                DokumentXLSerwisCzynnoscInfo.TwrTyp = srwZlcCzynnosc.SZC_TwrTyp;
                DokumentXLSerwisCzynnoscInfo.TwrNumer = srwZlcCzynnosc.SZC_TwrNumer;
                DokumentXLSerwisCzynnoscInfo.Ilosc = srwZlcCzynnosc.SZC_Ilosc.ToString();
                DokumentXLSerwisCzynnoscInfo.Opis = srwZlcCzynnosc.SZC_Opis;


                wynik = cdn_api.cdn_api.XLDodajCzynnoscSerwis(ref IDDokSrwZlcNag, DokumentXLSerwisCzynnoscInfo);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji wygenerujZlcSrwCzynnosc(" + srwZlcCzynnosc.SZC_Id + "):\n" + exc.Message, EventLogEntryType.Error);
            }

            if(wynik != 0)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji wygenerujZlcSrwCzynnosc(" + srwZlcCzynnosc.SZC_Id + ") result = " + wynik, EventLogEntryType.Error);
            }
            else
            {
                eventLog.WriteEntry("wygenerujZlcSrwCzynnosc(" + srwZlcCzynnosc.SZC_Id + ") iddokumentu = " + IDDokSrwZlcNag);
            }
            return wynik;
        }

        private int wygenerujZlcSrwSkladnik(SrwZlcSkladniki srwZlcSkladnik)
        {
            int wynik = -100;
            try
            {
                XLSerwisSkladnikInfo_20162 XlSerwisSkladnikInfo = new XLSerwisSkladnikInfo_20162();
                XlSerwisSkladnikInfo.Wersja = 20162;
                XlSerwisSkladnikInfo.TwrTyp = srwZlcSkladnik.SZS_TwrTyp;
                XlSerwisSkladnikInfo.TwrNumer = srwZlcSkladnik.SZS_TwrNumer;
                XlSerwisSkladnikInfo.Ilosc = srwZlcSkladnik.SZS_Ilosc.ToString();
                XlSerwisSkladnikInfo.Opis = srwZlcSkladnik.SZS_Opis;


                wynik = cdn_api.cdn_api.XLDodajSkladnikSerwis(ref IDDokSrwZlcNag, XlSerwisSkladnikInfo);
            }
            catch(Exception exc)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji wygenerujZlcSrwSkladnik(" + srwZlcSkladnik.SZS_Id + "):\n" + exc.Message, EventLogEntryType.Error);
            }

            if(wynik != 0)
            {
                eventLog.WriteEntry("Wystąpił błąd funkcji wygenerujZlcSrwSkladnik(" + srwZlcSkladnik.SZS_Id + ") result = " + wynik, EventLogEntryType.Error);
            }
            else
            {
                eventLog.WriteEntry("wygenerujZlcSrwSkladnik(" + srwZlcSkladnik.SZS_Id + ") iddokumentu = " + IDDokSrwZlcNag);
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
