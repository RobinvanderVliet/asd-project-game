using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace Item.Tests
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