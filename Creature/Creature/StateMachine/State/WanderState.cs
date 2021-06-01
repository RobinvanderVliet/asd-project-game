using System;
using ActionHandling;
using Creature.Creature.StateMachine.Data;
using Network;
using WorldGeneration;

namespace Creature.Creature.StateMachine.State
{
    public class WanderState : CreatureState
    {
        private MoveHandler _moveHandler = new MoveHandler(new ClientController(new NetworkComponent()), new WorldService());
        
        public override void Do()
        {
            int steps = new Random().Next(10);
            _moveHandler.SendMove(pickRandomDirection(), steps);
        }
        
        private string pickRandomDirection()
        {
            String _direction = "";
            int CaseSwitch = new Random().Next(1, 4);
            switch (CaseSwitch)
            {
                case 1:
                    _direction += "up";
                    break;
                case 2:
                    _direction += "right";
                    break;
                case 3:
                    _direction += "down";
                    break;
                case 4:
                    _direction += "left";
                    break;
            }
            return _direction;
        }
    }
}