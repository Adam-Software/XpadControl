using NUnit.Framework;
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
            ushort minTrigger = ushort.MinValue;
            ushort maxTrigger = ushort.MaxValue;

            Assert.That(minTrigger.TriggerToFloat(), Is.EqualTo(0));
            Assert.That(maxTrigger.TriggerToFloat(), Is.EqualTo(1));
        }
    }
}