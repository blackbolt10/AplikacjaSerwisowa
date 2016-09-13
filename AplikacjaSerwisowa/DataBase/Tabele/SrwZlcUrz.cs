using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("SrwZlcUrz")]
    public class SrwZlcUrz
    {
        [Column("_Id")]
        public Int32 ID { get; set; }
        [PrimaryKey, Unique]
        public Int32 SZU_Id { get; set; }
        public Int32 SZU_SZNId { get; set; }
        public Int32 SZU_SrUId { get; set; }
        public Int32 SZU_Pozycja { get; set; }
        public Int32 SZU_ToDo { get; set; }


        public SrwZlcUrz(Int32 _SZU_Id, Int32 _SZU_SZNId, Int32 _SZU_SrUId, Int32 _SZU_Pozycja, Int32 _SZU_ToDo)
        {
            SZU_Id = _SZU_Id;
            SZU_SZNId = _SZU_SZNId;
            SZU_SrUId = _SZU_SrUId;
            SZU_Pozycja = _SZU_Pozycja;
            SZU_ToDo = _SZU_ToDo;
        }

        public SrwZlcUrz() { }
    }
}