using Creature.Creature.StateMachine;
using Moq;
using NUnit.Framework;

namespace Creature.Tests
{
    [TestFixture]
    public class AgentTest
    {
        private Agent _sut;
        private Mock<ICreatureStateMachine> _creatureStateMachineMock;

        private string Id;
        private string Name;
        private string Symbol;
        private int XPosition;
        private int YPosition;
        
        [SetUp]
        public void Setup()
        {
            _creatureStateMachineMock = new Mock<ICreatureStateMachine>();
            Id = "01";
            Name = "AgentTest";
            Symbol = "S";
            XPosition = 10;
            YPosition = 10;
            
            _sut = new Agent(Name, XPosition, YPosition, Symbol, Id);
        }
        
    }
}