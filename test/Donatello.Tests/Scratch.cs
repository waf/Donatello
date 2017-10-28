using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Donatello.Services.Antlr.Generated;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using static Donatello.Services.Antlr.Generated.DonatelloParser;

namespace Donatello.Tests
{
    //public class Scratch
    //{
    //    [Fact]
    //    public void Test()
    //    {
    //        IParseTree tree = Parse("(+ 1 5)");
    //        var list = tree.GetChild(0).GetChild(0).GetChild(1) as FormsContext;
    //        var child1 = list.GetChild(0);
    //        var child2 = child1.GetChild(0);

    //        var form = new FormContext(list, 0);
    //        var literal = new LiteralContext(form, 0);
    //        var symbol = new SymbolContext(literal, 0);
    //        var node = new TerminalNodeImpl(new CommonToken(SYMBOL, "*"));

    //        list.children.RemoveAt(0);
    //        list.children.Insert(0, form);

    //        //list.AddChild(form);
    //        form.AddChild(literal);
    //        literal.AddChild(symbol);
    //        symbol.AddChild(node);

    //        var visitor = new ParseExpressionVisitor("ns", "cs", "Main");
    //        var result = visitor.Visit(tree).NormalizeWhitespace().ToFullString();
    //        return;

    //    }

    //    private static IParseTree Parse(string input)
    //    {
    //        using (var stream = new StringReader(input))
    //        {
    //            var inputStream = new AntlrInputStream(stream);
    //            var lexer = new DonatelloLexer(inputStream);
    //            var commonTokenStream = new CommonTokenStream(lexer);
    //            var parser = new DonatelloParser(commonTokenStream);
    //            return parser.file();
    //        }
    //    }
    //}
}
