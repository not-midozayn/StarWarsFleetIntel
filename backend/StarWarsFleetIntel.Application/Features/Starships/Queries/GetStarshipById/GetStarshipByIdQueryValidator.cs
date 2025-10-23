using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace StarWarsFleetIntel.Application.Features.Starships.Queries.GetStarshipById
{
    public class GetStarshipByIdQueryValidator : AbstractValidator<GetStarshipByIdQuery>
    {
        public GetStarshipByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Starship ID must be greater than 0");
        }
    }
}
