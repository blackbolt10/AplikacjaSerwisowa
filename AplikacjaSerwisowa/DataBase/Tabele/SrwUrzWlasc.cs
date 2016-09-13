using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("SrwUrzWlasc")]
    public class SrwUrzWlasc
    {
        [Column("_Id")]
        public Int32 ID { get; set; }
        [PrimaryKey]
        public Int32 SUW_SrUId { get; set; }
        public Int32 SUW_WlaNumer { get; set; }
        public Int32 SUW_ToDo { get; set; }

        public SrwUrzWlasc(Int32 _SUW_SrUId, Int32 _SUW_WlaNumer, Int32 _SUW_ToDo)
        {
            SUW_SrUId = _SUW_SrUId;
            SUW_WlaNumer = _SUW_WlaNumer;
            SUW_ToDo = _SUW_ToDo;
        }

        public SrwUrzWlasc() { }
    }
}