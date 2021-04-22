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
            Console.Write(direction);
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
            Console.Write(newMovement[0] + " " + newMovement[1]);
            newPosition = SendNewPosition(newMovement);

            // the next line of code should be changed by sending newPosition to a relevant method
            WriteCommand(newPosition);
        }
        public int[] SendNewPosition(int[] newMovement)
        {
            int[] newPlayerPosition = new int[2];

            // getPosition() should be replaced by another method that gets the coordinates of the player
            for (int i = 0; i <= 1; i++)
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

    public void HandleItemAction(string action)
    {
        // check tile item function;
        // als er iets ligt return Item
        // dan zou addinvetoryItem vanuit Player moeten werken met het item

        Console.Write("action: " + action);
        switch (action)
        {
            case "pickup":
              //  AddInventoryItem(CheckTileContainsItem());
                // add item to inventory
                
                break;

            case "drop"
                // nog niet bepaald of dat het hier om het huidige item gaat of iets uit inventory
                break;
        }
    }

    public Item CheckTileContainsItem()
    {
        // Als er op de Tile iets ligt moet deze het item returnen
        // anders return null;
    }

       public void HandleItemAction(string action) { Console.WriteLine("Action: " + action); }

       public void HandleAttackAction(Attack attack) { Console.WriteLine("Attack: " + attack); }

       public void HandleExitAction(Exit exit) { Console.WriteLine("Exit: " + exit); }

       public void HandlePauseAction(Pause pause) { Console.WriteLine("Pause: " + pause); }

       public void HandleReplaceAction(Replace replace) { Console.WriteLine("Replace: " + replace); }

       public void HandleResumeAction(Resume resume) { Console.WriteLine("Resume: " + resume); }

    // Test hier of de case uit de Evaluator gereached wordt.
       public void HandleSayAction(Say say) { Console.WriteLine("Say: " + say); }

       public void HandleShoutAction(Shout shout) { Console.WriteLine("Shout: " + shout); }
}