using NUnit.Framework;
using XpadControl.Interfaces.GamepadService.Dependencies;
using XpadControl.Linux.Services.Extensions;

namespace XpadControl.Linux.Services.Test
{
    public class ButtonsExtensionTests
    {
        [Test]
        public void TestConvertedResultByValues()
        {
            Assert.Multiple(() =>
            {
                //None
                Assert.That(((byte)0x0).ToButtons(), Is.EqualTo(Buttons.None));

                // D-Pad Up. This is one of the directional buttons.
                Assert.That(((byte)0x1).ToButtons(), Is.EqualTo(Buttons.DPadUp));


                // D-Pad Down. This is one of the directional buttons.
                Assert.That(((byte)0x2).ToButtons(), Is.EqualTo(Buttons.DPadDown));

                // D-Pad Left. This is one of the directional buttons.
                Assert.That(((byte)0x4).ToButtons(), Is.EqualTo(Buttons.DPadLeft));

                // D-Pad Right. This is one of the directional buttons.
                Assert.That(((byte)0x8).ToButtons(), Is.EqualTo(Buttons.DPadRight));

                // The Start button.
                Assert.That(((byte)0x10).ToButtons(), Is.EqualTo(Buttons.Start));

                // The Back button.
                Assert.That(((byte)0x20).ToButtons(), Is.EqualTo(Buttons.Back));
                //Back = 0x20,

                // The LS (Left Stick) button.
                Assert.That(((byte)0x40).ToButtons(), Is.EqualTo(Buttons.LS));

                // The RS (Right Stick) button.
                Assert.That(((byte)0x80).ToButtons(), Is.EqualTo(Buttons.RS));

                // The LB (Left Shoulder) button.
                //Assert.That(((byte)0x100).ToButtons(), Is.EqualTo(Buttons.DPadLeft));
                //LB = 0x100,

                // The RB (Right Shoulder).
                //RB = 0x200,

                // The A button.
                //A = 0x1000,

                // The B button.
                //B = 0x2000,

                // The X button.
                //X = 0x4000,

                // The Y button.
                //Y = 0x8000
            });
        }
    }
}