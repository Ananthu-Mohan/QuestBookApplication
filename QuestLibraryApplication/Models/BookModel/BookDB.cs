using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestLibraryApplication.Models.BookModel
{
    public class BookDB
    {
        [Key]
        public int BookID { get; set; }
        [Required]
        public string BookName { get; set; }
        [Required]
        public string BookAuthor { get; set; }
        [Required]
        public string BookDescription { get; set; }
        [Required]
        public int BookPrice { get; set; }
        public DateTime ReleasedDate { get; set; }

    }
}