using System.Diagnostics.CodeAnalysis;
using ASD_Game.UserInterface;
using ASD_Game.World.Services;
using Moq;
using NUnit.Framework;

namespace ASD_Game.Tests.WorldTests
{
    [ExcludeFromCodeCoverage]  
    [TestFixture]
    public class WorldServiceTest
    {
        //Declaration and initialisation of constant variables
 
        //Declaration of variables
 
        //Declaration of mocks
        private WorldService _sut;
        private Mock<IScreenHandler> _mockedScreenHandler;

        [SetUp]
        public void Setup()
        {
            //Initialisation of variables
            //Initialisation of mocks
            _mockedScreenHandler = new Mock<IScreenHandler>();
            //_sut = new WorldService(_mockedScreenHandler.Object);
        }
        
        [Test]
        public void Test_GenerateSeed_GeneratesARandomSeed() 
        {
            //Arrange ---------
            //Act ---------
            //Assert ---------
        }

    }
}