using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using XpadControl.Interfaces.GamepadService.Dependencies.Extensions;

namespace XpadControl.Math.Tests
{
    public class GamepadServiceExtensionTest
    {
        [Test]
        public void TestConvertsThumbToFloat()
        {
            short minAxis = -32768;
            short maxAxis = 32767;

            Assert.That(minAxis.ThumbToFloat(), Is.EqualTo(-1));
            Assert.That(maxAxis.ThumbToFloat(), Is.EqualTo(1));
        }

        [Test]  
        public void TestConvertsTriggerToFloat()
        {
            short minTrigger = short.MinValue;
            short maxTrigger = short.MaxValue;

            Assert.That(minTrigger.TriggerToFloat(), Is.EqualTo(0));
            Assert.That(maxTrigger.TriggerToFloat(), Is.EqualTo(1));
        }
    }
}