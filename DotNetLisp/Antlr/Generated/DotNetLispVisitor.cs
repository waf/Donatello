//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.5.3
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from DotNetLisp.g4 by ANTLR 4.5.3

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace DotNetLisp.Antlr.Generated {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="DotNetLispParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5.3")]
[System.CLSCompliant(false)]
public interface IDotNetLispVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFile([NotNull] DotNetLispParser.FileContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.forms"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitForms([NotNull] DotNetLispParser.FormsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.form"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitForm([NotNull] DotNetLispParser.FormContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitList([NotNull] DotNetLispParser.ListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.dictionary"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDictionary([NotNull] DotNetLispParser.DictionaryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.set"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSet([NotNull] DotNetLispParser.SetContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.vector"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVector([NotNull] DotNetLispParser.VectorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.lambdaParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLambdaParameter([NotNull] DotNetLispParser.LambdaParameterContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.lambda"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLambda([NotNull] DotNetLispParser.LambdaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral([NotNull] DotNetLispParser.LiteralContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] DotNetLispParser.StringContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber([NotNull] DotNetLispParser.NumberContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.symbol"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSymbol([NotNull] DotNetLispParser.SymbolContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="DotNetLispParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] DotNetLispParser.TypeContext context);
}
} // namespace DotNetLisp.Antlr.Generated
