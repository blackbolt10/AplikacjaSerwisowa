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
        public String Sru_Nazwa { get; set; }
        public String SrU_Opis { get; set; }
        public Int32 SrU_Archiwalne { get; set; }

        public SrwUrzadzenia(Int32 _SrU_Id, Int32 _SrU_SURId, String _Sru_Nazwa, String _SrU_Opis, Int32 _SrU_Archiwalne)
        {
            SrU_Id = _SrU_Id;
            SrU_SURId = _SrU_SURId;
            Sru_Nazwa = _Sru_Nazwa;
            SrU_Opis = _SrU_Opis;
            SrU_Archiwalne = _SrU_Archiwalne;
        }

        public SrwUrzadzenia() { }
    }
}