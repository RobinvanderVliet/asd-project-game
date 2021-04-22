namespace Player.Model
{
    public class Bitcoin : IBitcoin
    {
        public int Amount { get; set; }

        public Bitcoin(int amount)
        {
            Amount = amount;
        }

        public void AddAmount(int amount)
        {
            Amount += amount;
        }

        public void RemoveAmount(int amount)
        {
            Amount -= amount;
        }
    }
}
