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

        void HandleItemAction(string action);
        void HandleAttackAction(Attack attack);
        void HandleExitAction(Exit exit);
        void HandlePauseAction(Pause pause);
        void HandleReplaceAction(Replace replace);
        void HandleResumeAction(Resume resume);
        void HandleSayAction(Say say);
        void HandleShoutAction(Shout shout);
    }
}