# DotNetLisp

## Developer notes

- The grammar specification is in Antlr/DotNetLisp.g4
- Run `generate.cmd` to turn the grammar into parser code using ANTLR
- Use the generated visitor pattern to turn Abstract Syntax Tree nodes into .NET expressions
