using System.Diagnostics.CodeAnalysis;
using InputHandling.Antlr;
using Moq;
using NUnit.Framework;
using Session;
using UserInterface;

namespace InputHandling.Tests
{
    [ExcludeFromCodeCoverage]
    public class InputHandlerTest
    {
        IInputHandler _sut;
        private Mock<IPipeline> _mockedPipeline;
        private Mock<ISessionHandler> _mockedSessionHandler;
        private Mock<IScreenHandler> _mockedScreenHandler;
        
        [SetUp]
        public void Setup()
        {
            _mockedPipeline = new Mock<IPipeline>();
            _mockedScreenHandler = new Mock<IScreenHandler>();
            _mockedScreenHandler = new Mock<IScreenHandler>();
            _sut = new InputHandler();
        }
    }
}