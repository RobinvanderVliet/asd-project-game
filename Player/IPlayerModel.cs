using System;

namespace Player
{
    interface IPlayerModel
    {
        void HandleDirection(string direction, int steps);
        int[] SendNewPosition(int[] newMovement);
    }
}
