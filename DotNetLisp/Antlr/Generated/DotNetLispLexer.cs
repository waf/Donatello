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
using System;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5.3")]
[System.CLSCompliant(false)]
public partial class DotNetLispLexer : Lexer {
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, STRING=5, LONG=6, SYMBOL=7, TYPE=8, TRASH=9;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "STRING", "LONG", "SYMBOL", "TYPE", "WS", 
		"COMMENT", "TRASH"
	};


	public DotNetLispLexer(ICharStream input)
		: base(input)
	{
		Interpreter = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
		null, "'('", "')'", "'['", "']'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, "STRING", "LONG", "SYMBOL", "TYPE", "TRASH"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "DotNetLisp.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	private static string _serializedATN = _serializeATN();
	private static string _serializeATN()
	{
	    StringBuilder sb = new StringBuilder();
	    sb.Append("\x3\x430\xD6D1\x8206\xAD2D\x4417\xAEF1\x8D80\xAADD\x2\vQ");
		sb.Append("\b\x1\x4\x2\t\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6");
		sb.Append("\x4\a\t\a\x4\b\t\b\x4\t\t\t\x4\n\t\n\x4\v\t\v\x4\f\t\f\x3\x2");
		sb.Append("\x3\x2\x3\x3\x3\x3\x3\x4\x3\x4\x3\x5\x3\x5\x3\x6\x3\x6\x3\x6");
		sb.Append("\x3\x6\a\x6&\n\x6\f\x6\xE\x6)\v\x6\x3\x6\x3\x6\x3\a\x5\a.\n");
		sb.Append("\a\x3\a\x6\a\x31\n\a\r\a\xE\a\x32\x3\a\x5\a\x36\n\a\x3\b\x6");
		sb.Append("\b\x39\n\b\r\b\xE\b:\x3\t\x3\t\x6\t?\n\t\r\t\xE\t@\x3\n\x3\n");
		sb.Append("\x3\v\x3\v\a\vG\n\v\f\v\xE\vJ\v\v\x3\f\x3\f\x5\fN\n\f\x3\f\x3");
		sb.Append("\f\x2\x2\r\x3\x3\x5\x4\a\x5\t\x6\v\a\r\b\xF\t\x11\n\x13\x2\x15");
		sb.Append("\x2\x17\v\x3\x2\t\x3\x2$$\x3\x2\x32;\x4\x2NNnn\x6\x2--\x30\x30");
		sb.Append("\x43\\\x63|\x6\x2>>@@\x43\\\x63|\a\x2\v\f\xF\xF\"\"..^^\x4\x2");
		sb.Append("\f\f\xF\xFW\x2\x3\x3\x2\x2\x2\x2\x5\x3\x2\x2\x2\x2\a\x3\x2\x2");
		sb.Append("\x2\x2\t\x3\x2\x2\x2\x2\v\x3\x2\x2\x2\x2\r\x3\x2\x2\x2\x2\xF");
		sb.Append("\x3\x2\x2\x2\x2\x11\x3\x2\x2\x2\x2\x17\x3\x2\x2\x2\x3\x19\x3");
		sb.Append("\x2\x2\x2\x5\x1B\x3\x2\x2\x2\a\x1D\x3\x2\x2\x2\t\x1F\x3\x2\x2");
		sb.Append("\x2\v!\x3\x2\x2\x2\r-\x3\x2\x2\x2\xF\x38\x3\x2\x2\x2\x11<\x3");
		sb.Append("\x2\x2\x2\x13\x42\x3\x2\x2\x2\x15\x44\x3\x2\x2\x2\x17M\x3\x2");
		sb.Append("\x2\x2\x19\x1A\a*\x2\x2\x1A\x4\x3\x2\x2\x2\x1B\x1C\a+\x2\x2");
		sb.Append("\x1C\x6\x3\x2\x2\x2\x1D\x1E\a]\x2\x2\x1E\b\x3\x2\x2\x2\x1F ");
		sb.Append("\a_\x2\x2 \n\x3\x2\x2\x2!\'\a$\x2\x2\"&\n\x2\x2\x2#$\a^\x2\x2");
		sb.Append("$&\a$\x2\x2%\"\x3\x2\x2\x2%#\x3\x2\x2\x2&)\x3\x2\x2\x2\'%\x3");
		sb.Append("\x2\x2\x2\'(\x3\x2\x2\x2(*\x3\x2\x2\x2)\'\x3\x2\x2\x2*+\a$\x2");
		sb.Append("\x2+\f\x3\x2\x2\x2,.\a/\x2\x2-,\x3\x2\x2\x2-.\x3\x2\x2\x2.\x30");
		sb.Append("\x3\x2\x2\x2/\x31\t\x3\x2\x2\x30/\x3\x2\x2\x2\x31\x32\x3\x2");
		sb.Append("\x2\x2\x32\x30\x3\x2\x2\x2\x32\x33\x3\x2\x2\x2\x33\x35\x3\x2");
		sb.Append("\x2\x2\x34\x36\t\x4\x2\x2\x35\x34\x3\x2\x2\x2\x35\x36\x3\x2");
		sb.Append("\x2\x2\x36\xE\x3\x2\x2\x2\x37\x39\t\x5\x2\x2\x38\x37\x3\x2\x2");
		sb.Append("\x2\x39:\x3\x2\x2\x2:\x38\x3\x2\x2\x2:;\x3\x2\x2\x2;\x10\x3");
		sb.Append("\x2\x2\x2<>\a<\x2\x2=?\t\x6\x2\x2>=\x3\x2\x2\x2?@\x3\x2\x2\x2");
		sb.Append("@>\x3\x2\x2\x2@\x41\x3\x2\x2\x2\x41\x12\x3\x2\x2\x2\x42\x43");
		sb.Append("\t\a\x2\x2\x43\x14\x3\x2\x2\x2\x44H\a=\x2\x2\x45G\n\b\x2\x2");
		sb.Append("\x46\x45\x3\x2\x2\x2GJ\x3\x2\x2\x2H\x46\x3\x2\x2\x2HI\x3\x2");
		sb.Append("\x2\x2I\x16\x3\x2\x2\x2JH\x3\x2\x2\x2KN\x5\x13\n\x2LN\x5\x15");
		sb.Append("\v\x2MK\x3\x2\x2\x2ML\x3\x2\x2\x2NO\x3\x2\x2\x2OP\b\f\x2\x2");
		sb.Append("P\x18\x3\x2\x2\x2\f\x2%\'-\x32\x35:@HM\x3\x2\x3\x2");
	    return sb.ToString();
	}

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
} // namespace DotNetLisp.Antlr.Generated
