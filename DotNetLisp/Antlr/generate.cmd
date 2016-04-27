
java -jar antlr-4.5.3-complete.jar ^
     -Dlanguage=CSharp ^
     -o .\Generated ^
     -package DotNetLisp.Antlr.Generated ^
     -visitor ^
     DotNetLisp.g4
