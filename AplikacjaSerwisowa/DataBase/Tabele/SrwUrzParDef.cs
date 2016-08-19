using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("SrwUrzParDef")]
    public class SrwUrzParDef
    {
        public Int32 ID { get; set; }
        public Int32 SUD_Id { get; set; }
        public String SUD_Nazwa { get; set; }
        public String SUD_Format { get; set; }
        public Int32 SUD_Archiwalna { get; set; }

        public SrwUrzParDef(Int32 _SUD_Id, String _SUD_Nazwa, String _SUD_Format, Int32 _SUD_Archiwalna)
        {
            SUD_Id = _SUD_Id;
            SUD_Nazwa = _SUD_Nazwa;
            SUD_Format = _SUD_Format;
            SUD_Archiwalna = _SUD_Archiwalna;
        }

        public SrwUrzParDef() { }
    }
}