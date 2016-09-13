using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class TwrKartyTable
    {
        public Int32 Twr_GIDTyp { get; set; }

        public Int32 Twr_GIDNumer { get; set; }

        public String Twr_Kod { get; set; }

        public Int32 Twr_Typ { get; set; }

        public String Twr_Nazwa { get; set; }

        public String Twr_Nazwa1 { get; set; }

        public String Twr_Jm { get; set; }

        public Int32 Twr_ToDo { get; set; }

        public TwrKartyTable(Int32 _Twr_GIDTyp, Int32 _Twr_GIDNumer, String _Twr_Kod, Int32 _Twr_Typ, String _Twr_Nazwa, String _Twr_Nazwa1, String _Twr_Jm, Int32 _Twr_ToDo)
        {
            Twr_GIDTyp = _Twr_GIDTyp;
            Twr_GIDNumer = _Twr_GIDNumer;
            Twr_Kod = _Twr_Kod;
            Twr_Typ = _Twr_Typ;
            Twr_Nazwa = _Twr_Nazwa;
            Twr_Nazwa1 = _Twr_Nazwa1;
            Twr_Jm = _Twr_Jm;
            Twr_ToDo = _Twr_ToDo;
        }

        public TwrKartyTable() { }
    }
}