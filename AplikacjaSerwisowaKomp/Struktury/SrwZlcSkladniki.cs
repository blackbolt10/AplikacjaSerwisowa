using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplikacjaSerwisowaKomp
{
    class SrwZlcSkladniki
    {
        public Int32 ID { get; set; }
        public Int32 SZS_Id { get; set; }
        public Int32 SZS_SZNId { get; set; }
        public Int32 SZS_Synchronizacja { get; set; }
        public Int32 SZS_Pozycja { get; set; }
        public Int32 SZS_TwrTyp { get; set; }
        public Int32 SZS_TwrNumer { get; set; }
        public String SZS_Ilosc { get; set; }
        public String SZS_Opis { get; set; }

        public SrwZlcSkladniki(Int32 _SZS_Id, Int32 _SZS_SZNId, Int32 _SZS_Synchronizacja, Int32 _SZS_Pozycja, Int32 _SZS_TwrTyp, Int32 _SZS_TwrNumer, String _SZS_Ilosc, String _SZS_Opis)
        {
            this.SZS_Id = _SZS_Id;
            this.SZS_SZNId = _SZS_SZNId;
            this.SZS_Synchronizacja = _SZS_Synchronizacja;
            this.SZS_Pozycja = _SZS_Pozycja;
            this.SZS_TwrTyp = _SZS_TwrTyp;
            this.SZS_TwrNumer = _SZS_TwrNumer;
            this.SZS_Ilosc = _SZS_Ilosc;
            this.SZS_Opis = _SZS_Opis;
        }

        public SrwZlcSkladniki()
        { }
    }
}
