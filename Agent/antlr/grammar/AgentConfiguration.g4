grammar AgentConfiguration;

/*
 LEXER
*/
WS: [ \t\r\n]+ -> skip;
BRACKET_OPEN: '{';
BRACKET_CLOSE: '}';
DOUBLE_QUOTE: '"';
EQUALSIGN: '=';

//EVENTS
FINDS: 'finds';
NEARBY: 'nearby';

//CONDITIONS
WHEN: 'when';
OTHERWISE: 'otherwise';
THEN: 'then';

//COMPARISONS
GREATER_THAN : 'greater than';
LESS_THAN: 'less than';
EQUALS: 'equals';
CONTAINS: 'contains';
DOES_NOT_CONTAIN: 'does not contain';

//SUBJECTS
PLAYER: 'player';
NPC: 'npc';
INVENTORY: 'inventory';
OPPONENT: 'opponent';

//OBJECTS
POTION: 'potion';
ITEM: 'item';
WEAPON: 'weapon';
CURRENT: 'current';

//STATS
STRENGTH: 'strength';
HEALTH: 'health';
POWER: 'power';
STAMINA: 'stamina';

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

configuration: rule* settingBlock* EOF;
rule: (setting | STRING)  EQUALSIGN STRING;

settingBlock: setting BRACKET_OPEN (condition | actionBlock)+ BRACKET_CLOSE;
actionBlock: action BRACKET_OPEN condition+ BRACKET_CLOSE;
condition: whenClause | whenClause otherwiseClause; 
whenClause: WHEN comparable comparison comparable THEN (action | actionSubject);
otherwiseClause: OTHERWISE (action | actionSubject);
comparison: GREATER_THAN | LESS_THAN | EQUALS | CONTAINS | DOES_NOT_CONTAIN | NEARBY | FINDS;
setting: COMBAT | EXPLORE;
action: STRING;
actionSubject: action subject | action item;
string: DOUBLE_QUOTE STRING+ DOUBLE_QUOTE;

comparable: item | itemStat | subject | subjectStat | stat | INT;
itemStat: item stat;
subjectStat: subject stat;
subject: PLAYER #player | NPC #npc | OPPONENT #opponent | INVENTORY #inventory | CURRENT #current | STRING #tile;
item: ITEM | POTION | WEAPON | string;
stat: STRENGTH | POWER | HEALTH | STAMINA;