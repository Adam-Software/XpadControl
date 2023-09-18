using NUnit.Framework;
using System;
using System.Configuration;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies;

namespace XpadControl.Interfaces.Tests.WebSocketClientService
{
    public class UriCollectionTest
    {
        [Test]
        public void TestEmptyStringThowsException()
        {
            Assert.Throws<ConfigurationErrorsException>(() =>
            {
                UriCollection _1 = new("", "", "", "");
                UriCollection _2 = new("127.0.0.1", "9000", "", "");
                UriCollection _3 = new("127.0.0.1", "9000", "/TestWheelPart", "");
                UriCollection _4 = new("127.0.0.1", "9000", "", "/TestServosPart");
                UriCollection _5 = new("127.0.0.1", "", "/TestWheelPart", "/TestServosPart");
                UriCollection _6 = new("", "9000", "/TestWheelPart", "/TestServosPart");
            });
        }

        [Test]
        public void TestCreateUri()
        {
            UriCollection uriCollection = new("127.0.0.1", "9000", "/TestWheelPart", "/TestServosPart");

            Assert.Multiple(() =>
            {
                Assert.That(uriCollection.WheelWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestWheelPart")));
                Assert.That(uriCollection.ServosWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestServosPart")));
            });
        }
    }
}
