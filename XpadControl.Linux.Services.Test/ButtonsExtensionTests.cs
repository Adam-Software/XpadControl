using NUnit.Framework;
using XpadControl.Interfaces.GamepadService.Dependencies;
using XpadControl.Linux.Services.Extensions;
using MyButtonEventArgs = XpadControl.Interfaces.GamepadService.Dependencies.EventArgs.ButtonEventArgs;

namespace XpadControl.Linux.Services.Test
{
    public class ButtonsExtensionTests
    {
        [Test]
        public void TestConvertedByteToButtonsByValues()
        {
            Assert.Multiple(() =>
            {
                Assert.That(((byte)0x0).ToButtons(), Is.EqualTo(Buttons.A));
                Assert.That(((byte)0x1).ToButtons(), Is.EqualTo(Buttons.B));
                Assert.That(((byte)0x2).ToButtons(), Is.EqualTo(Buttons.X));
                Assert.That(((byte)0x3).ToButtons(), Is.EqualTo(Buttons.Y));
                Assert.That(((byte)0x4).ToButtons(), Is.EqualTo(Buttons.LB));
                Assert.That(((byte)0x5).ToButtons(), Is.EqualTo(Buttons.RB));
                Assert.That(((byte)0x6).ToButtons(), Is.EqualTo(Buttons.Back));
                Assert.That(((byte)0x7).ToButtons(), Is.EqualTo(Buttons.Start));
                Assert.That(((byte)0x9).ToButtons(), Is.EqualTo(Buttons.LS));
                Assert.That(((byte)0x10).ToButtons(), Is.EqualTo(Buttons.RS));
                Assert.That(((byte)0x11).ToButtons(), Is.EqualTo(Buttons.DPadLeft));
                Assert.That(((byte)0x12).ToButtons(), Is.EqualTo(Buttons.DPadRight));
                Assert.That(((byte)0x13).ToButtons(), Is.EqualTo(Buttons.DPadDown));
                Assert.That(((byte)0x14).ToButtons(), Is.EqualTo(Buttons.DPadUp));
            });
        }

        [Test]
        public void TestConvertedShortToDpadButtonsByValues()
        {
            short negativeValue = short.MinValue;
            short positiveValue = short.MaxValue;

            Assert.Multiple(() =>
            {
                // x axis
                Assert.That(negativeValue.ToButtonEventArgs(true).Button, Is.EqualTo(Buttons.DPadLeft));
                Assert.That(positiveValue.ToButtonEventArgs(true).Button, Is.EqualTo(Buttons.DPadRight));

                // y axis
                Assert.That(negativeValue.ToButtonEventArgs(false).Button, Is.EqualTo(Buttons.DPadUp));
                Assert.That(positiveValue.ToButtonEventArgs(false).Button, Is.EqualTo(Buttons.DPadDown));
            });
        }

        [Test]
        public void TestConvertedNegativeShortToDpadButtonsPressedReleaseLogicForOneButton()
        {
            short negativeValue = short.MinValue;
            short zeroValue = 0;

            MyButtonEventArgs actualArgs = negativeValue.ToButtonEventArgs(true);
            MyButtonEventArgs expectedArgs = new() { Button = Buttons.DPadLeft, Pressed = true };

            // button pressed check
            Assert.Multiple(() =>
            {
                Assert.That(actualArgs.Button, Is.EqualTo(expectedArgs.Button));
                Assert.That(actualArgs.Pressed, Is.EqualTo(expectedArgs.Pressed));
            });

            actualArgs = zeroValue.ToButtonEventArgs(true);
            expectedArgs = new() { Button = Buttons.DPadLeft, Pressed = false };

            // button released check
            Assert.Multiple(() =>
            {
                Assert.That(actualArgs.Button, Is.EqualTo(expectedArgs.Button));
                Assert.That(actualArgs.Pressed, Is.EqualTo(expectedArgs.Pressed));
            });
        }

        [Test]
        public void TestConvertedPositiveShortToDpadButtonsPressedReleaseLogicForOneButton()
        {
            short positiveValue = short.MaxValue;
            short zeroValue = 0;

            MyButtonEventArgs actualArgs = positiveValue.ToButtonEventArgs(true);
            MyButtonEventArgs expectedArgs = new() { Button = Buttons.DPadRight, Pressed = true };
            
            // button pressed check
            Assert.Multiple(() =>
            {
                Assert.That(actualArgs.Button, Is.EqualTo(expectedArgs.Button));
                Assert.That(actualArgs.Pressed, Is.EqualTo(expectedArgs.Pressed));
            });

            actualArgs = zeroValue.ToButtonEventArgs(true);
            expectedArgs = new() { Button = Buttons.DPadRight, Pressed = false };

            // button released check
            Assert.Multiple(() =>
            {
                Assert.That(actualArgs.Button, Is.EqualTo(expectedArgs.Button));
                Assert.That(actualArgs.Pressed, Is.EqualTo(expectedArgs.Pressed));
            });
        }

        [Test]
        public void TestConvertedNegativeShortToDpadButtonsPressedReleaseLogicForTwoButton()
        {
            short negativeValue = short.MinValue;
            short zeroValue = 0;

            MyButtonEventArgs actualArgsX = negativeValue.ToButtonEventArgs(true);
            MyButtonEventArgs expectedArgsX = new() { Button = Buttons.DPadLeft, Pressed = true };
            MyButtonEventArgs actualArgsY = negativeValue.ToButtonEventArgs(false);
            MyButtonEventArgs expectedArgsY = new() { Button = Buttons.DPadUp, Pressed = true };

            // button pressed check
            Assert.Multiple(() =>
            {
                Assert.That(actualArgsX.Button, Is.EqualTo(expectedArgsX.Button));
                Assert.That(actualArgsX.Pressed, Is.EqualTo(expectedArgsX.Pressed));
                Assert.That(actualArgsY.Button, Is.EqualTo(expectedArgsY.Button));
                Assert.That(actualArgsY.Pressed, Is.EqualTo(expectedArgsY.Pressed));
            });

            actualArgsX = zeroValue.ToButtonEventArgs(true);
            expectedArgsX = new() { Button = Buttons.DPadLeft, Pressed = false };
            actualArgsY = zeroValue.ToButtonEventArgs(false);
            expectedArgsY = new() { Button = Buttons.DPadUp, Pressed = false };

            // button released check
            Assert.Multiple(() =>
            {
                Assert.That(actualArgsX.Button, Is.EqualTo(expectedArgsX.Button));
                Assert.That(actualArgsX.Pressed, Is.EqualTo(expectedArgsX.Pressed));
                Assert.That(actualArgsY.Button, Is.EqualTo(expectedArgsY.Button));
                Assert.That(actualArgsY.Pressed, Is.EqualTo(expectedArgsY.Pressed));
            });
        }

        [Test]
        public void TestConvertedPositiveShortToDpadButtonsPressedReleaseLogicForTwoButton()
        {
            short positiveValue = short.MaxValue;
            short zeroValue = 0;

            MyButtonEventArgs actualArgsX = positiveValue.ToButtonEventArgs(true);
            MyButtonEventArgs expectedArgsX = new() { Button = Buttons.DPadRight, Pressed = true };
            MyButtonEventArgs actualArgsY = positiveValue.ToButtonEventArgs(false);
            MyButtonEventArgs expectedArgsY = new() { Button = Buttons.DPadDown, Pressed = true };

            // button pressed check
            Assert.Multiple(() =>
            {
                Assert.That(actualArgsX.Button, Is.EqualTo(expectedArgsX.Button));
                Assert.That(actualArgsX.Pressed, Is.EqualTo(expectedArgsX.Pressed));
                Assert.That(actualArgsY.Button, Is.EqualTo(expectedArgsY.Button));
                Assert.That(actualArgsY.Pressed, Is.EqualTo(expectedArgsY.Pressed));
            });

            actualArgsX = zeroValue.ToButtonEventArgs(true);
            expectedArgsX = new() { Button = Buttons.DPadRight, Pressed = false };
            actualArgsY = zeroValue.ToButtonEventArgs(false);
            expectedArgsY = new() { Button = Buttons.DPadDown, Pressed = false };

            // button released check
            Assert.Multiple(() =>
            {
                Assert.That(actualArgsX.Button, Is.EqualTo(expectedArgsX.Button));
                Assert.That(actualArgsX.Pressed, Is.EqualTo(expectedArgsX.Pressed));
                Assert.That(actualArgsY.Button, Is.EqualTo(expectedArgsY.Button));
                Assert.That(actualArgsY.Pressed, Is.EqualTo(expectedArgsY.Pressed));
            });
        }
    }
}