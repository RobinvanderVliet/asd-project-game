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

text: command EOF;

command:
    (MOVE | WALK | GO) SPACE direction (SPACE NUMBER)? #move;

forward: FORWARD | UP | NORTH;
backward: BACKWARD | DOWN | SOUTH;
left: LEFT | WEST;
right: RIGHT  | EAST;
direction: forward | backward | left | right;

