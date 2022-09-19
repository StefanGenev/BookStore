using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.ViewModels
{
    public class BookViewModel
    {
        [Required(ErrorMessage = "Непопълнено заглавие")]
        [DisplayName("Заглавие")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Непопълнено описание")]
        [DisplayName("Описание")]
        public string Description { get; set; }

        public List<Author> Authors { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Изберете автор")]
        [DisplayName("Автор")]
        public int AuthorId { get; set; }


        [Required]
        [Range(0, 9999.99, ErrorMessage = "Непопълнена стойност за цена. Цената трябва да е положително число ")]
        [DisplayName("Цена")]
        public double Price { get; set; }

        public List<Publisher> Publishers { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Изберете издателство")]
        [DisplayName("Издателство")]
        public int PublisherId { get; set; }

        [Required]
        [Range(1800, int.MaxValue, ErrorMessage = "Невалидна година на издаване")]
        [DisplayName("Година на издаване")]
        public int YearOfPublishing { get; set; }

        [Required]
        [DisplayName("Налични копия")]
        [Range(1, int.MaxValue, ErrorMessage = "Броят налични копия трябва да е положително число")]
        public int CopiesAvailable { get; set; }

        [DisplayName("Снимка на корица")]
        public IFormFile Image { get; set; }
    }
}
