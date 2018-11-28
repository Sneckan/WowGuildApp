using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WowGuildApp.Models
{
    public class PostViewModelValidator : AbstractValidator<PostsViewModel>
    {
        public PostViewModelValidator()
        {
            RuleFor(reg => reg.Text).NotEmpty();
        }
    }
}
