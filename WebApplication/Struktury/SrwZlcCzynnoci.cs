using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class SrwZlcCzynnoci
    {
        public Int32 szc_Id { get; set; }

        public Int32 szc_sznId { get; set; }

        public Int32 szc_Pozycja { get; set; }

        public Int32 szc_TwrNumer { get; set; }

        public Int32 szc_TwrTyp { get; set; }

        public Int32 SZC_Synchronizacja { get; set; }

        public double szc_Ilosc { get; set; }

        public String Twr_Jm { get; set; }

        public String szc_TwrNazwa { get; set; }

        public String Twr_Kod { get; set; }


        public SrwZlcCzynnoci(Int32 _szc_Id, Int32 _szc_sznId, Int32 _szc_Pozycja, Int32 _szc_TwrNumer, Int32 _szc_TwrTyp, Int32 _SZC_Synchronizacja, Double _szc_Ilosc, String _Twr_Jm, String _szc_TwrNazwa, String _Twr_Kod)
        {
            szc_Id = _szc_Id;
            szc_sznId = _szc_sznId;
            szc_Pozycja = _szc_Pozycja;
            szc_TwrNumer = _szc_TwrNumer;
            szc_TwrTyp = _szc_TwrTyp;
            SZC_Synchronizacja = _SZC_Synchronizacja;
            szc_Ilosc = _szc_Ilosc;
            Twr_Jm = _Twr_Jm;
            szc_TwrNazwa = _szc_TwrNazwa;
            Twr_Kod = _Twr_Kod;
        }
        public SrwZlcCzynnoci() { }
    }
}