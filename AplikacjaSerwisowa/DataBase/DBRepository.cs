using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    public class DBRepository
    {
        public string createDB()
        {
            String output = "";
            try
            {
                output += "creating database if it doesn't exist";
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");

                SQLiteConnection db = new SQLiteConnection(dbPath);
                output = "Database created...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.createDB() Error: " + exc.Message;
            }

            return output;
        }
        public string CreateTable()
        {
            String output = "";
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

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

        public String InsertRecord(OperatorzyTable item)
        {
            String output = "";
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);  
                              
                db.Insert(item);
                output = "Record added..";
            }
            catch (Exception exc)
            {
                output = "DBRepository.InsertRecord() Error: " + exc.Message;
            }

            return output;
        }

        public String GetAllRecords()
        {
            String output = "";

            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
                var table = db.Table<OperatorzyTable>();

                foreach(var item in table)
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
                    String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                    SQLiteConnection db = new SQLiteConnection(dbPath);
                    var table = db.Table<OperatorzyTable>();

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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch (Exception exc)
            {
                output = "DBRepository.kartyTowarow_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }
        public String kartyTowarow_GetAllRecords()
        {
            String output = "";

            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
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
                    String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                    SQLiteConnection db = new SQLiteConnection(dbPath);
                    var table = db.Table<KntKartyTable>();

                    var result = db.Query<KntKartyTable>("select * from KntKartyTable where Knt_GIDNumer = " + knt_GidNumer);

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

        public List<KntKartyTable> kntKarty_GetFilteredRecords(String filtr)
        {
            List<KntKartyTable> output = new List<KntKartyTable>();
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
                var table = db.Table<KntKartyTable>();
                string zapytanie = "select * from KntKartyTable ";

                if(filtr !="")
                {
                    zapytanie+= "where Knt_Akronim like '%" + filtr + "%' or Knt_nazwa1 like '%" + filtr + "%' or Knt_nazwa2 like '%" + filtr + "%' or Knt_nazwa3 like '%" + filtr + "%'";
                }

                var result = db.Query<KntKartyTable>(zapytanie);

                for(int i =0;i<result.Count;i++)
                { 
                    output.Add(result[i]);
                }
            }
            catch(Exception)
            {
            }

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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
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
                    String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                    SQLiteConnection db = new SQLiteConnection(dbPath);
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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
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
















        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SerwisoweZleceniaNaglowki**************|
            *---------------------------------------------------------------------------------
        */

        public string stworzSerwisoweZleceniaNaglowkiTable()
        {
            String output = "";
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

                try
                {
                    db.DropTable<SerwisoweZleceniaNaglowkiTable>();
                }
                catch(Exception) { }

                db.CreateTable<SerwisoweZleceniaNaglowkiTable>();

                output = "Tabela SerwisoweZleceniaNaglowkiTable zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSerwisoweZleceniaNaglowkiTable() Error: " + exc.Message;
            }

            return output;
        }
        public String SerwisoweZleceniaNaglowki_InsertRecord(SerwisoweZleceniaNaglowkiTable item)
        {
            String output = "";
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.SerwisoweZleceniaNaglowki_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }

        public SerwisoweZleceniaNaglowkiTable SerwisoweZleceniaNaglowki_GetRecord(String szn_ID)
        {
            SerwisoweZleceniaNaglowkiTable output = null;

            if(szn_ID.Length > 0)
            {
                try
                {
                    String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                    SQLiteConnection db = new SQLiteConnection(dbPath);
                    var table = db.Table<SerwisoweZleceniaNaglowkiTable>();

                    var result = db.Query<SerwisoweZleceniaNaglowkiTable>("select * from SerwisoweZleceniaNaglowkiTable where SZN_Id = " + szn_ID);

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









        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwZlcCzynnosci************************|
            *---------------------------------------------------------------------------------
        */

        public string stworzSrwZlcCynnosciTable()
        {
            String output = "";
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

                try
                {
                    db.DropTable<SrwZlcCzynnosciTable>();
                }
                catch(Exception) { }

                db.CreateTable<SrwZlcCzynnosciTable>();

                output = "Tabela SrwZlcCzynnosciTable zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcCynnosciTable() Error: " + exc.Message;
            }

            return output;
        }
        public String SrwZlcCynnosci_InsertRecord(SrwZlcCzynnosciTable item)
        {
            String output = "";
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcCynnosci_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }

        public List<SrwZlcCzynnosciTable> SrwZlcCynnosci_GetRecords(String szn_ID)
        {
            List<SrwZlcCzynnosciTable> output = new List<SrwZlcCzynnosciTable>();

            if(szn_ID.Length > 0)
            {
                try
                {
                    String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                    SQLiteConnection db = new SQLiteConnection(dbPath);
                    var table = db.Table<SrwZlcCzynnosciTable>();

                    var result = db.Query<SrwZlcCzynnosciTable>("select * from SrwZlcCzynnosciTable where szc_sznId = " + szn_ID);

                    if(result.Count > 0)
                    {
                        for(int i=0;i<result.Count;i++)
                        {
                            SrwZlcCzynnosciTable szc = new SrwZlcCzynnosciTable();
                            szc.szc_Id = result[i].szc_Id;
                            szc.szc_Ilosc = result[i].szc_Ilosc;
                            szc.szc_Pozycja = result[i].szc_Pozycja;
                            szc.szc_sznId = result[i].szc_sznId;
                            szc.Twr_Jm = result[i].Twr_Jm;
                            szc.szc_TwrNazwa = result[i].szc_TwrNazwa;
                            szc.Twr_Kod = result[i].Twr_Kod;

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

        public List<SrwZlcCzynnosciTable> SrwZlcCzynnosciTable_GetFilteredRecords(String filtr)
        {
            List<SrwZlcCzynnosciTable> output = new List<SrwZlcCzynnosciTable>();
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
                var table = db.Table<SrwZlcCzynnosciTable>();
                string zapytanie = "select * from SrwZlcCzynnosciTable ";

                if(filtr != "")
                {
                    zapytanie += "where szc_TwrNazwa like '%" + filtr + "%' or Twr_Kod like '%" + filtr + "%'";
                }

                var result = db.Query<SrwZlcCzynnosciTable>(zapytanie);

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










        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwZlcCzynnosci************************|
            *---------------------------------------------------------------------------------
        */

        public string stworzSrwZlcSkladnikiTable()
        {
            String output = "";
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

                try
                {
                    db.DropTable<SrwZlcSkladnikiTable>();
                }
                catch(Exception) { }

                db.CreateTable<SrwZlcSkladnikiTable>();

                output = "Tabela SrwZlcSkladnikiTable zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcSkladnikiTable() Error: " + exc.Message;
            }

            return output;
        }
        public String SrwZlcSkladniki_InsertRecord(SrwZlcSkladnikiTable item)
        {
            String output = "";
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

                db.Insert(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcSkladniki_InsertRecord() Error: " + exc.Message;
            }

            return output;
        }

        public List<SrwZlcSkladnikiTable> SrwZlcSkladniki_GetRecords(String szn_ID)
        {
            List<SrwZlcSkladnikiTable> output = new List<SrwZlcSkladnikiTable>();

            if(szn_ID.Length > 0)
            {
                try
                {
                    String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                    SQLiteConnection db = new SQLiteConnection(dbPath);
                    var table = db.Table<SrwZlcSkladnikiTable>();

                    var result = db.Query<SrwZlcSkladnikiTable>("select * from SrwZlcSkladnikiTable where szs_sznId = " + szn_ID);

                    if(result.Count > 0)
                    {
                        for(int i = 0; i < result.Count; i++)
                        {
                            SrwZlcSkladnikiTable szs = new SrwZlcSkladnikiTable();
                            szs.szs_Id = result[i].szs_Id;
                            szs.szs_Ilosc = result[i].szs_Ilosc;
                            szs.szs_Pozycja = result[i].szs_Pozycja;
                            szs.szs_sznId = result[i].szs_sznId;
                            szs.Twr_Jm = result[i].Twr_Jm;
                            szs.szs_TwrNazwa = result[i].szs_TwrNazwa;
                            szs.Twr_Kod = result[i].Twr_Kod;

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

        public List<SrwZlcSkladnikiTable> SrwZlcSkladnikiTable_GetFilteredRecords(String filtr)
        {
            List<SrwZlcSkladnikiTable> output = new List<SrwZlcSkladnikiTable>();
            try
            {
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
                var table = db.Table<SrwZlcSkladnikiTable>();
                string zapytanie = "select * from SrwZlcSkladnikiTable ";

                if(filtr != "")
                {
                    zapytanie += "where szs_TwrNazwa like '%" + filtr + "%' or Twr_Kod like '%" + filtr + "%'";
                }

                var result = db.Query<SrwZlcSkladnikiTable>(zapytanie);

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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);

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
                String dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdemo.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
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
    }
}







