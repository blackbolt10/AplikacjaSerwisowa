using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaSerwisowaUsluga
{
    class Synchroniacja
    {
        private ApiXL api_cdn_xl;
        private DataBase db;
        private EventLog eventLog;


        public Synchroniacja(DataBase _db, ApiXL _api_cdn_xl, EventLog _eventLog)
        {
            api_cdn_xl = _api_cdn_xl;
            db = _db;
            eventLog = _eventLog;
        }

        public void Start()
        {
            eventLog.WriteEntry("Synchronizacja start!");

            SrwZlcNagSynchronizacja();
            
        }

        private void SrwZlcNagSynchronizacja()
        {
            throw new NotImplementedException();
        }

        private void odczytajDaneZBazy()
        {
            DataTable noweZleceniaDT = db.pobierzNoweSrwZlc();

            if(noweZleceniaDT.Rows.Count>0)
            {
                for(int i=0;i<noweZleceniaDT.Rows.Count;i++)
                {
                    wprowadzWierszDoXL(noweZleceniaDT.Rows[i]);
                }
            }
        }

        private void wprowadzWierszDoXL(DataRow dataRow)
        {
            Int32 KntNumer = Convert.ToInt32(dataRow["KntNumer"].ToString());
            Int32 KntTyp = Convert.ToInt32(dataRow["KntTyp"].ToString());
            Int32 KnANumer = Convert.ToInt32(dataRow["KnANumer"].ToString());
            Int32 KnATyp = Convert.ToInt32(dataRow["KnATyp"].ToString());
            Int32 KndNumer = Convert.ToInt32(dataRow["KnANumer"].ToString());
            Int32 KndTyp = Convert.ToInt32(dataRow["KnATyp"].ToString());
            Int32 KnPNumer = Convert.ToInt32(dataRow["KnANumer"].ToString());
            Int32 KnPTyp = Convert.ToInt32(dataRow["KnATyp"].ToString());

            DateTime DataWystawienia = wygenerujDate(dataRow["DataWystawienia"].ToString());
            DateTime DataRozpoczecia = wygenerujDate(dataRow["DataRozpoczecia"].ToString());

            String Opis = dataRow["Opis"].ToString();

            SrwZlcNagStruct srwZlcNag = new SrwZlcNagStruct(KntTyp, KntNumer, KnATyp, KnANumer, KndTyp, KndNumer, KnPTyp, KnPNumer, DataWystawienia, DataRozpoczecia, Opis);

            Int32 result = api_cdn_xl.wygenerujZlcSrwNag(srwZlcNag);

            if(result == 0)
            {
                dodajCzynnosci();
                dodajSkladniki();

                api_cdn_xl.zmknijZlcSrwNag();
            }
        }

        private DateTime wygenerujDate(String data)
        {
            String[] dataArray = data.Split(':', ' ');
            DateTime date = new DateTime(Convert.ToInt32(dataArray[0]), Convert.ToInt32(dataArray[1]), Convert.ToInt32(dataArray[2]));
            return date;
        }

        private void dodajCzynnosci()
        {
        }

        private void dodajSkladniki()
        {
        }

        private void zamknijNaglowek()
        {
        }
    }
}
