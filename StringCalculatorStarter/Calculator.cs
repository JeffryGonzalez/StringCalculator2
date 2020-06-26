using System;
using System.Collections.Generic;
using System.Text;

namespace StringCalculatorStarter
{
    public class Calculator
    {
        private ILogger _logger;
        private IWebService _webService;

        public Calculator(ILogger logger, IWebService webService)
        {
            _logger = logger;
            _webService = webService;
        }

        public int Add(string numbers)
        {
            int answer = 0;
            if (numbers == "")
            {
                answer = 0;
            }
            else
            {
                answer = int.Parse(numbers);
            }
            try
            {
                _logger.Write(answer);
            }
            catch (LoggerException)
            {
                _webService.LogError(
                    "Error Logging " + numbers);
                
            }
            return answer;
        }
    }

    public interface ILogger
    {
        void Write(int result);
    }

    public interface IWebService
    {
        void LogError(string message);
    }

    public class LoggerException :  Exception { }
}
