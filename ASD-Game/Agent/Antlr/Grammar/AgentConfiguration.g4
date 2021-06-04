grammar AgentConfiguration;

/*
 LEXER
*/
WS: [ \t\r\n]+ -> skip;
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
AGENT: 'agent';

//OBJECTS
CURRENT: 'current';

//STATS
HEALTH: 'health';
ARMOR: 'armor';
RADIATION: 'radiation';
STAMINA: 'stamina';
HP: 'HP';
AP: 'AP';
RPP: 'RPP';
SP: 'SP';

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

settingBlock: setting (condition | actionBlock)+;
actionBlock: action condition+;
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
subject: PLAYER #player | NPC #npc | AGENT #agent | INVENTORY #inventory | CURRENT #current | STRING #tile;
item: string;
stat: HEALTH | ARMOR | RADIATION | STAMINA | HP | AP | RPP | SP;