using HyperLiquid.Net.Objects.Models;
using NUnit.Framework;
using System.Text.Json;

namespace HyperLiquid.Net.UnitTests
{
    [TestFixture]
    public class TokenToConverterTests
    {
        [Test]
        public void DeserializeTokenToAvailableAfterMaintenance()
        {
            var json = "{\"balances\":[{\"coin\":\"USDC\",\"token\":0,\"total\":\"24.759572\",\"hold\":\"0.0\",\"entryNtl\":\"0.0\"}],\"tokenToAvailableAfterMaintenance\":[[0,\"24.759572\"]]}";

            var result = JsonSerializer.Deserialize<HyperLiquidBalances>(json, HyperLiquidExchange._serializerContext);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.TokenAvailableAfterMaintenance, Is.Not.Null);
            Assert.That(result.TokenAvailableAfterMaintenance!.Count, Is.EqualTo(1));
            Assert.That(result.TokenAvailableAfterMaintenance[0], Is.EqualTo(24.759572m));
        }

        [Test]
        public void DeserializeTokenToAvailableAfterMaintenanceMultipleEntries()
        {
            var json = "{\"balances\":[],\"tokenToAvailableAfterMaintenance\":[[0,\"870.98516812\"],[150,\"0.0\"],[197,\"0.0\"]]}";

            var result = JsonSerializer.Deserialize<HyperLiquidBalances>(json, HyperLiquidExchange._serializerContext);

            Assert.That(result!.TokenAvailableAfterMaintenance!.Count, Is.EqualTo(3));
            Assert.That(result.TokenAvailableAfterMaintenance[0], Is.EqualTo(870.98516812m));
            Assert.That(result.TokenAvailableAfterMaintenance[150], Is.EqualTo(0m));
            Assert.That(result.TokenAvailableAfterMaintenance[197], Is.EqualTo(0m));
        }

        [Test]
        public void DeserializeTokenToAvailableAfterMaintenanceEmpty()
        {
            var json = "{\"balances\":[],\"tokenToAvailableAfterMaintenance\":[]}";

            var result = JsonSerializer.Deserialize<HyperLiquidBalances>(json, HyperLiquidExchange._serializerContext);

            Assert.That(result!.TokenAvailableAfterMaintenance, Is.Empty);
        }
    }
}
