namespace Player.Model
{
    public interface IBitcoin
    {
        public int Amount { get; set; }

        public void AddAmount(int amount);

        public void RemoveAmount(int amount);
    }
}
