/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 2.
     
    Goal of this file: Creating an interface for bitcoin.

*/

namespace Player
{
    interface IBitcoin
    {
        public int getAmount();

        public void setAmount(int amount);

        public void addAmount(int amount);

        public void removeAmount(int amount);
    }
}
