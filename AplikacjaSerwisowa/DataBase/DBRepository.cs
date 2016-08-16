using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Net;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AplikacjaSerwisowa
{
    public class DBRepository
    {
        private String dbPath;
        private SQLiteConnection db;

        public DBRepository()
        {
            dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");
            db = new SQLiteConnection(dbPath);
        }




        public string createDB()
        {
            String output = "";
            try
            {
                output += "creating database if it doesn't exist";
                String dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");

                SQLiteConnection db = new SQLiteConnection(dbPath);
                output = "Database created...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.createDB() Error: " + exc.Message;
            }

            return output;
        }

        public string OperatorzyTable_CreateTable()
        {
            String output = "";
            try
            {
                try
                {
                    db.DropTable<OperatorzyTable>();
                }
                catch (Exception) { }

                db.CreateTable<OperatorzyTable>();

                output = "Table Created succsessfully..";
            }
            catch (Exception exc)
            {
                output = "DBRepository.CreateTable() Error: " + exc.Message;
            }

            return output;
        }

        public String OperatorzyTable_InsertRecord(OperatorzyTable item)
        {
            String output = "";
            try
            {        
                db.Insert(item);
                output = "Record added..";
            }
            catch (Exception exc)
            {
                output = "DBRepository.InsertRecord() Error: " + exc.Message;
            }

            return output;
        }

        public String OperatorzyTable_GetAllRecords()
        {
            String output = "";

            try
            {
                TableQuery<OperatorzyTable> table = db.Table<OperatorzyTable>();

                foreach(OperatorzyTable item in table)
                {
                    output += "\n"+item.Id+": A:"+item.Akronim + ", H:" +item.Haslo + ", N:" +item.Nazwisko + ", I:" +item.Imie;
                }
            }
            catch (Exception exc)
            {
                output += "DBRepository.GetAllRecords() Error: " + exc.Message;
            }

            return output;
        }

        public String Login(String akronim, String haslo)
        {
            String output = "";
            if (akronim.Length > 0)
            {
                try
                {
                    var test = db.Query<OperatorzyTable>("select Haslo from OperatorzyTable where Akronim = '" + akronim + "'");

                    if (test.Count > 0)
                    {
                        if (test[0].Haslo == haslo)
                        {
                            output += "1";
                        }
                        else
                        {
                            output = "Has³o jest niepoprawne";
                        }
                    }
                    else
                    {
                        output = "Nie znaleziono akronimu";
                    }
                }
                catch (Exception exc)
                {
                    output = "DBRepository.Login() Error: " + exc.Message;
                }
            }
            else
            {
                output = "Akronim nie mo¿e byæ pusty";
            }

            return output;
        }






        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela towarów********************************|
            *---------------------------------------------------------------------------------
        */
        public string stworzKartyTowarowTabele()
        {
            String output = "";
            try
            {
                try
                {
                    db.DropTable<kartyTowarowTable>();
                }
                catch (Exception) { }

                db.CreateTable<kartyTowarowTable>();

                output = "Tabela towarów zosta³a stworzona...";
            }
            catch (Exception exc)
            {
                output = "DBRepository.stworzTabeleTowarow() Error: " + exc.Message;
            }

            return output;
        }
        public String kartyTowarow_InsertRecord(kartyTowarowTable item)
        {
            String output = "";
            try
            {
                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch (Exception exc)
            {
                output = "DBRepository.kartyTowarow_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }

        public KntAdresyTable sprawdzLiczbeAdresow(Int32 KNT_GIDNumer)
        {
            KntAdresyTable KNTAdres = null;

            try
            {
                List<KntAdresyTable> result = kntAdresy_GetFilteredRecords("", KNT_GIDNumer.ToString());
                if(result != null)
                {
                    if(result.Count == 1)
                    {
                        KNTAdres = result[0];
                    }
                }
            }
            catch(Exception) {}

            return KNTAdres;
        }

        public String kartyTowarow_GetAllRecords()
        {
            String output = "";

            try
            {
                var table = db.Table<kartyTowarowTable>();

                foreach (var item in table)
                {
                    output += item.Id + ": GIDNum:" + item.TWR_GIDNumer + ", T:" + item.TWR_Typ + ", K:" + item.TWR_Kod + ", N:" + item.TWR_Nazwa + "\n";
                }
            }
            catch (Exception exc)
            {
                output += "DBRepository.kartyTowarow_GetAllRecords() Error: " + exc.Message;
            }

            return output;
        }











        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela kontrahentow***************************|
            *---------------------------------------------------------------------------------
        */

        public string stworzKntKartyTabele()
        {
            String output = "";
            try
            {
                try
                {
                    db.DropTable<KntKartyTable>();
                }
                catch(Exception) { }

                db.CreateTable<KntKartyTable>();

                output = "Tabela kontrahentow zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzKntKartyTabele() Error: " + exc.Message;
            }

            return output;
        }
        public String kntKarty_InsertRecord(KntKartyTable item)
        {
            String output = "";
            try
            {
                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.kntKarty_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }
        public String kntKarty_GetAllRecords()
        {
            String output = "";

            try
            {
                var table = db.Table<KntKartyTable>();

                foreach(var item in table)
                {
                    output += item.Id + ": GIDNum:" + item.Knt_GIDNumer + ", A:" + item.Knt_Akronim + "\n";
                }
            }
            catch(Exception exc)
            {
                output += "DBRepository.kntKarty_GetAllRecords() Error: " + exc.Message;
            }

            return output;
        }

        public KntKartyTable kntKarty_GetRecord(String knt_GidNumer)
        {
            KntKartyTable output = null;

            if(knt_GidNumer.Length > 0)
            {
                try
                {
                    var table = db.Table<KntKartyTable>();

                    var result = db.Query<KntKartyTable>("select * from KntKartyTable where Knt_GIDNumer = " + knt_GidNumer);

                    if(result.Count > 0)
                    {
                        output = result[0];
                    }
                }
                catch(Exception) {}
            }

            return output;
        }

        private List<int> kntKarty_GetFiltredGidNumers(string filtr)
        {
            List<int> kntKartyList = new List<int>();
            
            try
            {
                List<KntKartyTable> result = db.Query<KntKartyTable>("select * from KntKartyTable where Knt_Akronim like '%" + filtr + "%'");

                if(result != null)
                {
                    foreach(var item in result)
                    {
                        kntKartyList.Add(item.Knt_GIDNumer);
                    }
                }
            }
            catch(Exception) {}

            return kntKartyList;
        }

        public List<KntKartyTable> kntKarty_GetFilteredRecords(String filtr)
        {
            List<KntKartyTable> output = new List<KntKartyTable>();
            try
            {
                string zapytanie = "select * from KntKartyTable ";

                if(filtr !="")
                {
                    zapytanie+= "where Knt_Akronim like '%" + filtr + "%' or Knt_nazwa1 like '%" + filtr + "%' or Knt_nazwa2 like '%" + filtr + "%' or Knt_nazwa3 like '%" + filtr + "%'";
                }

                output = db.Query<KntKartyTable>(zapytanie);
            }
            catch(Exception) {}

            return output;
        }









        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela kontrahentowAdresy*********************|
            *---------------------------------------------------------------------------------
        */

        public string stworzKntAdresyTabele()
        {
            String output = "";
            try
            {
                try
                {
                    db.DropTable<KntAdresyTable>();
                }
                catch(Exception) { }

                db.CreateTable<KntAdresyTable>();

                output = "Tabela kontrahentow adresy zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzKntAdresyTabele() Error: " + exc.Message;
            }

            return output;
        }
        public String kntAdresy_InsertRecord(KntAdresyTable item)
        {
            String output = "";
            try
            {
                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.kntAdresy_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }
        public String kntAdresy_GetAllRecords()
        {
            String output = "";

            try
            {
                var table = db.Table<KntAdresyTable>();

                foreach(var item in table)
                {
                    output += item.Id + ": GIDNum:" + item.Kna_GIDNumer + ", KntNumer:" + item.Kna_KntNumer + ", A:" + item.Kna_Akronim + "\n";
                }
            }
            catch(Exception exc)
            {
                output += "DBRepository.kntAdresy_GetAllRecords() Error: " + exc.Message;
            }

            return output;
        }

        public KntAdresyTable kntAdresy_GetRecord(String kna_GIDNumer)
        {
            KntAdresyTable output = null;

            if(kna_GIDNumer.Length > 0)
            {
                try
                {
                    var table = db.Table<KntAdresyTable>();

                    var result = db.Query<KntAdresyTable>("select * from KntAdresyTable where kna_GIDNumer = " + kna_GIDNumer);

                    if(result.Count > 0)
                    {
                        output = result[0];
                    }
                }
                catch(Exception)
                {
                }
            }

            return output;
        }

        public List<KntAdresyTable> kntAdresy_GetFilteredRecords(String filtr, String knt_GidNumer)
        {
            List<KntAdresyTable> output = new List<KntAdresyTable>();
            try
            {
                var table = db.Table<KntAdresyTable>();
                string zapytanie = "select * from KntAdresyTable where Kna_KntNumer = "+ knt_GidNumer + " ";

                if(filtr != "")
                {
                    zapytanie += "and (Kna_Akronim like '%" + filtr + "%' or Kna_nazwa1 like '%" + filtr + "%' or Kna_nazwa2 like '%" + filtr + "%' or Kna_nazwa3 like '%" + filtr + "%')";
                }

                var result = db.Query<KntAdresyTable>(zapytanie);

                for(int i = 0; i < result.Count; i++)
                {
                    output.Add(result[i]);
                }
            }
            catch(Exception)
            {
            }

            return output;
        }



        private List<int> kntAdresy_GetFiltredGidNumers(string filtr)
        {
            List<int> kntAdresyList = new List<int>();

            try
            {
                string zapytanie = "select * from KntAdresyTable where Kna_Akronim like '&" + filtr + "&'";
                List<KntAdresyTable> result = db.Query<KntAdresyTable>(zapytanie);

                foreach(var item in result)
                {
                    kntAdresyList.Add(item.Kna_GIDNumer);
                }
            }
            catch(Exception) {}

            return kntAdresyList;
        }














        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SerwisoweZleceniaNaglowki**************|
            *---------------------------------------------------------------------------------
        */

        public string stworzSrwZlcNag()
        {
            String output = "";
            try
            {
                try
                {
                    db.DropTable<SrwZlcNag>();
                }
                catch(Exception) { }

                db.CreateTable<SrwZlcNag>();

                output = "Tabela SrwZlcNag zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcNag() Error: " + exc.Message;
            }

            return output;
        }
        public String SrwZlcNag_InsertRecord(SrwZlcNag item)
        {
            String output = "";
            try
            {
                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcNag_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }
        
        public void SrwZlcNag_OznaczWyslane(List<int> wyslaneNagList, int wslaneParam)
        {
            for(int i = 0; i < wyslaneNagList.Count; i++)
            {
                var result = db.Query<SrwZlcNag>("UPDATE SrwZlcNag SET SZN_Synchronizacja = " + wslaneParam.ToString() + " where SZN_Id = " + wyslaneNagList[i].ToString());
            }
        }
        public void SrwZlcNag_OznaczWyslane(List<SrwZlcNag> wyslaneNagList, int wslaneParam)
        {
            for(int i = 0; i < wyslaneNagList.Count; i++)
            {
                var result = db.Query<SrwZlcNag>("UPDATE SrwZlcNag SET SZN_Synchronizacja = " + wslaneParam.ToString() + " where SZN_Id = " + wyslaneNagList[i].SZN_Id.ToString());
            }
        }

        public SrwZlcNag SrwZlcNag_GetRecordGetRecord(String szn_ID)
        {
            SrwZlcNag output = null;

            if(szn_ID.Length > 0)
            {
                try
                {
                    var table = db.Table<SrwZlcNag>();

                    var result = db.Query<SrwZlcNag>("select * from SrwZlcNag where SZN_Id = " + szn_ID);

                    if(result.Count > 0)
                    {
                        output = result[0];
                    }
                }
                catch(Exception)
                {
                }
            }

            return output;
        }

        public List<SrwZlcNag> pobierzListeSrwZlcNag(String filtr)
        {
            List<SrwZlcNag> output = new List<SrwZlcNag>();
            try
            {
                List<int> kntKarty = kntKarty_GetFiltredGidNumers(filtr);
                String kntKartyString = generujListeGuidNumer(kntKarty);

                List<int> kntAdresy = kntAdresy_GetFiltredGidNumers(filtr);
                String kntAdresyString = generujListeGuidNumer(kntKarty);

                String zapParam = "";

                if(filtr != "")
                {
                   zapParam = " where SZN_Dokument like '%"+filtr+ "%' or SZN_DataWystawienia like '%" + filtr + "%' or SZN_KntNumer in "+kntKartyString+ " or SZN_KnANumer in "+kntAdresyString;
                }

                List<SrwZlcNag> result = db.Query<SrwZlcNag>("select * from SrwZlcNag "+ zapParam +" order by SZN_DataWystawienia desc");

                if(result != null)
                {
                    output = result;
                }
            }
            catch(Exception) { }

            return output;
        }

        private string generujListeGuidNumer(List<int> kntKarty)
        {
            string result = "(-1";
            for(int i = 0;i<kntKarty.Count;i++)
            {
                result += ", " + kntKarty[i].ToString();
            }
            result += ")";
            return result;
        }

        public List<SrwZlcNag> SrwZlcNagSynchronizacja(int synchParam)
        {
            List<SrwZlcNag> srwZlcNagList = new List<SrwZlcNag>();

            try
            {
                srwZlcNagList = db.Query<SrwZlcNag>("select * from SrwZlcNag where  SZN_Synchronizacja = " + synchParam.ToString());
            }
            catch(Exception)
            {
            }
            return srwZlcNagList;
            //return lamadodajDosynch();
        }

        public int SrwZlcNagGenerujNoweID(Context kontekst)
        {
            int result = Odczyt(kontekst);
            result = result - 1;
            zapisDanychDoPamieciUrzadzenia(result, kontekst);
            
            return result;
        }
        private void zapisDanychDoPamieciUrzadzenia(Int32 SZN_ID, Context kontekst)
        {
            ISharedPreferences prefs = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(kontekst);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutInt("SZN_ID", SZN_ID);
            editor.Apply();
        }
        private Int32 Odczyt(Context kontekst)
        {
            ISharedPreferences prefs = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(kontekst);

            Int32 result = prefs.GetInt("SZN_ID", -1);
            return result;
        }






        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwZlcCzynnosci************************|
            *---------------------------------------------------------------------------------
        */

        public string stworzSrwZlcCzynnosci()
        {
            String output = "";
            try
            {
                try
                {
                    db.DropTable<SrwZlcCzynnosci>();
                }
                catch(Exception) { }

                db.CreateTable<SrwZlcCzynnosci>();

                output = "Tabela SrwZlcCzynnosci zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcCynnosciTable() Error: " + exc.Message;
            }

            return output;
        }

        public String SrwZlcCzynnosci_InsertRecord(SrwZlcCzynnosci item)
        {
            String output = "";
            try
            {
                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcCynnosci_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }

        public List<SrwZlcCzynnosci> SrwZlcCzynnosci_GetRecords(String szn_ID)
        {
            List<SrwZlcCzynnosci> output = new List<SrwZlcCzynnosci>();

            if(szn_ID.Length > 0)
            {
                try
                {
                    var table = db.Table<SrwZlcCzynnosci>();

                    var result = db.Query<SrwZlcCzynnosci>("select * from SrwZlcCzynnosci where szc_sznId = " + szn_ID);

                    if(result.Count > 0)
                    {
                        for(int i=0;i<result.Count;i++)
                        {
                            SrwZlcCzynnosci szc = new SrwZlcCzynnosci();
                            szc.SZC_Id = result[i].SZC_Id;
                            szc.SZC_SZNId = result[i].SZC_SZNId;
                            szc.SZC_SZUId = result[i].SZC_SZUId;
                            szc.SZC_Synchronizacja = result[i].SZC_Synchronizacja;
                            szc.SZC_Pozycja = result[i].SZC_Pozycja;
                            szc.SZC_TwrTyp = result[i].SZC_TwrTyp;
                            szc.SZC_TwrNumer = result[i].SZC_TwrNumer;
                            szc.SZC_TwrNazwa = result[i].SZC_TwrNazwa;
                            szc.SZC_Ilosc = result[i].SZC_Ilosc;
                            szc.SZC_Opis = result[i].SZC_Opis;

                            output.Add(szc);
                        }
                    }
                }
                catch(Exception)
                {
                }
            }

            return output;
        }

        public List<SrwZlcCzynnosci> SrwZlcCzynnosci_GetFilteredRecords(String filtr)
        {
            List<SrwZlcCzynnosci> output = new List<SrwZlcCzynnosci>();
            try
            {
                var table = db.Table<SrwZlcCzynnosci>();
                string zapytanie = "select * from SrwZlcCzynnosci ";

                if(filtr != "")
                {
                    zapytanie += "where szc_TwrNazwa like '%" + filtr + "%' or Twr_Kod like '%" + filtr + "%'";
                }

                var result = db.Query<SrwZlcCzynnosci>(zapytanie);

                for(int i = 0; i < result.Count; i++)
                {
                    output.Add(result[i]);
                }
            }
            catch(Exception)
            {
            }

            return output;
        }

        public List<SrwZlcCzynnosci> SrwZlcCzynnosciSynchronizacja(int synchParam)
        {
            List<SrwZlcCzynnosci> srwZlcCzynnosciList = new List<SrwZlcCzynnosci>();

            try
            {
                srwZlcCzynnosciList = db.Query<SrwZlcCzynnosci>("select * from SrwZlcCzynnosci where SZC_Synchronizacja = " + synchParam.ToString());
            }
            catch(Exception)
            {}

            return srwZlcCzynnosciList;
        }

        public void SrwZlcCzynnosci_OznaczWyslane(List<int> wyslaneCzynnosciList, int wslaneParam)
        {
            for(int i = 0; i < wyslaneCzynnosciList.Count; i++)
            {
                var result = db.Query<SrwZlcCzynnosci>("UPDATE SrwZlcCzynnosci SET SZC_Synchronizacja = " + wslaneParam.ToString() + " where szc_sznId = " + wyslaneCzynnosciList[i].ToString());
            }
        }
        public void SrwZlcCzynnosci_OznaczWyslane(List<SrwZlcCzynnosci> wyslaneCzynnosciList, int wslaneParam)
        {
            for(int i = 0; i < wyslaneCzynnosciList.Count; i++)
            {
                var result = db.Query<SrwZlcCzynnosci>("UPDATE SrwZlcCzynnosci SET SZC_Synchronizacja = " + wslaneParam.ToString() + " where szc_sznId = " + wyslaneCzynnosciList[i].SZC_Id.ToString());
            }
        }










        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwZlcCzynnosci************************|
            *---------------------------------------------------------------------------------
        */

        public string stworzSrwZlcSkladniki()
        {
            String output = "";
            try
            {
                try
                {
                    db.DropTable<SrwZlcSkladniki>();
                }
                catch(Exception) { }

                db.CreateTable<SrwZlcSkladniki>();

                output = "Tabela SrwZlcSkladniki zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcSkladniki() Error: " + exc.Message;
            }

            return output;
        }
        public String SrwZlcSkladniki_InsertRecord(SrwZlcSkladniki item)
        {
            String output = "";
            try
            {
                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcSkladniki_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }

        public List<SrwZlcSkladniki> SrwZlcSkladniki_GetRecords(String szn_ID)
        {
            List<SrwZlcSkladniki> output = new List<SrwZlcSkladniki>();

            if(szn_ID.Length > 0)
            {
                try
                {
                    var table = db.Table<SrwZlcSkladniki>();

                    var result = db.Query<SrwZlcSkladniki>("select * from SrwZlcSkladniki where szs_sznId = " + szn_ID);

                    if(result.Count > 0)
                    {
                        for(int i = 0; i < result.Count; i++)
                        {
                            SrwZlcSkladniki szs = new SrwZlcSkladniki();
                            szs.SZS_Id = result[i].SZS_Id;
                            szs.SZS_SZNId = result[i].SZS_SZNId;
                            szs.SZS_Synchronizacja = result[i].SZS_Synchronizacja;
                            szs.SZS_Pozycja = result[i].SZS_Pozycja;
                            szs.SZS_Ilosc = result[i].SZS_Ilosc;
                            szs.SZS_TwrNumer = result[i].SZS_TwrNumer;
                            szs.SZS_TwrTyp = result[i].SZS_TwrTyp;
                            szs.SZS_TwrNazwa = result[i].SZS_TwrNazwa;
                            szs.SZS_Opis = result[i].SZS_Opis;

                            output.Add(szs);
                        }
                    }
                }
                catch(Exception)
                {
                }
            }

            return output;
        }

        public List<SrwZlcSkladniki> SrwZlcSkladniki_GetFilteredRecords(String filtr)
        {
            List<SrwZlcSkladniki> output = new List<SrwZlcSkladniki>();
            try
            {
                var table = db.Table<SrwZlcSkladniki>();
                string zapytanie = "select * from SrwZlcSkladniki ";

                if(filtr != "")
                {
                    zapytanie += "where szs_TwrNazwa like '%" + filtr + "%' or Twr_Kod like '%" + filtr + "%'";
                }

                var result = db.Query<SrwZlcSkladniki>(zapytanie);

                for(int i = 0; i < result.Count; i++)
                {
                    output.Add(result[i]);
                }
            }
            catch(Exception)
            {
            }

            return output;
        }

        public List<SrwZlcSkladniki> SrwZlcSkladnikiSynchronizacja(int synchParam)
        {
            List<SrwZlcSkladniki> srwZlcSkladnikiList = new List<SrwZlcSkladniki>();

            try
            {
                srwZlcSkladnikiList = db.Query<SrwZlcSkladniki>("select * from SrwZlcSkladniki where SZS_Synchronizacja = " + synchParam.ToString());
            }
            catch(Exception)
            { }

            return srwZlcSkladnikiList;
        }

        public void SrwZlcSkladniki_OznaczWyslane(List<int> wyslaneSkladnikiList, int wslaneParam)
        {
            for(int i = 0; i < wyslaneSkladnikiList.Count; i++)
            {
                var result = db.Query<SrwZlcSkladniki>("UPDATE SrwZlcSkladniki SET SZs_Synchronizacja = " + wslaneParam.ToString() + " where szs_sznId = " + wyslaneSkladnikiList[i].ToString());
            }
        }
        public void SrwZlcSkladniki_OznaczWyslane(List<SrwZlcSkladniki> wyslaneSkladnikiList, int wslaneParam)
        {
            for(int i = 0; i < wyslaneSkladnikiList.Count; i++)
            {
                var result = db.Query<SrwZlcSkladniki>("UPDATE SrwZlcSkladniki SET SZs_Synchronizacja = " + wslaneParam.ToString() + " where szs_sznId = " + wyslaneSkladnikiList[i].SZS_Id.ToString());
            }
        }









        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela TwrKarty*******************************|
            *---------------------------------------------------------------------------------
        */

        public string stworzTwrKartyTable()
        {
            String output = "";
            try
            {
                try
                {
                    db.DropTable<TwrKartyTable>();
                }
                catch(Exception) { }

                db.CreateTable<TwrKartyTable>();

                output = "Tabela TwrKartyTable zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzTwrKartyTable() Error: " + exc.Message;
            }

            return output;
        }

        public String TwrKartyTable_InsertRecord(TwrKartyTable item)
        {
            String output = "";
            try
            {
                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.TwrKartyTable_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }
        public List<TwrKartyTable> TwrKartyTable_GetFilteredRecords(String filtrEditText, String filtr, Boolean czynnosci)
        {
            List<TwrKartyTable> output = new List<TwrKartyTable>();
            try
            {
                var table = db.Table<TwrKartyTable>();
                string zapytanie = "select * from TwrKartyTable where ";

                if(czynnosci)
                {
                    zapytanie += "twr_typ = 4";
                }
                else
                {
                    zapytanie += "twr_typ in (1,2)";
                }

                if(filtrEditText != "")
                {
                    zapytanie += " and (Twr_Kod like '%" + filtrEditText + "%' or Twr_Nazwa like '%" + filtrEditText + "%')";
                }

                if(filtr != "")
                {
                    zapytanie += " and Twr_GIDNumer not in ("+filtr+")";
                }

                var result = db.Query<TwrKartyTable>(zapytanie);

                for(int i = 0; i < result.Count; i++)
                {
                    output.Add(result[i]);
                }
            }
            catch(Exception)
            {
            }

            return output;
        }

        public TwrKartyTable TwrKartyTable_GetRecord(int TwrNumer)
        {
            TwrKartyTable output = new TwrKartyTable();

            try
            {
                var table = db.Table<TwrKartyTable>();
                string zapytanie = "select * from TwrKartyTable where Twr_GIDNumer = "+TwrNumer.ToString();

                var result = db.Query<TwrKartyTable>(zapytanie);

                if (result.Count == 1)
                {
                    output = result[0];
                }
            }
            catch(Exception)
            {
            }
            return output;
        }






        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwZlcPodpis***************************|
            *---------------------------------------------------------------------------------
        */

        public string stworzSrwZlcPodpisTable()
        {
            String output = "";
            try
            {
                try
                {
                    db.DropTable<SrwZlcPodpisTable>();
                }
                catch(Exception) { }

                db.CreateTable<SrwZlcPodpisTable>();

                output = "Tabela SrwZlcPodpisTable zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcPodpisTable() Error: " + exc.Message;
            }

            return output;
        }

        public String SrwZlcPodpis_InsertRecord(SrwZlcPodpisTable item)
        {
            String output = "";
            try
            {
                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcPodpis_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }

        public SrwZlcPodpisTable SrwZlcPodpis_GetRecord(Int32 SZN_Id)
        {
            SrwZlcPodpisTable output = null;
            
            try
            {
                var result = db.Query<SrwZlcPodpisTable>("select * from SrwZlcPodpisTable where SZN_Id = " + SZN_Id.ToString());

                if(result.Count > 0)
                {
                    output = result[0];
                }
            }
            catch(Exception)
            {}

            return output;
        }

    }
}







