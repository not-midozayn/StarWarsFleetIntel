using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StarWarsFleetIntel.Application.Common.Wrappers;
using StarWarsFleetIntel.Application.Features.Starships.Queries;
using StarWarsFleetIntel.Application.Features.Starships.Queries.GetStarshipById;
using StarWarsFleetIntel.Application.Interfaces;
using StarWarsFleetIntel.Domain.Entities;
using StarWarsFleetIntel.Domain.Enums;

namespace StarWarsFleetIntel.Tests.Features.Starships
{
    public class GetStarshipByIdQueryHandlerTests
    {
        private readonly Mock<ISwapiClient> _swapiClientMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICurrencyConverter> _currencyConverterMock;
        private readonly Mock<IStarshipDecoratorFactory> _decoratorFactoryMock;
        private readonly Mock<IPreFlightCheckHandler> _preFlightCheckHandlerMock;
        private readonly Mock<ILogger<GetStarshipByIdQueryHandler>> _loggerMock;
        private readonly GetStarshipByIdQueryHandler _handler;
        private readonly IFixture _fixture;

        public GetStarshipByIdQueryHandlerTests()
        {
            _swapiClientMock = new Mock<ISwapiClient>();
            _mapperMock = new Mock<IMapper>();
            _currencyConverterMock = new Mock<ICurrencyConverter>();
            _decoratorFactoryMock = new Mock<IStarshipDecoratorFactory>();
            _preFlightCheckHandlerMock = new Mock<IPreFlightCheckHandler>();
            _loggerMock = new Mock<ILogger<GetStarshipByIdQueryHandler>>();

            _handler = new GetStarshipByIdQueryHandler(
                _swapiClientMock.Object,
                _mapperMock.Object,
                _currencyConverterMock.Object,
                _decoratorFactoryMock.Object,
                _preFlightCheckHandlerMock.Object,
                _loggerMock.Object);

            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        [Fact]
        public async Task Handle_WhenStarshipExists_ReturnsSuccessResult()
        {
            // Arrange
            var starship = _fixture.Build<Starship>()
                .With(s => s.Name, "X-Wing")
                .With(s => s.CostInCredits, "149999")
                .Create();

            var query = new GetStarshipByIdQuery
            {
                Id = 12,
                RunPreFlightChecks = false
            };

            _swapiClientMock
                .Setup(x => x.GetStarshipAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Starship>.Success(starship));

            var expectedDto = _fixture.Create<GetStarshipResponse>();
            _mapperMock
                .Setup(x => x.Map<GetStarshipResponse>(It.IsAny<Starship>()))
                .Returns(expectedDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Data.Should().Be(expectedDto);

            _swapiClientMock.Verify(
                x => x.GetStarshipAsync(query.Id, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WhenStarshipNotFound_ReturnsFailureResult()
        {
            // Arrange
            var query = new GetStarshipByIdQuery { Id = 999 };
            var failureMessage = "Starship not found: 999";

            _swapiClientMock
                .Setup(x => x.GetStarshipAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Starship>.Failure(failureMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be(failureMessage);
        }

        [Theory]
        [InlineData(Currency.ImperialCredits)]
        [InlineData(Currency.Wupiupi)]
        public async Task Handle_WithCurrencyConversion_AppliesCorrectStrategy(Currency targetCurrency)
        {
            // Arrange
            var starship = _fixture.Build<Starship>()
                .With(s => s.CostInCredits, "100000")
                .Create();

            var query = new GetStarshipByIdQuery
            {
                Id = 1,
                TargetCurrency = targetCurrency,
                RunPreFlightChecks = false
            };

            _swapiClientMock
                .Setup(x => x.GetStarshipAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<Starship>.Success(starship));

            _currencyConverterMock
                .Setup(x => x.Convert(100000m, targetCurrency))
                .Returns(120000m);

            _currencyConverterMock
                .Setup(x => x.GetCurrencySymbol(targetCurrency))
                .Returns("₡");

            _mapperMock
                .Setup(x => x.Map<GetStarshipResponse>(It.IsAny<Starship>()))
                .Returns(new GetStarshipResponse());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _currencyConverterMock.Verify(
                x => x.Convert(100000m, targetCurrency),
                Times.Once);

            _currencyConverterMock.Verify(
                x => x.GetCurrencySymbol(targetCurrency),
                Times.Once);
        }
    }
}
