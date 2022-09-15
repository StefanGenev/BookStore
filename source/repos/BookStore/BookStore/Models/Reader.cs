
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Reader : IdentityUser
    {
        [Required(ErrorMessage = "Непопълнено име")]
        [DisplayName("Име")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Непопълнено презиме")]
        [DisplayName("Презиме")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Непопълнена фамилия")]
        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Непопълнен адрес")]
        [DisplayName("Адрес")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Неизбран пол")]
        [DisplayName("Пол")]
        public Gender Gender { get; set; }

        [DisplayName("Профилна снимка")]
        public string ImagePath { get; set; }
    }
}
