using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("OperatorzyTable")]
    public class OperatorzyTable
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }

        public String Akronim { get; set; }

        public String Haslo { get; set; }

        public String Imie { get; set; }
        
        public String Nazwisko { get; set; }
    }
}