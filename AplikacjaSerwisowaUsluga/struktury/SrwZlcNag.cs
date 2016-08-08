using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaSerwisowaUsluga
{
    class SrwZlcNag
    {
        public int Id { get; set; }
        public int SZN_Id { get; set; }
        public int SZN_Synchronizacja { get; set; }
        public int SZN_KntTyp { get; set; }
        public int SZN_KntNumer { get; set; }
        public int SZN_KnATyp { get; set; }
        public int SZN_KnANumer { get; set; }
        public String SZN_Dokument { get; set; }
        public String SZN_DataWystawienia { get; set; }
        public String SZN_DataRozpoczecia { get; set; }
        public String SZN_Stan { get; set; }
        public String SZN_Status { get; set; }
        public String SZN_Opis { get; set; }


        public SrwZlcNag(int _SZN_Id, int _SZN_Synchronizacja, int _SZN_KntTyp, int _SZN_KntNumer, int _SZN_KnATyp, int _SZN_KnANumer, String _SZN_Dokument, String _SZN_DataWystawienia, String _SZN_DataRozpoczecia, String _SZN_Stan, String _SZN_Status, String _SZN_Opis)
        {
            this.SZN_Id = _SZN_Id;
            this.SZN_Synchronizacja = _SZN_Synchronizacja;
            this.SZN_KntTyp = _SZN_KntTyp;
            this.SZN_KntNumer = _SZN_KntNumer;
            this.SZN_KnATyp = _SZN_KnATyp;
            this.SZN_KnANumer = _SZN_KnANumer;
            this.SZN_Dokument = _SZN_Dokument;
            this.SZN_DataWystawienia = _SZN_DataWystawienia;
            this.SZN_DataRozpoczecia = _SZN_DataRozpoczecia;
            this.SZN_Stan = _SZN_Stan;
            this.SZN_Status = _SZN_Status;
            this.SZN_Opis = _SZN_Opis;
        }

        public SrwZlcNag()
        { }
    }
}

