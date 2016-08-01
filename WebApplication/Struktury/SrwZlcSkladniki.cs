using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{    public class SrwZlcSkladniki
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
}