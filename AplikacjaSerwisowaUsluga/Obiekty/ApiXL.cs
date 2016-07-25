using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using cdn_api;

namespace AplikacjaSerwisowaUsluga
{
    class ApiXL2
    {
        private int IDDokSrwZlcNag = 0;
        private static Int32 Sesja;
        private EventLog eventlog;

        [DllImport("ClaRUN.dll")]
        public static extern void AttachThreadToClarion(bool x);

        public ApiXL2(EventLog _eventLog)
        {
            eventlog = _eventLog;
        }

        public Int32 APIConnect()
        {
            Hasla haslo = new Hasla(2);
            Sesja = 0;

            XLLoginInfo_20162 LogINFO_20162 = new XLLoginInfo_20162();
                        
            LogINFO_20162.Wersja = 20162;
            LogINFO_20162.ProgramID = "AplikacjaSerwisowaUsługa";
            LogINFO_20162.OpeIdent = haslo.GetOperatorXL();
            LogINFO_20162.Baza = haslo.GetBazaXL();
            LogINFO_20162.OpeHaslo = haslo.GetHasloXL();
            LogINFO_20162.PlikLog = "";
            LogINFO_20162.SerwerKlucza = "";
            LogINFO_20162.UtworzWlasnaSesje = 0;
            LogINFO_20162.TrybWsadowy = 1;

            eventlog.WriteEntry("haslo=" + haslo.GetHasloXL() + "\noperator=" + haslo.GetOperatorXL() + "\nbaza=" + haslo.GetBazaXL());

            AttachThreadToClarion(true);
            int WynikLogowania = cdn_api.cdn_api.XLLogin(LogINFO_20162, ref Sesja);

            if(WynikLogowania != 0)
            {
                eventlog.WriteEntry("Wystąpił błąd funkcji ApiXl.ApiConnect() result = " + WynikLogowania, EventLogEntryType.Error);
            }

            return WynikLogowania;
        }

        public Int32 ApiLogout()
        {
            int WynikWylogowania = cdn_api.cdn_api.XLLogout(Sesja);


            if(WynikWylogowania != 0 && WynikWylogowania != -1)
            {
                eventlog.WriteEntry("Wystąpił błąd funkcji ApiXl.ApiLogout() result = " + WynikWylogowania, EventLogEntryType.Error);
            }

            return WynikWylogowania;
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
                eventlog.WriteEntry("Wystąpił błąd funkcji ApiXL.wygenerujZlcSrwNag(" + srwZlcNag.Id + "):\n" + exc.Message, EventLogEntryType.Error);
            }

            if(wynik != 0)
            {
                eventlog.WriteEntry("Wystąpił błąd funkcji ApiXl.wygenerujZlcSrwNag(" + srwZlcNag.Id + ") result = " + wynik, EventLogEntryType.Error);
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
                eventlog.WriteEntry("Wystąpił błąd funkcji ApiXl.zmknijZlcSrwNag(" + srwZlcNagId + ") result = " + wynik, EventLogEntryType.Error);
            }

            return wynik;
        }

        public static int getsesja()
        {
            return Sesja;
        }
    }
}
