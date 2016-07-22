using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace AplikacjaSerwisowaUsluga
{
    public partial class Service1 : ServiceBase
    {
        private ApiXL api_cdn_xl = null;
        private DataBase db = null;
        private Timer timer;

        public Service1()
        {
            InitializeComponent();

            InitializeTimer();
            podlaczDoBazyDanych();
            InitializeEventLog();
        }

        protected override void OnStart(string[] args)
        {
            ApiConnect();
            timer.Start();
        }

        protected override void OnStop()
        {
            if(api_cdn_xl != null)
            {
                LogOut();
            }
            eventLog1.WriteEntry("OnStop() - Usługa zatrzymana");
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("OnContinue() - Usługa wznowiona");

            if(api_cdn_xl == null)
            {
                ApiConnect();
            }
        }

        private void InitializeEventLog()
        {
            if(!System.Diagnostics.EventLog.SourceExists("AplikacjaSerwisowaUsluga"))
            {
                System.Diagnostics.EventLog.CreateEventSource("AplikacjaSerwisowaUsluga", "AplikacjaSerwisowaUsluga");
            }

            eventLog1.Source = "AplikacjaSerwisowaUsluga";
            eventLog1.Log = "AplikacjaSerwisowaUsluga";
        }

        private void podlaczDoBazyDanych()
        {
            db = new DataBase(eventLog1);
            Boolean wynikPolaczenia = db.PolaczZBaza();

            if(wynikPolaczenia)
            {
                eventLog1.WriteEntry("podlaczDoBazyDanych() result = " + wynikPolaczenia);
            }
            else
            {
                eventLog1.WriteEntry("Service1.podlaczDoBazyDanych() result = " + wynikPolaczenia, EventLogEntryType.Warning);
                eventLog1.WriteEntry("AplikacjaSerwisowaUsluga - rozpoczynam zatrzymanie", EventLogEntryType.Error);
                this.Stop();
            }
        }

        private void ApiConnect()
        {
            eventLog1.WriteEntry("Usługa uruchomiona - rozpoczęcie ApiConnect()");

            api_cdn_xl = new ApiXL(eventLog1);
            Int32 wynikLogowania = api_cdn_xl.APIConnect();
            

            if(wynikLogowania != 0)
            {
                eventLog1.WriteEntry("ApiConnect() result = " + wynikLogowania, EventLogEntryType.Warning);
                eventLog1.WriteEntry("AplikacjaSerwisowaUsluga - rozpoczynam zatrzymanie", EventLogEntryType.Error);
                this.Stop();
            }
            else
            {
                eventLog1.WriteEntry("ApiConnect() result = " + wynikLogowania);
            }
        }

        private void LogOut()
        {
            Int32 wynikWylogowania = api_cdn_xl.ApiLogout();

            if(wynikWylogowania != 0 && wynikWylogowania != -1)
            {
                eventLog1.WriteEntry("ApiLogout() result = " + wynikWylogowania, EventLogEntryType.Warning);
            }
            else
            {
                eventLog1.WriteEntry("ApiLogout() result = " + wynikWylogowania);
            }
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 10000;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            Synchroniacja synch = new Synchroniacja(db, api_cdn_xl, eventLog1);
            synch.Start();

            timer.Start();
        }
    }
}
