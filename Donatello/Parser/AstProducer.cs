using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Donatello.Ast;
using Donatello.Parser.Generated;
using System;
using System.Linq;

namespace Donatello.Parser
{
	/// <summary>
	/// Traverses the ANTLR parse tree to create our Donatello.Ast tree
	/// </summary>
	internal class AstProducer : DonatelloBaseVisitor<IExpression>
    {
        public static FileUntypedExpression Parse(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text.ToString());
            DonatelloLexer lexer = new DonatelloLexer(inputStream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            DonatelloParser parser = new DonatelloParser(tokenStream);
            var file = parser.file();

            AstProducer visitor = new AstProducer();
            var expression = visitor.Visit(file) as FileUntypedExpression;
            return expression;
        }

        public override IExpression VisitFile([NotNull] DonatelloParser.FileContext context)
        {
            var statements = context.form().Select(form => Visit(form));
            return new FileUntypedExpression(statements);
        }

        public override IExpression VisitForm([NotNull] DonatelloParser.FormContext context)
        {
            return base.VisitForm(context);
        }

        public override IExpression VisitFunction([NotNull] DonatelloParser.FunctionContext context)
        {
            //TODO: get rid of symbol here? it can just be a string.
            var name = new SymbolUntypedExpression(context.identifier().GetText());
            var args = context.functionArgs().identifier().Select(Visit).Cast<SymbolUntypedExpression>();
            var body = context.form().Select(Visit);

            return new FunctionUntypedExpression(name, args, body);
        }

        public override IExpression VisitIdentifier([NotNull] DonatelloParser.IdentifierContext context)
        {
            return new SymbolUntypedExpression(context.GetText());
        }

        public override IExpression VisitDef([NotNull] DonatelloParser.DefContext context)
        {
            //TODO: get rid of symbol here? it can just be a string.
            var name = new SymbolUntypedExpression(context.identifier().GetText());
            var body = Visit(context.form());
            return new DefUntypedExpression(name, body);
        }

        public override IExpression VisitLet([NotNull] DonatelloParser.LetContext context)
        {
            throw new NotImplementedException();
            //var bindings = context.symbol().Zip(context.form(), (symbol, form) => (symbol, form));
            //var body = context.list().Select(Visit).Cast<ListExpression>();
            //return new LetExpression(bindings, body);
        }

        public override IExpression VisitList([NotNull] DonatelloParser.ListContext context)
        {
            var elements = context.form().Select(Visit);
            // a list is function invocation.
            // we don't know what the return type of this function invocation will be, so just set
            // it to a type variable
            //var args = context.form().Skip(1).Select(Visit);
            //var functionType = new FunctionType(args.Select(a => a.Type),  returnType);
            //var functionSymbol = new SymbolExpression(context.form().First().GetText(), functionType);
            return new ListUntypedExpression(elements);
        }

        public override IExpression VisitBoolean([NotNull] DonatelloParser.BooleanContext context)
        {
            if(context.BOOLEAN() is ITerminalNode boolean) return new BooleanExpression(boolean.ToString());
            throw new ArgumentException("Unknown boolean value");
        }

        public override IExpression VisitMap([NotNull] DonatelloParser.MapContext context)
        {
            return base.VisitMap(context);
        }

        public override IExpression VisitNumber([NotNull] DonatelloParser.NumberContext context)
        {
            if(context.LONG() is ITerminalNode @long) return new LongExpression(@long.ToString());
            if(context.FLOAT() is ITerminalNode @float) return new FloatExpression(@float.ToString());
            throw new ArgumentException("Unknown number type");
        }

        public override IExpression VisitSet([NotNull] DonatelloParser.SetContext context)
        {
            var elements = context.form().Select(form => Visit(form));
            return new SetUntypedExpression(elements);
        }

        public override IExpression VisitString([NotNull] DonatelloParser.StringContext context)
        {
            if(context.STRING() is ITerminalNode @string) return new StringExpression(@string.ToString());
            throw new ArgumentException("Unknown number type");
        }

        public override IExpression VisitSymbol([NotNull] DonatelloParser.SymbolContext context)
        {
            string name = context.GetText();
            return new SymbolUntypedExpression(name);
        }

        public override IExpression VisitVector([NotNull] DonatelloParser.VectorContext context)
        {
            var elements = context.form().Select(form => Visit(form));
            return new VectorUntypedExpression(elements);
        }

        public override IExpression VisitDefType([NotNull] DonatelloParser.DefTypeContext context)
        {
            var name = context.identifier().GetText();
            var declarations = context
                .propertyDeclaration()
                .Select(prop => new Property(prop.property().GetText(), prop.type().GetText()))
                .ToArray();
            return new DefTypeExpression(name, declarations);
        }
    }
}
