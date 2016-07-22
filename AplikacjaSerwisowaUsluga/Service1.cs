using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private Timer timer;

        public Service1()
        {
            InitializeComponent();
            InitializeTimer();

            if(!System.Diagnostics.EventLog.SourceExists("AplikacjaSerwisowaUsluga"))
            {
                System.Diagnostics.EventLog.CreateEventSource("AplikacjaSerwisowaUsluga", "AplikacjaSerwisowaUsluga");
            }

            eventLog1.Source = "AplikacjaSerwisowaUsluga";
            eventLog1.Log = "AplikacjaSerwisowaUsluga";
        }

        protected override void OnStart(string[] args)
        {
            ApiConnect();
        }

        protected override void OnStop()
        {
            if(api_cdn_xl == null)
            {
                LogOut();
            }
            eventLog1.WriteEntry("Usługa zatrzymana");
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("Usługa wznowiona");

            if(api_cdn_xl == null)
            {
                ApiConnect();
            }
        }

        private void ApiConnect()
        {
            eventLog1.WriteEntry("Usługa uruchomiona - rozpoczęcie ApiConnect()");

            api_cdn_xl = new ApiXL(eventLog1);
            Int32 wynikLogowania = api_cdn_xl.APIConnect();

            eventLog1.WriteEntry("ApiConnect() result = " + wynikLogowania);
        }

        private void LogOut()
        {
            Int32 wynikWylogowania = api_cdn_xl.ApiLogout();
            eventLog1.WriteEntry("ApiLogout() result = " + wynikWylogowania);
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



            timer.Start();
        }
    }
}
