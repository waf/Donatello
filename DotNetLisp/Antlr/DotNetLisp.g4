grammar DotNetLisp;

file: form *;

form: literal
    | list
    ;

forms: form* ;

list: '(' forms ')' ;

literal
    : string
    | number
    | symbol
    ;

string: STRING;
number: LONG;
symbol: SYMBOL;

// Lexers
//--------------------------------------------------------------------

STRING: '"' ( ~'"' | '\\' '"' )* '"' ;
LONG: '-'? [0-9]+[lL]?;
SYMBOL: ('A'..'Z' | 'a'..'z' | '+')+;
    

// Discard
//--------------------------------------------------------------------

fragment
WS : [ \n\r\t\,] ;

fragment
COMMENT: ';' ~[\r\n]* ;

TRASH
    : ( WS | COMMENT ) -> channel(HIDDEN)
    ;
