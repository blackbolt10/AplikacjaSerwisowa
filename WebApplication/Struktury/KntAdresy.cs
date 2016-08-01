using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
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
}