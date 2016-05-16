using DotNetLisp.Compilation;
using DotNetLisp.Parser;
using DotNetLisp.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DotNetLisp.Repl
{
    class ReadEvalPrintLoop
    {
        private const string NamespaceName = "Repl";
        private const string ClassName = "Program";
        private const string RunMethod = "DotNetLispReplRun";

        public static void Run()
        {
            // there's a slight delay when we load up roslyn and run a program for the first time. Do an
            // initial run to warm things up, so the user doesn't experience a delay for the first evaluation.
            Task.Run(() => Compiler.Compile(AntlrParser.Parse(@"""DotNetLisp""", NamespaceName, ClassName, RunMethod)));

            CompilationUnitSyntax previousProgram = null;
            while (true)
            {
                //read
                Console.Write("> ");
                string input = Console.ReadLine().Trim();

                if (input == string.Empty) { continue; }
                if (input == "exit") { break; }

                //eval
                string output = "";
                try
                {
                    var program = AntlrParser.Parse(input, NamespaceName, ClassName, RunMethod);
                    program = CombineWithPreviousProgram(previousProgram, program);
                    // either run the compiled output or handle the errors
                    output = Compiler.Compile(program).Match(
                        Ok: bytes =>
                        {
                            previousProgram = program;
                            return FormatProgramOutput(DynamicInvoke(bytes));
                        },
                        Error: errors => string.Join(Environment.NewLine, errors));
                }
                catch (Exception e)
                {
                    output = "Error: " + e.Message;
                }

                // print
                if(output != null)
                {
                    Console.WriteLine(output);
                }
            } // loop!
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

            // get rid of the old run method.
            var oldRunMethod = previousProgram
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .SingleOrDefault(m => m.Identifier.Text == RunMethod);
            if(oldRunMethod != null)
            {
                previousProgram = previousProgram.RemoveNode(oldRunMethod, SyntaxRemoveOptions.KeepNoTrivia);
            }

            // add/replace methods and fields into the previous program
            var methods = MergeMembers<MethodDeclarationSyntax>(previousProgram, newProgram,
                method => method.Identifier.Text)
                .Cast<MemberDeclarationSyntax>();
            var fields = MergeMembers<FieldDeclarationSyntax>(previousProgram, newProgram,
                field => field.Declaration.Variables.Single().Identifier.Text)
                .Cast<MemberDeclarationSyntax>();
            var oldClass = previousProgram.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
            var newClass = oldClass.WithMembers(List(methods.Union(fields)));
            return previousProgram.ReplaceNode(oldClass, newClass);
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

        private static object DynamicInvoke(byte[] bytes)
        {
            string FullyQualifiedClass = $"{NamespaceName}.{ClassName}";
            Assembly assembly = Assembly.Load(bytes);
            Type type = assembly.GetType(FullyQualifiedClass);
            try
            {
                return type.InvokeMember(RunMethod, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            }
            catch(MissingMethodException e) when (e.Message == $"Method '{FullyQualifiedClass}.{RunMethod}' not found.")
            {
                // this is a valid case; the user could have issue a statement with no return value (like defining a function)
                return null;
            }
        }


        private static string FormatProgramOutput(object output)
        {
            return output == null ? null : JsonConvert.SerializeObject(output, Formatting.Indented);
        }
    }
}
