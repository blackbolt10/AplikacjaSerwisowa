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
                            output = "Has�o jest niepoprawne";
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
                output = "Akronim nie mo�e by� pusty";
            }

            return output;
        }






        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela towar�w********************************|
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

                output = "Tabela towar�w zosta�a stworzona...";
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

                output = "Tabela kontrahentow zosta�a stworzona...";
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


        

        public List<String> kntKarty_GetRecord(String knt_GidNumer)
        {
            List<String> output = new List<String>();

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
                        output.Add(result[0].Knt_GIDNumer.ToString());
                        output.Add(result[0].Knt_Akronim.ToString());
                        output.Add(result[0].Knt_nazwa1.ToString());
                        output.Add(result[0].Knt_nazwa2.ToString());
                        output.Add(result[0].Knt_nazwa3.ToString());
                        output.Add(result[0].Knt_KodP.ToString());
                        output.Add(result[0].Knt_miasto.ToString());
                        output.Add(result[0].Knt_ulica.ToString());
                        output.Add(result[0].Knt_Adres.ToString());
                        output.Add(result[0].Knt_nip.ToString());
                        output.Add(result[0].Knt_telefon1.ToString());
                        output.Add(result[0].Knt_telefon2.ToString());
                        output.Add(result[0].Knt_telex.ToString());
                        output.Add(result[0].Knt_fax.ToString());
                        output.Add(result[0].Knt_email.ToString());
                        output.Add(result[0].Knt_url.ToString());
                    }
                }
                catch(Exception exc)
                {
                }
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

                output = "Tabela kontrahentow adresy zosta�a stworzona...";
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

                output = "Tabela SerwisoweZleceniaNaglowkiTable zosta�a stworzona...";
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
    }
}







