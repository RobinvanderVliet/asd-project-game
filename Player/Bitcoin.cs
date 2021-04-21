/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 2.
     
    Goal of this file: Keeping track of the bitcoin currency.

*/

namespace Player
{
    public class Bitcoin : IBitcoin
    {
        public int _amount { get; set; }

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
