using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Pokedex.Domain.Translation;
using Xunit;

namespace Pokedex.Domain.Tests.Translation
{
    public class YodaTranslationStrategyTests
    {
        [Fact]
        public async Task Successful_Translations_Are_Returned()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var value = "some_value";
            var translatedValue = "some_translated_value";
            var translationService = Substitute.For<ITranslationService>();
            translationService.Translate(value, "yoda", cancellationTokenSource.Token).Returns(translatedValue);
            var sut = new YodaTranslationStrategy(translationService, Substitute.For<ILoggerFactory>());
            
            // Act
            var result = await sut.Translate(value, cancellationTokenSource.Token);

            // Assert
            result.WasSuccessful.Should().BeTrue();
            result.Value.Should().Be(translatedValue);
        }

        [Fact]
        public async Task Unsuccessful_Translations_Are_Expressed()
        {
            // Arrange
            using var cancellationTokenSource = new CancellationTokenSource();
            var value = "some_value";
            var translationService = Substitute.For<ITranslationService>();
            translationService.Translate(value, "yoda", cancellationTokenSource.Token).Throws(new Exception("some_reason_for_failure"));
            var sut = new YodaTranslationStrategy(translationService, Substitute.For<ILoggerFactory>());
            
            // Act
            var result = await sut.Translate(value, cancellationTokenSource.Token);

            // Assert
            result.WasSuccessful.Should().BeFalse();
        }
    }
}