using System.Diagnostics.CodeAnalysis;
using ActionHandling;
using ASD_Game.ActionHandling;
using ASD_Game.Chat;
using ASD_Game.InputHandling.Antlr.Ast;
using ASD_Game.InputHandling.Antlr.Ast.Actions;
using ASD_Game.InputHandling.Antlr.Transformer;
using ASD_Game.InputHandling.Exceptions;
using ASD_Game.Network;
using ASD_Game.Network.Enum;
using ASD_Game.Session;
using ASD_Game.Session.DTO;
using ASD_Game.Session.GameConfiguration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ItemFrequency = ASD_Game.InputHandling.Antlr.Ast.Actions.ItemFrequency;
using MonsterDifficulty = ASD_Game.InputHandling.Antlr.Ast.Actions.MonsterDifficulty;

namespace ASD_Game.Tests.InputHandlingTests
{
    [ExcludeFromCodeCoverage]
    public class EvaluatorTests
    {
        private Evaluator _sut;
        private Mock<ISessionHandler> _mockedSessionHandler;
        private Mock<IMoveHandler> _mockedMoveHandler;
        private Mock<IGameSessionHandler> _mockedGameSessionHandler;
        private Mock<IChatHandler> _mockedChatHandler;
        private Mock<IAttackHandler> _mockedAttackHandler;
    
        private Mock<IClientController> _mockedClientController;
    
        private Mock<IInventoryHandler> _mockedInventoryHandler;

        [SetUp]
        public void Setup()
        {
            _mockedSessionHandler = new Mock<ISessionHandler>();
            _mockedMoveHandler = new Mock<IMoveHandler>();
            _mockedGameSessionHandler = new Mock<IGameSessionHandler>();
            _mockedChatHandler = new Mock<IChatHandler>();
            _mockedAttackHandler = new Mock<IAttackHandler>();
            _mockedClientController = new Mock<IClientController>();
            _mockedInventoryHandler = new Mock<IInventoryHandler>();
            _sut = new Evaluator(_mockedSessionHandler.Object, _mockedMoveHandler.Object, _mockedGameSessionHandler.Object, _mockedChatHandler.Object, _mockedAttackHandler.Object, _mockedInventoryHandler.Object, _mockedClientController.Object);
        }
    
        [Test]
        public void Test_HandleDirection_RunsCorrectly()
        {
            //arrange
            var ast = MoveAST(1, "up");
        
            _mockedMoveHandler.Setup(x => x.SendMove("up", 1));
            //act
            _sut.Apply(ast);
            //assert
            _mockedMoveHandler.VerifyAll();
        }
        
        [Test]
        public void Test_HandleDirection_ThrowsExceptionWithStepsLessThan1()
        {
            //arrange
            var ast = MoveAST(0, "up");
            //act & assert
            Assert.Throws<MoveException>(() => _sut.Apply(ast));
        }
    
        [Test]
        public void Test_HandleDirection_ThrowsExceptionWithStepsMoreThan10()
        {
            //arrange
            var ast = MoveAST(11, "up");
            //act & assert
            Assert.Throws<MoveException>(() => _sut.Apply(ast));
        }
    
        public static AST MoveAST(int steps, string direction)
        {
            Input move = new Input();
            move.AddChild(new Move()
                .AddChild(new Direction(direction))
                .AddChild(new Step(steps)));
            return new AST(move);
        }

        [Test]
        public void Test_Apply_HandleSayActionIsCalled()
        {
            //arrange
            var ast = SayAST("test");
            _mockedChatHandler.Setup(mockedChatHandler => mockedChatHandler.SendSay("test"));
            //act
            _sut.Apply(ast);
            //assert
            _mockedChatHandler.Verify(mockedChatHandler => mockedChatHandler.SendSay("test"), Times.Once);
        }
        
        public static AST SayAST(string message)
        {
            Input say = new Input();
            say.AddChild(new Say()
                .AddChild(new Message(message)));
            return new AST(say);
        }
    
        [Test]
        public void Test_Apply_HandleShoutActionIsCalled()
        {
            //arrange
            var ast = ShoutAST("test");
            _mockedChatHandler.Setup(mockedChatHandler => mockedChatHandler.SendShout("test"));
            //act
            _sut.Apply(ast);
            //assert
            _mockedChatHandler.Verify(mockedChatHandler => mockedChatHandler.SendShout("test"), Times.Once);
        }
    
        public static AST ShoutAST(string message)
        {
            Input say = new Input();
            say.AddChild(new Shout()
                .AddChild(new Message(message)));
            return new AST(say);
        }
        
        [Test]
        public void Test_Apply_HandleRequestSessionsActionIsCalled()
        {
            // Arrange
            var ast = RequestSessionsAst();
        
            // Act
            _sut.Apply(ast);
        
            // Assert
            _mockedSessionHandler.Verify(mockedSession => mockedSession.RequestSessions(), Times.Once);
        }
    
        private static AST SearchAst()
        {
            Input search = new Input();
            search.AddChild(new Search());
            return new AST(search);
        }

        [Test]
        public void Test_Apply_SearchActionIsCalled()
        {
            // Arrange
            var ast = SearchAst();

            // Act
            _sut.Apply(ast);

            // Assert
            _mockedInventoryHandler.Verify(mock => mock.Search(), Times.Once);
        }

        private static AST RequestSessionsAst()
        {
            Input requestSessions = new Input();
            requestSessions.AddChild(new RequestSessions());
            return new AST(requestSessions);
        }
    
        [Test]
        public void Test_Apply_HandleCreateSessionActionIsCalled()
        {
            // Arrange
            const string sessionName = "cool world";
            string hostName = "gerrit";
            var ast = CreateSessionAst(sessionName, hostName);
        
            // Act
            _sut.Apply(ast);
        
            // Assert
            _mockedSessionHandler.Verify(mockedSession => mockedSession.CreateSession(sessionName, hostName), Times.Once);
        }
    
        private static AST CreateSessionAst(string sessionName, string hostName)
        {
            Input createSession = new Input();
            createSession.AddChild(new CreateSession()
                .AddChild(new Message(sessionName)).AddChild(new Username(hostName)));
            return new AST(createSession);
        }
    
        [Test]
        public void Test_Apply_HandleJoinSessionActionIsCalled()
        {
            // Arrange
            const string sessionId = "1234-1234";
            var ast = JoinSessionAst(sessionId, "");
        
            // Act
            _sut.Apply(ast);
        
            // Assert
            _mockedSessionHandler.Verify(mockedSession => mockedSession.JoinSession(sessionId, ""), Times.Once);
        }
    
        private static AST JoinSessionAst(string sessionId, string username)
        {
            Input joinSession = new Input();
            joinSession.AddChild(new JoinSession()
                .AddChild(new Message(sessionId))
                .AddChild(new Username(username)));
            return new AST(joinSession);
        }
        
        [Test]
        public void Test_Apply_HandleStartSessionActionIsCalled()
        {
            // Arrange
            var ast = StartSessionAst();
        
            // Act
            _sut.Apply(ast);
        
            // Assert
            _mockedGameSessionHandler.Verify(mockedSession => mockedSession.SendGameSession(),Times.Once);
        }
    
        private static AST StartSessionAst()
        {
            Input startSession = new Input();
            startSession.AddChild(new StartSession());
            return new AST(startSession);
        }
        
        [Test]
        public void Test_Apply_InspectItemActionIsCalled()
        {
            //Arrange
            string inventorySlot = "armor";
            var ast = InspectAST(inventorySlot);
            
            //Act
            _sut.Apply(ast);
            
            //Assert
            _mockedInventoryHandler.Verify(mockedInventory => mockedInventory.InspectItem(inventorySlot), Times.Once);
        }
        
        [Test]
        public void Test_Inspect_ThrowsExceptionWithSlotDigit42()
        {
            //arrange
            var ast = InspectAST("slot 42");
            //act & assert
            Assert.Throws<SlotException>(() => _sut.Apply(ast));
        }
        
        [TestCase("easy")]
        [TestCase("medium")]
        [TestCase("hard")]
        [TestCase("impossible")]
        [Test]
        public void Test_Apply_HandleMonsterDifficultyHost(string difficulty)
        {
            // Arrange
            var ast = MonsterDifficultyAst(difficulty);
            _mockedClientController.Setup(x => x.IsHost()).Returns(true);
            _mockedClientController.Setup(x => x.SendPayload(It.IsAny<string>(), It.IsAny<PacketType>()));
        
            // Act
            _sut.Apply(ast);
        
            // Assert
            SessionDTO sessionDto = new SessionDTO
            {
                SessionType = SessionType.EditMonsterDifficulty,
                Name = GetDifficulty(difficulty).ToString()
            };
            var jsonObject = JsonConvert.SerializeObject(sessionDto);
            _mockedClientController.Verify(mock => mock.IsHost(), Times.Once);
            _mockedClientController.Verify(mock => mock.SendPayload(jsonObject, PacketType.Session), Times.Once);
        }
        
        [TestCase("easy")]
        [TestCase("medium")]
        [TestCase("hard")]
        [TestCase("impossible")]
        [Test]
        public void Test_Apply_HandleMonsterDifficultyNotHost(string difficulty)
        {
            // Arrange
            var ast = MonsterDifficultyAst(difficulty);
            _mockedClientController.Setup(x => x.IsHost()).Returns(false);
        
            // Act
            _sut.Apply(ast);
        
            // Assert
            _mockedClientController.Verify(mock => mock.IsHost(), Times.Once);
            _mockedClientController.Verify(mock => mock.SendPayload(It.IsAny<string>(), It.IsAny<PacketType>()), Times.Never);
        }
    
        private static AST MonsterDifficultyAst(string difficulty)
        {
            Input monster = new Input();
            monster.AddChild(new MonsterDifficulty(difficulty));
            return new AST(monster);
        }

        private static int GetDifficulty(string difficulty) => difficulty switch
        {
            "easy" => (int)Session.GameConfiguration.MonsterDifficulty.Easy,
            "medium" => (int)Session.GameConfiguration.MonsterDifficulty.Medium,
            "hard" => (int)Session.GameConfiguration.MonsterDifficulty.Hard,
            _ => (int)Session.GameConfiguration.MonsterDifficulty.Impossible
        };
        
        [TestCase("low")]
        [TestCase("medium")]
        [TestCase("high")]
        [Test]
        public void Test_Apply_HandleItemFrequencyHost(string frequency)
        {
            // Arrange
            var ast = ItemFrequencyAst(frequency);
            _mockedClientController.Setup(x => x.IsHost()).Returns(true);
        
            // Act
            _sut.Apply(ast);
        
            // Assert
            SessionDTO sessionDto = new SessionDTO
            {
                SessionType = SessionType.EditItemSpawnRate,
                Name = GetFrequency(frequency).ToString()
            };
            var jsonObject = JsonConvert.SerializeObject(sessionDto);
            _mockedClientController.Verify(mock => mock.IsHost(), Times.Once);
            _mockedClientController.Verify(mock => mock.SendPayload(jsonObject, PacketType.Session), Times.Once);
        }
        
        [TestCase("low")]
        [TestCase("medium")]
        [TestCase("high")]
        [Test]
        public void Test_Apply_HandleItemFrequencyNotHost(string frequency)
        {
            // Arrange
            var ast = ItemFrequencyAst(frequency);
            _mockedClientController.Setup(x => x.IsHost()).Returns(false);
        
            // Act
            _sut.Apply(ast);
        
            // Assert
            _mockedClientController.Verify(mock => mock.IsHost(), Times.Once);
            _mockedClientController.Verify(mock => mock.SendPayload(It.IsAny<string>(), It.IsAny<PacketType>()), Times.Never);
        }
    
        private static AST ItemFrequencyAst(string frequency)
        {
            Input monster = new Input();
            monster.AddChild(new ItemFrequency(frequency));
            return new AST(monster);
        }

        private static int GetFrequency(string frequency) => frequency switch
        {
            "low" => (int)ItemSpawnRate.Low,
            "medium" => (int)ItemSpawnRate.Medium,
            _ => (int)ItemSpawnRate.High
        };

        public static AST InspectAST(string inventorySlot)
        {
            Input inspect = new Input();
            inspect.AddChild(new Inspect()
                .AddChild(new InventorySlot(inventorySlot)));
            return new AST(inspect);
        }
    }
}