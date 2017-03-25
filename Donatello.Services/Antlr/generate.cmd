
java -jar antlr-4.6-complete.jar ^
     -Dlanguage=CSharp ^
     -o .\Generated ^
     -package Donatello.Services.Antlr.Generated ^
     -visitor ^
     Donatello.g4
