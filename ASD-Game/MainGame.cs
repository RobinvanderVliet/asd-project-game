using System;

using DatabaseHandler.Poco;
using DatabaseHandler.Repository;
using InputCommandHandler;
using Player.Model;
using Player.Services;
using System.Collections.Generic;
using System.Numerics;
using Agent.Mapper;
using Agent.Models;
using Agent.Services;
using WorldGeneration;
using Chat;
using Creature;
using Creature.Creature.StateMachine;
using Creature.Creature.StateMachine.CustomRuleSet;
using Creature.Creature.StateMachine.Data;
using DataTransfer.DTO.Character;
using Network;
using Session;
using Player.ActionHandlers;
using Creature.Services;
using Player = Creature.Player;
using InputHandling;
using UserInterface;


namespace ASD_project
{
    partial class Program
    {
        public class MainGame : IMainGame
        {
            private const bool DEBUG_INTERFACE = true; //TODO: remove when UI is complete, obviously
            private IInputHandler _inputHandler;
            private IScreenHandler _screenHandler;

            public MainGame(IInputHandler inputHandler, IScreenHandler screenHandler)
            {
                _screenHandler = screenHandler;
                _inputHandler = inputHandler;
            }

            public void Run()
            {
                if (!DEBUG_INTERFACE)
                {
                    _screenHandler.TransitionTo(new StartScreen());
                    while (true)
                    {
                        var currentScreen = _screenHandler.Screen;

                        if (currentScreen is StartScreen)
                        {
                            _inputHandler.HandleStartScreenCommands();
                        }
                        else if (currentScreen is SessionScreen)
                        {
                            _inputHandler.HandleSessionScreenCommands();
                        }
                        else if (currentScreen is ConfigurationScreen)
                        {
                        }
                        else if (currentScreen is WaitingScreen)
                        {
                        }
                        else if (currentScreen is GameScreen)
                        {
                            _inputHandler.HandleGameScreenCommands();
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        Console.WriteLine("Type input messages below");
                        // TODO: fix this
                        inputHandler.HandleCommands(playerService, sessionService, agentService);
                        _inputHandler.HandleGameScreenCommands();
                    }
                }
            }
        }
    }
}