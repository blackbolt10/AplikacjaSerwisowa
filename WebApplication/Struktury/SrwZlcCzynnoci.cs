using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    class SrwZlcCzynnosci
    {
        public Int32 ID { get; set; }
        public Int32 SZC_Id { get; set; }
        public Int32 SZC_SZNId { get; set; }
        public Int32 SZC_SZUId { get; set; }
        public Int32 SZC_Synchronizacja { get; set; }
        public Int32 SZC_Pozycja { get; set; }
        public Int32 SZC_TwrTyp { get; set; }
        public Int32 SZC_TwrNumer { get; set; }
        public String SZC_TwrNazwa { get; set; }
        public Double SZC_Ilosc { get; set; }
        public String SZC_Opis { get; set; }

        public SrwZlcCzynnosci(Int32 _SZC_Id, Int32 _SZC_SZNId, Int32 _SZC_SZUId, Int32 _SZC_Synchronizacja, Int32 _SZC_Pozycja, Int32 _SZC_TwrTyp, Int32 _SZC_TwrNumer, String _SZC_TwrNazwa, Double _SZC_Ilosc, String _SZC_Opis)
        {
            this.SZC_Id = _SZC_Id;
            this.SZC_SZNId = _SZC_SZNId;
            this.SZC_SZUId = _SZC_SZUId;
            this.SZC_Synchronizacja = _SZC_Synchronizacja;
            this.SZC_Pozycja = _SZC_Pozycja;
            this.SZC_TwrTyp = _SZC_TwrTyp;
            this.SZC_TwrNumer = _SZC_TwrNumer;
            this.SZC_TwrNazwa = _SZC_TwrNazwa;
            this.SZC_Ilosc = _SZC_Ilosc;
            this.SZC_Opis = _SZC_Opis;
        }

        public SrwZlcCzynnosci()
        { }
    }
}