﻿using blogpessoal.Model;
using FluentValidation;

namespace blogpessoal.Validator
{
    public class UserValidator: AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Nome)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(u => u.Usuario)
                .NotEmpty()
                .MaximumLength(255)
                .EmailAddress();

            RuleFor(u => u.Senha)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(255);


            RuleFor(u => u.Foto)
                .MaximumLength(5000);
        }
    }
}
