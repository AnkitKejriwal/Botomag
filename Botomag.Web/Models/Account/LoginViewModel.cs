using System;
using System.ComponentModel.DataAnnotations;

namespace Botomag.Web.Models.Account
{
    public class LoginViewModel : BaseViewModel<Guid>
    {
        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email является обязательным полем.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль явлется обязательным полем.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Минимальная длина пароля: {2}, максимальная: {1}.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool IsPersistent { get; set; }
    }
}