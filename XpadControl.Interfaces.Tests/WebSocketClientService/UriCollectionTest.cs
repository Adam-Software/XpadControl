using NUnit.Framework;
using System;
using System.Configuration;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies;

namespace XpadControl.Interfaces.Tests.WebSocketClientService
{
    public class UriCollectionTest
    {
        [Test]
        public void TestCreateUriEmptyStringThowsException()
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

        public void TestCreateUriWithUriBuilderEmptyStringThowsException()
        {
            Assert.Throws<ConfigurationErrorsException>(() =>
            {
                UriCollection _1 = new("", 0, "", "");
                UriCollection _2 = new("127.0.0.1", 9000, "", "");
                UriCollection _3 = new("127.0.0.1", 9000, "/TestWheelPart", "");
                UriCollection _4 = new("127.0.0.1", 9000, "", "/TestServosPart");
                UriCollection _5 = new("127.0.0.1", 0, "/TestWheelPart", "/TestServosPart");
                UriCollection _6 = new("", 9000, "/TestWheelPart", "/TestServosPart");
            });
        }

        [Test]
        public void TestCreateUriFromString()
        {
            UriCollection uriCollection = new("127.0.0.1", "9000", "/TestWheelPart", "/TestServosPart");

            Assert.Multiple(() =>
            {
                Assert.That(uriCollection.WheelWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestWheelPart")));
                Assert.That(uriCollection.ServosWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestServosPart")));
            });
        }

        [Test]
        public void TestCreateUriWithUriBuilder() 
        {
            
            UriCollection uriCollection = new("127.0.0.1", 9000, "TestWheelPart", "TestServosPart");
            UriCollection uriCollectionWithSecondPath = new("127.0.0.1", 9000, "TestWheelPart/Wheel1", "TestServosPart/Servo1");

            UriCollection uriCollectionWithSlash = new("127.0.0.1", 9000, "/TestWheelPart", "/TestServosPart");
            UriCollection uriCollectionWithSecondPathAndSlash = new("127.0.0.1", 9000, "/TestWheelPart/Wheel1", "/TestServosPart/Servo1");

            Assert.Multiple(() =>
            {
                Assert.That(uriCollection.WheelWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestWheelPart")));
                Assert.That(uriCollection.ServosWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestServosPart")));

                Assert.That(uriCollectionWithSecondPath.WheelWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestWheelPart/Wheel1")));
                Assert.That(uriCollectionWithSecondPath.ServosWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestServosPart/Servo1")));

                Assert.That(uriCollectionWithSlash.WheelWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestWheelPart")));
                Assert.That(uriCollectionWithSlash.ServosWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestServosPart")));

                Assert.That(uriCollectionWithSecondPathAndSlash.WheelWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestWheelPart/Wheel1")));
                Assert.That(uriCollectionWithSecondPathAndSlash.ServosWebSocketUri, Is.EqualTo(new Uri("ws://127.0.0.1:9000/TestServosPart/Servo1")));
            });
        }
    }
}
