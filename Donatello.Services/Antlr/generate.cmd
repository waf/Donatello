
java -jar antlr-4.5.3-complete.jar ^
     -Dlanguage=CSharp ^
     -o .\Generated ^
     -package Donatello.Services.Antlr.Generated ^
     -visitor ^
     Donatello.g4
