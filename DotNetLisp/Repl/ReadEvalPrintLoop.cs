using DotNetLisp.Compilation;
using DotNetLisp.Parser;
using DotNetLisp.StandardLibrary;
using DotNetLisp.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DotNetLisp.Repl
{
    public class ReadEvalPrintLoop
    {
        const string NamespaceName = "Repl";
        const string ClassName = "Program";
        const string RunMethod = "DotNetLispReplRun";

        public void Run()
        {
            // there's a slight delay when we load up roslyn and run a program for the first time. Do an
            // initial run to warm things up, so the user doesn't experience a delay for the first evaluation.
            Task.Run(() => Compiler.Compile(
                NamespaceName,
                OutputType.DynamicallyLinkedLibrary,
                AntlrParser.Parse(@"""DotNetLisp""", NamespaceName, ClassName, RunMethod)));

            CompilationUnitSyntax previousProgram = null;
            while (true)
            {
                //read
                Console.Write("> ");
                string text = Console.ReadLine().Trim();

                if (text == string.Empty) { continue; }
                if (text == "exit") { break; }

                try
                {
                    //eval
                    var program = AntlrParser.Parse(text, NamespaceName, ClassName);
                    program = CombineWithPreviousProgram(previousProgram, program);
                    program = WrapLastLineWithPrintStatement(program);
                    var result = Compiler.Compile(NamespaceName, OutputType.DynamicallyLinkedLibrary, program);
                    AssemblyRunner.RunClassConstructor(result, NamespaceName, ClassName); //print

                    previousProgram = program;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            } // loop!
        }

        /// <summary>
        /// Wrap the last line of the constructor with a print statement.
        /// </summary>
        private CompilationUnitSyntax WrapLastLineWithPrintStatement(CompilationUnitSyntax program)
        {
            var lastLineOfConstructor = program
                .DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>().SingleOrDefault()?
                .Body.Statements.Cast<ExpressionStatementSyntax>().LastOrDefault();

            if(lastLineOfConstructor == null)
            {
                return program;
            }

            var lastLineWrappedWithPrint =
                ExpressionStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(nameof(ReplUtil)),
                            IdentifierName(nameof(ReplUtil.Print))))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(ParenthesizedLambdaExpression(lastLineOfConstructor.Expression))))));

            return program.ReplaceNode(lastLineOfConstructor, lastLineWrappedWithPrint);
        }


        /// <summary>
        /// Given a previous program and new program, merge the new members (field and methods) of the two programs.
        /// The new program will overwrite the fields/methods of the old program when a name collision occurs.
        /// This is used so each line in the repl can "build" on the previous lines.
        /// 
        /// Example, from an empty session
        /// > (def a:int 5)             is the previous program
        /// > (def b:int 6) (+ a b)     is the new program
        /// we want the second line to know about variables defined in the first line. By merging each
        /// successive line into the previous lines, we build up a history of defined members.
        /// </summary>
        /// <param name="previousProgram"></param>
        /// <param name="newProgram"></param>
        /// <returns>the merged program</returns>
        private static CompilationUnitSyntax CombineWithPreviousProgram(CompilationUnitSyntax previousProgram, CompilationUnitSyntax newProgram)
        {
            if(previousProgram == null)
            {
                return newProgram;
            }

            // get rid of the old constructor. This way, if the user evaluates something
            // that doesn't have a constructor (like a variable definition), we don't run
            // the old constructor
            var oldConstructor = previousProgram
                .DescendantNodes()
                .OfType<ConstructorDeclarationSyntax>();
            if(oldConstructor != null)
            {
                previousProgram = previousProgram.RemoveNodes(oldConstructor, SyntaxRemoveOptions.KeepNoTrivia);
            }

            // add/replace methods and fields into the previous program
            var constructors = MergeMembers<ConstructorDeclarationSyntax>(previousProgram, newProgram,
                constructor => constructor.Identifier.Text)
                .Cast<MemberDeclarationSyntax>();
            var methods = MergeMembers<MethodDeclarationSyntax>(previousProgram, newProgram,
                method => method.Identifier.Text)
                .Cast<MemberDeclarationSyntax>();
            var fields = MergeMembers<FieldDeclarationSyntax>(previousProgram, newProgram,
                field => field.Declaration.Variables.Single().Identifier.Text)
                .Cast<MemberDeclarationSyntax>();
            var oldClass = previousProgram.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
            var newClass = oldClass.WithMembers(List(constructors.Union(methods).Union(fields)));

            var usings = previousProgram.Usings.Union(newProgram.Usings)
                .GroupBy(usingStatement => usingStatement.ToString())
                .Select(grouping => grouping.First()).ToArray();

            return previousProgram.ReplaceNode(oldClass, newClass)
                    .WithUsings(List(usings));
        }

        private static IEnumerable<T> MergeMembers<T>(
            CompilationUnitSyntax previous,
            CompilationUnitSyntax program,
            Func<T, string> NameProperty
            )
            where T : MemberDeclarationSyntax
        {
            var oldMembers = previous.DescendantNodes().OfType<T>();
            var newMembers = program.DescendantNodes().OfType<T>();
            // inner join on old members and new members
            var oldMembersToReplace = oldMembers.Join(newMembers, NameProperty, NameProperty, (old, _) => old);
            return newMembers.Union(oldMembers.Except(oldMembersToReplace));
        }
    }
}
