grammar PlayerCommands;

//LEXER

SPACE: ' ';
THEN: 'then';

ATTACK: 'attack';
SLASH: 'slash';
STRIKE: 'strike';
GET: 'get';
PICKUP: 'pickup';
DROP: 'drop';
EXIT: 'exit';
LEAVE: 'leave';
SAY: 'say';
SHOUT: 'shout';
WHISPER: 'whisper';

FORWARD: 'forward';
BACKWARD: 'backward';
LEFT: 'left';
RIGHT: 'right';

//PARSER

text: command (SPACE THEN SPACE command)* EOF;

command:
    (ATTACK | SLASH | STRIKE) SPACE (FORWARD | BACKWARD | LEFT | RIGHT) #attack |
    (GET | PICKUP) #get |
    DROP #drop |
    (EXIT | LEAVE) #exit;
