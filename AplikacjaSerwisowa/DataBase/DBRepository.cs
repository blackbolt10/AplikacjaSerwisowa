using System;
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
                    output += item.Id + ": GIDNum:" + item.Knt_GIDNumer + ", A:" + item.Knt_Akrnonim + "\n";
                }
            }
            catch(Exception exc)
            {
                output += "DBRepository.kntKarty_GetAllRecords() Error: " + exc.Message;
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
                    output += item.Id + ": GIDNum:" + item.Kna_GIDNumer + ", KntNumer:" + item.Kna_KntNumer + ", A:" + item.Kna_Akrnonim + "\n";
                }
            }
            catch(Exception exc)
            {
                output += "DBRepository.kntAdresy_GetAllRecords() Error: " + exc.Message;
            }

            return output;
        }
    }
}