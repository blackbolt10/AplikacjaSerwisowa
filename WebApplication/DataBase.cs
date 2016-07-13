using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebApplication
{
    class DataBase
    {
        private SqlConnection uchwytBD;
        private SqlCommand polecenieSQL;

        public DataBase()
        {
            podlaczDoBazyDanych();
        }

        private Boolean podlaczDoBazyDanych()
        {
            Boolean wynikLogowaniaDoBD = false;
            hasla haslo = new hasla(2);

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

        private SqlDataAdapter zapytanie(string zapytanie1)
        {
            SqlDataAdapter wynik = new SqlDataAdapter();
            polecenieSQL = new SqlCommand(zapytanie1);
            polecenieSQL.CommandTimeout = 240;
            polecenieSQL.Connection = uchwytBD;
            wynik = new SqlDataAdapter(polecenieSQL);

            return wynik;
        }

        private void zapiszDB(string zapytanie1)
        {
            polecenieSQL = new SqlCommand(zapytanie1);
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

        public List<SerwisoweZleceniaNaglownki> wygenerujListeSerwisowychZlecenNaglowki()
        {
            DateTime data = DateTime.Now.AddMonths(-6);
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
            --WHERE ( (SZN_ROK=" + data.Year.ToString() +@" AND Month(DATEADD(dd,SZN_DataWystawienia,CONVERT(DATETIME,'18001228',11)))="+ data.Month.ToString() + @") ) 
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

        private List<SerwisoweZleceniaNaglownki> wygenerujListeSerwisowychZlecen(DataTable pomDataTable)
        {
            List<SerwisoweZleceniaNaglownki> result = new List<SerwisoweZleceniaNaglownki>();

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

                result.Add(new SerwisoweZleceniaNaglownki(Dokument, SZN_Id, SZN_KntTyp, SZN_KntNumer, SZN_KnATyp, SZN_KnANumer, SZN_KnDTyp, SZN_KnDNumer, SZN_AdWTyp, SZN_AdWNumer, SZN_DataWystawienia, SZN_DataRozpoczecia, SZN_Stan, SZN_Status, SZN_CechaOpis, SZN_Opis));
            }
            return result;
        }





        


    }
}















[Table("SerwisoweZleceniaNaglownki")]
public class SerwisoweZleceniaNaglownki
{
    public int ID { get; set; }
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


    public SerwisoweZleceniaNaglownki(String _Dokument, int _SZN_Id, int _SZN_KntTyp, int _SZN_KntNumer, int _SZN_KnATyp, int _SZN_KnANumer, int _SZN_KnDTyp, int _SZN_KnDNumer, int _SZN_AdWTyp, int _SZN_AdWNumer, String _SZN_DataWystawienia, String _SZN_DataRozpoczecia, String _SZN_Stan, String _SZN_Status, String _SZN_CechaOpis, String _SZN_Opis)
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
    }
    public SerwisoweZleceniaNaglownki() { }
}