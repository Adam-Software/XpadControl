using NUnit.Framework;
using XpadControl.Interfaces.Common.Dependencies.SettingsCollection;

namespace XpadControl.Interfaces.Tests.Common
{
    public class IntervalCollectionTest
    {
        [Test]
        public void TestConvertsToMs()
        {
            UpdateIntervalCollection updateIntervalCollection = new(10, 0.5, 10);

            Assert.Multiple(() =>
            {
                Assert.That(updateIntervalCollection.LinuxGamepadUpdatePolling, Is.EqualTo(10000));
                Assert.That(updateIntervalCollection.WindowGamepadUpdatePolling, Is.EqualTo(500));
                Assert.That(updateIntervalCollection.WebsocketReconnectInterval, Is.EqualTo(10000));
            });
        }

        [Test]
        public void TestNegativeValuesToDefaut()
        {
            UpdateIntervalCollection updateIntervalCollection = new(-10, -0.5, -10);

            Assert.Multiple(() =>
            {
                Assert.That(updateIntervalCollection.LinuxGamepadUpdatePolling, Is.EqualTo(2000));
                Assert.That(updateIntervalCollection.WindowGamepadUpdatePolling, Is.EqualTo(100));
                Assert.That(updateIntervalCollection.WebsocketReconnectInterval, Is.EqualTo(5000));
            });
        }

        [Test]
        public void TestRoundedValuesToDefaut()
        {
            UpdateIntervalCollection updateIntervalCollection = new(10, 0.5656565545646, 10);

            Assert.Multiple(() =>
            {
                Assert.That(updateIntervalCollection.LinuxGamepadUpdatePolling, Is.EqualTo(10000));
                Assert.That(updateIntervalCollection.WindowGamepadUpdatePolling, Is.EqualTo(566));
                Assert.That(updateIntervalCollection.WebsocketReconnectInterval, Is.EqualTo(10000));
            });
        }

        [Test]
        public void TestEmptyValuesToDefaut()
        {
            UpdateIntervalCollection updateIntervalCollection = new();

            Assert.Multiple(() =>
            {
                Assert.That(updateIntervalCollection.LinuxGamepadUpdatePolling, Is.EqualTo(2000));
                Assert.That(updateIntervalCollection.WindowGamepadUpdatePolling, Is.EqualTo(100));
                Assert.That(updateIntervalCollection.WebsocketReconnectInterval, Is.EqualTo(5000));
            });
        }
    }
}
