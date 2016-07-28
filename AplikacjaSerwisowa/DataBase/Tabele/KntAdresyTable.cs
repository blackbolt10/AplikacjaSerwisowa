using System;
using System.Data;
using System.IO;
using SQLite;

namespace AplikacjaSerwisowa
{
    [Table("KntAdresyTable")]
    public class KntAdresyTable
    {
        [PrimaryKey, AutoIncrement, Column("_Id")]
        public int Id { get; set; }

        [Unique]
        public Int32 Kna_GIDNumer { get; set; }

        public Int32 Kna_GIDTyp { get; set; }

        public Int32 Kna_KntNumer { get; set; }

        [MaxLength(20), Unique]
        public String Kna_Akronim { get; set; }

        [MaxLength(50)]
        public String Kna_nazwa1 { get; set; }

        [MaxLength(50)]
        public String Kna_nazwa2 { get; set; }

        [MaxLength(250)]
        public String Kna_nazwa3 { get; set; }

        [MaxLength(10)]
        public String Kna_KodP { get; set; }

        [MaxLength(30)]
        public String Kna_miasto { get; set; }

        [MaxLength(30)]
        public String Kna_ulica { get; set; }

        [MaxLength(30)]
        public String Kna_Adres { get; set; }

        [MaxLength(13)]
        public String Kna_nip { get; set; }

        [MaxLength(30)]
        public String Kna_telefon1 { get; set; }

        [MaxLength(30)]
        public String Kna_telefon2 { get; set; }

        [MaxLength(30)]
        public String Kna_telex { get; set; }

        [MaxLength(30)]
        public String Kna_fax { get; set; }

        [MaxLength(255)]
        public String Kna_email { get; set; }
    }
}