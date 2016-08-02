using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class Operatorzy
    {
        public int Id { get; set; }

        public String Akronim { get; set; }

        public String Haslo { get; set; }

        public String Imie { get; set; }

        public String Nazwisko { get; set; }

        public Operatorzy(String _Akronim, String _Haslo, String _Imie, String _Nazwisko)
        {
            Akronim = _Akronim;
            Haslo = _Haslo;
            Imie = _Imie;
            Nazwisko = _Nazwisko;
        }
        public Operatorzy() {}
    }
}