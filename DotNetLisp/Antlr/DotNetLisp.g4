grammar DotNetLisp;

file: form *;

forms: form* ;

form: literal
    | list
	| vector
    ;

list: '(' forms ')' ;

vector: '[' form * ']' ;

literal
    : string
    | number
    | symbol
	| type
    ;

string: STRING;
number: LONG;
symbol: SYMBOL;
type: TYPE;

// Lexers
//--------------------------------------------------------------------

STRING: '"' ( ~'"' | '\\' '"' )* '"' ;
LONG: '-'? [0-9]+[lL]?;
SYMBOL: ('A'..'Z' | 'a'..'z' | '+' | '.')+;
TYPE: ':' ('A'..'Z' | 'a'..'z' | '<' | '>')+;

// Discard
//--------------------------------------------------------------------

fragment
WS : [ \n\r\t\,] ;

fragment
COMMENT: ';' ~[\r\n]* ;

TRASH
    : ( WS | COMMENT ) -> channel(HIDDEN)
    ;
