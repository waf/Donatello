using Donatello.Services.Compilation;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Donatello.Build
{
    public class FileBuilder
    {

        public static void CompileFile(string[] inputFile, string[] references, string outputFilename)
        {
            var fileContent = (from file in inputFile
                               select new
                               {
                                   NamespaceName = Directory.GetParent(file).Name,
                                   ClassName = Path.GetFileNameWithoutExtension(file),
                                   Content = File.ReadAllText(file)
                               })
                              .ToDictionary(
                                    unit => Tuple.Create(unit.NamespaceName, unit.ClassName),
                                    unit => unit.Content);
            string assemblyName = Path.GetFileNameWithoutExtension(outputFilename);
            string extension = Path.GetExtension(outputFilename);
            var outputKind = extension == ".dll" ? OutputType.DynamicallyLinkedLibrary :
                             extension == ".exe" ? OutputType.ConsoleApplication :
                             throw new Exception("unknown extension");
            var bytes = CompileContent(fileContent, references, assemblyName, outputKind);
            File.WriteAllBytes(outputFilename, bytes);
        }

        public static byte[] CompileContent(
            IDictionary<Tuple<string, string>, string> files,
            IList<string> references,
            string assemblyName,
            OutputType outputKind)
        {
            var compilationUnit = files
                .Select(file => AntlrParser.ParseAsClass(file.Value, file.Key.Item1, file.Key.Item2) as CompilationUnitSyntax)
                .ToArray();

            var assembly = Compiler.Compile(assemblyName, references, outputKind, compilationUnit);

            return assembly;
        }

    }
}
