﻿using Antlr4.Runtime.Misc;
using Donatello.Services.Antlr.Generated;
using Donatello.StandardLibrary;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello.Services.Parser
{
    public partial class ParseExpressionVisitor : DonatelloBaseVisitor<CSharpSyntaxNode>
    {
        public override CSharpSyntaxNode VisitVector([NotNull] DonatelloParser.VectorContext context)
        {
            var children = context.form();

            var arguments = children
                .Select(child => Argument(Visit(child) as ExpressionSyntax))
                .ToArray();

            return InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(nameof(ImmutableArray)),
                        IdentifierName(nameof(ImmutableArray.Create))))
                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }

        public override CSharpSyntaxNode VisitSet([NotNull] DonatelloParser.SetContext context)
        {
            var children = context.form();

            var arguments = children
                .Select(child => Argument(Visit(child) as ExpressionSyntax))
                .ToArray();

            return InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(nameof(ImmutableHashSet)),
                        IdentifierName(nameof(ImmutableHashSet.Create))))
                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }

        public override CSharpSyntaxNode VisitDictionary([NotNull] DonatelloParser.DictionaryContext context)
        {
            var children = context.form();

            var keyValueList = new List<ExpressionSyntax>();
            for(int i = 0; i < children.Length; i += 2) //select every two key/value pair
            {
                keyValueList.Add(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(nameof(Constructors)),
                            IdentifierName(nameof(Constructors.CreateKeyValuePair))))
                    .WithArgumentList(
                        ArgumentList(
                            SeparatedList(new[] {
                                Argument(this.Visit(children[i]) as ExpressionSyntax), //key 
                                Argument(this.Visit(children[i + 1]) as ExpressionSyntax)})))); //value
            }
            return
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(nameof(ImmutableDictionary)),
                            IdentifierName(nameof(ImmutableDictionary.CreateRange))))
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                            Argument(
                                ImplicitArrayCreationExpression(
                                    InitializerExpression(
                                        SyntaxKind.ArrayInitializerExpression,
                                        SeparatedList(keyValueList)))))));
        }
    }
}
