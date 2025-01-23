namespace CsvApp.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Chemical
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public bool Natural { get; set; }
        public string CAS { get; set; }
        public long Parts { get; set; }
        public double Cost { get; set; }
        public DateTime LatestDate { get; set; }
    }
}