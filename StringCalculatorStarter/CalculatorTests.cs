using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StringCalculatorStarter
{
    public class CalculatorTests
    {
        Calculator _calculator;
        Mock<ILogger> _loggerMock;
        Mock<IWebService> _webServiceMock;
        public CalculatorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _webServiceMock = new Mock<IWebService>();
            _calculator = new Calculator(_loggerMock.Object, _webServiceMock.Object);
        }
        [Fact]
        public void EmptyStringReturnsZero()
        {
            
            var result = _calculator.Add("");

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        [InlineData("12", 12)]
        public void SingleDigits(string numbers, int expected)
        {
            var result = _calculator.Add(numbers);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        [InlineData("12", 12)]
        public void ResultsAreLogged(string numbers, int expected)
        {
            var result = _calculator.Add(numbers);

            _loggerMock.Verify(m => m.Write(result));
        }

        [Fact]
        public void LoggerExceptionSwallows()
        {
            _loggerMock.Setup(m => m.Write(
                It.IsAny<int>())).Throws<LoggerException>();

            var result = _calculator.Add("1");

            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("12")]
        public void WebServiceCalledWithException(string numbers)
        {
            _loggerMock.Setup(m => m.Write(
                It.IsAny<int>())).Throws<LoggerException>();

            var result = _calculator.Add(numbers);

            _webServiceMock.Verify(m =>
                m.LogError("Error Logging " + numbers)

            ) ;
        }


        [Fact]
        public void WebServiceNotCalledWithNoExceptions()
        {
            _calculator.Add("99");

            _webServiceMock.Verify(
                m => m.LogError(It.IsAny<string>()), Times.Never);
        }
    }
}
