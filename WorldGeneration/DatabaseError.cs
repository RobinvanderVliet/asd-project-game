/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD project.
 
    This file is created by team: 3.
     
    Goal of this file: Special error to make it clear what has gone wrong.
     
*/

#nullable enable
using System;

namespace WorldGeneration
{
    public class DatabaseError : Exception
    {
        public DatabaseError(string? message) : base(message)
        {
        }
    }
}