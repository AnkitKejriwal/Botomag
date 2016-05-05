using System;
using System.ComponentModel.DataAnnotations;

namespace Botomag.Web.Models.Account
{
    /// <summary>
    /// Used for register new user in app
    /// </summary>
    public class RegisterViewModel : BaseViewModel<Guid>
    {
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email является обязательным полем.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль явлется обязательным полем.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Минимальная длина пароля: {0}, максимальная: {1}.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Поля \"Пароль\" и \"Подтверждение пароля\" должны совпадать")]
        public string ConfirmPassword { get; set; }
    }
}