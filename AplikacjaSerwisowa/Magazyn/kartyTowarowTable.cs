using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("kartyTowarowTable")]
    public class kartyTowarowTable
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }

        [MaxLength(40), Unique]
        public String TWR_Kod { get; set; }
        
        public Int32 TWR_GIDNumer { get; set; }
        
        public Int32 TWR_Typ { get; set; }

        [MaxLength(255)]
        public String TWR_Nazwa { get; set; }
    }
}