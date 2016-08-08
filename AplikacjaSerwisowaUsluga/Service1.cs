using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;

using cdn_api;

namespace AplikacjaSerwisowaUsluga
{
    public partial class Service1 : ServiceBase
    {
        private DataBase dbXL = null;
        private DataBase dbSERWIS = null;
        private System.Timers.Timer timer;
        
        public Service1()
        {
            InitializeComponent();
            InitializeEventLog();
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("OnStart() - Usługa uruchomiona");

            InitializeTimer();
            podlaczDoBazyDanych(0);
            podlaczDoBazyDanych(1);

            timer.Start();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("OnStop() - Usługa zatrzymana");
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("OnContinue() - Usługa jest wznowiana");
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

        private void podlaczDoBazyDanych(int baza)
        {
            Boolean wynikPolaczenia = false;
            if(baza == 0)
            {
                dbXL = new DataBase(eventLog1);
                wynikPolaczenia = dbXL.PolaczZBaza(baza);
            }
            else
            {
                dbSERWIS = new DataBase(eventLog1);
                wynikPolaczenia = dbSERWIS.PolaczZBaza(baza);
            }

            if(wynikPolaczenia)
            {
                //zalogowano do bazy
               // eventLog1.WriteEntry("podlaczDoBazyDanych("+ baza + ") result = " + wynikPolaczenia);
            }
            else
            {
                eventLog1.WriteEntry("Service1.podlaczDoBazyDanych(" + baza + ") result = " + wynikPolaczenia, EventLogEntryType.Warning);
                eventLog1.WriteEntry("AplikacjaSerwisowaUsluga - rozpoczynam zatrzymanie", EventLogEntryType.Error);
                this.Stop();
            }
        }

        private void InitializeTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 10000;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            eventLog1.WriteEntry("Synchronizacja start!");

            Synchronizacja synch = new Synchronizacja(dbXL, dbSERWIS, eventLog1);
            synch.Start();

            eventLog1.WriteEntry("Synchronizacja end!");
            timer.Start();
        }
    }
}