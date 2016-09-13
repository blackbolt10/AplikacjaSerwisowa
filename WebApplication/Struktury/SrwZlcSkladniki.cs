using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication
{
    class SrwZlcSkladniki
    {
        public Int32 ID { get; set; }
        public Int32 SZS_Id { get; set; }
        public Int32 SZS_SZNId { get; set; }
        public Int32 SZS_Synchronizacja { get; set; }
        public Int32 SZS_Pozycja { get; set; }
        public Double SZS_Ilosc { get; set; }
        public Int32 SZS_TwrNumer { get; set; }
        public Int32 SZS_TwrTyp { get; set; }
        public String SZS_TwrNazwa { get; set; }
        public String SZS_Opis { get; set; }
        public Int32 SZS_ToDo { get; set; }

        public SrwZlcSkladniki(Int32 _SZS_Id, Int32 _SZS_SZNId, Int32 _SZS_Synchronizacja, Int32 _SZS_Pozycja, Double _SZS_Ilosc, Int32 _SZS_TwrNumer, Int32 _SZS_TwrTyp, String _SZS_TwrNazwa, String _SZS_Opis, Int32 _SZS_ToDo)
        {
            this.SZS_Id = _SZS_Id;
            this.SZS_SZNId = _SZS_SZNId;
            this.SZS_Synchronizacja = _SZS_Synchronizacja;
            this.SZS_Pozycja = _SZS_Pozycja;
            this.SZS_Ilosc = _SZS_Ilosc;
            this.SZS_TwrNumer = _SZS_TwrNumer;
            this.SZS_TwrTyp = _SZS_TwrTyp;
            this.SZS_TwrNazwa = _SZS_TwrNazwa;
            this.SZS_Opis = _SZS_Opis;
            this.SZS_ToDo = _SZS_ToDo;
        }

        public SrwZlcSkladniki()
        { }
    }
}