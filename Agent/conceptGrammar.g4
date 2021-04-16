grammar conceptGrammar;

/*
 LEXER
*/
COMMA: ',';
SEMICOLON: ';';
WS: [ \t\r\n]+ -> skip;

//ACTIONS
FIGHT: 'Fight' | 'fight';
PICKUP: 'Pick up' | 'pick up';
FLEE: 'Flee' | 'flee';
REPLACE: 'Replace' | 'replace';

//CONDITIONS
WHEN: 'when';
SEEING: 'seeing';
FINDING: 'finding';

//IF, OTHERWISE
IF: 'if';
OTHERWISE: 'otherwise';

//IFCONDITIONS
MOREPOWER: 'I have more power';
WEAPONHASLOWERPOWER: 'current weapon has lower power';

//SUBJECTS
PLAYER: 'another player';
MONSTER: 'a monster';
ATTRIBUTE: 'an attribute';
WEAPON: 'a weapon';

/*
 PARSER
*/
rule: FLEE COMMA WHEN SEEING PLAYER SEMICOLON;



rules: conditionRule+;
conditionRule: action ifClause? COMMA WHEN condition subject otherwiseCondition? SEMICOLON;
condition: SEEING | FINDING;
subject: PLAYER | MONSTER | ATTRIBUTE | WEAPON;
action: FIGHT | PICKUP | FLEE | REPLACE;
ifClause: IF ifCondition;
ifCondition: MOREPOWER | WEAPONHASLOWERPOWER;
otherwiseCondition: COMMA OTHERWISE action;