using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.ViewModels
{
    public class RegisterViewModel
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

        [Required(ErrorMessage = "Непопълнен имейл")]
        [DisplayName("Имейл")]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Reader")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Непопълнен адрес")]
        [DisplayName("Адрес")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Неизбран пол")]
        [DisplayName("Пол")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Непопълнен телефонен номер")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Невалиден телефонен номер")]
        [DisplayName("Телефонен номер")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DisplayName("Профилна снимка")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Невалидна парола")]
        [DisplayName("Парола")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Потвърдете паролата")]
        [DisplayName("Потвърждаване на парола")]
        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessage = "Двете пароли не съвпадат")]
        public string ConfirmPassword { get; set; }
    }
}
