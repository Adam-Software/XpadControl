using NUnit.Framework;
using System;
using XInputium.XInput;
using XpadControl.Interfaces.GamepadService.Dependencies;
using XpadControl.Windows.Services.Extensions;

namespace XpadControl.Windows.Services.Test
{
    public class ButtonsExtensionTests
    {
        [Test]
        public void TestConvertedResultByType()
        {
            Buttons result = XButtons.B.ToButtons();
            Assert.That(result, Is.InstanceOf<Buttons>());
        }

        [Test]
        public void TestConvertedResultByName()
        {
            string[] xNames = Enum.GetNames(typeof(XButtons));
            string[] myNames = Enum.GetNames(typeof(Buttons));

            CollectionAssert.AreEqual(xNames, myNames);
        }
    }
}