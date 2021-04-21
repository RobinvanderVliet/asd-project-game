/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 2.
     
    Goal of this file: Creating an interface for bitcoin.

*/

namespace Player
{
    public interface IBitcoin
    {
        public int _amount { get; set; }

        public void AddAmount(int amount);

        public void RemoveAmount(int amount);
    }
}
