/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 2.
     
    Goal of this file: changing the position of the player after input.
     
*/

namespace Player
{
    interface IPlayerModel
    {
        void HandleDirection(string direction, int steps);
        int[] SendNewPosition(int[] newMovement);
    }
}
