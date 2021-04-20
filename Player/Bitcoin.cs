/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 2.
     
    Goal of this file: Keeping track of the bitcoin currency.

*/

namespace Player
{
    public class Bitcoin
    {
        private int _amount;

        public Bitcoin(int amount)
        {
            _amount = amount;
        }

        public int getAmount()
        {
            return _amount;
        }

        public void setAmount(int amount)
        {
            _amount = amount;
        }

        public void addAmount(int amount)
        {
            _amount += amount;
        }

        public void removeAmount(int amount)
        {
            _amount -= amount;
        }
    }
}
