using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaSerwisowaUsluga
{
    class SrwZlcSkladnikiStruct
    {
        public Int32 GZS_Id;
        public Int32 GZS_Sync;
        public Int32 GZS_GZNId;
        public Int32 GZS_Pozycja;
        public Int32 GZS_TwrTyp;
        public Int32 GZS_TwrNumer;

        public String GZS_Ilosc;
        public String GZS_Opis;

        public SrwZlcSkladnikiStruct(Int32 _GZS_Id, Int32 _GZS_Sync, Int32 _GZS_GZNId, Int32 _GZS_Pozycja, Int32 _GZS_TwrTyp, Int32 _GZS_TwrNumer, String _GZS_Ilosc, String _GZS_Opis)
        {
            GZS_Id = _GZS_Id;
            GZS_Sync = _GZS_Sync;
            GZS_GZNId = _GZS_GZNId;
            GZS_Pozycja = _GZS_Pozycja;
            GZS_TwrTyp = _GZS_TwrTyp;
            GZS_TwrNumer = _GZS_TwrNumer;
            GZS_Ilosc = _GZS_Ilosc;
            GZS_Opis = _GZS_Opis;
        }

        public SrwZlcSkladnikiStruct() { }
    }
}

