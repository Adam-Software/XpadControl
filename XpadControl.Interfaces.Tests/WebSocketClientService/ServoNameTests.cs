using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies;

namespace XpadControl.Interfaces.Tests.WebSocketClientService
{
    public class ServoNameTests
    {
        IList<Servo>? mServos;

        [SetUp]
        public async Task SetUp() 
        {
            using HttpClient client = new();
            string expectedJson = await client.GetStringAsync("https://raw.githubusercontent.com/Adam-Software/Adam-SDK/main/examples/servo_range.config");
            mServos = JsonSerializer.Deserialize<List<Servo>>(expectedJson);
        }

        [Test]
        public void TestGitHubConfigWithServoNamesWithEnumByName()
        {
            string expectedNames = JsonSerializer.Serialize(mServos?.Select(x => x.Name));
            string actualNames = JsonSerializer.Serialize(Enum.GetValues(typeof(ServoNames)));

            Assert.That(actual: actualNames, Is.EqualTo(expected: expectedNames));
        }

        [Test]
        public void TestGitHubConfigWithServoNamesWithEnumById()
        {
            string expectedIds = JsonSerializer.Serialize(mServos?.Select(x => x?.Joint?.Id));
            string actualIds = JsonSerializer.Serialize(Enum.GetValues(typeof(ServoNames)).Cast<int>());
         
            Assert.That(actual: actualIds, Is.EqualTo(expected: expectedIds));
        }
    }

    public class Servo
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("joint")]
        public Joint? Joint { get; set; }
    }

    public class Joint
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
