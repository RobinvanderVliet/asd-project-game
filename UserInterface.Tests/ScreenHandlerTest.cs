using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace UserInterface.Tests
{
    public class ScreenHandlerTest
    {
        [ExcludeFromCodeCoverage]
        public class StartScreenTest
        {
            private IScreenHandler _sut;
            [SetUp]
            public void Setup()
            {
                _sut = new ScreenHandler();
            }

            [Test]
            public void Test_TransitionTo_ChangesScreen()
            {

            }
        }
    }
}