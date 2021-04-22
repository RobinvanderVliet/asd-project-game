/* Project name: ASD - Project.

    This file is created by team: 2.

    Goal of this file: changing the position of the player after input.
*/

namespace Player
{
    public interface IPlayerModel
    {
        void HandleDirection(string direction, int steps);
        int[] SendNewPosition(int[] newMovement);

        // ATTACK: 'attack';
        // SLASH: 'slash';
        // STRIKE: 'strike';
        // void HandleAttack();

        // PICKUP: 'pickup';
        // GET: 'get';
        // DROP: 'drop';
        void HandleItemAction(string action);

        // EXIT: 'exit';
        // LEAVE: 'leave';
        // PAUSE : 'pause';
        // RESUME : 'resume';
        // void HandleGameAction();

        // SAY: 'say';
        // SHOUT: 'shout';
        // REPLACE: 'replace';
        // void HandleChatAction();
    }
}