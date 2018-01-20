grammar Donatello;
// based on https://github.com/antlr/grammars-v4/blob/master/clojure/Clojure.g4

file: form * EOF;

form: literal
    | list
	| def
	| function
	| defType
	| let
    | vector
    | map
    ;


list: '(' form* ')' ;
vector: '[' form* ']' ;
map: '{' (form form)* '}' ;
set: '|' form* '|' ;
functionArgs: '[' identifier* ']' ;
def: '(' 'def' identifier form ')' ;
function: '(' 'defn' identifier functionArgs form* ')' ;
binding: (identifier form);
let: '(' 'let' '[' binding* ']' form* ')' ;

propertyDeclaration: (property type);
defType: '(' 'deftype' identifier propertyDeclaration+ ')' ;


symbol: PROPERTY | METHOD | NAME | QUALIFIED_NAME | MATH;
property: PROPERTY;
type: NAME;
identifier: NAME;
PROPERTY: '-' NAME;
METHOD: '.' NAME;

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

NAME: (LETTER | '_') (LETTER | DIGIT | '_')*;
QUALIFIED_NAME: (LETTER | '_') (LETTER | DIGIT | '_' | '.')*;
MATH: '+' | '-' | '/' | '*' | '%';

fragment LETTER : LOWER | UPPER;
fragment LOWER  : 'a'..'z';
fragment UPPER  : 'A'..'Z';
fragment DIGIT  : '0'..'9';

fragment WS : [ \n\r\t,] ;
TRASH
    : WS -> channel(HIDDEN)
    ;
