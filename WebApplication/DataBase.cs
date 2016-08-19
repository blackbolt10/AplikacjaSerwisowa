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
            int hasloSRV = 2;                               //modyfikacja hasel do bazy

            podlaczDoBazyDanych(hasloSRV);
            podlaczDoBazyDanychSerwis(hasloSRV);
        }

        private Boolean podlaczDoBazyDanych(int hasloSRV)
        {
            Boolean wynikLogowaniaDoBD = false;
            hasla haslo = new hasla(hasloSRV);

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

        private Boolean podlaczDoBazyDanychSerwis(int hasloSRV)
        {
            Boolean wynikLogowaniaDoBD = false;
            hasla haslo = new hasla(hasloSRV);

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

        public List<SrwZlcPodpisTable> wygenerujListeSrwZlcPodpis()
        {
            List<SrwZlcPodpisTable> result = new List<SrwZlcPodpisTable>();
            

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "Select * from [GAL].[SrwZlcPodpis] where [GZP_Synchronizacja] <> 4";
                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                result = wygenerujListeSerwisowychZlecenPodpisy(pomDataTable);
            }
            return result;
        }

        private List<SrwZlcPodpisTable> wygenerujListeSerwisowychZlecenPodpisy(DataTable pomDataTable)
        {
            List<SrwZlcPodpisTable> result = new List<SrwZlcPodpisTable>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                int GZP_Id = Convert.ToInt32(pomDataTable.Rows[i]["GZP_Id"].ToString());
                int GZP_Synchronizacja = 0;
                String Podpis = pomDataTable.Rows[i]["GZP_Podpis"].ToString();
                String OsobaPodpisujaca = pomDataTable.Rows[i]["GZP_OsobaPodpisujaca"].ToString();

                result.Add(new SrwZlcPodpisTable(GZP_Id, GZP_Synchronizacja, Podpis,OsobaPodpisujaca));
            }
            return result;
        }

        public List<SrwUrzParDef> wygenerujListeSrwUrzParDef()
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select SUD_Id, SUD_Nazwa, SUD_Format, SUD_Archiwalna from CDN.SrwUrzParDef";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeSrwUrzParDef(pomDataTable);
            }
            else
            {
                return new List<SrwUrzParDef>();
            }
        }

        private List<SrwUrzParDef> wygenerujListeSrwUrzParDef(DataTable pomDataTable)
        {
            List<SrwUrzParDef> result = new List<SrwUrzParDef>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                int SUD_Id = Convert.ToInt32(pomDataTable.Rows[i]["SUD_Id"].ToString());
                String SUD_Nazwa = pomDataTable.Rows[i]["SUD_Nazwa"].ToString();
                String SUD_Format = pomDataTable.Rows[i]["SUD_Format"].ToString();
                int SUD_Archiwalna = Convert.ToInt32(pomDataTable.Rows[i]["SUD_Archiwalna"].ToString());

                result.Add(new SrwUrzParDef(SUD_Id, SUD_Nazwa, SUD_Format, SUD_Archiwalna));
            }
            return result;
        }

        public List<SrwUrzRodzaje> wygenerujListeSrwUrzRodzaje()
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"SELECT SUR_Id, SUR_Kod, SUR_Nazwa FROM CDN.SrwUrzRodzaje";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeSrwUrzRodzaje(pomDataTable);
            }
            else
            {
                return new List<SrwUrzRodzaje>();
            }
        }

        private List<SrwUrzRodzaje> wygenerujListeSrwUrzRodzaje(DataTable pomDataTable)
        {
            List<SrwUrzRodzaje> result = new List<SrwUrzRodzaje>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                int SUR_Id = Convert.ToInt32(pomDataTable.Rows[i]["SUR_Id"].ToString());
                String SUR_Kod = pomDataTable.Rows[i]["SUR_Kod"].ToString();
                String SUR_Nazwa = pomDataTable.Rows[i]["SUR_Nazwa"].ToString();

                result.Add(new SrwUrzRodzaje(SUR_Id, SUR_Kod, SUR_Nazwa));
            }
            return result;
        }

        public List<SrwUrzRodzPar> wygenerujListeSrwUrzRodzPar()
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select SRP_Id, SRP_SURId, SRP_SUDId, SRP_Lp from CDN.SrwUrzRodzPar";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeSrwUrzRodzPar(pomDataTable);
            }
            else
            {
                return new List<SrwUrzRodzPar>();
            }
        }

        private List<SrwUrzRodzPar> wygenerujListeSrwUrzRodzPar(DataTable pomDataTable)
        {
            List<SrwUrzRodzPar> result = new List<SrwUrzRodzPar>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                int SRP_Id = Convert.ToInt32(pomDataTable.Rows[i]["SRP_Id"].ToString());
                int SRP_SURId = Convert.ToInt32(pomDataTable.Rows[i]["SRP_SURId"].ToString());
                int SRP_SUDId = Convert.ToInt32(pomDataTable.Rows[i]["SRP_SUDId"].ToString());
                int SRP_Lp = Convert.ToInt32(pomDataTable.Rows[i]["SRP_Lp"].ToString());

                result.Add(new SrwUrzRodzPar(SRP_Id, SRP_SURId, SRP_SUDId, SRP_Lp));
            }
            return result;
        }
    

        public List<SrwUrzadzenia> wygenerujListeSrwUrzadzenia()
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select SrU_Id, SrU_SURId, Sru_Nazwa, SrU_Opis, SrU_Archiwalne from cdn.SrwUrzadzenia";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeSrwUrzadzenia(pomDataTable);
            }
            else
            {
                return new List<SrwUrzadzenia>();
            }
        }

        private List<SrwUrzadzenia> wygenerujListeSrwUrzadzenia(DataTable pomDataTable)
        {
            List<SrwUrzadzenia> result = new List<SrwUrzadzenia>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                int SrU_Id = Convert.ToInt32(pomDataTable.Rows[i]["SrU_Id"].ToString());
                int SrU_SURId = Convert.ToInt32(pomDataTable.Rows[i]["SrU_SURId"].ToString());
                String Sru_Nazwa = pomDataTable.Rows[i]["Sru_Nazwa"].ToString();
                String SrU_Opis = pomDataTable.Rows[i]["SrU_Opis"].ToString();
                int SrU_Archiwalne = Convert.ToInt32(pomDataTable.Rows[i]["SrU_Archiwalne"].ToString());

                result.Add(new SrwUrzadzenia(SrU_Id, SrU_SURId, Sru_Nazwa, SrU_Opis, SrU_Archiwalne));
            }
            return result;
        }

        public List<SrwZlcNag> wygenerujListeSerwisowychZlecenNaglowki()
        {
            List<SrwZlcNag> result = new List<SrwZlcNag>();
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

            --, A.SZN_CechaOpis
            , A.SZN_Opis
            --, A.SZN_MagZNumer, A.SZN_SlwStatus, B.SLW_ID, B.SLW_Predefiniowany

            FROM  CDN.SrwZlcNag A 
            LEFT OUTER JOIN CDN.Slowniki B ON  A.SZN_SlwStatus= B.SLW_ID --
            LEFT OUTER JOIN CDN.Slowniki C ON  A.SZN_FiaskoID= C.SLW_ID 
            --LEFT OUTER JOIN CDN.KntAdresy D ON  A.SZN_AdWTyp= D.KnA_GIDTyp AND  A.SZN_AdWNumer= D.KnA_GIDNumer 
            LEFT OUTER JOIN CDN.KntKarty E ON  A.SZN_KnDTyp= E.Knt_GIDTyp AND  A.SZN_KnDNumer= E.Knt_GIDNumer --Docelowy
            --LEFT OUTER JOIN CDN.KntAdresy F ON  A.SZN_KnATyp= F.KnA_GIDTyp AND  A.SZN_KnANumer= F.KnA_GIDNumer 
            LEFT OUTER JOIN CDN.KntKarty G ON  A.SZN_KntTyp= G.Knt_GIDTyp AND  A.SZN_KntNumer= G.Knt_GIDNumer   --Kontrahent
            WHERE (DATEADD(DAY,A.SZN_DataWystawienia,CONVERT(DATETIME,'1800-12-28',120) )>'" + data.Year.ToString() + "-" + data.Month.ToString() + @"-01') 
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
                result =  wygenerujListeSerwisowychZlecen(pomDataTable);
            }
            return result;
        }

        private List<SrwZlcNag> wygenerujListeSerwisowychZlecen(DataTable pomDataTable)
        {
            List<SrwZlcNag> result = new List<SrwZlcNag>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                int SZN_Id = Convert.ToInt32(pomDataTable.Rows[i]["SZN_Id"].ToString());
                int SZN_Synchronizacja = 0;
                int SZN_KntTyp = Convert.ToInt32(pomDataTable.Rows[i]["SZN_KntTyp"].ToString());
                int SZN_KntNumer = Convert.ToInt32(pomDataTable.Rows[i]["SZN_KntNumer"].ToString());
                int SZN_KnATyp = Convert.ToInt32(pomDataTable.Rows[i]["SZN_KnATyp"].ToString());
                int SZN_KnANumer = Convert.ToInt32(pomDataTable.Rows[i]["SZN_KnANumer"].ToString());
                String SZN_Dokument = pomDataTable.Rows[i]["Dokument"].ToString();
                String SZN_DataWystawienia = pomDataTable.Rows[i]["DataWystawienia"].ToString();
                String SZN_DataRozpoczecia = pomDataTable.Rows[i]["DataRozpoczecia"].ToString();
                String SZN_Stan = pomDataTable.Rows[i]["Stan"].ToString();
                String SZN_Status = pomDataTable.Rows[i]["Status"].ToString();
                String SZN_Opis = pomDataTable.Rows[i]["SZN_Opis"].ToString();

                result.Add(new SrwZlcNag(SZN_Id, SZN_Synchronizacja, SZN_KntTyp, SZN_KntNumer, SZN_KnATyp, SZN_KnANumer, SZN_Dokument, SZN_DataWystawienia, SZN_DataRozpoczecia, SZN_Stan, SZN_Status, SZN_Opis));
            }
            return result;
        }

        public List<SrwZlcCzynnosci> wygenerujListeSrwZlcCzynnosci()
        {
            DateTime data = DateTime.Now.AddMonths(liczbaMiesiecyWstecz);
            List<SrwZlcCzynnosci> result = new List<SrwZlcCzynnosci>();

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select SZC_Id, SZC_SZNId, SZC_SZUId, SZC_Pozycja, SZC_Ilosc, SZC_TwrNumer, SZC_TwrTyp, SZC_TwrNazwa, SZC_Opis
                    from cdn.SrwZlcCzynnosci
                    LEFT OUTER JOIN cdn.srwzlcnag szn on szn.szn_id = SZC_sznid
                    where (DATEADD(DAY,szn.SZN_DataWystawienia,CONVERT(DATETIME,'1800-12-28',120) )>'" + data.Year.ToString() + "-" + data.Month.ToString() + "-01')";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                result = wygenerujListeSrwZlcCzynnosci(pomDataTable);
            }
            return result;
        }

        private List<SrwZlcCzynnosci> wygenerujListeSrwZlcCzynnosci(DataTable pomDataTable)
        {
            List<SrwZlcCzynnosci> result = new List<SrwZlcCzynnosci>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 SZC_Id = Convert.ToInt32(pomDataTable.Rows[i]["SZC_Id"].ToString());
                Int32 SZC_SZNId = Convert.ToInt32(pomDataTable.Rows[i]["SZC_SZNId"].ToString());
                Int32 SZC_SZUId = Convert.ToInt32(pomDataTable.Rows[i]["SZC_SZUId"].ToString());
                Int32 SZC_Synchronizacja = 0;
                Int32 SZC_Pozycja = Convert.ToInt32(pomDataTable.Rows[i]["SZC_Pozycja"].ToString());
                Int32 SZC_TwrTyp = Convert.ToInt32(pomDataTable.Rows[i]["SZC_TwrTyp"].ToString());
                Int32 SZC_TwrNumer = Convert.ToInt32(pomDataTable.Rows[i]["SZC_TwrNumer"].ToString());
                String SZC_TwrNazwa = pomDataTable.Rows[i]["SZC_TwrNazwa"].ToString();
                Double SZC_Ilosc = Convert.ToDouble(pomDataTable.Rows[i]["SZC_Ilosc"].ToString());
                String SZC_Opis = pomDataTable.Rows[i]["SZC_Opis"].ToString();

                result.Add(new SrwZlcCzynnosci(SZC_Id, SZC_SZNId, SZC_SZUId, SZC_Synchronizacja, SZC_Pozycja, SZC_TwrTyp, SZC_TwrNumer, SZC_TwrNazwa, SZC_Ilosc, SZC_Opis));
            }
            return result;
        }

        public List<SrwZlcSkladniki> wygenerujListeSrwZlcSkladniki()
        {
            List<SrwZlcSkladniki> result = new List<SrwZlcSkladniki>();
            DateTime data = DateTime.Now.AddMonths(liczbaMiesiecyWstecz);

            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select SZS_Id, SZS_SZNId, SZS_Pozycja, SZS_TwrTyp, SZS_TwrNumer, SZS_TwrNazwa, SZS_Ilosc, SZS_Opis
                    from cdn.SrwZlcSkladniki
                    LEFT OUTER JOIN cdn.srwzlcnag szn on szn.szn_id = szs_sznid
                    where (DATEADD(DAY,szn.SZN_DataWystawienia,CONVERT(DATETIME,'1800-12-28',120) )>'" + data.Year.ToString() + "-" + data.Month.ToString() + "-01')";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception)
            {}

            if(pomDataTable.Rows.Count > 0)
            {
                result = wygenerujListeSrwZlcSkladniki(pomDataTable);
            }
            return result;
        }
        private List<SrwZlcSkladniki> wygenerujListeSrwZlcSkladniki(DataTable pomDataTable)
        {
            List<SrwZlcSkladniki> result = new List<SrwZlcSkladniki>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 SZS_Id = Convert.ToInt32(pomDataTable.Rows[i]["SZS_Id"].ToString());
                Int32 SZS_sznId = Convert.ToInt32(pomDataTable.Rows[i]["SZS_SZNId"].ToString());
                Int32 SZS_Synchronizacja = 0;
                Int32 SZS_Pozycja = Convert.ToInt32(pomDataTable.Rows[i]["SZS_Pozycja"].ToString());
                Int32 SZS_TwrTyp = Convert.ToInt32(pomDataTable.Rows[i]["SZS_TwrTyp"].ToString());
                Int32 SZS_TwrNumer = Convert.ToInt32(pomDataTable.Rows[i]["SZS_TwrNumer"].ToString());
                String SZS_TwrNazwa = pomDataTable.Rows[i]["SZS_TwrNazwa"].ToString();
                Double SZS_Ilosc = Convert.ToDouble(pomDataTable.Rows[i]["SZS_Ilosc"].ToString());
                String SZS_Opis= pomDataTable.Rows[i]["SZS_Opis"].ToString();

                result.Add(new SrwZlcSkladniki(SZS_Id, SZS_sznId, SZS_Synchronizacja, SZS_Pozycja, SZS_Ilosc, SZS_TwrNumer, SZS_TwrTyp, SZS_TwrNazwa, SZS_Opis));
            }
            return result;
        }
        public List<Operatorzy> wygenerujListeOperatorow()
        {
            List<Operatorzy> result = new List<Operatorzy>();
            DataTable pomDataTable = new DataTable();

            try
            {
                String zapytanieString = @"SELECT GZO_Id, GZO_Akronim, GZO_Haslo, GZO_Imie, GZO_Nazwisko 
                    FROM [GALXL_Serwis].[GAL].[Operatorzy]";

                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                result = wygenerujListeOperatorow(pomDataTable);
            }
            return result;
        }

        private List<Operatorzy> wygenerujListeOperatorow(DataTable pomDataTable)
        {
            List<Operatorzy> result = new List<Operatorzy>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                String GZO_Akronim = pomDataTable.Rows[i]["GZO_Akronim"].ToString();
                String GZO_Haslo = pomDataTable.Rows[i]["GZO_Haslo"].ToString();
                String GZO_Imie = pomDataTable.Rows[i]["GZO_Imie"].ToString();
                String GZO_Nazwisko = pomDataTable.Rows[i]["GZO_Nazwisko"].ToString();

                result.Add(new Operatorzy(GZO_Akronim, GZO_Haslo, GZO_Imie, GZO_Nazwisko));
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

                result.Add(new KntAdresy(Kna_GIDNumer, Kna_GIDTyp, Kna_KntNumer, Kna_Akronim, Kna_nazwa1, Kna_nazwa2, Kna_nazwa3, Kna_KodP, Kna_miasto, Kna_ulica, Kna_Adres, Kna_nip, Kna_telefon1, Kna_telefon2, Kna_telex, Kna_fax, Kna_email));
            }
            return result;
        }

        public List<TwrKartyTable> wygenerujListeTwrKarty()
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select Twr_GIDTyp, Twr_GIDNumer, Twr_Kod, Twr_Typ, Twr_Nazwa, Twr_Nazwa1, Twr_Jm  from cdn.twrkarty 
                        order by Twr_Nazwa";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeTwrKarty(pomDataTable);
            }
            else
            {
                return new List<TwrKartyTable>();
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

        public List<int> synchronizujSrwZlcNag(string inputJSON)
        {
            List<int> result = new List<int>();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SrwZlcNag> records = ser.Deserialize<List<SrwZlcNag>>(inputJSON);

            for(int i = 0; i < records.Count; i++)
            {
                Boolean wynik = zapiszSrwZlcNagSerwis(records[i]);
                if(wynik)
                {
                    result.Add(records[i].SZN_Id);
                }
            }

            return result;
        }

        private Boolean zapiszSrwZlcNagSerwis(SrwZlcNag SZN)
        {
            DataTable pomdatatable = new DataTable();

            try
            {
                String zapytanieString = @"INSERT INTO [GAL].[SrwZlcNag] VALUES("+ SZN .SZN_Id+ ", 0, " + SZN.SZN_KntTyp + ", " + SZN.SZN_KntNumer + ", " + SZN.SZN_KnATyp + ", " + SZN.SZN_KnANumer + ", '" + SZN.SZN_Dokument + "', '" + SZN.SZN_DataWystawienia + "', '" + SZN.SZN_DataRozpoczecia + "', '" + SZN.SZN_Stan + "', '" + SZN.SZN_Status + "', '" + SZN.SZN_Opis + "')";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public List<int> synchronizujSrwZlcCzynnosci(string inputJSON)
        {
            List<int> result = new List<int>();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SrwZlcCzynnosci> records = ser.Deserialize<List<SrwZlcCzynnosci>>(inputJSON);

            for(int i = 0; i < records.Count; i++)
            {
                Boolean wynik = zapiszSrwZlcCzynnosciSerwis(records[i]);
                if(wynik)
                {
                    result.Add(records[i].SZC_Id);
                }
            }

            return result;
        }

        private Boolean zapiszSrwZlcCzynnosciSerwis(SrwZlcCzynnosci SZC)
        {
            DataTable pomdatatable = new DataTable();

            try
            {
                String zapytanieString = "INSERT INTO [GAL].[SrwZlcCzynnosci] VALUES(" + SZC.SZC_Id + ", " + SZC.SZC_SZNId + ", " + SZC.SZC_SZUId + ", 0, " + SZC.SZC_Pozycja + ", " + SZC.SZC_TwrTyp+", "+ SZC.SZC_TwrNumer+", '"+ SZC.SZC_TwrNazwa+"', '" + SZC.SZC_Ilosc + "', '" + SZC.SZC_Opis + "')";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public List<int> synchronizujSrwZlcSkladniki(string inputJSON)
        {
            List<int> result = new List<int>();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SrwZlcSkladniki> records = ser.Deserialize<List<SrwZlcSkladniki>>(inputJSON);

            for(int i = 0; i < records.Count; i++)
            {
                Boolean wynik = zapiszSrwZlcSkladnikiSerwis(records[i]);
                if(wynik)
                {
                    result.Add(records[i].SZS_Id);
                }
            }

            return result;
        }

        private Boolean zapiszSrwZlcSkladnikiSerwis(SrwZlcSkladniki SZS)
        {
            DataTable pomdatatable = new DataTable();

            try
            {
                String zapytanieString = "INSERT INTO [GAL].[SrwZlcSkladniki] VALUES(" + SZS.SZS_Id + ", " + SZS.SZS_SZNId + ", 0, " + SZS.SZS_Pozycja + ", " + SZS.SZS_TwrTyp + ", " + SZS.SZS_TwrNumer + ", '" + SZS.SZS_TwrNazwa + "', '" + SZS.SZS_Ilosc + "', '" + SZS.SZS_Opis + "')";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public List<SrwZlcNag> GalSrv_Generuj_SrwZlcNag()
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "SELECT * FROM [GALXL_Serwis].[GAL].[SrwZlcNag] where [GZN_Synchronizacja] <> 4";

                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return GalSrv_GenerujListe_SrwZlcNag(pomDataTable);
            }
            else
            {
                return null;
            }
        }

        private List<SrwZlcNag> GalSrv_GenerujListe_SrwZlcNag(DataTable pomDataTable)
        {
            List<SrwZlcNag> result = new List<SrwZlcNag>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                int GZN_Id = Convert.ToInt32(pomDataTable.Rows[i]["GZN_Id"].ToString());
                int GZN_Synchronizacja = Convert.ToInt32(pomDataTable.Rows[i]["[GZN_Synchronizacja]"].ToString());
                int GZN_KntTyp = Convert.ToInt32(pomDataTable.Rows[i]["GZN_KntTyp"].ToString());
                int GZN_KntNumer = Convert.ToInt32(pomDataTable.Rows[i]["GZN_KntNumer"].ToString());
                int GZN_KnATyp = Convert.ToInt32(pomDataTable.Rows[i]["GZN_KnATyp"].ToString());
                int GZN_KnANumer = Convert.ToInt32(pomDataTable.Rows[i]["GZN_KnANumer"].ToString());
                String Dokument = pomDataTable.Rows[i]["GZN_Dokument"].ToString();
                String GZN_DataWystawienia = pomDataTable.Rows[i]["GZN_DataWystawienia"].ToString();
                String GZN_DataRozpoczecia = pomDataTable.Rows[i]["GZN_DataRozpoczecia"].ToString();
                String GZN_Stan = pomDataTable.Rows[i]["GZN_Stan"].ToString();
                String GZN_Status = pomDataTable.Rows[i]["GZN_Status"].ToString();
                String GZN_Opis = pomDataTable.Rows[i]["GZN_Opis"].ToString();

                result.Add(new SrwZlcNag(GZN_Id, GZN_Synchronizacja,GZN_KntTyp,GZN_KntNumer,GZN_KnATyp,GZN_KnANumer,Dokument,GZN_DataWystawienia,GZN_DataRozpoczecia,GZN_Stan,GZN_Status,GZN_Opis));
            }
            return result;
        }

        public string GalSrv_Potwierdz_SrwZlcNag(String listaPotwierdzonych)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Int32> records = ser.Deserialize<List<Int32>>(listaPotwierdzonych);
            string result = "";

            if(records != null)
            {
                for(int i = 0; i < records.Count; i++)
                {
                    try
                    {
                        String zapytanieString = "UPDATE [GALXL_Serwis].[GAL].[SrwZlcNag] SET [GZN_Synchronizacja] = 4 WHERE [GZN_Id] = " + records[i].ToString();
                        SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                        DataTable pomdatatable = new DataTable();
                        da.Fill(pomdatatable);

                        result += "zapisano: " + zapytanieString + "\n";
                        result += "zapisano: " + records[i].ToString() + "\n";
                    }
                    catch(Exception exc)
                    {
                        result += "błąd: " + exc.Message + "\n";
                    }
                }
            }
            return result;
        }

        public List<SrwZlcCzynnosci> GalSrv_Generuj_SrwZlcCzynnosci()
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "SELECT * FROM [GALXL_Serwis].[GAL].[SrwZlcCzynnosci] where [GZC_Synchronizacja] <> 4";

                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return GalSrv_GenerujListe_SrwZlcCzynnosci(pomDataTable);
            }
            else
            {
                return null;
            }
        }

        private List<SrwZlcCzynnosci> GalSrv_GenerujListe_SrwZlcCzynnosci(DataTable pomDataTable)
        {
            List<SrwZlcCzynnosci> result = new List<SrwZlcCzynnosci>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 GZC_Id = Convert.ToInt32(pomDataTable.Rows[i]["GZC_Id"].ToString());
                Int32 GZC_GZCId = Convert.ToInt32(pomDataTable.Rows[i]["GZC_GZCId"].ToString());
                Int32 GZC_GZNId = Convert.ToInt32(pomDataTable.Rows[i]["GZC_GZNId"].ToString());
                Int32 GZC_GZUId = Convert.ToInt32(pomDataTable.Rows[i]["GZC_GZUId"].ToString());
                Int32 GZC_Synchronizacja = Convert.ToInt32(pomDataTable.Rows[i]["GZC_Synchronizacja"].ToString());
                Int32 GZC_Pozycja = Convert.ToInt32(pomDataTable.Rows[i]["GZC_Pozycja"].ToString());
                Int32 GZC_TwrTyp = Convert.ToInt32(pomDataTable.Rows[i]["GZC_TwrTyp"].ToString());
                Int32 GZC_TwrNumer = Convert.ToInt32(pomDataTable.Rows[i]["GZC_TwrNumer"].ToString());
                String GZC_TwrNazwa = pomDataTable.Rows[i]["GZC_TwrNazwa"].ToString();
                Double GZC_Ilosc = Convert.ToDouble(pomDataTable.Rows[i]["GZC_Ilosc"].ToString());
                String GZC_Opis = pomDataTable.Rows[i]["GZC_Opis"].ToString();

                result.Add(new SrwZlcCzynnosci(GZC_GZCId, GZC_GZNId, GZC_GZUId,GZC_Synchronizacja, GZC_Pozycja,GZC_TwrTyp,GZC_TwrNumer,GZC_TwrNazwa,GZC_Ilosc,GZC_Opis));
            }
            return result;
        }

        public string GalSrv_Potwierdz_SrwZlcCzynnosci(string listaPotwierdzonych)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Int32> records = ser.Deserialize<List<Int32>>(listaPotwierdzonych);
            string result = "";
            if(records != null)
            {
                for(int i = 0; i < records.Count; i++)
                {
                    try
                    {
                        String zapytanieString = "UPDATE [GALXL_Serwis].[GAL].[SrwZlcCzynnosci] SET [GZC_Synchronizacja] = 4 WHERE [GZC_Id] = " + records[i].ToString();
                        SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                        DataTable pomdatatable = new DataTable();
                        da.Fill(pomdatatable);

                        result += "zapisano: " + zapytanieString + "\n";
                        result += "zapisano: " + records[i].ToString() + "\n";
                    }
                    catch(Exception exc)
                    {
                        result += "błąd: " + exc.Message + "\n";
                    }
                }
            }
            return result;
        }

        public List<SrwZlcSkladniki> GalSrv_Generuj_SrwZlcSkladniki()
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "SELECT * FROM [GALXL_Serwis].[GAL].[SrwZlcSkladniki] where [GZS_Synchronizacja] <> 4";

                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return GalSrv_GenerujListe_SrwZlcSkladniki(pomDataTable);
            }
            else
            {
                return null;
            }
        }

        private List<SrwZlcSkladniki> GalSrv_GenerujListe_SrwZlcSkladniki(DataTable pomDataTable)
        {
            List<SrwZlcSkladniki> result = new List<SrwZlcSkladniki>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 GZS_Id = Convert.ToInt32(pomDataTable.Rows[i]["GZS_Id"].ToString());
                Int32 GZS_GZSId = Convert.ToInt32(pomDataTable.Rows[i]["GZS_GZSId"].ToString());
                Int32 GZS_GZSNId = Convert.ToInt32(pomDataTable.Rows[i]["GZS_GZNId"].ToString());
                Int32 GZS_Synchronizacja = Convert.ToInt32(pomDataTable.Rows[i]["GZS_Synchronizacja"].ToString());
                Int32 GZS_Pozycja = Convert.ToInt32(pomDataTable.Rows[i]["GZS_Pozycja"].ToString());
                Int32 GZS_TwrTyp = Convert.ToInt32(pomDataTable.Rows[i]["GZS_TwrTyp"].ToString());
                Int32 GZS_TwrNumer = Convert.ToInt32(pomDataTable.Rows[i]["GZS_TwrNumer"].ToString());
                String GZS_TwrNazwa = pomDataTable.Rows[i]["GZS_TwrNazwa"].ToString();
                Double GZS_Ilosc = Convert.ToDouble(pomDataTable.Rows[i]["GZS_Ilosc"].ToString());
                String GZS_Opis = pomDataTable.Rows[i]["GZS_Opis"].ToString();

                result.Add(new SrwZlcSkladniki(GZS_Id,GZS_GZSId,GZS_Synchronizacja,GZS_Pozycja,GZS_Ilosc,GZS_TwrNumer,GZS_TwrTyp,GZS_TwrNazwa,GZS_Opis));
            }
            return result;
        }

        public string GalSrv_Potwierdz_SrwZlcSkladniki(string listaPotwierdzonych)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Int32> records = ser.Deserialize<List<Int32>>(listaPotwierdzonych);
            string result = "";
            if(records != null)
            {
                for(int i = 0; i < records.Count; i++)
                {
                    try
                    {
                        String zapytanieString = "UPDATE [GALXL_Serwis].[GAL].[SrwZlcSkladniki] SET [GZS_Synchronizacja] = 4 WHERE [GZS_Id] = " + records[i].ToString();
                        SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                        DataTable pomdatatable = new DataTable();
                        da.Fill(pomdatatable);

                        result += "zapisano: " + zapytanieString + "\n";
                        result += "zapisano: " + records[i].ToString() + "\n";
                    }
                    catch(Exception exc)
                    {
                        result += "błąd: " + exc.Message + "\n";
                    }
                }
            }
            return result;
        }
    }
}