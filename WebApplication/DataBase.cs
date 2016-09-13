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
        private int liczbaMiesiecyWstecz = 3;

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

        private void RaportBleduSerwis(String funkcja, String blad, String param1="", String param2 = "")
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = "insert into GAL.Ustawienia VALUES('" + funkcja + "', '" + blad + "', '" + param1+"', '"+param2+"')";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }
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
                Int32 SUD_ToDo = -11111;

                result.Add(new SrwUrzParDef(SUD_Id, SUD_Nazwa, SUD_Format, SUD_Archiwalna, SUD_ToDo));
            }
            return result;
        }

        public List<SrwZlcUrz> wygenerujListeSrwZlcUrz()
        {
            DataTable pomDataTable = new DataTable();
            DateTime data = DateTime.Now.AddMonths(liczbaMiesiecyWstecz);
            Exception exc = new Exception();

            try
            {
                String zapytanieString = @"Select  SZU_Id, SZU_SZNId, SZU_SrUId, SZU_Pozycja from cdn.SrwZlcUrz
                    join cdn.srwzlcnag SZN on SZN.SZN_Id = SZU_SZNId
                    where (DATEADD(DAY, SZN.SZN_DataWystawienia,CONVERT(DATETIME,'1800-12-28',120) )>'" + data.Year.ToString() + "-" + data.Month.ToString() + "-01')";

                SqlDataAdapter da = zapytanie(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                return wygenerujListeSrwZlcUrz(pomDataTable);
            }
            else
            {
                return new List<SrwZlcUrz>();
            }
        }

        private List<SrwZlcUrz> wygenerujListeSrwZlcUrz(DataTable pomDataTable)
        {
            List<SrwZlcUrz> result = new List<SrwZlcUrz>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                int SZU_Id = Convert.ToInt32(pomDataTable.Rows[i]["SZU_Id"].ToString());
                int SZU_SZNId = Convert.ToInt32(pomDataTable.Rows[i]["SZU_SZNId"].ToString());
                int SZU_SrUId = Convert.ToInt32(pomDataTable.Rows[i]["SZU_SrUId"].ToString());
                int SZU_Pozycja = Convert.ToInt32(pomDataTable.Rows[i]["SZU_Pozycja"].ToString());
                int SZU_ToDo = -1111;

                result.Add(new SrwZlcUrz(SZU_Id, SZU_SZNId, SZU_SrUId, SZU_Pozycja, SZU_ToDo));
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
                int SUR_ToDo = -111;

                result.Add(new SrwUrzRodzaje(SUR_Id, SUR_Kod, SUR_Nazwa, SUR_ToDo));
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
                int SRP_ToDo = -111;

                result.Add(new SrwUrzRodzPar(SRP_Id, SRP_SURId, SRP_SUDId, SRP_Lp, SRP_ToDo));
            }
            return result;
        }
    

        public List<SrwUrzadzenia> wygenerujListeSrwUrzadzenia()
        {
            DataTable pomDataTable = new DataTable();
            try
            {
                String zapytanieString = @"select SU.SrU_Id, SU.SrU_SURId, SU.Sru_Kod, SU.Sru_Nazwa, SU.SrU_Opis, SU.SrU_Archiwalne, SUW.SUW_WlaNumer from CDN.SrwUrzadzenia SU join [CDN].[SrwUrzWlasc] SUW on SU.SrU_Id = SUW.SUW_SrUId";

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
                String Sru_Kod = pomDataTable.Rows[i]["Sru_Kod"].ToString();
                String Sru_Nazwa = pomDataTable.Rows[i]["Sru_Nazwa"].ToString();
                String SrU_Opis = pomDataTable.Rows[i]["SrU_Opis"].ToString();
                int SrU_Archiwalne = Convert.ToInt32(pomDataTable.Rows[i]["SrU_Archiwalne"].ToString());
                Int32 SrU_ToDo = -1111;

                result.Add(new SrwUrzadzenia(SrU_Id, SrU_SURId, Sru_Kod, Sru_Nazwa, SrU_Opis, SrU_Archiwalne, SrU_ToDo));
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
                int SZN_ToDo = -111;

                result.Add(new SrwZlcNag(SZN_Id, SZN_Synchronizacja, SZN_KntTyp, SZN_KntNumer, SZN_KnATyp, SZN_KnANumer, SZN_Dokument, SZN_DataWystawienia, SZN_DataRozpoczecia, SZN_Stan, SZN_Status, SZN_Opis, SZN_ToDo));
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
                Int32 SZC_ToDo = -1111;

                result.Add(new SrwZlcCzynnosci(SZC_Id, SZC_SZNId, SZC_SZUId, SZC_Synchronizacja, SZC_Pozycja, SZC_TwrTyp, SZC_TwrNumer, SZC_TwrNazwa, SZC_Ilosc, SZC_Opis, SZC_ToDo));
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
                Int32 SZS_ToDo = -1111;

                result.Add(new SrwZlcSkladniki(SZS_Id, SZS_sznId, SZS_Synchronizacja, SZS_Pozycja, SZS_Ilosc, SZS_TwrNumer, SZS_TwrTyp, SZS_TwrNazwa, SZS_Opis, SZS_ToDo));
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
                Int32 Knt_Archiwalny = -111111;
                Int32 Knt_ToDo = -1111111;

                result.Add(new KntKarty(Knt_GIDNumer, Knt_Akronim, Knt_nazwa1, Knt_nazwa2, Knt_nazwa3, Knt_KodP, Knt_miasto, Knt_ulica, Knt_Adres, Knt_nip, Knt_telefon1, Knt_telefon2, Knt_telex, Knt_fax, Knt_email, Knt_url, Knt_Archiwalny, Knt_ToDo));
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
                Int32 Kna_ToDo = -1111;

                result.Add(new KntAdresy(Kna_GIDNumer, Kna_GIDTyp, Kna_KntNumer, Kna_Akronim, Kna_nazwa1, Kna_nazwa2, Kna_nazwa3, Kna_KodP, Kna_miasto, Kna_ulica, Kna_Adres, Kna_nip, Kna_telefon1, Kna_telefon2, Kna_telex, Kna_fax, Kna_email, Kna_ToDo));
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
                Int32 Twr_ToDo = -1000;


                result.Add(new TwrKartyTable(Twr_GIDTyp, Twr_GIDNumer, Twr_Kod, Twr_Typ, Twr_Nazwa, Twr_Nazwa1, Twr_Jm, Twr_ToDo));
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
                Int32 GZN_ToDo = -1111;

                result.Add(new SrwZlcNag(GZN_Id, GZN_Synchronizacja,GZN_KntTyp,GZN_KntNumer,GZN_KnATyp,GZN_KnANumer,Dokument,GZN_DataWystawienia,GZN_DataRozpoczecia,GZN_Stan,GZN_Status,GZN_Opis, GZN_ToDo));
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
                Int32 GZC_ToDo = -1111;

                result.Add(new SrwZlcCzynnosci(GZC_GZCId, GZC_GZNId, GZC_GZUId,GZC_Synchronizacja, GZC_Pozycja,GZC_TwrTyp,GZC_TwrNumer,GZC_TwrNazwa,GZC_Ilosc,GZC_Opis, GZC_ToDo));
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
                Int32 GZS_ToDo = -1111;

                result.Add(new SrwZlcSkladniki(GZS_Id,GZS_GZSId,GZS_Synchronizacja,GZS_Pozycja,GZS_Ilosc,GZS_TwrNumer,GZS_TwrTyp,GZS_TwrNazwa,GZS_Opis, GZS_ToDo));
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





























        public void KntKarty_ZapiszDaneUrzadzenia(string inputJSON)
        {
            DataTable pomDataTable = new DataTable();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<int> listaInt = ser.Deserialize<List<int>>(inputJSON);

            if(listaInt != null)
            {
                try
                {
                    String zapytanieString = "delete GAL.kntsync";
                    SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                    da.Fill(pomDataTable);
                }
                catch(Exception) { }

                for(int i =0;i<listaInt.Count;i++)
                {
                    try
                    {
                        String zapytanieString = "insert into GAL.kntsync values("+listaInt[i]+")";
                        SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                        da.Fill(pomDataTable);
                    }
                    catch(Exception) { }
                }
            }
        }

        public List<KntKarty> kntKarty_ZwrocNowych()
        {
            List<KntKarty> output = new List<KntKarty>();
            DataTable pomDataTable = new DataTable();
            
            try
            {
                String zapytanieString = "exec [GAL].[KntSyncAdd]";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count>0)
            {
                output = wygenerujKntKartyList(pomDataTable);
            }

            return output;
        }

        private List<KntKarty> wygenerujKntKartyList(DataTable pomDataTable)
        {
            List<KntKarty> result = new List<KntKarty>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 Knt_GIDNumer = Convert.ToInt32(pomDataTable.Rows[i]["GKnt_GIDNumer"].ToString());
                String Knt_Akronim = pomDataTable.Rows[i]["GKnt_Akronim"].ToString();
                String Knt_nazwa1 = pomDataTable.Rows[i]["GKnt_nazwa1"].ToString();
                String Knt_nazwa2 = pomDataTable.Rows[i]["GKnt_nazwa2"].ToString();
                String Knt_nazwa3 = pomDataTable.Rows[i]["GKnt_nazwa3"].ToString();
                String Knt_KodP = pomDataTable.Rows[i]["GKnt_KodP"].ToString();
                String Knt_miasto = pomDataTable.Rows[i]["GKnt_miasto"].ToString();
                String Knt_ulica = pomDataTable.Rows[i]["GKnt_ulica"].ToString();
                String Knt_Adres = pomDataTable.Rows[i]["GKnt_Adres"].ToString();
                String Knt_nip = pomDataTable.Rows[i]["GKnt_nip"].ToString();
                String Knt_telefon1 = pomDataTable.Rows[i]["GKnt_telefon1"].ToString();
                String Knt_telefon2 = pomDataTable.Rows[i]["GKnt_telefon2"].ToString();
                String Knt_telex = pomDataTable.Rows[i]["GKnt_telex"].ToString();
                String Knt_fax = pomDataTable.Rows[i]["GKnt_fax"].ToString();
                String Knt_email = pomDataTable.Rows[i]["GKnt_email"].ToString();
                String Knt_url = pomDataTable.Rows[i]["GKnt_url"].ToString();
                Int32 Knt_Archiwalny = Convert.ToInt32(pomDataTable.Rows[i]["GKnt_Archiwalny"].ToString());
                Int32 Knt_ToDo = Convert.ToInt32(pomDataTable.Rows[i]["GKnt_ToDo"].ToString());

                result.Add(new KntKarty(Knt_GIDNumer, Knt_Akronim, Knt_nazwa1, Knt_nazwa2, Knt_nazwa3, Knt_KodP, Knt_miasto, Knt_ulica, Knt_Adres, Knt_nip, Knt_telefon1, Knt_telefon2, Knt_telex, Knt_fax, Knt_email, Knt_url, Knt_Archiwalny, Knt_ToDo));
            }
            return result;
        }

        public List<KntKarty> kntKarty_ZwrocZmodyfikowanych()
        {
            List<KntKarty> output = new List<KntKarty>();
            DataTable pomDataTable = new DataTable();

            try
            {
                String zapytanieString = "exec [GAL].[KntSyncMod]";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                output = wygenerujKntKartyList(pomDataTable);
            }

            return output;
        }

        public List<int> kntKarty_ZwrocUsunietych()
        {
            List<int> output = new List<int>();
            DataTable pomDataTable = new DataTable();

            try
            {
                String zapytanieString = "exec [GAL].[KntSyncDel]";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                output = wygenerujIntList(pomDataTable);
            }

            return output;
        }

        private List<int> wygenerujIntList(DataTable pomDataTable)
        {
            List<int> result = new List<int>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                result.Add(Convert.ToInt32(pomDataTable.Rows[i]["gkns_id"].ToString()));
            }

            return result;
        }







        public void KntAdresy_ZapiszDaneUrzadzenia(string inputJSON)
        {
            DataTable pomDataTable = new DataTable();

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<int> listaInt = ser.Deserialize<List<int>>(inputJSON);

            if(listaInt != null)
            {
                try
                {
                    String zapytanieString = "delete GAL.knasync";
                    SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                    da.Fill(pomDataTable);
                }
                catch(Exception) { }

                for(int i = 0; i < listaInt.Count; i++)
                {
                    try
                    {
                        String zapytanieString = "insert into GAL.knasync values(" + listaInt[i] + ")";
                        SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                        da.Fill(pomDataTable);
                    }
                    catch(Exception) { }
                }
            }
        }

        public List<KntAdresy> KntAdresy_ZwrocNowych()
        {
            List<KntAdresy> output = new List<KntAdresy>();
            DataTable pomDataTable = new DataTable();

            try
            {
                String zapytanieString = "exec [GAL].[KnaSyncAdd]";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                output = wygenerujKntAdresyList(pomDataTable);
            }

            return output;
        }

        private List<KntAdresy> wygenerujKntAdresyList(DataTable pomDataTable)
        {
            List<KntAdresy> result = new List<KntAdresy>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                Int32 Kna_GIDNumer = Convert.ToInt32(pomDataTable.Rows[i]["GKna_GIDNumer"].ToString());
                Int32 Kna_GIDTyp = Convert.ToInt32(pomDataTable.Rows[i]["GKna_GIDTyp"].ToString());
                Int32 Kna_KntNumer = Convert.ToInt32(pomDataTable.Rows[i]["Gkna_kntnumer"].ToString());
                String Kna_Akronim = pomDataTable.Rows[i]["GKna_Akronim"].ToString();
                String Kna_nazwa1 = pomDataTable.Rows[i]["GKna_nazwa1"].ToString();
                String Kna_nazwa2 = pomDataTable.Rows[i]["GKna_nazwa2"].ToString();
                String Kna_nazwa3 = pomDataTable.Rows[i]["GKna_nazwa3"].ToString();
                String Kna_KodP = pomDataTable.Rows[i]["GKna_KodP"].ToString();
                String Kna_miasto = pomDataTable.Rows[i]["GKna_miasto"].ToString();
                String Kna_ulica = pomDataTable.Rows[i]["GKna_ulica"].ToString();
                String Kna_Adres = pomDataTable.Rows[i]["GKna_Adres"].ToString();
                String Kna_nip = pomDataTable.Rows[i]["GKna_nip"].ToString();
                String Kna_telefon1 = pomDataTable.Rows[i]["GKna_telefon1"].ToString();
                String Kna_telefon2 = pomDataTable.Rows[i]["GKna_telefon2"].ToString();
                String Kna_telex = pomDataTable.Rows[i]["GKna_telex"].ToString();
                String Kna_fax = pomDataTable.Rows[i]["GKna_fax"].ToString();
                String Kna_email = pomDataTable.Rows[i]["GKna_email"].ToString();
                Int32 Kna_ToDo = -11111;

                result.Add(new KntAdresy(Kna_GIDNumer, Kna_GIDTyp, Kna_KntNumer, Kna_Akronim, Kna_nazwa1, Kna_nazwa2, Kna_nazwa3, Kna_KodP, Kna_miasto, Kna_ulica, Kna_Adres, Kna_nip, Kna_telefon1, Kna_telefon2, Kna_telex, Kna_fax, Kna_email, Kna_ToDo));
            }
            return result;
        }

        internal List<KntAdresy> KntAdresy_ZwrocZmodyfikowanych()
        {
            List<KntAdresy> output = new List<KntAdresy>();
            DataTable pomDataTable = new DataTable();

            try
            {
                String zapytanieString = "exec [GAL].[KnaSyncMod]";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                output = wygenerujKntAdresyList(pomDataTable);
            }

            return output;
        }

        public List<int> KntAdresy_ZwrocUsunietych()
        {
            List<int> output = new List<int>();
            DataTable pomDataTable = new DataTable();

            try
            {
                String zapytanieString = "exec [GAL].[KnaSyncDel]";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomDataTable);
            }
            catch(Exception) { }

            if(pomDataTable.Rows.Count > 0)
            {
                output = wygenerujIntListeUsunietych(pomDataTable);
            }

            return output;
        }
        private List<int> wygenerujIntListeUsunietych(DataTable pomDataTable)
        {
            List<int> result = new List<int>();

            for(int i = 0; i < pomDataTable.Rows.Count; i++)
            {
                result.Add(Convert.ToInt32(pomDataTable.Rows[i]["gkna_id"].ToString()));
            }

            return result;
        }










        public void Zapisz_Dane_Kontrahentow(int idOperatora, string kntKartyString, string kntAdresyString)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Int32> kntKartyList = ser.Deserialize<List<Int32>>(kntKartyString);
            List<Int32> KntAdresyList = ser.Deserialize<List<Int32>>(kntAdresyString);

            wyczyscTabeleAndroidID(idOperatora);

            KntKarty_ZapiszListe(idOperatora, kntKartyList);
            KntAdresy_ZapiszListe(idOperatora, KntAdresyList);
        }

        public void Zapisz_Dane_Towary(int idOperatora, string twrKartyString)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Int32> TwrKartyList = ser.Deserialize<List<Int32>>(twrKartyString);

            TwrKarty_ZapiszListe(idOperatora, TwrKartyList);
        }

        public void Zapisz_Dane_Zlecenia(int idOperatora, string srwZlcNagString, string srwZlcCzynnosciString, string srwZlcSkladnikiString, string SrwZlcUrzString)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Int32> SrwZlcNagList = ser.Deserialize<List<Int32>>(srwZlcNagString);
            List<Int32> SrwZlcCzynnosciList = ser.Deserialize<List<Int32>>(srwZlcCzynnosciString);
            List<Int32> SrwZlcSkladnikiList = ser.Deserialize<List<Int32>>(srwZlcSkladnikiString);
            List<Int32> SrwZlcUrzList = ser.Deserialize<List<Int32>>(SrwZlcUrzString);

            SrwZlcCzynnosci_ZapiszListe(idOperatora, SrwZlcCzynnosciList);
            SrwZlcSkladniki_ZapiszListe(idOperatora, SrwZlcSkladnikiList);
            SrwZlcUrz_ZapiszListe(idOperatora, SrwZlcUrzList);
            SrwZlcNag_ZapiszListe(idOperatora, SrwZlcNagList);
        }

        public void Zapisz_Dane_Urzadzenia(int idOperatora,string SrwUrzadzeniaString, string SrwUrzWlascString, string SrwUrzParDefString, string SrwUrzRodzajeString, string SrwUrzRodzParString)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Int32> SrwUrzWlascList = ser.Deserialize<List<Int32>>(SrwUrzWlascString);
            List<Int32> SrwUrzadzeniaList = ser.Deserialize<List<Int32>>(SrwUrzadzeniaString);
            List<Int32> SrwUrzParDefList = ser.Deserialize<List<Int32>>(SrwUrzParDefString);
            List<Int32> SrwUrzRodzajeList = ser.Deserialize<List<Int32>>(SrwUrzRodzajeString);
            List<Int32> SrwUrzRodzParList = ser.Deserialize<List<Int32>>(SrwUrzRodzParString);

            SrwUrzWlasc_ZapiszListe(idOperatora, SrwUrzWlascList);
            SrwUrzadzenia_ZapiszListe(idOperatora, SrwUrzadzeniaList);
            SrwUrzParDef_ZapiszListe(idOperatora, SrwUrzParDefList);
            SrwUrzRodzaje_ZapiszListe(idOperatora, SrwUrzRodzajeList);
            SrwUrzRodzPar_ZapiszListe(idOperatora, SrwUrzRodzParList);
        }

        private void wyczyscTabeleAndroidID(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            try
            {
                String zapytanieString = "delete gal.androidid where AID_OpeId="+idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                String blad = exc.Message;
            }
        }

        private void KntKarty_ZapiszListe(Int32 idOperatora, List<int> kntKartyList)
        {
            for( int i=0;i<kntKartyList.Count;i++)
            {
                // 10 - typ KntKarty
                AndroidID_InsertRecord(idOperatora, 10, kntKartyList[i]);
            }
        }

        private void KntAdresy_ZapiszListe(Int32 idOperatora, List<int> KntAdresyList)
        {
            for(int i = 0; i < KntAdresyList.Count; i++)
            {
                // 11 - typ KntAdresy
                AndroidID_InsertRecord(idOperatora, 11, KntAdresyList[i]);
            }
        }

        private void SrwUrzWlasc_ZapiszListe(Int32 idOperatora, List<int> SrwUrzWlascList)
        {
            for(int i = 0; i < SrwUrzWlascList.Count; i++)
            {
                // 14 - typ TwrKarty
                AndroidID_InsertRecord(idOperatora, 14, SrwUrzWlascList[i]);
            }
        }

        private void TwrKarty_ZapiszListe(Int32 idOperatora, List<int> TwrKartyList)
        {
            for(int i = 0; i < TwrKartyList.Count; i++)
            {
                // 15 - typ TwrKarty
                AndroidID_InsertRecord(idOperatora, 15, TwrKartyList[i]);
            }
        }

        private void SrwUrzadzenia_ZapiszListe(Int32 idOperatora, List<int> SrwUrzadzeniaList)
        {
            for(int i = 0; i < SrwUrzadzeniaList.Count; i++)
            {
                // 16 - typ SrwUrzadzenia
                AndroidID_InsertRecord(idOperatora, 16, SrwUrzadzeniaList[i]);
            }
        }

        private void SrwUrzParDef_ZapiszListe(Int32 idOperatora, List<int> SrwUrzParDefList)
        {
            for(int i = 0; i < SrwUrzParDefList.Count; i++)
            {
                // 17 - typ SrwUrzParDef
                AndroidID_InsertRecord(idOperatora, 17, SrwUrzParDefList[i]);
            }
        }

        private void SrwUrzRodzaje_ZapiszListe(Int32 idOperatora, List<int> SrwUrzRodzajeList)
        {
            for(int i = 0; i < SrwUrzRodzajeList.Count; i++)
            {
                // 18 - typ SrwUrzRodzaje
                AndroidID_InsertRecord(idOperatora, 18, SrwUrzRodzajeList[i]);
            }
        }
        
        private void SrwUrzRodzPar_ZapiszListe(Int32 idOperatora, List<int> SrwUrzRodzParRodzajeList)
        {
            for(int i = 0; i < SrwUrzRodzParRodzajeList.Count; i++)
            {
                // 19 - typ SrwUrzRodzPar
                AndroidID_InsertRecord(idOperatora, 19, SrwUrzRodzParRodzajeList[i]);
            }
        }

        private void SrwZlcCzynnosci_ZapiszListe(Int32 idOperatora, List<int> SrwZlcCzynnosciList)
        {
            for(int i = 0; i < SrwZlcCzynnosciList.Count; i++)
            {
                // 21 - typ SrwZlcCzynnosci
                AndroidID_InsertRecord(idOperatora, 21, SrwZlcCzynnosciList[i]);
            }
        }

        private void SrwZlcSkladniki_ZapiszListe(Int32 idOperatora, List<int> SrwZlcSkladnikiList)
        {
            for(int i = 0; i < SrwZlcSkladnikiList.Count; i++)
            {
                // 22 - typ SrwZlcSkladniki
                AndroidID_InsertRecord(idOperatora, 22, SrwZlcSkladnikiList[i]);
            }
        }

        private void SrwZlcUrz_ZapiszListe(int idOperatora, List<int> srwZlcUrzList)
        {
            for(int i = 0; i < srwZlcUrzList.Count; i++)
            {
                // 23 - typ srwZlcUrzList
                AndroidID_InsertRecord(idOperatora, 23, srwZlcUrzList[i]);
            }
        }

        private void SrwZlcNag_ZapiszListe(Int32 idOperatora, List<int> SrwZlcNagList)
        {
            for(int i = 0; i < SrwZlcNagList.Count; i++)
            {
                // 20 - typ SrwZlcNag
                AndroidID_InsertRecord(idOperatora, 20, SrwZlcNagList[i]);
            }
        }

        private void AndroidID_InsertRecord(int idOperatora, int typ, int wartosc)
        {
            DataTable pomdatatable = new DataTable();

            try
            {
                String zapytanieString = "INSERT INTO gal.AndroidID VALUES (" + idOperatora + ", " + typ + ", " + wartosc + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("AndroidID_InsertRecord", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }
        }


        public List<KntKarty> WS_KntKartyWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<KntKarty> kntKartyList = new List<KntKarty>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncKontrahenci] "+idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_KntKartyWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            kntKartyList = WS_KntKartyWygenerujListe(pomdatatable);

            return kntKartyList;
        }

        private List<KntKarty> WS_KntKartyWygenerujListe(DataTable pomdatatable)
        {
            List<KntKarty> kntKartyList = new List<KntKarty>();

            for(int i=0;i<pomdatatable.Rows.Count;i++)
            {
                KntKarty kntKarta = new KntKarty();

                kntKarta.Knt_GIDNumer = Convert.ToInt32(pomdatatable.Rows[i]["GKnt_GidNumer"].ToString());
                kntKarta.Knt_Akronim = pomdatatable.Rows[i]["GKnt_Akronim"].ToString();
                kntKarta.Knt_nazwa1 = pomdatatable.Rows[i]["GKnt_Nazwa1"].ToString();
                kntKarta.Knt_nazwa2 = pomdatatable.Rows[i]["GKnt_Nazwa2"].ToString();
                kntKarta.Knt_nazwa3 = pomdatatable.Rows[i]["GKnt_Nazwa3"].ToString();
                kntKarta.Knt_KodP = pomdatatable.Rows[i]["GKnt_KodP"].ToString();
                kntKarta.Knt_miasto = pomdatatable.Rows[i]["GKnt_Miasto"].ToString();
                kntKarta.Knt_ulica = pomdatatable.Rows[i]["GKnt_Ulica"].ToString();
                kntKarta.Knt_Adres = pomdatatable.Rows[i]["GKnt_Adres"].ToString();
                kntKarta.Knt_nip = pomdatatable.Rows[i]["GKnt_Nip"].ToString();
                kntKarta.Knt_telefon1 = pomdatatable.Rows[i]["GKnt_Telefon1"].ToString();
                kntKarta.Knt_telefon2 = pomdatatable.Rows[i]["GKnt_Telefon2"].ToString();
                kntKarta.Knt_telex = pomdatatable.Rows[i]["GKnt_Telex"].ToString();
                kntKarta.Knt_fax = pomdatatable.Rows[i]["GKnt_Fax"].ToString();
                kntKarta.Knt_email = pomdatatable.Rows[i]["GKnt_Email"].ToString();
                kntKarta.Knt_url = pomdatatable.Rows[i]["GKnt_Url"].ToString();
                kntKarta.Knt_Archiwalny = Convert.ToInt32(pomdatatable.Rows[i]["GKnt_Archiwalny"].ToString());
                kntKarta.Knt_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GKnt_ToDo"].ToString());

                kntKartyList.Add(kntKarta);
            }

            return kntKartyList;
        }

        public void WS_KntKartyPotwierdz(Int32 idOperatora, String kntGidNumerList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.kartykontrahenci set gknt_todo=0 where gknt_opeid=" + idOperatora + " and gknt_gidnumer in (" + kntGidNumerList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_KntKartyPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), kntGidNumerList);
            }
        }

        public List<KntAdresy> WS_KntAdresyWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<KntAdresy> kntAdresyList = new List<KntAdresy>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncAdresy] " + idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_KntAdresyWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            kntAdresyList = WS_KntAdresyWygenerujListe(pomdatatable);

            return kntAdresyList;
        }

        private List<KntAdresy> WS_KntAdresyWygenerujListe(DataTable pomdatatable)
        {
            List<KntAdresy> kntAdresyList = new List<KntAdresy>();

            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                KntAdresy kntAdres = new KntAdresy();

                kntAdres.Kna_GIDNumer = Convert.ToInt32(pomdatatable.Rows[i]["GknA_GIDNumer"].ToString());
                kntAdres.Kna_GIDTyp = Convert.ToInt32(pomdatatable.Rows[i]["GknA_GIDTyp"].ToString());
                kntAdres.Kna_KntNumer = Convert.ToInt32(pomdatatable.Rows[i]["GknA_kntnumer"].ToString());
                kntAdres.Kna_Adres = pomdatatable.Rows[i]["GKnA_Akronim"].ToString();
                kntAdres.Kna_Akronim = pomdatatable.Rows[i]["GknA_Akronim"].ToString();
                kntAdres.Kna_nazwa1 = pomdatatable.Rows[i]["GKnA_Nazwa1"].ToString();
                kntAdres.Kna_nazwa2 = pomdatatable.Rows[i]["GKnA_Nazwa2"].ToString();
                kntAdres.Kna_nazwa3 = pomdatatable.Rows[i]["GKnA_Nazwa3"].ToString();
                kntAdres.Kna_KodP = pomdatatable.Rows[i]["GknA_KodP"].ToString();
                kntAdres.Kna_miasto = pomdatatable.Rows[i]["GKnA_Miasto"].ToString();
                kntAdres.Kna_ulica = pomdatatable.Rows[i]["GKnA_Ulica"].ToString();
                kntAdres.Kna_Adres = pomdatatable.Rows[i]["GKnA_Adres"].ToString();
                kntAdres.Kna_nip = pomdatatable.Rows[i]["GKnA_Nip"].ToString();
                kntAdres.Kna_telefon1 = pomdatatable.Rows[i]["GKnA_Telefon1"].ToString();
                kntAdres.Kna_telefon2 = pomdatatable.Rows[i]["GKnA_Telefon2"].ToString();
                kntAdres.Kna_telex = pomdatatable.Rows[i]["GKnA_Telex"].ToString();
                kntAdres.Kna_fax = pomdatatable.Rows[i]["GKnA_Fax"].ToString();
                kntAdres.Kna_email = pomdatatable.Rows[i]["GKnA_Email"].ToString();
                kntAdres.Kna_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GKnA_ToDo"].ToString());

                kntAdresyList.Add(kntAdres);
            }

            return kntAdresyList;
        }

        public void WS_KntAdresyPotwierdz(int idOperatora, string kntGidNumerList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.KartyAdresy set GKnA_ToDo = 0 where GKnA_OpeId = " + idOperatora + " and GKnA_GIDNumer in (" + kntGidNumerList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_KntAdresyPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), kntGidNumerList);
            }
        }

        public List<TwrKartyTable> WS_TwrKartyWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<TwrKartyTable> kntAdresyList = new List<TwrKartyTable>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncTowary] " + idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_TwrKartyWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            kntAdresyList = WS_TwrKartyWygenerujListe(pomdatatable);

            return kntAdresyList;
        }

        private List<TwrKartyTable> WS_TwrKartyWygenerujListe(DataTable pomdatatable)
        {
            List<TwrKartyTable> TwrKartyList = new List<TwrKartyTable>();

            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                TwrKartyTable TwrKarta = new TwrKartyTable();

                TwrKarta.Twr_GIDNumer = Convert.ToInt32(pomdatatable.Rows[i]["GTwr_GIDnumer"].ToString());
                TwrKarta.Twr_GIDTyp = Convert.ToInt32(pomdatatable.Rows[i]["GTwr_GIDTyp"].ToString());
                TwrKarta.Twr_Kod =pomdatatable.Rows[i]["GTwr_Kod"].ToString();
                TwrKarta.Twr_Typ = Convert.ToInt32(pomdatatable.Rows[i]["GTwr_Typ"].ToString());
                TwrKarta.Twr_Nazwa = pomdatatable.Rows[i]["GTwr_Nazwa"].ToString();
                TwrKarta.Twr_Nazwa1 = pomdatatable.Rows[i]["GTwr_Nazwa1"].ToString();
                TwrKarta.Twr_Jm = pomdatatable.Rows[i]["GTwr_Jm"].ToString();
                TwrKarta.Twr_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GTwr_ToDo"].ToString());
                
                TwrKartyList.Add(TwrKarta);
            }

            return TwrKartyList;
        }

        public void WS_TwrKartyPotwierdz(int idOperatora, string Twr_GIDNumerList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.KartyTowary set GTwr_ToDo = 0 where GTwr_OpeId = " + idOperatora + " and GTwr_GIDnumer in (" + Twr_GIDNumerList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_TwrKartyPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), Twr_GIDNumerList);
            }
        }


        public List<SrwZlcNag> WS_SrwZlcNagWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<SrwZlcNag> SrwZlcNagList = new List<SrwZlcNag>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncZleceniaNag] " + idOperatora+", "+liczbaMiesiecyWstecz;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwZlcNagWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            SrwZlcNagList = WS_SrwZlcNagWygenerujListe(pomdatatable);

            return SrwZlcNagList;
        }

        private List<SrwZlcNag> WS_SrwZlcNagWygenerujListe(DataTable pomdatatable)
        {
            List<SrwZlcNag> TwrKartyList = new List<SrwZlcNag>();

            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                SrwZlcNag SrwZlcNaglowek = new SrwZlcNag();

                SrwZlcNaglowek.SZN_Dokument = pomdatatable.Rows[i]["GSZN_Dokument"].ToString();
                SrwZlcNaglowek.SZN_Id = Convert.ToInt32(pomdatatable.Rows[i]["GSZN_Id"].ToString());
                SrwZlcNaglowek.SZN_KntTyp = Convert.ToInt32(pomdatatable.Rows[i]["GSZN_KntTyp"].ToString());
                SrwZlcNaglowek.SZN_KntNumer = Convert.ToInt32(pomdatatable.Rows[i]["GSZN_KntNumer"].ToString());
                SrwZlcNaglowek.SZN_KnATyp = Convert.ToInt32(pomdatatable.Rows[i]["GSZN_KnATyp"].ToString());
                SrwZlcNaglowek.SZN_KnANumer = Convert.ToInt32(pomdatatable.Rows[i]["GSZN_KnANumer"].ToString());
                SrwZlcNaglowek.SZN_DataWystawienia = pomdatatable.Rows[i]["GSZN_DataWystawienia"].ToString();
                SrwZlcNaglowek.SZN_DataRozpoczecia = pomdatatable.Rows[i]["GSZN_DataRozpoczecia"].ToString();
                SrwZlcNaglowek.SZN_Stan = pomdatatable.Rows[i]["GSZN_Stan"].ToString();
                SrwZlcNaglowek.SZN_Status = pomdatatable.Rows[i]["GSZN_Status"].ToString();
                SrwZlcNaglowek.SZN_Opis = pomdatatable.Rows[i]["GSZN_Opis"].ToString();
                SrwZlcNaglowek.SZN_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GSZN_ToDo"].ToString());

                TwrKartyList.Add(SrwZlcNaglowek);
            }

            return TwrKartyList;
        }

        public void WS_SrwZlcNagPotwierdz(int idOperatora, string SZN_IDList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.ZleceniaNag set GSZN_ToDo = 0 where GSZN_OpeId = " + idOperatora + " and GSZN_Id in (" + SZN_IDList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwZlcNagPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), SZN_IDList);
            }
        }

        public List<SrwZlcCzynnosci> WS_SrwZlcCzynnosciWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<SrwZlcCzynnosci> SrwZlcCzynnosciList = new List<SrwZlcCzynnosci>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncZleceniaCzynnosci] " + idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwZlcCzynnosciWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            SrwZlcCzynnosciList = WS_SrwZlcCzynnosciWygenerujListe(pomdatatable);

            return SrwZlcCzynnosciList;
        }

        private List<SrwZlcCzynnosci> WS_SrwZlcCzynnosciWygenerujListe(DataTable pomdatatable)
        {
            List<SrwZlcCzynnosci> SrwZlcCzynnosciList = new List<SrwZlcCzynnosci>();
            
            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                SrwZlcCzynnosci SrwZlcCzynnosc = new SrwZlcCzynnosci();

                SrwZlcCzynnosc.SZC_Id = Convert.ToInt32(pomdatatable.Rows[i]["GSZC_Id"].ToString());
                SrwZlcCzynnosc.SZC_SZNId = Convert.ToInt32(pomdatatable.Rows[i]["GSZC_SZNId"].ToString());
                SrwZlcCzynnosc.SZC_SZUId = Convert.ToInt32(pomdatatable.Rows[i]["GSZC_SZUid"].ToString());
                SrwZlcCzynnosc.SZC_Synchronizacja = 0;
                SrwZlcCzynnosc.SZC_Pozycja = Convert.ToInt32(pomdatatable.Rows[i]["GSZC_Pozycja"].ToString());
                SrwZlcCzynnosc.SZC_TwrTyp = Convert.ToInt32(pomdatatable.Rows[i]["GSZC_TwrTyp"].ToString());
                SrwZlcCzynnosc.SZC_TwrNumer = Convert.ToInt32(pomdatatable.Rows[i]["GSZC_TwrNumer"].ToString());
                SrwZlcCzynnosc.SZC_TwrNazwa = pomdatatable.Rows[i]["GSZC_TwrNazwa"].ToString();
                SrwZlcCzynnosc.SZC_Ilosc = Convert.ToDouble(pomdatatable.Rows[i]["GSZC_Ilosc"].ToString());
                SrwZlcCzynnosc.SZC_Opis = pomdatatable.Rows[i]["GSZC_Opis"].ToString();
                SrwZlcCzynnosc.SZC_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GSZC_ToDo"].ToString());

                SrwZlcCzynnosciList.Add(SrwZlcCzynnosc);
            }

            return SrwZlcCzynnosciList;
        }

        public void WS_SrwZlcCzynnosciPotwierdz(int idOperatora, string SZC_IdList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.ZleceniaCzynnosci set GSZC_ToDo = 0 where GSZC_OpeId = " + idOperatora + " and GSZC_Id in (" + SZC_IdList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwZlcCzynnosciPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), SZC_IdList);
            }
        }

        public List<SrwZlcSkladniki> WS_SrwZlcSkladnikiWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<SrwZlcSkladniki> SrwZlcSkladnikiList = new List<SrwZlcSkladniki>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncZleceniaSkladniki] " + idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwZlcSkladnikiWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            SrwZlcSkladnikiList = WS_SrwZlcSkladnikiWygenerujListe(pomdatatable);

            return SrwZlcSkladnikiList;
        }

        private List<SrwZlcSkladniki> WS_SrwZlcSkladnikiWygenerujListe(DataTable pomdatatable)
        {
            List<SrwZlcSkladniki> SrwZlcSkladnikiList = new List<SrwZlcSkladniki>();

            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                SrwZlcSkladniki SrwZlcSkladnik = new SrwZlcSkladniki();

                SrwZlcSkladnik.SZS_Id = Convert.ToInt32(pomdatatable.Rows[i]["GSZS_Id"].ToString());
                SrwZlcSkladnik.SZS_SZNId = Convert.ToInt32(pomdatatable.Rows[i]["GSZS_SZNId"].ToString());
                SrwZlcSkladnik.SZS_Synchronizacja = 0;
                SrwZlcSkladnik.SZS_Pozycja = Convert.ToInt32(pomdatatable.Rows[i]["GSZS_Pozycja"].ToString());
                SrwZlcSkladnik.SZS_TwrTyp = Convert.ToInt32(pomdatatable.Rows[i]["GSZS_TwrTyp"].ToString());
                SrwZlcSkladnik.SZS_TwrNumer = Convert.ToInt32(pomdatatable.Rows[i]["GSZS_TwrNumer"].ToString());
                SrwZlcSkladnik.SZS_TwrNazwa = pomdatatable.Rows[i]["GSZS_TwrNazwa"].ToString();
                SrwZlcSkladnik.SZS_Ilosc = Convert.ToDouble(pomdatatable.Rows[i]["GSZS_Ilosc"].ToString());
                SrwZlcSkladnik.SZS_Opis = pomdatatable.Rows[i]["GSZS_Opis"].ToString();
                SrwZlcSkladnik.SZS_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GSZS_ToDo"].ToString());

                SrwZlcSkladnikiList.Add(SrwZlcSkladnik);
            }

            return SrwZlcSkladnikiList;
        }

        public void WS_SrwZlcSkladnikiPotwierdz(int idOperatora, string SZS_IdList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.ZleceniaSkladniki set GSZS_ToDo = 0 where GSZS_OpeId = " + idOperatora + " and GSZS_Id in (" + SZS_IdList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwZlcSkladnikiPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), SZS_IdList);
            }
        }

        public List<SrwUrzadzenia> WS_SrwUrzadzeniaWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<SrwUrzadzenia> SrwUrzadzeniaList = new List<SrwUrzadzenia>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncUrzadzenia] " + idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwUrzadzeniaWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            SrwUrzadzeniaList = WS_SrwUrzadzeniaWygenerujListe(pomdatatable);

            return SrwUrzadzeniaList;
        }

        private List<SrwUrzadzenia> WS_SrwUrzadzeniaWygenerujListe(DataTable pomdatatable)
        {
            List<SrwUrzadzenia> SrwUrzadzeniaList = new List<SrwUrzadzenia>();

            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                SrwUrzadzenia SrwUrzadzenie = new SrwUrzadzenia();

                SrwUrzadzenie.SrU_Id = Convert.ToInt32(pomdatatable.Rows[i]["GSrU_Id"].ToString());
                SrwUrzadzenie.SrU_SURId = Convert.ToInt32(pomdatatable.Rows[i]["GSrU_SURId"].ToString());
                SrwUrzadzenie.SrU_Kod = pomdatatable.Rows[i]["GSrU_Kod"].ToString();
                SrwUrzadzenie.Sru_Nazwa = pomdatatable.Rows[i]["GSrU_Nazwa"].ToString();
                SrwUrzadzenie.SrU_Opis = pomdatatable.Rows[i]["GSrU_Opis"].ToString();
                SrwUrzadzenie.SrU_Archiwalne = Convert.ToInt32(pomdatatable.Rows[i]["GSrU_Archiwalne"].ToString());
                SrwUrzadzenie.SrU_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GSrU_ToDo"].ToString());

                SrwUrzadzeniaList.Add(SrwUrzadzenie);
            }

            return SrwUrzadzeniaList;
        }

        public void WS_SrwUrzadzeniaPotwierdz(int idOperatora, string GSrU_IdList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.KartyUrzadzenia set GSrU_ToDo = 0 where GSrU_OpeId = " + idOperatora + " and GSrU_Id in (" + GSrU_IdList + ")";
                RaportBleduSerwis("WS_SrwUrzadzeniaPotwierdz", zapytanieString);
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwUrzadzeniaPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), GSrU_IdList);
            }
        }

        public List<SrwUrzWlasc> WS_SrwUrzWlascWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<SrwUrzWlasc> SrwUrzadzeniaList = new List<SrwUrzWlasc>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncUrzadzeniaWla] " + idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwUrzWlascWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            SrwUrzadzeniaList = WS_SrwUrzWlascWygenerujListe(pomdatatable);

            return SrwUrzadzeniaList;
        }

        private List<SrwUrzWlasc> WS_SrwUrzWlascWygenerujListe(DataTable pomdatatable)
        {
            List<SrwUrzWlasc> SrwUrzWlascList = new List<SrwUrzWlasc>();

            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                SrwUrzWlasc SrwUrzWlasciciel = new SrwUrzWlasc();

                SrwUrzWlasciciel.SUW_SrUId = Convert.ToInt32(pomdatatable.Rows[i]["GSUW_SruId"].ToString());
                SrwUrzWlasciciel.SUW_WlaNumer = Convert.ToInt32(pomdatatable.Rows[i]["GSUW_WlaNumer"].ToString());
                SrwUrzWlasciciel.SUW_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GSUW_ToDo"].ToString());

                SrwUrzWlascList.Add(SrwUrzWlasciciel);
            }

            return SrwUrzWlascList;
        }

        public void WS_SrwUrzWlascPotwierdz(int idOperatora, string GSUW_SruIdList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.KartyUrzadzeniaWla set GSUW_ToDo = 0 where GSUW_OpeId = " + idOperatora + " and GSUW_SruId in (" + GSUW_SruIdList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwUrzWlascPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), GSUW_SruIdList);
            }
        }
        
        public List<SrwUrzParDef> WS_SrwUrzParDefWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<SrwUrzParDef> SrwUrzParDefList = new List<SrwUrzParDef>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncUrzadzeniaParDef] " + idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwUrzParDefWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            SrwUrzParDefList = WS_SrwUrzParDefWygenerujListe(pomdatatable);

            return SrwUrzParDefList;
        }

        private List<SrwUrzParDef> WS_SrwUrzParDefWygenerujListe(DataTable pomdatatable)
        {
            List<SrwUrzParDef> SrwUrzParDefList = new List<SrwUrzParDef>();

            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                SrwUrzParDef SrwUrzParDefinicja = new SrwUrzParDef();

                SrwUrzParDefinicja.SUD_Id = Convert.ToInt32(pomdatatable.Rows[i]["GSUD_Id"].ToString());
                SrwUrzParDefinicja.SUD_Archiwalna = Convert.ToInt32(pomdatatable.Rows[i]["GSUD_Archiwalna"].ToString());
                SrwUrzParDefinicja.SUD_Nazwa = pomdatatable.Rows[i]["GSUD_Nazwa"].ToString();
                SrwUrzParDefinicja.SUD_Format = pomdatatable.Rows[i]["GSUD_Format"].ToString();
                SrwUrzParDefinicja.SUD_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GSUD_ToDo"].ToString());

                SrwUrzParDefList.Add(SrwUrzParDefinicja);
            }

            return SrwUrzParDefList;
        }

        public void WS_SrwUrzParDefPotwierdz(int idOperatora, string SUD_IdList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.KartyUrzadzeniaParDef set GSUD_ToDo = 0 where GSUD_OpeId = " + idOperatora + " and GSUD_Id in (" + SUD_IdList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwUrzParDefPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), SUD_IdList);
            }
        }

        public List<SrwUrzRodzaje> WS_SrwUrzRodzajeWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<SrwUrzRodzaje> SrwUrzRodzajeList = new List<SrwUrzRodzaje>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncUrzadzeniaRodzaje] " + idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwUrzRodzajeWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            SrwUrzRodzajeList = WS_SrwUrzRodzajeWygenerujListe(pomdatatable);

            return SrwUrzRodzajeList;
        }

        private List<SrwUrzRodzaje> WS_SrwUrzRodzajeWygenerujListe(DataTable pomdatatable)
        {
            List<SrwUrzRodzaje> SrwUrzRodzajeList = new List<SrwUrzRodzaje>();

            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                SrwUrzRodzaje SrwUrzRodzaj = new SrwUrzRodzaje();

                SrwUrzRodzaj.SUR_Id = Convert.ToInt32(pomdatatable.Rows[i]["GSUR_Id"].ToString());
                SrwUrzRodzaj.SUR_Kod = pomdatatable.Rows[i]["GSUR_Kod"].ToString();
                SrwUrzRodzaj.SUR_Nazwa = pomdatatable.Rows[i]["GSUR_Nazwa"].ToString();
                SrwUrzRodzaj.SUR_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GSUR_ToDo"].ToString());

                SrwUrzRodzajeList.Add(SrwUrzRodzaj);
            }

            return SrwUrzRodzajeList;
        }

        public void WS_SrwUrzRodzajePotwierdz(int idOperatora, string SUR_IdList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.KartyUrzadzeniaRodzaje set GSUR_ToDo = 0 where GSUR_OpeId = " + idOperatora + " and GSUR_Id in (" + SUR_IdList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwUrzRodzajePotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), SUR_IdList);
            }
        }

        public List<SrwUrzRodzPar> WS_SrwUrzRodzParWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<SrwUrzRodzPar> SrwUrzRodzParList = new List<SrwUrzRodzPar>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncUrzadzeniaRodzPar] " + idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwUrzRodzParWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            SrwUrzRodzParList = WS_SrwUrzRodzParWygenerujListe(pomdatatable);

            return SrwUrzRodzParList;
        }

        private List<SrwUrzRodzPar> WS_SrwUrzRodzParWygenerujListe(DataTable pomdatatable)
        {
            List<SrwUrzRodzPar> SrwUrzRodzParList = new List<SrwUrzRodzPar>();

            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                SrwUrzRodzPar SrwUrzRodzParametr = new SrwUrzRodzPar();

                SrwUrzRodzParametr.SRP_Id = Convert.ToInt32(pomdatatable.Rows[i]["GSRP_Id"].ToString());
                SrwUrzRodzParametr.SRP_SURId = Convert.ToInt32(pomdatatable.Rows[i]["GSRP_SURId"].ToString());
                SrwUrzRodzParametr.SRP_SUDId = Convert.ToInt32(pomdatatable.Rows[i]["GSRP_SUDId"].ToString());
                SrwUrzRodzParametr.SRP_Lp = Convert.ToInt32(pomdatatable.Rows[i]["GSRP_Lp"].ToString());
                SrwUrzRodzParametr.SRP_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GSRP_ToDo"].ToString());

                SrwUrzRodzParList.Add(SrwUrzRodzParametr);
            }

            return SrwUrzRodzParList;
        }

        public void WS_SrwUrzRodzParPotwierdz(int idOperatora, string SGSRP_IdList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.KartyUrzadzeniaRodzPar set GSRP_ToDo = 0 where GSRP_OpeId = " + idOperatora + " and GSRP_Id in (" + SGSRP_IdList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwUrzRodzParPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), SGSRP_IdList);
            }
        }

        public List<SrwZlcUrz> WS_SrwZlcUrzWygenerujListe(int idOperatora)
        {
            DataTable pomdatatable = new DataTable();
            List<SrwZlcUrz> SrwZlcUrzList = new List<SrwZlcUrz>();

            try
            {
                String zapytanieString = "exec [GAL].[SyncZleceniaUrzadzenia] " + idOperatora;
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwZlcUrzWygenerujListe", exc.Message, "IdOperatora=" + idOperatora.ToString());
            }

            SrwZlcUrzList = WS_SrwZlcUrzWygenerujListe(pomdatatable);

            return SrwZlcUrzList;
        }

        private List<SrwZlcUrz> WS_SrwZlcUrzWygenerujListe(DataTable pomdatatable)
        {
            List<SrwZlcUrz> SrwZlcUrzList = new List<SrwZlcUrz>();

            for(int i = 0; i < pomdatatable.Rows.Count; i++)
            {
                SrwZlcUrz SrwZlcUrzadzenie = new SrwZlcUrz();

                SrwZlcUrzadzenie.SZU_Id = Convert.ToInt32(pomdatatable.Rows[i]["GSZU_Id"].ToString());
                SrwZlcUrzadzenie.SZU_SZNId = Convert.ToInt32(pomdatatable.Rows[i]["GSZU_SZNId"].ToString());
                SrwZlcUrzadzenie.SZU_SrUId = Convert.ToInt32(pomdatatable.Rows[i]["GSZU_SrUId"].ToString());
                SrwZlcUrzadzenie.SZU_Pozycja = Convert.ToInt32(pomdatatable.Rows[i]["GSZU_Pozycja"].ToString());
                SrwZlcUrzadzenie.SZU_ToDo = Convert.ToInt32(pomdatatable.Rows[i]["GSZU_ToDo"].ToString());

                SrwZlcUrzList.Add(SrwZlcUrzadzenie);
            }

            return SrwZlcUrzList;
        }

        public void WS_SrwZlcUrzPotwierdz(int idOperatora, string SZU_IdList)
        {
            try
            {
                DataTable pomdatatable = new DataTable();
                String zapytanieString = "update gal.ZleceniaUrzadzenia set GSZU_ToDo = 0 where GSZU_OpeId = " + idOperatora + " and GSZU_Id in (" + SZU_IdList + ")";
                SqlDataAdapter da = zapytanieSerwis(zapytanieString);
                da.Fill(pomdatatable);
            }
            catch(Exception exc)
            {
                RaportBleduSerwis("WS_SrwZlcUrzPotwierdz", exc.Message, "IdOperatora=" + idOperatora.ToString(), SZU_IdList);
            }
        }
    }
}