using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
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
}