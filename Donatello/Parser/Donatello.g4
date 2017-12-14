grammar Donatello;
// based on https://github.com/antlr/grammars-v4/blob/master/clojure/Clojure.g4

file: form * EOF;

form: literal
    | list
	| def
	| function
	| let
    | vector
    | map
    ;

list: '(' form* ')' ;
vector: '[' form* ']' ;
map: '{' (form form)* '}' ;
set: '|' form* '|' ;
functionArgs: '[' symbol* ']' ;
def: '(' 'def' name=symbol form ')' ;
function: '(' 'defn' name=symbol functionArgs form* ')' ;
let: '(' 'let' '[' (symbol form)* ']' form* ')' ;

literal
    : string
    | boolean
    | number
    | symbol
	;

string: STRING;
boolean: BOOLEAN;

STRING
	: '"' ( ~'"' | '\\' '"' )* '"'
	;

number
	: FLOAT
	| LONG
	;

FLOAT
    : '-'? [0-9]+ FLOAT_DECIMAL
    ;

fragment
FLOAT_DECIMAL
    : '.' [0-9]+
    ;

LONG
	: '-'? [0-9]+
	;

BOOLEAN
	: 'true'
	| 'false'
	;

symbol: NAME | MATH;

MATH: '+' | '-' | '/' | '*' | '%';

NAME: LETTER (LETTER | DIGIT | '_')*;

fragment LETTER : LOWER | UPPER;
fragment LOWER  : 'a'..'z';
fragment UPPER  : 'A'..'Z';
fragment DIGIT  : '0'..'9';

fragment WS : [ \n\r\t,] ;
TRASH
    : WS -> channel(HIDDEN)
    ;
