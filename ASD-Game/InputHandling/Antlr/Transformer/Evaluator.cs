using System;
using ActionHandling;
using ASD_Game.ActionHandling;
using ASD_Game.Chat;
using ASD_Game.InputHandling.Antlr.Ast;
using ASD_Game.InputHandling.Antlr.Ast.Actions;
using ASD_Game.InputHandling.Exceptions;
using ASD_Game.Network;
using ASD_Game.Network.Enum;
using ASD_Game.Session;
using ASD_Game.Session.DTO;
using ASD_Game.Session.GameConfiguration;
using InputCommandHandler.Antlr.Ast.Actions;
using ASD_Game.World.Services;
using Creature;
using Newtonsoft.Json;

using Session;
using MonsterDifficulty = ASD_Game.InputHandling.Antlr.Ast.Actions.MonsterDifficulty;

namespace ASD_Game.InputHandling.Antlr.Transformer
{
    public class Evaluator : IEvaluator
    {
        private readonly ISessionHandler _sessionHandler;
        private readonly IMoveHandler _moveHandler;
        private readonly IGameSessionHandler _gameSessionHandler;
        private readonly IChatHandler _chatHandler;
        private readonly IClientController _clientController;
        private readonly IAgentHandler _agentHandler;
        private readonly IWorldService _worldService;
        private readonly IInventoryHandler _inventoryHandler;
        private readonly IGamesSessionService _gamesSessionService;
        private readonly IAttackHandler _attackHandler;
        
        private const int MINIMUM_STEPS = 1;
        private const int MAXIMUM_STEPS = 10;
        private string _commando;
        
        public Evaluator(ISessionHandler sessionHandler, IMoveHandler moveHandler, IGameSessionHandler gameSessionHandler, IChatHandler chatHandler, IAttackHandler attackHandler, IInventoryHandler inventoryHandler, IClientController clientController, IGamesSessionService gamesSessionService, IAgentHandler agentHandler, IWorldService worldService)
        {
            _sessionHandler = sessionHandler;
            _moveHandler = moveHandler;
            _gameSessionHandler = gameSessionHandler;
            _chatHandler = chatHandler;
            _agentHandler = agentHandler;
            _worldService = worldService;
            _worldService = worldService;
            _attackHandler = attackHandler;
            _clientController = clientController;
            _gamesSessionService = gamesSessionService;
            _agentHandler = agentHandler;
            _inventoryHandler = inventoryHandler;
        }

        public void Apply(AST ast)
        {
            TransformNode(ast.Root);
        }

        private void TransformNode(ASTNode node)
        {
            var input = (Input)node;
            var nodeBody = input.Body;
            for (int i = 0; i < nodeBody.Count; i++)
                switch (nodeBody[i])
                {
                    case Attack:
                        TransformAttack((Attack)nodeBody[i]);
                        break;
                    case CreateSession:
                        TransformCreateSession((CreateSession)nodeBody[i]);
                        break;
                    case Drop:
                        TransformDrop((Drop)nodeBody[i]);
                        break;
                    case Exit:
                        TransformExit();
                        break;
                    case Move:
                        TransformMove((Move)nodeBody[i]);
                        break;
                    case Pause:
                        TransformPause();
                        break;
                    case Pickup:
                        TransformPickup((Pickup)nodeBody[i]);
                        break;
                    case Replace:
                        TransformReplace();
                        break;
                    case Resume:
                        TransformResume();
                        break;
                    case Say:
                        TransformSay((Say)nodeBody[i]);
                        break;
                    case Shout:
                        TransformShout((Shout)nodeBody[i]);
                        break;
                    case JoinSession:
                        TransformJoinSession((JoinSession)nodeBody[i]);
                        break;
                    case RequestSessions:
                        TransformRequestSessions((RequestSessions)nodeBody[i]);
                        break;
                    case StartSession:
                        TransformStartSession((StartSession)nodeBody[i]);
                        break;
                    case LoadGame:
                        TransformLoadGame((LoadGame)nodeBody[i]);
                        break;
                    case RequestSavedGames:
                        TransformRequestSavedGames((RequestSavedGames)nodeBody[i]);
                        break;
                    case MonsterDifficulty:
                        TransformMonsterDifficulty((MonsterDifficulty)nodeBody[i]);
                        break;
                    case ItemFrequency:
                        TransformItemFrequency((ItemFrequency)nodeBody[i]);
                        break;
                    case Inspect:
                        TransformInspect((Inspect)nodeBody[i]);
                        break;
                    case Look:
                        TransformLook((Look)nodeBody[i]);
                        break;
                    case Use:
                        TransformUse((Use)nodeBody[i]);
                        break;
                    case Search:
                        TransformSearch();
                        break;
                }
        }

        private void TransformUse(Use use)
        {
            _inventoryHandler.UseItem(use.Step.StepValue);
        }

        private void TransformMove(Move move)
        {
            switch (move.Steps.StepValue)
            {
                case < MINIMUM_STEPS:
                    throw new MoveException($"Too few steps, the minimum is {MINIMUM_STEPS}.");
                case > MAXIMUM_STEPS:
                    throw new MoveException($"Too many steps, the maximum is {MAXIMUM_STEPS}.");
                default:
                    _moveHandler.SendMove(move.Direction.DirectionValue, move.Steps.StepValue);
                    break;
            }
        }

        private void TransformPickup(Pickup pickup)
        {
            _inventoryHandler.PickupItem(pickup.Item.StepValue);
        }

        private void TransformDrop(Drop drop)
        {
            _inventoryHandler.DropItem(drop.InventorySlot.InventorySlotValue);
        }

        private void TransformAttack(Attack attack)
        {
            _attackHandler.SendAttack(attack.Direction.DirectionValue);
        }

        private void TransformExit()
        {
            Environment.Exit(0);
            //_screenHandler.TransitionTo(new StartScreen());
        }

        private void TransformPause()
        {
            // TODO: Remove? Must be replaced by save
        }

        private void TransformReplace()
        {
            _agentHandler.Replace(_worldService.GetCurrentPlayer().Id);
        }

        private void TransformResume()
        {
            // TODO: Remove? Must be replaced by load functionality
        }

        private void TransformSay(Say say)
        {
            _chatHandler.SendSay(say.Message.MessageValue);
        }

        private void TransformShout(Shout shout)
        {
            _chatHandler.SendShout(shout.Message.MessageValue);
        }

        private void TransformCreateSession(CreateSession createSession)
        {
            _sessionHandler.CreateSession(createSession.Message.MessageValue, createSession.Username.UsernameValue, false, null, null);
        }

        private void TransformJoinSession(JoinSession joinSession)
        {
            _sessionHandler.JoinSession(joinSession.Message.MessageValue, joinSession.Username.UsernameValue);
        }

        private void TransformRequestSessions(RequestSessions requestSessions)
        {
            _sessionHandler.RequestSessions();
        }

        private void TransformStartSession(StartSession startSession)
        {
            _gameSessionHandler.SendGameSession();
        }

        private void TransformMonsterDifficulty(MonsterDifficulty monster)
        {
            if (!_clientController.IsHost())//TODO Check GameStarted wordt toegevoegd in andere feature
            {
                return;
            }

            int difficulty = -1;
            switch (monster.Difficulty)
            {
                case "easy":
                    difficulty = (int) Session.GameConfiguration.MonsterDifficulty.Easy;
                    break;
                case "medium":
                    difficulty = (int) Session.GameConfiguration.MonsterDifficulty.Medium;
                    break;
                case "hard":
                    difficulty = (int) Session.GameConfiguration.MonsterDifficulty.Hard;
                    break;
                case "impossible":
                    difficulty = (int) Session.GameConfiguration.MonsterDifficulty.Impossible;
                    break;
            }

            SessionDTO sessionDto = new SessionDTO
            {
                SessionType = SessionType.EditMonsterDifficulty,
                Name = difficulty.ToString()
            };
            SendPayload(sessionDto);
        }

        private void TransformItemFrequency(ItemFrequency itemFrequency)
        {
            if (!_clientController.IsHost()) //TODO Check GameStarted wordt toegevoegd in andere feature
            {
                return;
            }
            
            int frequency = -1;
            switch (itemFrequency.Frequency)
            {
                case "low":
                    frequency = (int) ItemSpawnRate.Low;
                    break;
                case "medium":
                    frequency = (int) ItemSpawnRate.Medium;
                    break;
                case "high":
                    frequency = (int) ItemSpawnRate.High;
                    break;
            }
            SessionDTO sessionDto = new SessionDTO
            {
                SessionType = SessionType.EditItemSpawnRate,
                Name = frequency.ToString()
            };
            SendPayload(sessionDto);
        }

        private void SendPayload(SessionDTO sessionDto)
        {
            var jsonObject = JsonConvert.SerializeObject(sessionDto);
            _clientController.SendPayload(jsonObject, PacketType.Session);
        }
        private void TransformInspect(Inspect inspect)
        {
            string slot = inspect.InventorySlot.InventorySlotValue;
            if (slot == "armor" | slot == "weapon" | slot == "helmet" | slot == "slot 1" | slot == "slot 2" | slot == "slot 3")
            {
                _inventoryHandler.InspectItem(inspect.InventorySlot.InventorySlotValue);
            }
            else
            {
                throw new SlotException($"The slot you provided {slot} is not valid.");
            }
        }
        
        private void TransformLook(Look look)
        {
            _moveHandler.SearchNearestPlayer();
        }

        private void TransformSearch()
        {
            _inventoryHandler.Search();
        }
    

        private void TransformLoadGame(LoadGame loadGame)
        {
            _gamesSessionService.LoadGame(loadGame.Message.MessageValue);
        }

        private void TransformRequestSavedGames(RequestSavedGames requestSavedGames)
        {
            _gamesSessionService.RequestSavedGames();
        }
        
    }
}