grammar DotNetLisp;

file: form *;

forms: form* ;

form: literal
    | list
    | lambda
	| vector
	| set
	| dictionary
    ;

list: '(' forms ')' ;

// collection types
dictionary: '{' form * '}';
set: '|' form * '|';
vector: '[' form * ']' ;

LAMBDA_PARAMETER: '^' [0-9]+;
lambdaParameter: LAMBDA_PARAMETER;
lambda: '\\(' forms ')';

literal
    : string
    | number
    | symbol
    | lambdaParameter
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
SYMBOL: ('A'..'Z' | 'a'..'z' | '0'..'9' | '+' | '.' | '<' | ',' | ', ' | '>' | '*')+;
TYPE: ':' ('A'..'Z' | 'a'..'z' | '<' | ',' | ', ' | '>')+; //TODO: need to tighten this up.

// Discard
//--------------------------------------------------------------------

fragment
WS : [ \n\r\t\,] ;

fragment
COMMENT: '//' ~[\r\n]* ;

TRASH
    : ( WS | COMMENT ) -> channel(HIDDEN)
    ;
