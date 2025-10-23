using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using StarWarsFleetIntel.Application.Features.Starships.Queries.GetStarshipById;

namespace StarWarsFleetIntel.Application.Features.Starships.Queries.GetStarshipsPaginated
{
    public class GetStarshipsQueryValidator : AbstractValidator<GetStarshipsQuery>
    {
        public GetStarshipsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.");
        }
    }
}
