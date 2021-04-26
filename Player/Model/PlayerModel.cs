using System;

namespace Player.Model
{
    public class PlayerModel : IPlayerModel
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Stamina { get; set; }

        //public Tile _currentTile { get; set; }
        public IInventory Inventory { get; set; }
        public IBitcoin Bitcoins { get; set; }
        public IRadiationLevel RadiationLevel { get; set; }

        private int[] currentposition = {26, 11};

        public int[] GetNewPosition { get; private set; } = new int[2];

        //random default values for health&stamina for now
        private const int HEALTHCAP = 100;
        private const int STAMINACAP = 10;
        private const int DEFAULT_STEPS = 0;

        public PlayerModel(string name, IInventory inventory, IBitcoin bitcoins, IRadiationLevel radiationLevel //, Tile tile
                                      )
        {
            Name = name;
            Health = HEALTHCAP;
            Stamina = STAMINACAP;
            //_currentTile = tile;
            Inventory = inventory;
            //random default value for now
            Bitcoins = bitcoins;
            RadiationLevel = radiationLevel;
        }
        
        public void AddHealth(int amount)
        {
            if (Health + amount >= HEALTHCAP)
            {
                Health = HEALTHCAP;
            } else
            {
                Health += amount;
            }
        }

        public void RemoveHealth(int amount)
        {
            if (Health - amount <= 0)
            {
                Health = 0;
                //extra code for when a player dies goes here
            }
            else
            {
                Health -= amount;
            }
        }

        public void AddStamina(int amount)
        {
            if (Stamina + amount >= STAMINACAP)
            {
                Stamina = STAMINACAP;
            }
            else
            {
                Stamina += amount;
            }
        }

        public void RemoveStamina(int amount)
        {
            if (Stamina - amount <= 0)
            {
                Stamina = 0;
            }
            else
            {
                Stamina -= amount;
            }
        }

        public Item GetItem(string itemName)
        {
            return Inventory.GetItem(itemName);
        }

        public void AddInventoryItem(Item item)
        {
            Inventory.AddItem(item);
        }

        public void RemoveInventoryItem(Item item)
        {
            Inventory.RemoveItem(item);
        }

        public void EmptyInventory()
        {
            Inventory.EmptyInventory();
        }

        public void AddBitcoins(int amount)
        {
            Bitcoins.AddAmount(amount);
        }

        public void RemoveBitcoins(int amount)
        {
            Bitcoins.RemoveAmount(amount);
        }

        public int GetAttackDamage()
        {
            //random default attack damage for now
            int dmg = 5 + GetItemDamage();
            return dmg;
        }

        private int GetItemDamage()
        {
            //things like passive damage items go here
            return 0;
        }

        public void PickupItem()
        {
            //Item item = currentTile.pickupItem();
            //addInventoryItem(item);
            Console.WriteLine("Item opgepakt!");
        }

        public void DropItem(string itemName)
        {
            Item item = Inventory.GetItem(itemName);
            if (item != null)
            {
                RemoveInventoryItem(item);
                Console.WriteLine(item.ItemName + " laten vallen.");
            }
            else
            {
                Console.WriteLine("Je hebt geen " + itemName + " item in je inventory!");
            }
        }

        public void HandleDirection(string direction, int steps)
        {
            var newMovement = new int[2];
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

            GetNewPosition = SendNewPosition(newMovement);

            // the next line of code should be changed by sending newPosition to a relevant method
            WriteCommand(GetNewPosition);
        }

        public int[] SendNewPosition(int[] newMovement)
        {
            var newPlayerPosition = new int[2];

            // getPosition() should be replaced by another method that gets the coordinates of the player
            for (var i = 0; i <= 1; i++) newPlayerPosition[i] = currentposition[i] + newMovement[i];

            return newPlayerPosition;
        }

        // !!! METHODS BELOW ARE TEMPORARY, PROTOTYPE ONLY !!!
        private void WriteCommand(int[] newPosition)
        {
            // returns the new position
            currentposition = newPosition;
            Console.WriteLine("X: " + newPosition[0] + ". Y: " + newPosition[1]);
        }

        public Item CheckTileContainsItem()
        {
            // Als er op de Tile iets ligt moet deze het item returnen
            // anders return null;
            return null;
        }

        public void HandleAttackAction()
        {
            Console.WriteLine("Attack");
        }

        public void HandleExitAction()
        {
            Console.WriteLine("Exit");
        }

        public void HandlePauseAction()
        {
            Console.WriteLine("Pause");
        }

        public void HandleReplaceAction()
        {
            Console.WriteLine("Replace");
        }

        public void HandleResumeAction()
        {
            Console.WriteLine("Resume");
        }
        
        public void HandleSayAction(string sayMessage)
        {
            Console.WriteLine("player said: " + sayMessage);
        }

        public void HandleShoutAction(string shoutMessage)
        {
            Console.WriteLine("player shouted: " + shoutMessage);        
        }
    }
}
