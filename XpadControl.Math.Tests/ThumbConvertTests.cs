using NUnit.Framework;
using System;

namespace XpadControl.Math.Tests
{
    public class ThumbConvertTests
    {
        [Test]
        public void Test1()
        {
            Console.WriteLine(ConvertThumbToFloat(100));
            Console.WriteLine(ConvertThumbToFloat(-100));

            Console.WriteLine(ConvertThumbToFloat(32767));
            Console.WriteLine(ConvertThumbToFloat(-32767));

            Console.WriteLine(ConvertThumbToFloat(32768));
            Console.WriteLine(ConvertThumbToFloat(-32768));
        }

        private static float ConvertThumbToFloat(short axis)
        {
            return ((float)axis) / (axis >= 0 ? 32767 : 32768);
        }
    }
}