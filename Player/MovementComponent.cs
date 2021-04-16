﻿/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-Project.
 
    This file is created by team: 2.
     
    Goal of this file: changing the position of the player after input.
     
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Player
{
    public class MovementComponent
    {
        public MovementComponent()
        {

        }
        public void Test()
        {
            var command = Console.ReadLine();
            HandleDirection(GetDirection(command), GetSteps(command));
        }
        // above is a temporary method, instead of calling test from the main,
        // handleDirection should be called with direction & steps from elsewhere

        public void HandleDirection(String direction, int steps)
        {
            int[] newMovement = new int[2];
            switch (direction)
            {
                case "right":
                    newMovement[0] = steps;
                    newMovement[1] = 0;
                    break;
                case "left":
                    newMovement[0] = -steps;
                    newMovement[1] = 0;
                    break;
                case "forward":
                    newMovement[0] = 0;
                    newMovement[1] = -steps;
                    break;
                case "backward":
                    newMovement[0] = 0;
                    newMovement[1] = steps;
                    break;
            }
            SendNewPosition(newMovement);
        }
        public void SendNewPosition(int[] newMovement)
        {
            int[] newPlayerPosition = new int[2];

            // getPosition() should be replaced by another method that gets the coordinates of the player
            for (int i = 0; i<=1; i++)
            {
                newPlayerPosition[i] = GetPosition()[i] + newMovement[i];
            }

            // the next line of code should be changed by sending newPosition to a relevant method
            WriteCommand(newPlayerPosition);
        }

        // !!! METHODS BELOW ARE TEMPORARY, PROTOTYPE ONLY !!!
        public String GetCommand(String command, int directionOrSteps)
        {
            // splits i.e. "right 1" into "right" and "1"
            string[] substr = command.Split(' ');
            switch (directionOrSteps)
            {
                case 0: return substr[0];
                case 1: return substr[1];
                default: return "";
            }
        }
        public String GetDirection(String command)
        {
            // gets the direction from the command
            return GetCommand(command, 0);
        }
        public int GetSteps(String command)
        {
            // gets the steps from the command
            return Int32.Parse(GetCommand(command, 1));
        }
        public int[] GetPosition()
        {
            // gets the current position of the player
            int[] positionRequestedPlayer = new int[2];
            positionRequestedPlayer[0] = 26;
            positionRequestedPlayer[1] = 11;
            return positionRequestedPlayer;
        }
        public void WriteCommand(int[] newPosition)
        {
            // returns the new position
            Console.WriteLine("X: " + newPosition[0] + ". Y: " + newPosition[1]);
        }
    }
}
