using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaSerwisowaUsluga
{
    class SrwZlcCzynnosciStruct
    {
        public Int32 GZC_Id;
        public Int32 GZC_Sync;
        public Int32 GZC_GZNId;
        public Int32 GZC_Pozycja;
        public Int32 GZC_TwrTyp;
        public Int32 GZC_TwrNumer;

        public String GZC_Ilosc;
        public String GZC_Opis;

        public SrwZlcCzynnosciStruct(Int32 _GZC_Id, Int32 _GZC_Sync, Int32 _GZC_GZNId, Int32 _GZC_Pozycja, Int32 _GZC_TwrTyp, Int32 _GZC_TwrNumer, String _GZC_Ilosc, String _GZC_Opis)
        {
            GZC_Id = _GZC_Id;
            GZC_Sync = _GZC_Sync;
            GZC_GZNId = _GZC_GZNId;
            GZC_Pozycja = _GZC_Pozycja;
            GZC_TwrTyp = _GZC_TwrTyp;
            GZC_TwrNumer = _GZC_TwrNumer;
            GZC_Ilosc = _GZC_Ilosc;
            GZC_Opis = _GZC_Opis;
        }

        public SrwZlcCzynnosciStruct() { }
    }
}

