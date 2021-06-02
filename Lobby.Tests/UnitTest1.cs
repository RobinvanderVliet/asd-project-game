using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Lobby.Tests
{
    [ExcludeFromCodeCoverage]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}