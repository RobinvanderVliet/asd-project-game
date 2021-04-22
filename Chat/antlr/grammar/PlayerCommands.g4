/*
    AIM SD ASD 2020/2021 S2 project
     
    Project name: ASD-project-game.
 
    This file is created by team: 2
     
    Goal of this file: Grammer rules player actions.
     
*/
grammar PlayerCommands;

//LEXER

SPACE: ' '+;

MOVE: 'move';
WALK: 'walk';
GO: 'go';
ATTACK: 'attack';
SLASH: 'slash';
STRIKE: 'strike';
PICKUP: 'pickup';
GET: 'get';
DROP: 'drop';
EXIT: 'exit';
LEAVE: 'leave';
SAY: 'say';
SHOUT: 'shout';
REPLACE: 'replace';
PAUSE : 'pause';
RESUME : 'resume';

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

NUMBER: '0' | [1-9] [0-9]*;
MESSAGE: '"' ~'"'+ '"';
//MESSAGE: ~[\r\n]+;

//PARSER

input: command EOF;

step: NUMBER;
message: MESSAGE;

command:
    (MOVE | WALK | GO) SPACE direction (SPACE step)? #move |
    (ATTACK | SLASH | STRIKE) SPACE direction #attack |
    (PICKUP | GET) #pickup |
    DROP #drop |  
    (EXIT | LEAVE) #exit |
    SAY SPACE message #say |
    SHOUT SPACE message #shout |
    REPLACE #replace |
    PAUSE #pause |
    RESUME #resume;


forward: FORWARD | UP | NORTH;
backward: BACKWARD | DOWN | SOUTH;
left: LEFT | WEST;
right: RIGHT  | EAST;
direction: forward | backward | left | right;
