using System;

namespace Player
{
    public class PlayerModel : IPlayerModel
    {

        private int[] newPosition = new int[2];
        private int[] currentposition = { 26, 11 };
        //line above is temporary and shows the new position of a player,
        //assuming it will be defined like that elsewhere. for the sake of NUnit

        private const int DEFAULT_STEPS = 0;

        public PlayerModel()
        {

        }

        public void HandleDirection(String direction, int steps)
        {
            int[] newMovement = new int[2];
            switch (direction)
            {
                case "right":
                case "east":
                    newMovement[0] = steps;
                    newMovement[1] = DEFAULT_STEPS;
                    break;
                case "left":
                case "west":
                    newMovement[0] = -steps;
                    newMovement[1] = DEFAULT_STEPS;
                    break;
                case "forward":
                case "up":
                case "north":
                    newMovement[0] = DEFAULT_STEPS;
                    newMovement[1] = -steps;
                    break;
                case "backward":
                case "down":
                case "south": 
                    newMovement[0] = DEFAULT_STEPS;
                    newMovement[1] = steps;
                    break;
            }
            newPosition = SendNewPosition(newMovement);

            // the next line of code should be changed by sending newPosition to a relevant method
            WriteCommand(newPosition);
        }
        public int[] SendNewPosition(int[] newMovement)
        {
            int[] newPlayerPosition = new int[2];

            // getPosition() should be replaced by another method that gets the coordinates of the player
            for (int i = 0; i<=1; i++)
            {
                newPlayerPosition[i] = currentposition[i] + newMovement[i];
            }

            return newPlayerPosition;
        }

        // !!! METHODS BELOW ARE TEMPORARY, PROTOTYPE ONLY !!!
        private void WriteCommand(int[] newPosition)
        {
            // returns the new position
            currentposition = newPosition;
            Console.WriteLine("X: " + newPosition[0] + ". Y: " + newPosition[1]);
        }
        public int[] GetNewPosition
        {
            get { return newPosition; }
        }
    }
}
