using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using XpadControl.Interfaces.Tests.WebSocketClientService.JsonModel;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies;

namespace XpadControl.Interfaces.Tests.WebSocketClientService
{
    public class ServoNameTests
    {
        private const string cAdamSdkBranch = "devel";
        private const string cAdamSdkUrl = $"https://raw.githubusercontent.com/Adam-Software/Adam-SDK/{cAdamSdkBranch}/examples/servo_range.config";
        private IList<Servo>? mServos;

        [SetUp]
        public async Task SetUp() 
        {
            using HttpClient client = new();
            string expectedJson = await client.GetStringAsync(cAdamSdkUrl);
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
}
