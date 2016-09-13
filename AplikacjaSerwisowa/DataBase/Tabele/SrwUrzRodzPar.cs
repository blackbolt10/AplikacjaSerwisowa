using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("SrwUrzRodzPar")]
    public class SrwUrzRodzPar
    {
        [Column("_Id")]
        public Int32 ID { get; set; }
        [PrimaryKey, Unique]
        public Int32 SRP_Id { get; set; }
        public Int32 SRP_SURId { get; set; }
        public Int32 SRP_SUDId { get; set; }
        public Int32 SRP_Lp { get; set; }
        public Int32 SRP_ToDo { get; set; }

        public SrwUrzRodzPar(Int32 _SRP_Id, Int32 _SRP_SURId, Int32 _SRP_SUDId, Int32 _SRP_Lp, Int32 _SRP_ToDo)
        {
            SRP_Id = _SRP_Id;
            SRP_SURId = _SRP_SURId;
            SRP_SUDId = _SRP_SUDId;
            SRP_Lp = _SRP_Lp;
            SRP_ToDo = _SRP_ToDo;
        }

        public SrwUrzRodzPar() { }
    }
}