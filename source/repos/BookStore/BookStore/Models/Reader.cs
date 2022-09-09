﻿using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class Reader
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public bool IsMale { get; set; }
    }
}
