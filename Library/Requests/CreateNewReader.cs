﻿using System.ComponentModel.DataAnnotations;

namespace Library.Requests
{
    public class CreateNewReader
    {
        [Required]
        public string First_name { get; set; }
        [Required]
        public string Last_name { get; set; }
        [Required]
        public int Birth_year { get; set; }
        [Required]
        public string Contact_info { get; set; }
        [Required]
        public DateTime? RegistrationDate { get; set; } 
    }
}
