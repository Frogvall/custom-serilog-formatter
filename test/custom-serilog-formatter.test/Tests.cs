using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Parsing;
using Xunit;

namespace CustomSerilogFormatter.test
{
    public class Tests
    {
       [Fact]
        public void TestLogEvent_GivenJustMandatoryParameters_ReturnsFormattedLogText()
       {
            //Arrange
            var expected = "{\"app\":\"TestApp\",\"timestamp\":\"2017-03-08T05:38:50.2860586Z\",\"message\":\"TestMessage\",\"level\":\"Debug\",\"exception\":\"System.Exception: Test exception\",\"version\":\"1.2.3,\"logVersion\":\"1}";
            var formatter = new CustomSerilogFormatter("TestApp", "1.2.3");
            var result = new StringBuilder();
            var output = new StringWriter(result);            
            var date = DateTime.Parse("2017 - 03 - 08T05: 38:50.2860586Z");
            var properties = new List<LogEventProperty>();
            var message = new MessageTemplate("TestMessage", new List<MessageTemplateToken>());
            var logEvent = new LogEvent(date, LogEventLevel.Debug, new Exception("Test exception"),
                message, properties);
            
            //Act
            formatter.FormatEvent(logEvent, output, new JsonValueFormatter(typeTagName: "$type"));           
            
            //Assert
            Assert.Equal(expected, result.ToString());
        }
    }
}
