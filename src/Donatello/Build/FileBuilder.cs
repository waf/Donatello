using Donatello.Services.Compilation;
using Donatello.Services.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Donatello.Build
{
    /// <summary>
    /// Reads a collection of files and invokes the <see cref="Compiler"/> on them.
    /// </summary>
    public class FileCompiler
    {
        /// <summary>
        /// Compiles input files into an output file.
        /// </summary>
        /// <param name="inputFileNames">A list of filenames of Donatello source files</param>
        /// <param name="references">A list of DLLs to reference</param>
        /// <param name="outputFilename">The output filename, with a file extension of DLL or EXE</param>
        public static void CompileFiles(
            IReadOnlyCollection<string> inputFileNames,
            IReadOnlyCollection<string> references,
            string outputFilename)
        {
            var content = inputFileNames.Select(file => (
               NamespaceName: Directory.GetParent(file).Name,
               ClassName: Path.GetFileNameWithoutExtension(file),
               Content: File.ReadAllText(file)
            )).ToArray();
            string assemblyName = Path.GetFileNameWithoutExtension(outputFilename);
            string extension = Path.GetExtension(outputFilename);
            OutputType outputKind = extension == ".dll" ? OutputType.DynamicallyLinkedLibrary :
                                    extension == ".exe" ? OutputType.ConsoleApplication :
                                    throw new ArgumentException($"unknown output extension: '{extension}'");
            var assembly = CompileSource(content, references, assemblyName, outputKind);
            using (var fileStream = File.Create(outputFilename))
            {
                assembly.CopyTo(fileStream);
            }
        }

        /// <summary>
        /// Compiles strings of source code into a byte array.
        /// </summary>
        /// <param name="inputSources">a collection of tuples with input source and output namespace, classname</param>
        /// <param name="references">a list of DLLs to reference</param>
        /// <param name="assemblyName">the name of the output assembly</param>
        /// <param name="outputKind">the desired output format</param>
        /// <returns>a byte array of the compiled assembly</returns>
        public static Stream CompileSource(
            IReadOnlyCollection<(string NamespaceName, string ClassName, string Content)> inputSources,
            IReadOnlyCollection<string> references,
            string assemblyName,
            OutputType outputKind)
        {
            var compilationUnit = inputSources
                .Select(file => AntlrParser.ParseAsClass(file.Content, file.NamespaceName, file.ClassName))
                .ToArray();

            var assembly = Compiler.Compile(assemblyName, references, outputKind, compilationUnit);

            return assembly;
        }

    }
}
