using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLogin.App.Models
{
    public class UserInfo
    {
        public string Pin { get; set; }
        public string Password { get; set; }
        public class UserInfoValidator : AbstractValidator<UserInfo>
        {
            public UserInfoValidator()
            {
                RuleFor(x => x.Password)
                    .MinimumLength(6).WithMessage("Şifrə minimum 6 və maksimum 32 xarakterdən ibarət ola bilər")
                    .MaximumLength(32).WithMessage("Şifrə minimum 6 və maksimum 32 xarakterdən ibarət ola bilər")
                    .NotNull().WithMessage("İstifadəçi şifrəsi məcburidir");

                RuleFor(x => x.Pin)
                    .MaximumLength(50)
                    .NotNull()
                    .WithMessage("FİN sahəsi boş ola bilməz");
            }
        }
    }
}
