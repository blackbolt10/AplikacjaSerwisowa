using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("KntAdresyTable")]
    public class KntAdresyTable
    {
        [Column("_Id")]
        public int Id { get; set; }

        [PrimaryKey, Unique]
        public Int32 Kna_GIDNumer { get; set; }

        public Int32 Kna_GIDTyp { get; set; }

        public Int32 Kna_KntNumer { get; set; }

        public String Kna_Akronim { get; set; }

        public String Kna_nazwa1 { get; set; }

        public String Kna_nazwa2 { get; set; }

        public String Kna_nazwa3 { get; set; }

        public String Kna_KodP { get; set; }

        public String Kna_miasto { get; set; }

        public String Kna_ulica { get; set; }

        public String Kna_Adres { get; set; }

        public String Kna_nip { get; set; }

        public String Kna_telefon1 { get; set; }

        public String Kna_telefon2 { get; set; }

        public String Kna_telex { get; set; }

        public String Kna_fax { get; set; }

        public String Kna_email { get; set; }

        public Int32 Kna_ToDo { get; set; }
    }
}