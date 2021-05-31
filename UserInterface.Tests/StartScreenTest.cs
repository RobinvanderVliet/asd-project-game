using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace UserInterface.Tests
{
    [ExcludeFromCodeCoverage]
    public class StartScreenTest
    {
        private StartScreen _sut;
        [SetUp]
        public void Setup()
        {
            _sut = new StartScreen();
        }
    }
}