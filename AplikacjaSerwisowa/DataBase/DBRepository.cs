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
using Android.Graphics;

namespace AplikacjaSerwisowa
{
    public class DBRepository
    {
        private String dbPath;
        private SQLiteConnection db;
        private Context kontekst;
        private Dialog dialog;

        public DBRepository(Context _kontekst=null, Dialog _dialog =null)
        {
            this.dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");
            this.db = new SQLiteConnection(dbPath);
            this.kontekst = _kontekst;
            this.dialog = _dialog;
        }

        private void RaportBledu(String funkcja, String blad)
        {
            if(kontekst != null)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(kontekst);
                alert.SetIconAttribute(Android.Resource.Attribute.AlertDialogIcon);

                alert.SetTitle("Wyst¹pi³ b³¹d");
                alert.SetMessage("Wyst¹pi³ b³¹d funkcji \"" + funkcja + "\":\n" + blad);
                alert.SetPositiveButton("OK", (senderAlert, args) => { });

                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }

        public string createDB()
        {
            String output = "";
            try
            {
                output += "creating database if it doesn't exist";
                String dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ormdemo.db3");

                SQLiteConnection db = new SQLiteConnection(dbPath);
                output = "Database created...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.createDB() Error: " + exc.Message;
                RaportBledu("createDB", exc.Message);
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
                catch (Exception exc)
                {
                    string blad = exc.Message;
                    RaportBledu("OperatorzyTable_CreateTable - droptable", exc.Message);
                }

                db.CreateTable<OperatorzyTable>();

                output = "Table Created succsessfully..";
            }
            catch (Exception exc)
            {
                output = "DBRepository.CreateTable() Error: " + exc.Message;
                RaportBledu("OperatorzyTable_CreateTable", exc.Message);
            }

            return output;
        }

        public string dropAllTables()
        {
            String output = "";
            try
            {
                db.DropTable<KntKartyTable>();
            }
            catch(Exception exc)
            {
                output += "KntKartyTable.Error=" + exc.Message + "\n";
            }

            try
            {
                db.DropTable<KntAdresyTable>();
            }
            catch(Exception exc)
            {
                output += "KntAdresyTable.Error=" + exc.Message + "\n";
            }
            try
            {
                db.DropTable<TwrKartyTable>();
            }
            catch(Exception exc)
            {
                output += "TwrKartyTable.Error=" + exc.Message + "\n";
            }

            try
            {
                db.DropTable<SrwZlcNag>();
            }
            catch(Exception exc)
            {
                output += "SrwZlcNag.Error=" + exc.Message + "\n";
            }

            try
            {
                    db.DropTable<SrwZlcCzynnosci>();
            }
            catch(Exception exc)
            {
                output += "SrwZlcCzynnosci.Error=" + exc.Message + "\n";
            }

            try
            {
                db.DropTable<SrwZlcSkladniki>();
            }
            catch(Exception exc)
            {
                output += "SrwZlcSkladniki.Error=" + exc.Message + "\n";
            }

            try
            {
                db.DropTable<SrwZlcUrz>();
            }
            catch(Exception exc)
            {
                output += "SrwZlcUrz.Error=" + exc.Message + "\n";
            }

            try
            {
                db.DropTable<SrwUrzadzenia>();
            }
            catch(Exception exc)
            {
                output += "SrwUrzadzenia.Error=" + exc.Message + "\n";
            }

            try
            {
                db.DropTable<SrwUrzWlasc>();
            }
            catch(Exception exc)
            {
                output += "SrwUrzWlasc.Error=" + exc.Message + "\n";
            }

            try
            {
                db.DropTable<SrwUrzParDef>();
            }
            catch(Exception exc)
            {
                output += "SrwUrzParDef.Error=" + exc.Message + "\n";
            }

            try
            {
                db.DropTable<SrwUrzRodzaje>();
            }
            catch(Exception exc)
            {
                output += "SrwUrzRodzaje.Error=" + exc.Message + "\n";
            }

            try
            {
                db.DropTable<SrwUrzRodzPar>();
            }
            catch(Exception exc)
            {
                output += "SrwUrzRodzPar.Error=" + exc.Message + "\n";
            }
            return output;
        }

        public String OperatorzyTable_InsertRecord(OperatorzyTable item)
        {
            String output = "";
            try
            {        
                db.InsertOrReplace(item);
                output = "Record added..";
            }
            catch (Exception exc)
            {
                output = "DBRepository.InsertRecord() Error: " + exc.Message;
                RaportBledu("OperatorzyTable_InsertRecord", exc.Message);
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
                RaportBledu("OperatorzyTable_GetAllRecords", exc.Message);
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
                    RaportBledu("Login", exc.Message);
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
            *|*********************************Tabela kontrahentow***************************|
            *---------------------------------------------------------------------------------
        */

        public string stworzKntKartyTabele()
        {
            String output = "";
            try
            {
                db.CreateTable<KntKartyTable>();

                output = "Tabela kontrahentow zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzKntKartyTabele() Error: " + exc.Message;
                RaportBledu("stworzKntKartyTabele", exc.Message);
            }

            return output;
        }

        public Boolean kntKarty_UpdateRecord(KntKartyTable kntKarta)
        {
            String output = "";
            try
            {
                db.Update(kntKarta);
            }
            catch(Exception exc)
            {
                output = "DBRepository.kntKarty_UpdateRecord() Error: " + exc.Message;
                RaportBledu("kntKarty_UpdateRecord", exc.Message);
                return false;
            }
            return true;
        }

        internal Boolean kntKarty_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<KntKartyTable>(recordID);
            }
            catch(Exception exc)
            {
                output = "DBRepository.kntKarty_DeleteRecord() Error: " + exc.Message;
                RaportBledu("kntKarty_DeleteRecord", exc.Message);
                return false;
            }
            return true;
        }

        public Boolean kntKarty_InsertRecord(KntKartyTable item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
            }
            catch(Exception exc)
            {
                output = "DBRepository.kntKarty_InsertRecord() Error: " + exc.Message;
                RaportBledu("kntKarty_InsertRecord", exc.Message);
                return false;
            }
            return true;
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
                RaportBledu("kntKarty_GetAllRecords", exc.Message);
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
                catch(Exception exc)
                {
                    string blad = exc.Message;
                    RaportBledu("kntKarty_GetRecord", exc.Message);
                }
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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("kntKarty_GetFiltredGidNumers", exc.Message);
            }

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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("kntKarty_GetFilteredRecords", exc.Message);
            }

            return output;
        }

        public List<int> kntKarty_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<KntKartyTable> kntKartyList = new List<KntKartyTable>();

            try
            {
                String zapytanieString = "select Knt_GIDNumer from KntKartyTable";
                kntKartyList = db.Query<KntKartyTable>(zapytanieString);
            }
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("kntKarty_generujListeZapisanch", exc.Message);
            }

            for(int i =0;i<kntKartyList.Count;i++)
            {
                output.Add(kntKartyList[i].Knt_GIDNumer);
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
                db.CreateTable<KntAdresyTable>();

                output = "Tabela kontrahentow adresy zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzKntAdresyTabele() Error: " + exc.Message;
                RaportBledu("stworzKntAdresyTabele", exc.Message);
            }

            return output;
        }

        internal List<int> KntAdresy_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<KntAdresyTable> kntAdresyList = new List<KntAdresyTable>();

            try
            {
                String zapytanieString = "select Kna_GIDNumer from KntAdresyTable";
                kntAdresyList = db.Query<KntAdresyTable>(zapytanieString);
            }
            catch(Exception exc)
            {
                String blad = "DBRepository.KntAdresy_generujListeZapisanch() Error: " + exc.Message;
                RaportBledu("KntAdresy_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < kntAdresyList.Count; i++)
            {
                output.Add(kntAdresyList[i].Kna_GIDNumer);
            }

            return output;
        }

        public Boolean kntAdresy_UpdateRecord(KntAdresyTable kntAdres)
        {
            String output = "";
            try
            {
                db.Update(kntAdres);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.KntAdresy_UpdateRecord() Error: " + exc.Message;
                RaportBledu("kntAdresy_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean kntAdresy_InsertRecord(KntAdresyTable item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.kntAdresy_InsertRecord() Error: " + exc.Message;
                RaportBledu("kntAdresy_InsertRecord", exc.Message);
                return false;
            }
        }

        public Boolean kntAdresy_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<KntAdresyTable>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.KntAdresy_DeleteRecord() Error: " + exc.Message;
                RaportBledu("kntAdresy_DeleteRecord", exc.Message);
                return false;
            }
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
                RaportBledu("kntAdresy_GetAllRecords", exc.Message);
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
                catch(Exception exc)
                {
                    string blad = exc.Message;
                    RaportBledu("kntAdresy_GetRecord", exc.Message);
                }
            }

            return output;
        }

        public List<KntAdresyTable> kntAdresy_GetRecords(String KNT_GIDNumer)
        {
            List<KntAdresyTable> output = null;

            if(KNT_GIDNumer.Length > 0)
            {
                try
                {
                    var result = db.Query<KntAdresyTable>("select * from KntAdresyTable where Kna_KntNumer = " + KNT_GIDNumer);

                    if(result.Count > 0)
                    {
                        output = result;
                    }
                }
                catch(Exception exc)
                {
                    string blad = exc.Message;
                    RaportBledu("kntAdresy_GetRecords", exc.Message);
                }
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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("sprawdzLiczbeAdresow", exc.Message);
            }

            return KNTAdres;
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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("kntAdresy_GetFilteredRecords", exc.Message);
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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("kntAdresy_GetFiltredGidNumers", exc.Message);
            }

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
                db.CreateTable<SrwZlcNag>();

                output = "Tabela SrwZlcNag zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcNag() Error: " + exc.Message;
                RaportBledu("stworzSrwZlcNag", exc.Message);
            }

            return output;
        }
        public Boolean SrwZlcNag_InsertRecord(SrwZlcNag item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcNag_InsertRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcNag_InsertRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwZlcNag_UpdateRecord(SrwZlcNag TwrKarta)
        {
            String output = "";
            try
            {
                db.Update(TwrKarta);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcNag_UpdateRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcNag_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwZlcNag_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<SrwZlcNag>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcNag_DeleteRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcNag_DeleteRecord", exc.Message);
                return false;
            }
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
                catch(Exception exc)
                {
                    string blad = exc.Message;
                    RaportBledu("SrwZlcNag_GetRecordGetRecord", exc.Message);
                }
            }

            return output;
        }

        public List<SrwZlcNag> pobierzListeSrwZlcNag(String filtr)
        {
            List<SrwZlcNag> output = new List<SrwZlcNag>();
            try
            {

                String zapParam = "";

                if(filtr != "")
                {
                    List<int> kntKarty = kntKarty_GetFiltredGidNumers(filtr);
                    String kntKartyString = generujListeGuidNumer(kntKarty);

                    List<int> kntAdresy = kntAdresy_GetFiltredGidNumers(filtr);
                    String kntAdresyString = generujListeGuidNumer(kntKarty);

                    zapParam = " where SZN_Dokument like '%"+filtr+ "%' or SZN_DataWystawienia like '%" + filtr + "%' or SZN_KntNumer in "+kntKartyString+ " or SZN_KnANumer in "+kntAdresyString;
                }

                List<SrwZlcNag> result = db.Query<SrwZlcNag>("select * from SrwZlcNag "+ zapParam + " order by SZN_DataWystawienia desc, SZN_Id asc");

                if(result != null)
                {
                    output = result;
                }
            }
            catch(Exception exc)
            {
                string test = exc.Message;
                RaportBledu("pobierzListeSrwZlcNag", exc.Message);
            }

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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwZlcNagSynchronizacja", exc.Message);
            }

            return srwZlcNagList;
        }

        public int SrwZlcNagGenerujNoweID(Context kontekst)
        {
            int result = Odczyt(kontekst);
            result = result - 1;
            zapisDanychDoPamieciUrzadzenia(result, kontekst);
            
            return result;
        }

        public List<int> SrwZlcNag_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<SrwZlcNag> SrwZlcNagList = new List<SrwZlcNag>();

            try
            {
                String zapytanieString = "select SZN_Id from SrwZlcNag";
                SrwZlcNagList = db.Query<SrwZlcNag>(zapytanieString);
            }
            catch(Exception exc)
            {
                String blad = "DBRepository.SrwZlcNag_generujListeZapisanch() Error: " + exc.Message;
                RaportBledu("SrwZlcNag_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < SrwZlcNagList.Count; i++)
            {
                output.Add(SrwZlcNagList[i].SZN_Id);
            }

            return output;
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
                db.CreateTable<SrwZlcCzynnosci>();

                output = "Tabela SrwZlcCzynnosci zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcCynnosciTable() Error: " + exc.Message;
                RaportBledu("stworzSrwZlcCzynnosci", exc.Message);
            }

            return output;
        }

        public Boolean SrwZlcCzynnosci_InsertRecord(SrwZlcCzynnosci item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcCynnosci_InsertRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcCzynnosci_InsertRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwZlcCzynnosci_UpdateRecord(SrwZlcCzynnosci SrwZlcCzynnosc)
        {
            String output = "";
            try
            {
                db.Update(SrwZlcCzynnosc);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcCzynnosci_UpdateRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcCzynnosci_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwZlcCzynnosci_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<SrwZlcCzynnosci>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcCzynnosci_DeleteRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcCzynnosci_DeleteRecord", exc.Message);
                return false;
            }
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
                catch(Exception exc)
                {
                    string blad = exc.Message;
                    RaportBledu("SrwZlcCzynnosci_GetRecords", exc.Message);
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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwZlcCzynnosci_GetFilteredRecords", exc.Message);
            }

            return output;
        }
        public List<int> SrwZlcCzynnosci_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<SrwZlcCzynnosci> SrwZlcCzynnosciList = new List<SrwZlcCzynnosci>();

            try
            {
                String zapytanieString = "select SZC_Id from SrwZlcCzynnosci";
                SrwZlcCzynnosciList = db.Query<SrwZlcCzynnosci>(zapytanieString);
            }
            catch(Exception exc)
            {
                String blad = "DBRepository.SrwZlcCzynnosci_generujListeZapisanch() Error: " + exc.Message;
                RaportBledu("SrwZlcCzynnosci_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < SrwZlcCzynnosciList.Count; i++)
            {
                output.Add(SrwZlcCzynnosciList[i].SZC_Id);
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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwZlcCzynnosciSynchronizacja", exc.Message);
            }

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
                db.CreateTable<SrwZlcSkladniki>();

                output = "Tabela SrwZlcSkladniki zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcSkladniki() Error: " + exc.Message;
                RaportBledu("stworzSrwZlcSkladniki", exc.Message);
            }

            return output;
        }
        public Boolean SrwZlcSkladniki_InsertRecord(SrwZlcSkladniki item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcSkladniki_InsertRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcSkladniki_InsertRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwZlcSkladniki_UpdateRecord(SrwZlcSkladniki SrwZlcSkladnik)
        {
            String output = "";
            try
            {
                db.Update(SrwZlcSkladnik);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcSkladniki_UpdateRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcSkladniki_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwZlcSkladniki_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<SrwZlcSkladniki>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcSkladniki_DeleteRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcSkladniki_DeleteRecord", exc.Message);
                return false;
            }
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
                catch(Exception exc)
                {
                    string blad = exc.Message;
                    RaportBledu("SrwZlcSkladniki_GetRecords", exc.Message);
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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwZlcSkladniki_GetFilteredRecords", exc.Message);
            }

            return output;
        }

        public List<int> SrwZlcSkladniki_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<SrwZlcSkladniki> SrwZlcSkladnikiList = new List<SrwZlcSkladniki>();

            try
            {
                String zapytanieString = "select SZS_Id from SrwZlcSkladniki";
                SrwZlcSkladnikiList = db.Query<SrwZlcSkladniki>(zapytanieString);
            }
            catch(Exception exc)
            {
                String blad = "DBRepository.SrwZlcSkladniki_generujListeZapisanch() Error: " + exc.Message;
                RaportBledu("SrwZlcSkladniki_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < SrwZlcSkladnikiList.Count; i++)
            {
                output.Add(SrwZlcSkladnikiList[i].SZS_Id);
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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwZlcSkladnikiSynchronizacja", exc.Message);
            }

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
                db.CreateTable<TwrKartyTable>();

                output = "Tabela TwrKartyTable zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzTwrKartyTable() Error: " + exc.Message;
                RaportBledu("stworzTwrKartyTable", exc.Message);
            }

            return output;
        }

        public Boolean TwrKartyTable_InsertRecord(TwrKartyTable item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.TwrKartyTable_InsertRecord() Error: " + exc.Message;
                RaportBledu("TwrKartyTable_InsertRecord", exc.Message);
                return false;
            }
        }

        public List<int> TwrKarty_generujListeZapisanch()
        {

            List<int> output = new List<int>();
            List<TwrKartyTable> twrKartyList = new List<TwrKartyTable>();

            try
            {
                String zapytanieString = "select * from TwrKartyTable";
                var table = db.Table<TwrKartyTable>();
                twrKartyList = db.Query<TwrKartyTable>(zapytanieString);
            }
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("TwrKarty_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < twrKartyList.Count; i++)
            {
                output.Add(twrKartyList[i].Twr_GIDNumer);
            }

            return output;
        }

        public Boolean TwrKartyTable_UpdateRecord(TwrKartyTable TwrKarta)
        {
            String output = "";
            try
            {
                db.Update(TwrKarta);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.TwrKartyTable_UpdateRecord() Error: " + exc.Message;
                RaportBledu("TwrKartyTable_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean TwrKartyTable_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<TwrKartyTable>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.TwrKartyTable_DeleteRecord() Error: " + exc.Message;
                RaportBledu("TwrKartyTable_DeleteRecord", exc.Message);
                return false;
            }
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
            catch(Exception exc)
            { 
                string blad = exc.Message;
                RaportBledu("TwrKartyTable_GetFilteredRecords", exc.Message);
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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("TwrKartyTable_GetRecord", exc.Message);
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
                catch(Exception exc)
                {
                    string blad = exc.Message;
                    RaportBledu("stworzSrwZlcPodpisTable-drop", exc.Message);
                }

                db.CreateTable<SrwZlcPodpisTable>();

                output = "Tabela SrwZlcPodpisTable zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcPodpisTable() Error: " + exc.Message;
                RaportBledu("stworzSrwZlcPodpisTable", exc.Message);
            }

            return output;
        }

        public String SrwZlcPodpis_InsertRecord(SrwZlcPodpisTable item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                output = "Wpis dodany..";
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcPodpis_InsertRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcPodpis_InsertRecord", exc.Message);
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
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwZlcPodpis_GetRecord", exc.Message);
            }

            return output;
        }

        public byte[] pobierzPodpis(string SZN_Id)
        {
            byte[] byteArray = new byte[0];

            try
            {
                var result = db.Query<SrwZlcPodpisTable>("select * from SrwZlcPodpisTable where SZN_Id = " + SZN_Id.ToString());

                if(result.Count > 0)
                {
                    byteArray = wygenerujByteArray(result[0].Podpis);
                }
            }
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("pobierzPodpis", exc.Message);
            }

            return byteArray;
        }

        private byte[] wygenerujByteArray(string podpis)
        {
            String[] stringArray = podpis.Split(',');
            byte[] byteArray = new byte[stringArray.Length];

            for(int i=0;i<stringArray.Length;i++)
            {
                byteArray[i] = Convert.ToByte(stringArray[i]);
            }

            return byteArray;
        }


        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwUrzadzenia**************************|
            *---------------------------------------------------------------------------------
        */
        public string stworzSrwUrzadzenia()
        {
            String output = "";
            try
            {
                db.CreateTable<SrwUrzadzenia>();

                output = "Tabela SrwUrzadzenia zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwUrzadzenia() Error: " + exc.Message;
                RaportBledu("stworzSrwUrzadzenia", exc.Message);
            }

            return output;
        }

        public Boolean SrwUrzadzenia_InsertRecord(SrwUrzadzenia item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzadzenia_InsertRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzadzenia_InsertRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwUrzadzenia_UpdateRecord(SrwUrzadzenia SrwZlcCzynnosc)
        {
            String output = "";
            try
            {
                db.Update(SrwZlcCzynnosc);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzadzenia_UpdateRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzadzenia_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwUrzadzenia_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<SrwUrzadzenia>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzadzenia_DeleteRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzadzenia_DeleteRecord", exc.Message);
                return false;
            }
        }

        public int SrwUrzadzenia_GenerujNoweID(Context kontekst)
        {
            int result = OdczytUrzadzen(kontekst);
            result = result - 1;
            zapisIdUrzadzeniaDoPamieciUrzadzenia(result, kontekst);

            return result;
        }
        private Int32 OdczytUrzadzen(Context kontekst)
        {
            ISharedPreferences prefs = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(kontekst);

            Int32 result = prefs.GetInt("SrU_Id", -1);
            return result;
        }
        private void zapisIdUrzadzeniaDoPamieciUrzadzenia(Int32 SrU_Id, Context kontekst)
        {
            ISharedPreferences prefs = Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(kontekst);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutInt("SrU_Id", SrU_Id);
            editor.Apply();
        }

        public List<SrwUrzadzenia> SrwUrzadzenia_GetFilteredRecords(string filtrEditText, string filtr, bool KntFiltr, Int32 KNT_GIDNumer)
        {
            List<SrwUrzadzenia> output = new List<SrwUrzadzenia>();
            try
            {
                string zapytanie = "select * from SrwUrzadzenia where SrU_Archiwalne <> 1";

                if(filtrEditText != "")
                {
                    zapytanie += " and Sru_Nazwa like '%" + filtrEditText + "%'";
                }

                if(!KntFiltr && KNT_GIDNumer != -1)
                {
                    zapytanie += " and SUW_WlaNumer = " + KNT_GIDNumer;
                }

                if(filtr != "")
                {
                    zapytanie += " and SrU_SURId not in (" + filtr + ")";
                }

                var result = db.Query<SrwUrzadzenia>(zapytanie);

                for(int i = 0; i < result.Count; i++)
                {
                    output.Add(result[i]);
                }
            }
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwUrzadzenia_GetFilteredRecords", exc.Message);
            }

            return output;
        }


        public SrwUrzadzenia SrwUrzadzenia_GetRecord(int sZU_SrUId)
        {
            SrwUrzadzenia output = new SrwUrzadzenia();
            
            try
            {
                var result = db.Query<SrwUrzadzenia>("select * from SrwUrzadzenia where SrU_Id = " + sZU_SrUId);

                if(result.Count == 1)
                {
                    output = result[0];
                }
            }
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwUrzadzenia_GetRecord", exc.Message);
            }

            return output;
        }


        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwUrzParDef***************************|
            *---------------------------------------------------------------------------------
        */
        public string stworzSrwUrzParDef()
        {
            String output = "";
            try
            {
                db.CreateTable<SrwUrzParDef>();

                output = "Tabela SrwUrzParDef zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwUrzParDef() Error: " + exc.Message;
                RaportBledu("stworzSrwUrzParDef", exc.Message);
            }

            return output;
        }

        public List<int> SrwUrzParDef_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<SrwUrzParDef> SrwUrzParDefList = new List<SrwUrzParDef>();

            try
            {
                String zapytanieString = "select * from SrwUrzParDef";
                SrwUrzParDefList = db.Query<SrwUrzParDef>(zapytanieString);
            }
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwUrzParDef_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < SrwUrzParDefList.Count; i++)
            {
                output.Add(SrwUrzParDefList[i].SUD_Id);
            }

            return output;
        }

        public Boolean SrwUrzParDef_InsertRecord(SrwUrzParDef item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzParDef_InsertRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzParDef_InsertRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwUrzParDef_UpdateRecord(SrwUrzParDef SrwUrzParDefinicja)
        {
            String output = "";
            try
            {
                db.Update(SrwUrzParDefinicja);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzParDef_UpdateRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzParDef_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwUrzParDef_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<SrwUrzParDef>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzParDef_DeleteRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzParDef_DeleteRecord", exc.Message);
                return false;
            }
        }

        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwUrzRodzPar**************************|
            *---------------------------------------------------------------------------------
        */
        public string stworzSrwUrzRodzPar()
        {
            String output = "";
            try
            {
                db.CreateTable<SrwUrzRodzPar>();

                output = "Tabela SrwUrzRodzPar zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwUrzRodzPar() Error: " + exc.Message;
                RaportBledu("stworzSrwUrzRodzPar", exc.Message);
            }

            return output;
        }

        public Boolean SrwUrzRodzPar_InsertRecord(SrwUrzRodzPar item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzRodzPar_InsertRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzRodzPar_InsertRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwUrzRodzPar_UpdateRecord(SrwUrzRodzPar SrwUrzRodzParametry)
        {
            String output = "";
            try
            {
                db.Update(SrwUrzRodzParametry);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzRodzPar_UpdateRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzRodzPar_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwUrzRodzPar_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<SrwUrzRodzPar>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzRodzPar_DeleteRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzRodzPar_DeleteRecord", exc.Message);
                return false;
            }
        }



        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwUrzRodzaje**************************|
            *---------------------------------------------------------------------------------
        */
        public string stworzSrwUrzRodzaje()
        {
            String output = "";
            try
            {
                db.CreateTable<SrwUrzRodzaje>();

                output = "Tabela SrwUrzRodzaje zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwUrzRodzaje() Error: " + exc.Message;
                RaportBledu("stworzSrwUrzRodzaje", exc.Message);
            }

            return output;
        }

        public List<int> SrwUrzRodzaje_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<SrwUrzRodzaje> SrwUrzRodzajeList = new List<SrwUrzRodzaje>();

            try
            {
                String zapytanieString = "select * from SrwUrzRodzaje";
                SrwUrzRodzajeList = db.Query<SrwUrzRodzaje>(zapytanieString);
            }
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwUrzRodzaje_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < SrwUrzRodzajeList.Count; i++)
            {
                output.Add(SrwUrzRodzajeList[i].SUR_Id);
            }

            return output;
        }

        public Boolean SrwUrzRodzaje_InsertRecord(SrwUrzRodzaje item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzRodzaje_InsertRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzRodzaje_InsertRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwUrzRodzaje_UpdateRecord(SrwUrzRodzaje SrwUrzRodzaj)
        {
            String output = "";
            try
            {
                db.Update(SrwUrzRodzaj);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzRodzaje_UpdateRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzRodzaje_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwUrzRodzaje_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<SrwUrzRodzaje>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzRodzaje_DeleteRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzRodzaje_DeleteRecord", exc.Message);
                return false;
            }
        }

        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwZlcUrz******************************|
            *---------------------------------------------------------------------------------
        */
        public string stworzSrwZlcUrz()
        {
            String output = "";
            try
            {
                db.CreateTable<SrwZlcUrz>();

                output = "Tabela SrwZlcUrz zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwZlcUrz() Error: " + exc.Message;
                RaportBledu("stworzSrwZlcUrz", exc.Message);
            }

            return output;
        }

        public List<int> SrwUrzadzenia_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<SrwUrzadzenia> SrwUrzadzeniaList = new List<SrwUrzadzenia>();

            try
            {
                String zapytanieString = "select SrU_Id from SrwUrzadzenia";
                SrwUrzadzeniaList = db.Query<SrwUrzadzenia>(zapytanieString);
            }
            catch(Exception exc)
            {
                String blad = "DBRepository.SrwUrzadzenia_generujListeZapisanch() Error: " + exc.Message;
                RaportBledu("SrwUrzadzenia_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < SrwUrzadzeniaList.Count; i++)
            {
                output.Add(SrwUrzadzeniaList[i].SrU_Id);
            }

            return output;
        }

        public Boolean SrwZlcUrz_InsertRecord(SrwZlcUrz item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcUrz_InsertRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcUrz_InsertRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwZlcUrz_UpdateRecord(SrwZlcUrz SrwZlcUrzParametry)
        {
            String output = "";
            try
            {
                db.Update(SrwZlcUrzParametry);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcUrz_UpdateRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcUrz_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwZlcUrz_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<SrwZlcUrz>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwZlcUrz_DeleteRecord() Error: " + exc.Message;
                RaportBledu("SrwZlcUrz_DeleteRecord", exc.Message);
                return false;
            }
        }

        public List<SrwZlcUrz> SrwZlcUrz_GetRecords(string szn_ID)
        {
            List<SrwZlcUrz> output = null;

            if(szn_ID.Length > 0)
            {
                try
                {
                    var result = db.Query<SrwZlcUrz>("select * from SrwZlcUrz where SZU_SZNId = " + szn_ID);

                    if(result.Count > 0)
                    {
                        output = result;
                    }
                }
                catch(Exception exc)
                {
                    string blad = exc.Message;
                    RaportBledu("SrwZlcUrz_GetRecords", exc.Message);
                }
            }

            return output;
        }

        public List<int> SrwZlcSUrz_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<SrwZlcUrz> SrwZlcUrzList = new List<SrwZlcUrz>();

            try
            {
                String zapytanieString = "select SZU_Id from SrwZlcUrz";
                SrwZlcUrzList = db.Query<SrwZlcUrz>(zapytanieString);
            }
            catch(Exception exc)
            {
                String blad = "DBRepository.SrwZlcSUrz_generujListeZapisanch() Error: " + exc.Message;
                RaportBledu("SrwZlcSUrz_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < SrwZlcUrzList.Count; i++)
            {
                output.Add(SrwZlcUrzList[i].SZU_Id);
            }

            return output;
        }

        /*
            *---------------------------------------------------------------------------------
            *|*********************************Tabela SrwZlcUrz******************************|
            *---------------------------------------------------------------------------------
        */
        public string stworzSrwUrzWlasc()
        {
            String output = "";
            try
            {
                db.CreateTable<SrwUrzWlasc>();

                output = "Tabela SrwUrzWlasc zosta³a stworzona...";
            }
            catch(Exception exc)
            {
                output = "DBRepository.stworzSrwUrzWlasc() Error: " + exc.Message;
                RaportBledu("stworzSrwUrzWlasc", exc.Message);
            }

            return output;
        }

        public List<int> SrwUrzWlasc_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<SrwUrzWlasc> SrwUrzWlascList = new List<SrwUrzWlasc>();

            try
            {
                String zapytanieString = "select SUW_SrUId from SrwUrzWlasc";
                SrwUrzWlascList = db.Query<SrwUrzWlasc>(zapytanieString);
            }
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwUrzWlasc_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < SrwUrzWlascList.Count; i++)
            {
                output.Add(SrwUrzWlascList[i].SUW_SrUId);
            }

            return output;
        }

        public Boolean SrwUrzWlasc_InsertRecord(SrwUrzWlasc item)
        {
            String output = "";
            try
            {
                db.InsertOrReplace(item);
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzWlasc_InsertRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzWlasc_InsertRecord", exc.Message);
                return false;
            }
            return true;
        }

        public Boolean SrwUrzWlasc_UpdateRecord(SrwUrzWlasc SrwUrzWlasciciel)
        {
            String output = "";
            try
            {
                db.Update(SrwUrzWlasciciel);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzWlasc_UpdateRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzWlasc_UpdateRecord", exc.Message);
                return false;
            }
        }

        public Boolean SrwUrzWlasc_DeleteRecord(int recordID)
        {
            String output = "";
            try
            {
                db.Delete<SrwUrzWlasc>(recordID);
                return true;
            }
            catch(Exception exc)
            {
                output = "DBRepository.SrwUrzWlasc_DeleteRecord() Error: " + exc.Message;
                RaportBledu("SrwUrzWlasc_DeleteRecord", exc.Message);
                return false;
            }
        }

        public List<int> SrwUrzRodzPar_generujListeZapisanch()
        {
            List<int> output = new List<int>();
            List<SrwUrzRodzPar> SrwUrzRodzParList = new List<SrwUrzRodzPar>();

            try
            {
                String zapytanieString = "select * from SrwUrzRodzPar";
                SrwUrzRodzParList = db.Query<SrwUrzRodzPar>(zapytanieString);
            }
            catch(Exception exc)
            {
                string blad = exc.Message;
                RaportBledu("SrwUrzRodzPar_generujListeZapisanch", exc.Message);
            }

            for(int i = 0; i < SrwUrzRodzParList.Count; i++)
            {
                output.Add(SrwUrzRodzParList[i].SRP_Id);
            }

            return output;
        }
    }
}







