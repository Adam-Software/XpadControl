using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies.JsonModel;
using XpadControl.JsonModel;
using Vector = XpadControl.JsonModel.Vector;

namespace XpadControl.Interfaces.Tests.WebSocketClientService
{
    public class JsonModelSerializeTests
    {
        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(0.5f, 0.2f, 0.1f)]
        [TestCase(-0.5f, -0.2f, -0.1f)]
        public void TestSerializeToVector(float x, float y, float z)
        {
            // CultureInfo.InvariantCulture for dot in result
            string expectedJson = "{\"move\":{\"x\":" + 
                $"{x.ToString(CultureInfo.InvariantCulture)}" + 
                ",\"y\":"+ $"{y.ToString(CultureInfo.InvariantCulture)}" + 
                ",\"z\":" + $"{z.ToString(CultureInfo.InvariantCulture)}" + 
                "}}";

            Vector vector = new()
            {
                Move = new VectorItem
                {
                    X = x,
                    Y = y,
                    Z = z
                }
            };

            string json = JsonSerializer.Serialize(vector);
            Assert.That(json, Is.EqualTo(expectedJson));
        }

        [Test]
        public void TestSerializeServoCommands()
        {
            string expectedJson = "{\"motors\":[{\"name\":\"head\",\"goal_position\":100}]}";

            ServoCommands servoCommands = new()
            {
                Motors = new List<ServoCommandsItem>
                {
                    new ServoCommandsItem {  Name = "head", GoalPosition = 100 }
                }
            };

            string json = JsonSerializer.Serialize(servoCommands);
            Assert.That(json, Is.EqualTo(expectedJson));
        }

        

       
    }
}
