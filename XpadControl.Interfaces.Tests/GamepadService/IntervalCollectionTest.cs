using NUnit.Framework;
using XpadControl.Interfaces.GamepadService.Dependencies.SettingsCollection;

namespace XpadControl.Interfaces.Tests.GamepadService
{
    public class IntervalCollectionTest
    {
        [Test]
        public void TestConvertsToMs()
        {
            UpdateIntervalCollection updateIntervalCollection = new(10, 0.5);

            Assert.Multiple(() =>
            {
                Assert.That(updateIntervalCollection.LinuxGamepadUpdatePolling, Is.EqualTo(10000));
                Assert.That(updateIntervalCollection.WindowGamepadUpdatePolling, Is.EqualTo(500));
            });
        }

        [Test]
        public void TestNegativeValuesToDefaut()
        {
            UpdateIntervalCollection updateIntervalCollection = new(-10, -0.5);

            Assert.Multiple(() =>
            {
                Assert.That(updateIntervalCollection.LinuxGamepadUpdatePolling, Is.EqualTo(2000));
                Assert.That(updateIntervalCollection.WindowGamepadUpdatePolling, Is.EqualTo(100));
            });
        }


        [Test]
        public void TestRoundedValuesToDefaut()
        {
            UpdateIntervalCollection updateIntervalCollection = new(10, 0.5656565545646);

            Assert.Multiple(() =>
            {
                Assert.That(updateIntervalCollection.LinuxGamepadUpdatePolling, Is.EqualTo(10000));
                Assert.That(updateIntervalCollection.WindowGamepadUpdatePolling, Is.EqualTo(566));
            });
        }
    }
}
