using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("SrwUrzadzenia")]
    public class SrwUrzadzenia
    {
        public Int32 ID { get; set; }
        public Int32 SrU_Id { get; set; }
        public Int32 SrU_SURId { get; set; }
        public String Sru_Kod { get; set; }
        public String Sru_Nazwa { get; set; }
        public String SrU_Opis { get; set; }
        public Int32 SrU_Archiwalne { get; set; }
        public Int32 SUW_WlaNumer { get; set; }
        public Boolean zaznaczone { get; set; }


        public SrwUrzadzenia(Int32 _SrU_Id, Int32 _SrU_SURId, String _Sru_Kod, String _Sru_Nazwa, String _SrU_Opis, Int32 _SrU_Archiwalne, Int32 _SUW_WlaNumer, Boolean _zaznaczone)
        {
            SrU_Id = _SrU_Id;
            SrU_SURId = _SrU_SURId;
            Sru_Kod = _Sru_Kod;
            Sru_Nazwa = _Sru_Nazwa;
            SrU_Opis = _SrU_Opis;
            SrU_Archiwalne = _SrU_Archiwalne;
            SUW_WlaNumer = _SUW_WlaNumer;
            zaznaczone = _zaznaczone;
        }

        public SrwUrzadzenia() { }
    }
}