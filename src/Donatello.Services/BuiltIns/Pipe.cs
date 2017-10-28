using System;
using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Donatello.Services.BuiltIns;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Donatello.Services.Antlr.Generated.DonatelloParser;
using System.Linq;

namespace Donatello.Services.BuiltIns
{
    internal class Pipe : IBuiltIn
    {
        private Func<IEnumerable<FormContext>, FormContext, IEnumerable<FormContext>> insertOperation;

        /// <param name="insertOperation">operation that inserts a FormContext into an IEnumerable of FormContext</param>
        public Pipe(Func<IEnumerable<FormContext>, FormContext, IEnumerable<FormContext>> insertOperation)
        {
            this.insertOperation = insertOperation;
        }

        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            /*
              (pipe 5
                  (* 3)
                  (+ 10))
             */
            var piped = children
                .Skip(2).Cast<FormContext>() // skip pipe character and input argument
                .Aggregate(
                    children.ElementAt(1) as FormContext, // initial seed is the input argument
                    (previousOutput, partialFunction) =>
                        // create a new list that is the partialFunction with the previousOutput prepended/appended.
                        NewList(insertOperation(partialFunction.list().form(), previousOutput).ToArray())
                );

            var result = visitor.Visit(piped);
            return result;
        }

        private static FormContext NewList(params FormContext[] forms)
        {
            var list = new ListContext(null, 0);
            foreach (var element in forms)
            {
                list.AddChild(element);
                element.Parent = list;
            }
            var form = new FormContext(null, 0);
            form.AddChild(list);
            return form;
        }
    }
}