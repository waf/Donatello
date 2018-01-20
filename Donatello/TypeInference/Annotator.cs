using Donatello.Ast;
using Donatello.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.TypeInference
{
    class Annotator : UntypedAstOperation<ITypedExpression>
    {
        /// <summary>
        /// Converts an AST of untyped expressions to the corresponding AST
        /// with typed expressions. Most of the types will be type variables,
        /// but some will be concrete types (e.g. boolean literals).
        /// </summary>
        internal static ITypedExpression Annotate(IExpression tree)
        {
            var annotator = new Annotator();
            return annotator.Apply(tree);
        }

        protected override ITypedExpression Set(SetUntypedExpression set)
        {
            throw new NotImplementedException();
        }

        protected override ITypedExpression Vector(VectorUntypedExpression vector)
        {
            throw new NotImplementedException();
        }

        protected override ITypedExpression List(ListUntypedExpression list)
        {
            var elements = list.Elements.Select(Apply).ToList();
            // a list is function invocation.
            // we don't know what the return type of this function invocation will be, so just set
            // it to a type variable
            var returnType = TypeVariable.Next();
            return new ListExpression(elements, returnType);
        }

        protected override ITypedExpression Function(FunctionUntypedExpression function)
        {
            var symbol = (SymbolExpression)Identifier(function.Symbol);
            var arguments = function.Arguments.Select(arg => (SymbolExpression)Identifier(arg)).ToList();
            var body = function.Body.Select(Apply).ToList();
            var type = new FunctionType(
                arguments.Select(a => a.Type),
                body.Last().Type);

            return new FunctionExpression(symbol, arguments, body, type);
        }

        protected override ITypedExpression Identifier(SymbolUntypedExpression symbol)
        {
            IType type = TypeEnvironment.GetTypeForName(symbol.Name);
            return new SymbolExpression(symbol.Name, type);
        }

        protected override ITypedExpression Def(DefUntypedExpression def)
        {
            var symbol = (SymbolExpression)Identifier(def.Symbol);
            var body = Apply(def.Body);
            return new DefExpression(symbol, body, body.Type);
        }

        protected override ITypedExpression File(FileUntypedExpression file)
        {
            var statements = file.Statements.Select(Apply).ToList();
            return new FileExpression(statements);
        }

        protected override ITypedExpression StringLiteral(StringExpression stringLiteral)
        {
            return stringLiteral;
        }

        protected override ITypedExpression FloatLiteral(FloatExpression floatLiteral)
        {
            return floatLiteral;
        }

        protected override ITypedExpression LongLiteral(LongExpression longLiteral)
        {
            return longLiteral;
        }

        protected override ITypedExpression BooleanLiteral(BooleanExpression booleanLiteral)
        {
            return booleanLiteral;
        }

        protected override ITypedExpression DefType(DefTypeExpression defType)
        {
            return defType;
        }
    }
}
