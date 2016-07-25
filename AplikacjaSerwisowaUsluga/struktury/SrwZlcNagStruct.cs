using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaSerwisowaUsluga
{
    class SrwZlcNagStruct
    {
        public Int32 Id;

        public Int32 KntTyp;
        public Int32 KntNumer;

        public Int32 KnATyp;
        public Int32 KnANumer;

        public Int32 KndTyp;
        public Int32 KndNumer;

        public Int32 KnPTyp;
        public Int32 KnPNumer;

        public DateTime DataWystawienia;
        public DateTime DataRozpoczecia;

        public String Opis;

        public SrwZlcNagStruct(Int32 _Id, Int32 _KntTyp, Int32 _KntNumer, Int32 _KnATyp, Int32 _KnANumer, Int32 _KndTyp, Int32 _KndNumer, Int32 _KnPTyp, Int32 _KnPNumer, DateTime _DataWystawienia, DateTime _DataRozpoczecia, String _Opis)
        {
            Id = _Id;
            KntTyp = _KntTyp;
            KntNumer = _KntNumer;
            KnATyp = _KnATyp;
            KnANumer = _KnANumer;
            KndTyp = _KndTyp;
            KndNumer = _KndNumer;
            KnPTyp = _KnPTyp;
            KnPNumer = _KnPNumer;
            DataWystawienia = _DataWystawienia;
            DataRozpoczecia = _DataRozpoczecia;
            Opis = _Opis;
        }

        public SrwZlcNagStruct() { }
    }
}

