grammar AgentConfiguration;

/*
 LEXER
*/
WS: [ \t\r\n]+ -> skip;
BRACKET_OPEN: '{';
BRACKET_CLOSE: '}';
DOUBLE_QUOTE: '"';
EQUALSIGN: '=';

//ACTIONS
USE: 'use';
DROP: 'drop' | 'throw';
DROPALL: 'drop all' | 'throw all';
SET: 'set';

//EVENTS
FINDS: 'finds';
NEARBY: 'nearby';

//CONDITIONS
WHEN: 'when';
OTHERWISE: 'otherwise';
THEN: 'then';

//COMPARISONS
GREATER_THAN : 'greater than';
LOWER_THAN: 'lower than';
EQUALS: 'equals';
CONTAINS: 'contains';
DOES_NOT_CONTAIN: 'does not contain';

//SUBJECTS
PLAYER: 'player';
NPC: 'npc';
INVENTORY: 'inventory';

//OBJECTS
POTION: 'potion';
ITEM: 'item';
WEAPON: 'weapon';
CURRENT: 'current';

//STATS
STRENGTH: 'strength';
HEALTH: 'health';

//SETTINGS
GENERAL: 'general';
COMBAT: 'combat';
EXPLORE: 'explore';

//INPUT
INT: [0-9]+;
STRING: [a-z0-9\-]+;

/*
 PARSER
*/

configuration: (generalBlock settingBlock+ | generalBlock | settingBlock+) EOF;
generalBlock: GENERAL BRACKET_OPEN rule+ BRACKET_CLOSE;
rule: (setting | STRING)  EQUALSIGN STRING;

settingBlock: setting BRACKET_OPEN (condition | actionBlock)+ BRACKET_CLOSE;
actionBlock: action BRACKET_OPEN condition+ BRACKET_CLOSE;
condition: whenClause | whenClause otherwiseClause; 
whenClause: WHEN (subject | itemStat) comparison (subject | INT | object | itemStat) THEN action;
otherwiseClause: OTHERWISE action;
comparison: GREATER_THAN | LOWER_THAN | EQUALS | CONTAINS | DOES_NOT_CONTAIN | NEARBY | FINDS;
subject: PLAYER #player | NPC #npc | INVENTORY #inventory | ITEM #item | CURRENT #current | HEALTH #health;
setting: COMBAT | EXPLORE;
action: STRING | useItem;
itemStat: ITEM (INT | STRENGTH);
useItem: USE object;
object: POTION | WEAPON | string;
string: DOUBLE_QUOTE STRING DOUBLE_QUOTE;