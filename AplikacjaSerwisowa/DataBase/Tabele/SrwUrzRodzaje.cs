using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("SrwUrzRodzaje")]
    public class SrwUrzRodzaje
    {
        [Column("_Id")]
        public Int32 ID { get; set; }
        [PrimaryKey, Unique]
        public Int32 SUR_Id { get; set; }
        public String SUR_Kod { get; set; }
        public String SUR_Nazwa { get; set; }
        public Int32 SUR_ToDo { get; set; }

        public SrwUrzRodzaje(Int32 _SUR_Id, String _SUR_Kod, String _SUR_Nazwa, Int32 _SUR_ToDo)
        {
            SUR_Id = _SUR_Id;
            SUR_Kod = _SUR_Kod;
            SUR_Nazwa = _SUR_Nazwa;
            SUR_ToDo = _SUR_ToDo;
        }

        public SrwUrzRodzaje() { }
    }
}