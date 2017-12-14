java -jar antlr-4.7-complete.jar ^
    -Dlanguage=CSharp Donatello.g4 ^
    -o Generated -package Donatello.Parser.Generated ^
    -visitor -no-listener
