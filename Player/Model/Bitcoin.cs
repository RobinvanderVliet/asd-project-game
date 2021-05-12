namespace Player.Model
{
    public class Bitcoin : IBitcoin
    {
        private int _amount;
        public int Amount { get => _amount; set => _amount = value; }

        public Bitcoin(int amount)
        {
            _amount = amount;
        }

        public void AddAmount(int amount)
        {
            _amount += amount;
        }

        public void RemoveAmount(int amount)
        {
            _amount -= amount;
        }
    }
}
