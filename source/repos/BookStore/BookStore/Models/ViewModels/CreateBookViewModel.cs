﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.NewFolder
{
    public class DateAttribute : RangeAttribute
    {
        public DateAttribute()
          : base(typeof(DateTime), "1/1/1800", DateTime.Now.ToShortDateString()) 
        {
            //TODO different file
        }
    }
    public class CreateBookViewModel
    {
        [Required]
        [DisplayName("Заглавие")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Описание")]
        public string Description { get; set; }

        public List<Author> Authors { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Изберете автор")]
        [DisplayName("Автор")]
        public int AuthorId { get; set; }
       

        [Required]
        [Range(0.01 , double.MaxValue, ErrorMessage = "Некоректна стойност за цена")]
        [DisplayName("Цена")]
        public double Price { get; set; }

        public List<Publisher> Publishers { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Изберете издателство")]
        [DisplayName("Издателство")]
        public int PublisherId { get; set; }

        private static int _currentYear = DateTime.Now.Year;

        [Required]
        [Date(ErrorMessage = "Невалидна година на издаване")]
        [DisplayName("Година на издаване")]
        public int YearOfPublishing { get; set; }

        [Required]
        [DisplayName("Снимка на корица")]
        public IFormFile Image { get; set; }
    }
}
