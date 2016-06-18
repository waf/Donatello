# Donatello

Donatello is a statically-typed, lisp-like language for the .NET platform. You get the power of the .NET platform, with a nice clean lisp syntax.

At this point, Donatello is just an experimental syntax transformation to [Roslyn syntax trees](https://github.com/dotnet/roslyn), rather than a full-blown, production-ready lisp implementation. This will probably upset people, and it should be considered pre-alpha.

## Developer notes

- The grammar specification is in Antlr/Donatello.g4
- Run `generate.cmd` to turn the grammar into parser code using ANTLR
- Use the generated visitor pattern to turn abstract syntax tree nodes into Roslyn syntax trees
