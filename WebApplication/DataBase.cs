using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;

namespace WebApplication
{
    class DataBase
    {
        private SqlConnection uchwytBD;
        private SqlConnection uchwytBDSerwis;
        private int liczbaMiesiecyWstecz = -9;

        public DataBase()
        {
            podlaczDoBazyDanych();
            podlaczDoBazyDanychSerwis();
        }

        private Boolean podlaczDoBazyDanych()
        {
            Boolean wynikLogowaniaDoBD = false;
            hasla haslo = new hasla(4);

            try
            {
                String loginBD = haslo.GetInstanceUserName();
                String hasloBD = haslo.GetInstancePassword();
                String instancja = haslo.GetInstanceName();
                String bazaDanych = haslo.GetDataBaseName();

                uchwytBD = new SqlConnection(@"user id=" + loginBD + "; password=" + hasloBD + "; Data Source=" + instancja + "; Initial Catalog=" + bazaDanych + ";");
                uchwytBD.Open();
                wynikLogowaniaDoBD = true;
            }
            catch(Exception) { }

            return wynikLogowaniaDoBD;
        }

        private Boolean podlaczDoBazyDanychSerwis()
        {
            Boolean wynikLogowaniaDoBD = false;
            hasla haslo = new hasla(4);

            try
            {
                String loginBD = haslo.GetInstanceUserName();
                String hasloBD = haslo.GetInstancePassword();
                String instancja = haslo.GetInstanceName();
                String bazaDanych = haslo.GetDataBaseSerwisName();

                uchwytBDSerwis = new SqlConnection(@"user id=" + loginBD + "; password=" + hasloBD + "; Data Source=" + instancja + "; Initial Catalog=" + bazaDanych + ";");
                uchwytBDSerwis.Open();
                wynikLogowaniaDoBD = true;
            }
            catch(Exception) { }

            return wynikLogowaniaDoBD;
        }

        private SqlDataAdapter zapytanie(string zapytanieString)
        {
            SqlDataAdapter wynik = new SqlDataAdapter();
            SqlCommand polecenieSQL = new SqlCommand(zapytanieString);
            polecenieSQL.CommandTimeout = 240;
            polecenieSQL.Connection = uchwytBD;
            wynik = new SqlDataAdapter(polecenieSQL);

            return wynik;
        }

        private SqlDataAdapter zapytanieSerwis(string zapytanieString)
        {
            SqlDataAdapter wynik = new SqlDataAdapter();
            SqlCommand polecenieSQL = new SqlCommand(zapytanieString);
            polecenieSQL.CommandTimeout = 240;
            polecenieSQL.Connection = uchwytBDSerwis;
            wynik = new SqlDataAdapter(polecenieSQL);

            return wynik;
        }

        private void zapiszDB(string zapytanie1)
        {
            SqlCommand polecenieSQL = new SqlCommand(zapytanie1);
            polecenieSQL.Connection = uchwytBD;
            polecenieSQL.ExecuteNonQuery();
        }

        public String pobierzKntKarty()
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "select knt.knt_GIDNumer, knt.knt_Akronim, knt.knt_nazwa1, knt.knt_nazwa2, knt.knt_nazwa3, knt.knt_KodP, knt.knt_miasto, knt.knt_ulica, knt.knt_Adres, knt.knt_nip, knt.knt_telefon1, knt.knt_telefon2, knt.knt_telex, knt.knt_fax, knt.knt_email, knt.knt_url from cdn.kntkarty knt";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                String result = wygenerujPlikXML("KntKarty", "KntKarta", pomDataTable);
                return result;
            }
            else
            {
                return null;
            }
        }

        private String wygenerujPlikXML(String glownaNazwa, String pomocniczaNazwa, DataTable pomDataTable)
        {
            String output = "";

            DataSet ds = new DataSet(glownaNazwa);
            ds.Tables.Add(pomDataTable);
            ds.Tables[0].TableName = pomocniczaNazwa;

            using(StringWriter stringWriter = new StringWriter())
            {
                ds.WriteXml(new XmlTextWriter(stringWriter));
                output = stringWriter.ToString();
            };

            return output;
        }
         
        public List<SrwZlcNag> wygenerujListeSerwisowychZlecenNaglowki()
        {
            DateTime data = DateTime.Now.AddMonths(liczbaMiesiecyWstecz);
            String zapytanieSerwisowaLista = @"SELECT 
            CDN.NumerDokumentu(4700,4700,4700,SZN_Numer,SZN_Rok,SZN_Seria,SZN_Miesiac) as Dokument

                , A.SZN_Id
                , A.SZN_KntTyp,  A.SZN_KntNumer, A.SZN_KnATyp, A.SZN_KnANumer --Kontrahent
                --, G.Knt_Akronim as Knt_Akronim, G.Knt_Miasto as Knt_Miasto
                , A.SZN_KnDTyp, A.SZN_KnDNumer, A.SZN_AdWTyp, A.SZN_AdWNumer --Docelowy
                --, E.Knt_Akronim as Doc_Akronim, E.Knt_Miasto as Doc_Miasto
                --, A.SZN_KnPNumer, A.SZN_DataWystawienia 
                , DATEADD(DAY,A.SZN_DataWystawienia,CONVERT(DATETIME,'1800-12-28',120) ) as DataWystawienia
                --, A.SZN_DataRozpoczecia
                , DATEADD(DAY,A.SZN_DataRozpoczecia,CONVERT(DATETIME,'1800-12-28',120) ) as DataRozpoczecia
  
            , case when  A.SZN_Stan in (1,2) then  'Niezatwierdzone'
                    when  A.SZN_Stan = 3 then  'Do realizacji'
                    when  A.SZN_Stan = 4 then  'Zatwierdzone'
                    when  A.SZN_Stan = 5 then  'W realizacji'
                    when  A.SZN_Stan = 6 then  'Zamknięte'
                    when  A.SZN_Stan = 7 then  'Anulowane'
                    when  A.SZN_Stan = 8 then  'Odrzucone'
                end as Stan

            , isNull(B.SLW_WartoscS,'') as Status

            ,  A.SZN_CechaOpis, A.SZN_Opis
            --, A.SZN_MagZNumer, A.SZN_SlwStatus, B.SLW_ID, B.SLW_Predefiniowany

            FROM  CDN.SrwZlcNag A 
            LEFT OUTER JOIN CDN.Slowniki B ON  A.SZN_SlwStatus= B.SLW_ID --
            LEFT OUTER JOIN CDN.Slowniki C ON  A.SZN_FiaskoID= C.SLW_ID 
            --LEFT OUTER JOIN CDN.KntAdresy D ON  A.SZN_AdWTyp= D.KnA_GIDTyp AND  A.SZN_AdWNumer= D.KnA_GIDNumer 
            LEFT OUTER JOIN CDN.KntKarty E ON  A.SZN_KnDTyp= E.Knt_GIDTyp AND  A.SZN_KnDNumer= E.Knt_GIDNumer --Docelowy
            --LEFT OUTER JOIN CDN.KntAdresy F ON  A.SZN_KnATyp= F.KnA_GIDTyp AND  A.SZN_KnANumer= F.KnA_GIDNumer 
            LEFT OUTER JOIN CDN.KntKarty G ON  A.SZN_KntTyp= G.Knt_GIDTyp AND  A.SZN_KntNumer= G.Knt_GIDNumer   --Kontrahent
            WHERE (DATEADD(DAY,A.SZN_DataWystawienia,CONVERT(DATETIME,'1800-12-28',120) )>'"+data.Year.ToString()+"-"+data.Month.ToString()+@"-01') 
            ORDER BY  A.SZN_Rok,  A.SZN_Miesiac,  A.SZN_Seria,  A.SZN_Numer";

        DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = zapytanieSerwisowaLista;
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeSerwisowychZlecen(pomDataTable);
            }
            else
            {
                return null;
            }
        }

        public List<int> synchronizujSrwZlcNag(string inputJSON)
        {
            List<int> result = new List<int>();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SrwZlcNag> records = ser.Deserialize<List<SrwZlcNag>>(inputJSON);

            for(int i =0;i<records.Count;i++)
            {
                Boolean wynik = zapiszSrwZlcNagSerwis(records[i]);
                if(wynik)
                {
                    result.Add(records[i].SZN_Id);
                }
            }

            return result;
        }

        private Boolean zapiszSrwZlcNagSerwis(SrwZlcNag srwZlcNag)
        {
            DataTable pomdatatable = new DataTable();

            try
            {
                String zapytanieString = "INSERT INTO [GAL].[SrwZlcNag] VALUES(0, "+srwZlcNag.SZN_KntTyp+ ", " + srwZlcNag.SZN_KntNumer + ", " + srwZlcNag.SZN_KnATyp + ", " + srwZlcNag.SZN_KnANumer + ", " + srwZlcNag.SZN_KntTyp + ", " + srwZlcNag.SZN_KntNumer + ", " + srwZlcNag.SZN_KntTyp + ", " + srwZlcNag.SZN_KntNumer + ", '"+srwZlcNag.SZN_DataWystawienia+ "', '" + srwZlcNag.SZN_DataRozpoczecia + "', '" + srwZlcNag.SZN_Opis+ "')";
                SqlDataAdapter da =  zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private List<SrwZlcNag> wygenerujListeSerwisowychZlecen(DataTable pomDataTable)
        {
            List<SrwZlcNag> result = new List<SrwZlcNag>();

            for(int i=0;i<pomDataTable.Rows.Count;i++)
            {
                String Dokument = pomDataTable.Rows[i]["Dokument"].ToString();
                int SZN_Id = Convert.ToInt32(pomDataTable.Rows[i]["SZN_Id"].ToString());
                int SZN_KntTyp = Convert.ToInt32(pomDataTable.Rows[i]["SZN_KntTyp"].ToString());
                int SZN_KntNumer = Convert.ToInt32(pomDataTable.Rows[i]["SZN_KntNumer"].ToString());
                int SZN_KnATyp = Convert.ToInt32(pomDataTable.Rows[i]["SZN_KnATyp"].ToString());
                int SZN_KnANumer = Convert.ToInt32(pomDataTable.Rows[i]["SZN_KnANumer"].ToString());
                int SZN_KnDTyp = Convert.ToInt32(pomDataTable.Rows[i]["SZN_KnDTyp"].ToString());
                int SZN_KnDNumer = Convert.ToInt32(pomDataTable.Rows[i]["SZN_KnDNumer"].ToString());
                int SZN_AdWTyp = Convert.ToInt32(pomDataTable.Rows[i]["SZN_AdWTyp"].ToString());
                int SZN_AdWNumer = Convert.ToInt32(pomDataTable.Rows[i]["SZN_AdWNumer"].ToString());
                String SZN_DataWystawienia = pomDataTable.Rows[i]["DataWystawienia"].ToString();
                String SZN_DataRozpoczecia = pomDataTable.Rows[i]["DataRozpoczecia"].ToString();
                String SZN_Stan = pomDataTable.Rows[i]["Stan"].ToString();
                String SZN_Status = pomDataTable.Rows[i]["Status"].ToString();
                String SZN_CechaOpis = pomDataTable.Rows[i]["SZN_CechaOpis"].ToString();
                String SZN_Opis = pomDataTable.Rows[i]["SZN_Opis"].ToString();

                result.Add(new SrwZlcNag(Dokument, SZN_Id, SZN_KntTyp, SZN_KntNumer, SZN_KnATyp, SZN_KnANumer, SZN_KnDTyp, SZN_KnDNumer, SZN_AdWTyp, SZN_AdWNumer, SZN_DataWystawienia, SZN_DataRozpoczecia, SZN_Stan, SZN_Status, SZN_CechaOpis, SZN_Opis, 0));
            }
            return result;
        }






        public List<KntKarty> wygenerujListeKntKarty()
        {

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select knt.knt_GIDNumer, knt.knt_Akronim, knt.knt_nazwa1, knt.knt_nazwa2, knt.knt_nazwa3, knt.knt_KodP, knt.knt_miasto,
                    knt.knt_ulica, knt.knt_Adres, knt.knt_nip, knt.knt_telefon1, knt.knt_telefon2, knt.knt_telex,
                    knt.knt_fax, knt.knt_email, knt.knt_url from cdn.kntkarty knt";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeKntKarty(pomDataTable);
            }
            else
            {
                return null;
            }
        }
        private List<KntKarty> wygenerujListeKntKarty(DataTable pomDataTable)
        {
            List<KntKarty> result = new List<KntKarty>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 Knt_GIDNumer = Convert.ToInt32(pomDataTable.Rows[i]["Knt_GIDNumer"].ToString());
                String Knt_Akronim = pomDataTable.Rows[i]["Knt_Akronim"].ToString();
                String Knt_nazwa1 = pomDataTable.Rows[i]["Knt_nazwa1"].ToString();
                String Knt_nazwa2 = pomDataTable.Rows[i]["Knt_nazwa2"].ToString();
                String Knt_nazwa3 = pomDataTable.Rows[i]["Knt_nazwa3"].ToString();
                String Knt_KodP = pomDataTable.Rows[i]["Knt_KodP"].ToString();
                String Knt_miasto = pomDataTable.Rows[i]["Knt_miasto"].ToString();
                String Knt_ulica = pomDataTable.Rows[i]["Knt_ulica"].ToString();
                String Knt_Adres = pomDataTable.Rows[i]["Knt_Adres"].ToString();
                String Knt_nip = pomDataTable.Rows[i]["Knt_nip"].ToString();
                String Knt_telefon1 = pomDataTable.Rows[i]["Knt_telefon1"].ToString();
                String Knt_telefon2 = pomDataTable.Rows[i]["Knt_telefon2"].ToString();
                String Knt_telex = pomDataTable.Rows[i]["Knt_telex"].ToString();
                String Knt_fax = pomDataTable.Rows[i]["Knt_fax"].ToString();
                String Knt_email = pomDataTable.Rows[i]["Knt_email"].ToString();
                String Knt_url = pomDataTable.Rows[i]["Knt_url"].ToString();

                result.Add(new KntKarty(Knt_GIDNumer, Knt_Akronim, Knt_nazwa1, Knt_nazwa2, Knt_nazwa3, Knt_KodP, Knt_miasto, Knt_ulica, Knt_Adres, Knt_nip, Knt_telefon1, Knt_telefon2, Knt_telex, Knt_fax, Knt_email, Knt_url));
            }
            return result;
        }

        public List<KntAdresy> wygenerujListeKntAdresy()
        {

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select kna_GIDNumer, kna_GIDTyp, kna_kntnumer, kna_Akronim, kna_nazwa1, kna_nazwa2, kna_nazwa3, kna_KodP, kna_miasto,
                    kna_ulica, kna_Adres, kna_nip, kna_telefon1, kna_telefon2, kna_telex, kna_fax, kna_email from cdn.kntadresy";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeKntAdresy(pomDataTable);
            }
            else
            {
                return null;
            }
        }
        private List<KntAdresy> wygenerujListeKntAdresy(DataTable pomDataTable)
        {
            List<KntAdresy> result = new List<KntAdresy>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 Kna_GIDNumer = Convert.ToInt32(pomDataTable.Rows[i]["Kna_GIDNumer"].ToString());
                Int32 Kna_GIDTyp = Convert.ToInt32(pomDataTable.Rows[i]["Kna_GIDTyp"].ToString());
                Int32 Kna_KntNumer = Convert.ToInt32(pomDataTable.Rows[i]["kna_kntnumer"].ToString());
                String Kna_Akronim = pomDataTable.Rows[i]["Kna_Akronim"].ToString();
                String Kna_nazwa1 = pomDataTable.Rows[i]["Kna_nazwa1"].ToString();
                String Kna_nazwa2 = pomDataTable.Rows[i]["Kna_nazwa2"].ToString();
                String Kna_nazwa3 = pomDataTable.Rows[i]["Kna_nazwa3"].ToString();
                String Kna_KodP = pomDataTable.Rows[i]["Kna_KodP"].ToString();
                String Kna_miasto = pomDataTable.Rows[i]["Kna_miasto"].ToString();
                String Kna_ulica = pomDataTable.Rows[i]["Kna_ulica"].ToString();
                String Kna_Adres = pomDataTable.Rows[i]["Kna_Adres"].ToString();
                String Kna_nip = pomDataTable.Rows[i]["Kna_nip"].ToString();
                String Kna_telefon1 = pomDataTable.Rows[i]["Kna_telefon1"].ToString();
                String Kna_telefon2 = pomDataTable.Rows[i]["Kna_telefon2"].ToString();
                String Kna_telex = pomDataTable.Rows[i]["Kna_telex"].ToString();
                String Kna_fax = pomDataTable.Rows[i]["Kna_fax"].ToString();
                String Kna_email = pomDataTable.Rows[i]["Kna_email"].ToString();

                result.Add(new KntAdresy(Kna_GIDNumer, Kna_GIDTyp, Kna_KntNumer,Kna_Akronim,Kna_nazwa1,Kna_nazwa2,Kna_nazwa3,Kna_KodP,Kna_miasto,Kna_ulica,Kna_Adres,Kna_nip,Kna_telefon1,Kna_telefon2,Kna_telex,Kna_fax,Kna_email));
            }
            return result;
        }






        public List<SrwZlcCzynnoci> wygenerujListeSrwZlcCzynnoci()
        {
            DateTime data = DateTime.Now.AddMonths(liczbaMiesiecyWstecz);

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select szc_Id, szc_sznId, szc_Pozycja, szc_TwrNumer, szc_TwrTyp, szc_Ilosc, twrk.twr_jm, szc_TwrNazwa, twrk.Twr_Kod from cdn.SrwZlcCzynnosci
                        LEFT OUTER JOIN cdn.srwzlcnag szn on szn.szn_id = szc_sznid
                        left outer join cdn.twrkarty twrk on twrk.twr_gidnumer = szc_twrnumer
                        where (DATEADD(DAY,szn.SZN_DataWystawienia,CONVERT(DATETIME,'1800-12-28',120) )>'" + data.Year.ToString()+"-"+data.Month.ToString()+"-01')";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeSrwZlcCzynnoci(pomDataTable);
            }
            else
            {
                return null;
            }
        }
        private List<SrwZlcCzynnoci> wygenerujListeSrwZlcCzynnoci(DataTable pomDataTable)
        {
            List<SrwZlcCzynnoci> result = new List<SrwZlcCzynnoci>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 szc_Id = Convert.ToInt32(pomDataTable.Rows[i]["szc_Id"].ToString());
                Int32 szc_sznId = Convert.ToInt32(pomDataTable.Rows[i]["szc_sznId"].ToString());
                Int32 szc_Pozycja = Convert.ToInt32(pomDataTable.Rows[i]["szc_Pozycja"].ToString());
                Int32 szc_TwrNumer = Convert.ToInt32(pomDataTable.Rows[i]["szc_TwrNumer"].ToString());
                Int32 szc_TwrTyp = Convert.ToInt32(pomDataTable.Rows[i]["szc_TwrTyp"].ToString());
                Double szc_Ilosc = Convert.ToDouble(pomDataTable.Rows[i]["szc_Ilosc"].ToString());
                String szc_TwrNazwa = pomDataTable.Rows[i]["szc_TwrNazwa"].ToString();
                String Twr_Jm = pomDataTable.Rows[i]["Twr_Jm"].ToString();
                String Twr_Kod = pomDataTable.Rows[i]["Twr_Kod"].ToString();

                result.Add(new SrwZlcCzynnoci(szc_Id, szc_sznId, szc_Pozycja, szc_TwrNumer, szc_TwrTyp, szc_Ilosc, Twr_Jm, szc_TwrNazwa, Twr_Kod));
            }
            return result;
        }












        public List<SrwZlcSkladniki> wygenerujListeSrwZlcSkladniki()
        {
            DateTime data = DateTime.Now.AddMonths(liczbaMiesiecyWstecz);
            Exception exc = new Exception();

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select szs_Id, szs_sznId, szs_Pozycja, szs_TwrNumer, szs_TwrTyp, szs_Ilosc, twrk.twr_jm, szs_TwrNazwa, twrk.Twr_Kod from cdn.SrwZlcSkladniki
                        LEFT OUTER JOIN cdn.srwzlcnag szn on szn.szn_id = szs_sznid
                        left outer join cdn.twrkarty twrk on twrk.twr_gidnumer = szs_twrnumer
                        where (DATEADD(DAY,szn.SZN_DataWystawienia,CONVERT(DATETIME,'1800-12-28',120) )>'" + data.Year.ToString() + "-" + data.Month.ToString() + "-01')";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception _exc)
            {
                exc = _exc;
            }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeSrwZlcSkladniki(pomDataTable);
            }
            else
            {
                List<SrwZlcSkladniki> result = new List<SrwZlcSkladniki>();
                result.Add(new SrwZlcSkladniki(0, 0, 0, 0, 0, 0, null, exc.Message, null));
                return result;
            }
        }
        private List<SrwZlcSkladniki> wygenerujListeSrwZlcSkladniki(DataTable pomDataTable)
        {
            List<SrwZlcSkladniki> result = new List<SrwZlcSkladniki>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 szs_Id = Convert.ToInt32(pomDataTable.Rows[i]["szs_Id"].ToString());
                Int32 szs_sznId = Convert.ToInt32(pomDataTable.Rows[i]["szs_sznId"].ToString());
                Int32 szs_Pozycja = Convert.ToInt32(pomDataTable.Rows[i]["szs_Pozycja"].ToString());
                Int32 szs_TwrNumer = Convert.ToInt32(pomDataTable.Rows[i]["szs_TwrNumer"].ToString());
                Int32 szs_TwrTyp = Convert.ToInt32(pomDataTable.Rows[i]["szs_TwrTyp"].ToString());
                Double szs_Ilosc = Convert.ToDouble(pomDataTable.Rows[i]["szs_Ilosc"].ToString());
                String szs_TwrNazwa = pomDataTable.Rows[i]["szs_TwrNazwa"].ToString();
                String Twr_Jm = pomDataTable.Rows[i]["Twr_Jm"].ToString();
                String Twr_Kod = pomDataTable.Rows[i]["Twr_Kod"].ToString();

                result.Add(new SrwZlcSkladniki(szs_Id, szs_sznId, szs_Pozycja, szs_TwrNumer, szs_TwrTyp, szs_Ilosc, Twr_Jm, szs_TwrNazwa, Twr_Kod));
            }
            return result;
        }










        public List<TwrKartyTable> wygenerujListeTwrKarty()
        {
            Exception exc = new Exception();

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select Twr_GIDTyp, Twr_GIDNumer, Twr_Kod, Twr_Typ, Twr_Nazwa, Twr_Nazwa1, Twr_Jm  from cdn.twrkarty 
                        order by Twr_Nazwa";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception _exc)
            {
                exc = _exc;
            }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeTwrKarty(pomDataTable);
            }
            else
            {
                List<TwrKartyTable> result = new List<TwrKartyTable>();
                result.Add(new TwrKartyTable(0, 0, exc.Message, 0, "", "", ""));
                return result;
            }
        }
        private List<TwrKartyTable> wygenerujListeTwrKarty(DataTable pomDataTable)
        {
            List<TwrKartyTable> result = new List<TwrKartyTable>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 Twr_GIDTyp = Convert.ToInt32(pomDataTable.Rows[i]["Twr_GIDTyp"].ToString());
                Int32 Twr_GIDNumer = Convert.ToInt32(pomDataTable.Rows[i]["Twr_GIDNumer"].ToString());
                String Twr_Kod = pomDataTable.Rows[i]["Twr_Kod"].ToString();
                Int32 Twr_Typ = Convert.ToInt32(pomDataTable.Rows[i]["Twr_Typ"].ToString());
                String Twr_Nazwa = pomDataTable.Rows[i]["Twr_Nazwa"].ToString();
                String Twr_Nazwa1 = pomDataTable.Rows[i]["Twr_Nazwa1"].ToString();
                String Twr_Jm = pomDataTable.Rows[i]["Twr_Jm"].ToString();


                result.Add(new TwrKartyTable(Twr_GIDTyp, Twr_GIDNumer, Twr_Kod, Twr_Typ, Twr_Nazwa, Twr_Nazwa1, Twr_Jm));
            }
            return result;
        }


    }
}















public class SrwZlcNag
{
    public String Dokument { get; set; }
    public int SZN_Id { get; set; }
    public int SZN_KntTyp { get; set; }
    public int SZN_KntNumer { get; set; }
    public int SZN_KnATyp { get; set; }
    public int SZN_KnANumer { get; set; }
    public int SZN_KnDTyp { get; set; }
    public int SZN_KnDNumer { get; set; }
    public int SZN_AdWTyp { get; set; }
    public int SZN_AdWNumer { get; set; }
    public String SZN_DataWystawienia { get; set; }
    public String SZN_DataRozpoczecia { get; set; }
    public String SZN_Stan { get; set; }
    public String SZN_Status { get; set; }
    public String SZN_CechaOpis { get; set; }
    public String SZN_Opis { get; set; }
    public int SZN_Synchronizacja { get; set; }


    public SrwZlcNag(String _Dokument, int _SZN_Id, int _SZN_KntTyp, int _SZN_KntNumer, int _SZN_KnATyp, int _SZN_KnANumer, int _SZN_KnDTyp, int _SZN_KnDNumer, int _SZN_AdWTyp, int _SZN_AdWNumer, String _SZN_DataWystawienia, String _SZN_DataRozpoczecia, String _SZN_Stan, String _SZN_Status, String _SZN_CechaOpis, String _SZN_Opis, int _SZN_Synchronizacja)
    {
        Dokument = _Dokument;
        SZN_Id = _SZN_Id;
        SZN_KntTyp = _SZN_KntTyp;
        SZN_KntNumer = _SZN_KntNumer;
        SZN_KnATyp = _SZN_KnATyp;
        SZN_KnANumer = _SZN_KnANumer;
        SZN_KnDTyp = _SZN_KnDTyp;
        SZN_KnDNumer = _SZN_KnDNumer;
        SZN_AdWTyp = _SZN_AdWTyp;
        SZN_AdWNumer = _SZN_AdWNumer;
        SZN_DataWystawienia = _SZN_DataWystawienia;
        SZN_DataRozpoczecia = _SZN_DataRozpoczecia;
        SZN_Stan = _SZN_Stan;
        SZN_Status = _SZN_Status;
        SZN_CechaOpis = _SZN_CechaOpis;
        SZN_Opis = _SZN_Opis;
        SZN_Synchronizacja = _SZN_Synchronizacja;
    }
    public SrwZlcNag() { }
}

public class KntKarty
{
    public Int32 Knt_GIDNumer { get; set; }
    public String Knt_Akronim { get; set; }
    public String Knt_nazwa1 { get; set; }
    public String Knt_nazwa2 { get; set; }
    public String Knt_nazwa3 { get; set; }
    public String Knt_KodP { get; set; }
    public String Knt_miasto { get; set; }
    public String Knt_ulica { get; set; }
    public String Knt_Adres { get; set; }
    public String Knt_nip { get; set; }
    public String Knt_telefon1 { get; set; }
    public String Knt_telefon2 { get; set; }
    public String Knt_telex { get; set; }
    public String Knt_fax { get; set; }
    public String Knt_email { get; set; }
    public String Knt_url { get; set; }

    public KntKarty(Int32 _Knt_GIDNumer, String _Knt_Akronim, String _Knt_nazwa1, String _Knt_nazwa2, String _Knt_nazwa3, String _Knt_KodP, String _Knt_miasto, String _Knt_ulica, String _Knt_Adres, String _Knt_nip, String _Knt_telefon1, String _Knt_telefon2, String _Knt_telex, String _Knt_fax, String _Knt_email, String _Knt_url)
    {
        Knt_GIDNumer = _Knt_GIDNumer;
        Knt_Akronim = _Knt_Akronim;
        Knt_nazwa1 = _Knt_nazwa1;
        Knt_nazwa2 = _Knt_nazwa2;
        Knt_nazwa3 = _Knt_nazwa3;
        Knt_KodP = _Knt_KodP;
        Knt_miasto = _Knt_miasto;
        Knt_ulica = _Knt_ulica;
        Knt_Adres = _Knt_Adres;
        Knt_nip = _Knt_nip;
        Knt_telefon1 = _Knt_telefon1;
        Knt_telefon2 = _Knt_telefon2;
        Knt_telex = _Knt_telex;
        Knt_fax = _Knt_fax;
        Knt_email = _Knt_email;
        Knt_url = _Knt_url;
    }

    public KntKarty() { }
}

public class KntAdresy
{
    public Int32 Kna_GIDNumer { get; set; }
    public Int32 Kna_GIDTyp { get; set; }
    public Int32 Kna_KntNumer { get; set; }
    public String Kna_Akronim { get; set; }
    public String Kna_nazwa1 { get; set; }
    public String Kna_nazwa2 { get; set; }
    public String Kna_nazwa3 { get; set; }
    public String Kna_KodP { get; set; }
    public String Kna_miasto { get; set; }
    public String Kna_ulica { get; set; }
    public String Kna_Adres { get; set; }
    public String Kna_nip { get; set; }
    public String Kna_telefon1 { get; set; }
    public String Kna_telefon2 { get; set; }
    public String Kna_telex { get; set; }
    public String Kna_fax { get; set; }
    public String Kna_email { get; set; }

    public KntAdresy(Int32 _Kna_GIDNumer, Int32 _Kna_GIDTyp, Int32 _Kna_KntNumer, String _Kna_Akronim, String _Kna_nazwa1, String _Kna_nazwa2, String _Kna_nazwa3, String _Kna_KodP, String _Kna_miasto, String _Kna_ulica, String _Kna_Adres, String _Kna_nip, String _Kna_telefon1, String _Kna_telefon2, String _Kna_telex, String _Kna_fax, String _Kna_email)
    {
        Kna_GIDNumer = _Kna_GIDNumer;
        Kna_GIDTyp = _Kna_GIDTyp;
        Kna_KntNumer = _Kna_KntNumer;
        Kna_Akronim = _Kna_Akronim;
        Kna_nazwa1 = _Kna_nazwa1;
        Kna_nazwa2 = _Kna_nazwa2;
        Kna_nazwa3 = _Kna_nazwa3;
        Kna_KodP = _Kna_KodP;
        Kna_miasto = _Kna_miasto;
        Kna_ulica = _Kna_ulica;
        Kna_Adres = _Kna_Adres;
        Kna_nip = _Kna_nip;
        Kna_telefon1 = _Kna_telefon1;
        Kna_telefon2 = _Kna_telefon2;
        Kna_telex = _Kna_telex;
        Kna_fax = _Kna_fax;
        Kna_email = _Kna_email;
    }
    public KntAdresy() { }
}

public class SrwZlcCzynnoci
{
    public Int32 szc_Id { get; set; }
    public Int32 szc_sznId { get; set; }
    public Int32 szc_Pozycja { get; set; }
    public Int32 szc_TwrNumer { get; set; }
    public Int32 szc_TwrTyp { get; set; }
    public Double szc_Ilosc { get; set; }
    public String Twr_Jm { get; set; }
    public String szc_TwrNazwa { get; set; }
    public String Twr_Kod { get; set; }

    public SrwZlcCzynnoci(Int32 _szc_Id, Int32 _szc_sznId, Int32 _szc_Pozycja, Int32 _szc_TwrNumer, Int32 _szc_TwrTyp, Double _szc_Ilosc, String _Twr_Jm, String _szc_TwrNazwa, String _Twr_Kod)
    {
        szc_Id = _szc_Id;
        szc_sznId = _szc_sznId;
        szc_Pozycja = _szc_Pozycja;
        szc_TwrNumer = _szc_TwrNumer;
        szc_TwrTyp = _szc_TwrTyp;
        szc_Ilosc = _szc_Ilosc;
        Twr_Jm = _Twr_Jm;
        szc_TwrNazwa = _szc_TwrNazwa;
        Twr_Kod = _Twr_Kod;
    }
    public SrwZlcCzynnoci() { }
}



public class SrwZlcSkladniki
{
    public Int32 szs_Id { get; set; }
    public Int32 szs_sznId { get; set; }
    public Int32 szs_Pozycja { get; set; }
    public Int32 szs_TwrNumer { get; set; }
    public Int32 szs_TwrTyp { get; set; }
    public Double szs_Ilosc { get; set; }
    public String Twr_Jm { get; set; }
    public String szs_TwrNazwa { get; set; }
    public String Twr_Kod { get; set; }

    public SrwZlcSkladniki(Int32 _szs_Id, Int32 _szs_sznId, Int32 _szs_Pozycja, Int32 _szs_TwrNumer, Int32 _szs_TwrTyp, Double _szs_Ilosc, String _Twr_Jm, String _szs_TwrNazwa, String _Twr_Kod)
    {
        szs_Id = _szs_Id;
        szs_sznId = _szs_sznId;
        szs_Pozycja = _szs_Pozycja;
        szs_TwrNumer = _szs_TwrNumer;
        szs_TwrTyp = _szs_TwrTyp;
        szs_Ilosc = _szs_Ilosc;
        Twr_Jm = _Twr_Jm;
        szs_TwrNazwa = _szs_TwrNazwa;
        Twr_Kod = _Twr_Kod;
    }
    public SrwZlcSkladniki() { }
}

public class TwrKartyTable
{
    public Int32 Twr_GIDTyp { get; set; }

    public Int32 Twr_GIDNumer { get; set; }
    
    public String Twr_Kod { get; set; }

    public Int32 Twr_Typ { get; set; }
    
    public String Twr_Nazwa { get; set; }

    public String Twr_Nazwa1 { get; set; }

    public String Twr_Jm { get; set; }

    public TwrKartyTable(Int32 _Twr_GIDTyp, Int32 _Twr_GIDNumer, String _Twr_Kod, Int32 _Twr_Typ, String _Twr_Nazwa, String _Twr_Nazwa1, String _Twr_Jm)
    {
        Twr_GIDTyp = _Twr_GIDTyp;
        Twr_GIDNumer = _Twr_GIDNumer;
        Twr_Kod = _Twr_Kod;
        Twr_Typ = _Twr_Typ;
        Twr_Nazwa = _Twr_Nazwa;
        Twr_Nazwa1 = _Twr_Nazwa1;
        Twr_Jm = _Twr_Jm;
    }

    public TwrKartyTable() { }
}