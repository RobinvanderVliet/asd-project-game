using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Agent.Tests
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