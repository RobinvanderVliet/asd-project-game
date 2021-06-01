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
CREATE_SESSION : 'create_session';
JOIN_SESSION : 'join_session';
REQUEST_SESSIONS : 'request_sessions';
START_SESSION : 'start_session';
SET_MONSTER_DIFFICULTY : 'monster_difficulty';
SET_ITEM_FREQUENCY : 'item_frequency';

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
EASY: 'easy';
MEDIUM: 'medium';
HARD: 'hard';
IMPOSSIBLE: 'impossible';
LOW : 'low';
HIGH : 'high';


NUMBER: '0' | [0-9]*;
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
    RESUME #resume |
    CREATE_SESSION SPACE message #createSession |
    JOIN_SESSION SPACE message #joinSession |
    REQUEST_SESSIONS #requestSessions |
    START_SESSION #startSession |
    SET_MONSTER_DIFFICULTY SPACE (EASY | MEDIUM | HARD | IMPOSSIBLE) #monsterdifficulty | 
    SET_ITEM_FREQUENCY SPACE (LOW | MEDIUM | HIGH) #itemfrequency;

forward: FORWARD | UP | NORTH;
backward: BACKWARD | DOWN | SOUTH;
left: LEFT | WEST;
right: RIGHT  | EAST;
direction: forward | backward | left | right;
