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
            short minAxis = short.MinValue;
            short maxAxis = short.MaxValue;

            Assert.Multiple(() =>
            {
                Assert.That(minAxis.ThumbToFloat(), Is.EqualTo(-1));
                Assert.That(maxAxis.ThumbToFloat(), Is.EqualTo(1));
            });
        }

        [Test]
        public void TestConvertsThumbRange()
        {
            short minAxis = short.MinValue;
            short maxAxis = short.MaxValue;

            for(short i = minAxis; i<= maxAxis; i++)
            {
                Assert.That(i.ThumbToFloat(), Is.InRange(-1f, 1f));
            }
        }

        [Test]  
        public void TestConvertsTriggerToFloat()
        {
            short minTrigger = short.MinValue;
            short deadZoneValue = -32767;
            short maxTrigger = short.MaxValue;
            
            Assert.Multiple(() =>
            {
                Assert.That(minTrigger.TriggerToFloat(), Is.EqualTo(0));
                Assert.That(deadZoneValue.TriggerToFloat(), Is.EqualTo(0));
                Assert.That(maxTrigger.TriggerToFloat(), Is.EqualTo(1));
            });
        }
    }
}