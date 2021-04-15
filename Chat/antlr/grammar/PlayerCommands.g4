/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team 2: Danny, Fedor, Luke, Martijn, Robin, Robin, Sven.
     
    Goal of this file: Grammer rules player actions.
     
*/
grammar PlayerCommands;

//LEXER

SPACE: ' '+;

MOVE: 'move';
WALK: 'walk';
GO: 'go';

FORWARD: 'forward';
UP: 'up';
NORTH: 'north';
BACKWARD: 'backward';
DOWN: 'down';
SOUTH: 'south';
LEFT: 'left';
WEST: 'west';
RIGHT: 'right';
EAST: 'east';

NUMBER: [1-9] | '10';

//PARSER

input: command EOF;

step: NUMBER;

command:
    (MOVE | WALK | GO) SPACE direction (SPACE step)? #move;

forward: FORWARD | UP | NORTH;
backward: BACKWARD | DOWN | SOUTH;
left: LEFT | WEST;
right: RIGHT  | EAST;
direction: forward | backward | left | right;

