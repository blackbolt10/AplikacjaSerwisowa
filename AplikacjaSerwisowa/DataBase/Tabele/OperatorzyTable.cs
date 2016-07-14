using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("OperatorzyTable")]
    public class OperatorzyTable
    {
        [PrimaryKey,AutoIncrement, Column("_Id")]
        public int Id { get; set; }

        [MaxLength(50)]
        public String Imie { get; set; }

        [MaxLength(50)]
        public String Nazwisko { get; set; }

        [MaxLength(20)]
        public String Haslo { get; set; }

        [MaxLength(10),Unique]
        public String Akronim { get; set; }

    }
}