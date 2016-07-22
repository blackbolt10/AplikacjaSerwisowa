using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using cdn_api;

namespace AplikacjaSerwisowaUsluga
{
    class ApiXL
    {
        private Int32 Sesja;
        private EventLog eventlog;

        public ApiXL(EventLog _eventLog)
        {
            eventlog = _eventLog;
        }

        public Int32 APIConnect()
        {
            Hasla haslo = new Hasla();

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
            
            int WynikLogowania = cdn_api.cdn_api.XLLogin(LogINFO_20162, ref Sesja);
            

            return WynikLogowania;
        }
        public Int32 ApiLogout()
        {
            int WynikWylogowania = cdn_api.cdn_api.XLLogout(Sesja);
            return WynikWylogowania;
        }

    }
}
