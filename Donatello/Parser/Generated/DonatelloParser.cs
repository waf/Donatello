//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Donatello.g4 by ANTLR 4.7

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Donatello.Parser.Generated {
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public partial class DonatelloParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, STRING=11, FLOAT=12, LONG=13, BOOLEAN=14, MATH=15, NAME=16, TRASH=17;
	public const int
		RULE_file = 0, RULE_form = 1, RULE_list = 2, RULE_vector = 3, RULE_map = 4, 
		RULE_set = 5, RULE_functionArgs = 6, RULE_def = 7, RULE_function = 8, 
		RULE_let = 9, RULE_literal = 10, RULE_string = 11, RULE_boolean = 12, 
		RULE_number = 13, RULE_symbol = 14;
	public static readonly string[] ruleNames = {
		"file", "form", "list", "vector", "map", "set", "functionArgs", "def", 
		"function", "let", "literal", "string", "boolean", "number", "symbol"
	};

	private static readonly string[] _LiteralNames = {
		null, "'('", "')'", "'['", "']'", "'{'", "'}'", "'|'", "'def'", "'defn'", 
		"'let'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, "STRING", 
		"FLOAT", "LONG", "BOOLEAN", "MATH", "NAME", "TRASH"
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

	public override string GrammarFileName { get { return "Donatello.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static DonatelloParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public DonatelloParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public DonatelloParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}
	public partial class FileContext : ParserRuleContext {
		public ITerminalNode Eof() { return GetToken(DonatelloParser.Eof, 0); }
		public FormContext[] form() {
			return GetRuleContexts<FormContext>();
		}
		public FormContext form(int i) {
			return GetRuleContext<FormContext>(i);
		}
		public FileContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_file; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFile(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FileContext file() {
		FileContext _localctx = new FileContext(Context, State);
		EnterRule(_localctx, 0, RULE_file);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 33;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__2) | (1L << T__4) | (1L << STRING) | (1L << FLOAT) | (1L << LONG) | (1L << BOOLEAN) | (1L << MATH) | (1L << NAME))) != 0)) {
				{
				{
				State = 30; form();
				}
				}
				State = 35;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 36; Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FormContext : ParserRuleContext {
		public LiteralContext literal() {
			return GetRuleContext<LiteralContext>(0);
		}
		public ListContext list() {
			return GetRuleContext<ListContext>(0);
		}
		public DefContext def() {
			return GetRuleContext<DefContext>(0);
		}
		public FunctionContext function() {
			return GetRuleContext<FunctionContext>(0);
		}
		public LetContext let() {
			return GetRuleContext<LetContext>(0);
		}
		public VectorContext vector() {
			return GetRuleContext<VectorContext>(0);
		}
		public MapContext map() {
			return GetRuleContext<MapContext>(0);
		}
		public FormContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_form; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitForm(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FormContext form() {
		FormContext _localctx = new FormContext(Context, State);
		EnterRule(_localctx, 2, RULE_form);
		try {
			State = 45;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,1,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 38; literal();
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 39; list();
				}
				break;
			case 3:
				EnterOuterAlt(_localctx, 3);
				{
				State = 40; def();
				}
				break;
			case 4:
				EnterOuterAlt(_localctx, 4);
				{
				State = 41; function();
				}
				break;
			case 5:
				EnterOuterAlt(_localctx, 5);
				{
				State = 42; let();
				}
				break;
			case 6:
				EnterOuterAlt(_localctx, 6);
				{
				State = 43; vector();
				}
				break;
			case 7:
				EnterOuterAlt(_localctx, 7);
				{
				State = 44; map();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ListContext : ParserRuleContext {
		public FormContext[] form() {
			return GetRuleContexts<FormContext>();
		}
		public FormContext form(int i) {
			return GetRuleContext<FormContext>(i);
		}
		public ListContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_list; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitList(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public ListContext list() {
		ListContext _localctx = new ListContext(Context, State);
		EnterRule(_localctx, 4, RULE_list);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 47; Match(T__0);
			State = 51;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__2) | (1L << T__4) | (1L << STRING) | (1L << FLOAT) | (1L << LONG) | (1L << BOOLEAN) | (1L << MATH) | (1L << NAME))) != 0)) {
				{
				{
				State = 48; form();
				}
				}
				State = 53;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 54; Match(T__1);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class VectorContext : ParserRuleContext {
		public FormContext[] form() {
			return GetRuleContexts<FormContext>();
		}
		public FormContext form(int i) {
			return GetRuleContext<FormContext>(i);
		}
		public VectorContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_vector; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitVector(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public VectorContext vector() {
		VectorContext _localctx = new VectorContext(Context, State);
		EnterRule(_localctx, 6, RULE_vector);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 56; Match(T__2);
			State = 60;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__2) | (1L << T__4) | (1L << STRING) | (1L << FLOAT) | (1L << LONG) | (1L << BOOLEAN) | (1L << MATH) | (1L << NAME))) != 0)) {
				{
				{
				State = 57; form();
				}
				}
				State = 62;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 63; Match(T__3);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class MapContext : ParserRuleContext {
		public FormContext[] form() {
			return GetRuleContexts<FormContext>();
		}
		public FormContext form(int i) {
			return GetRuleContext<FormContext>(i);
		}
		public MapContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_map; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitMap(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public MapContext map() {
		MapContext _localctx = new MapContext(Context, State);
		EnterRule(_localctx, 8, RULE_map);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 65; Match(T__4);
			State = 71;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__2) | (1L << T__4) | (1L << STRING) | (1L << FLOAT) | (1L << LONG) | (1L << BOOLEAN) | (1L << MATH) | (1L << NAME))) != 0)) {
				{
				{
				State = 66; form();
				State = 67; form();
				}
				}
				State = 73;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 74; Match(T__5);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class SetContext : ParserRuleContext {
		public FormContext[] form() {
			return GetRuleContexts<FormContext>();
		}
		public FormContext form(int i) {
			return GetRuleContext<FormContext>(i);
		}
		public SetContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_set; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitSet(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public SetContext set() {
		SetContext _localctx = new SetContext(Context, State);
		EnterRule(_localctx, 10, RULE_set);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 76; Match(T__6);
			State = 80;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__2) | (1L << T__4) | (1L << STRING) | (1L << FLOAT) | (1L << LONG) | (1L << BOOLEAN) | (1L << MATH) | (1L << NAME))) != 0)) {
				{
				{
				State = 77; form();
				}
				}
				State = 82;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 83; Match(T__6);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FunctionArgsContext : ParserRuleContext {
		public SymbolContext[] symbol() {
			return GetRuleContexts<SymbolContext>();
		}
		public SymbolContext symbol(int i) {
			return GetRuleContext<SymbolContext>(i);
		}
		public FunctionArgsContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_functionArgs; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFunctionArgs(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FunctionArgsContext functionArgs() {
		FunctionArgsContext _localctx = new FunctionArgsContext(Context, State);
		EnterRule(_localctx, 12, RULE_functionArgs);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 85; Match(T__2);
			State = 89;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==MATH || _la==NAME) {
				{
				{
				State = 86; symbol();
				}
				}
				State = 91;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 92; Match(T__3);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class DefContext : ParserRuleContext {
		public SymbolContext name;
		public FormContext form() {
			return GetRuleContext<FormContext>(0);
		}
		public SymbolContext symbol() {
			return GetRuleContext<SymbolContext>(0);
		}
		public DefContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_def; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitDef(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public DefContext def() {
		DefContext _localctx = new DefContext(Context, State);
		EnterRule(_localctx, 14, RULE_def);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 94; Match(T__0);
			State = 95; Match(T__7);
			State = 96; _localctx.name = symbol();
			State = 97; form();
			State = 98; Match(T__1);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FunctionContext : ParserRuleContext {
		public SymbolContext name;
		public FunctionArgsContext functionArgs() {
			return GetRuleContext<FunctionArgsContext>(0);
		}
		public SymbolContext symbol() {
			return GetRuleContext<SymbolContext>(0);
		}
		public FormContext[] form() {
			return GetRuleContexts<FormContext>();
		}
		public FormContext form(int i) {
			return GetRuleContext<FormContext>(i);
		}
		public FunctionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_function; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitFunction(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public FunctionContext function() {
		FunctionContext _localctx = new FunctionContext(Context, State);
		EnterRule(_localctx, 16, RULE_function);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 100; Match(T__0);
			State = 101; Match(T__8);
			State = 102; _localctx.name = symbol();
			State = 103; functionArgs();
			State = 107;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__2) | (1L << T__4) | (1L << STRING) | (1L << FLOAT) | (1L << LONG) | (1L << BOOLEAN) | (1L << MATH) | (1L << NAME))) != 0)) {
				{
				{
				State = 104; form();
				}
				}
				State = 109;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 110; Match(T__1);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class LetContext : ParserRuleContext {
		public SymbolContext[] symbol() {
			return GetRuleContexts<SymbolContext>();
		}
		public SymbolContext symbol(int i) {
			return GetRuleContext<SymbolContext>(i);
		}
		public FormContext[] form() {
			return GetRuleContexts<FormContext>();
		}
		public FormContext form(int i) {
			return GetRuleContext<FormContext>(i);
		}
		public LetContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_let; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitLet(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public LetContext let() {
		LetContext _localctx = new LetContext(Context, State);
		EnterRule(_localctx, 18, RULE_let);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 112; Match(T__0);
			State = 113; Match(T__9);
			State = 114; Match(T__2);
			State = 120;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==MATH || _la==NAME) {
				{
				{
				State = 115; symbol();
				State = 116; form();
				}
				}
				State = 122;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 123; Match(T__3);
			State = 127;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << T__0) | (1L << T__2) | (1L << T__4) | (1L << STRING) | (1L << FLOAT) | (1L << LONG) | (1L << BOOLEAN) | (1L << MATH) | (1L << NAME))) != 0)) {
				{
				{
				State = 124; form();
				}
				}
				State = 129;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 130; Match(T__1);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class LiteralContext : ParserRuleContext {
		public StringContext @string() {
			return GetRuleContext<StringContext>(0);
		}
		public BooleanContext boolean() {
			return GetRuleContext<BooleanContext>(0);
		}
		public NumberContext number() {
			return GetRuleContext<NumberContext>(0);
		}
		public SymbolContext symbol() {
			return GetRuleContext<SymbolContext>(0);
		}
		public LiteralContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_literal; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitLiteral(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public LiteralContext literal() {
		LiteralContext _localctx = new LiteralContext(Context, State);
		EnterRule(_localctx, 20, RULE_literal);
		try {
			State = 136;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case STRING:
				EnterOuterAlt(_localctx, 1);
				{
				State = 132; @string();
				}
				break;
			case BOOLEAN:
				EnterOuterAlt(_localctx, 2);
				{
				State = 133; boolean();
				}
				break;
			case FLOAT:
			case LONG:
				EnterOuterAlt(_localctx, 3);
				{
				State = 134; number();
				}
				break;
			case MATH:
			case NAME:
				EnterOuterAlt(_localctx, 4);
				{
				State = 135; symbol();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class StringContext : ParserRuleContext {
		public ITerminalNode STRING() { return GetToken(DonatelloParser.STRING, 0); }
		public StringContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_string; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitString(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public StringContext @string() {
		StringContext _localctx = new StringContext(Context, State);
		EnterRule(_localctx, 22, RULE_string);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 138; Match(STRING);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class BooleanContext : ParserRuleContext {
		public ITerminalNode BOOLEAN() { return GetToken(DonatelloParser.BOOLEAN, 0); }
		public BooleanContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_boolean; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitBoolean(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public BooleanContext boolean() {
		BooleanContext _localctx = new BooleanContext(Context, State);
		EnterRule(_localctx, 24, RULE_boolean);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 140; Match(BOOLEAN);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class NumberContext : ParserRuleContext {
		public ITerminalNode FLOAT() { return GetToken(DonatelloParser.FLOAT, 0); }
		public ITerminalNode LONG() { return GetToken(DonatelloParser.LONG, 0); }
		public NumberContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_number; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitNumber(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public NumberContext number() {
		NumberContext _localctx = new NumberContext(Context, State);
		EnterRule(_localctx, 26, RULE_number);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 142;
			_la = TokenStream.LA(1);
			if ( !(_la==FLOAT || _la==LONG) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class SymbolContext : ParserRuleContext {
		public ITerminalNode NAME() { return GetToken(DonatelloParser.NAME, 0); }
		public ITerminalNode MATH() { return GetToken(DonatelloParser.MATH, 0); }
		public SymbolContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_symbol; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IDonatelloVisitor<TResult> typedVisitor = visitor as IDonatelloVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitSymbol(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public SymbolContext symbol() {
		SymbolContext _localctx = new SymbolContext(Context, State);
		EnterRule(_localctx, 28, RULE_symbol);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 144;
			_la = TokenStream.LA(1);
			if ( !(_la==MATH || _la==NAME) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x3', '\x13', '\x95', '\x4', '\x2', '\t', '\x2', '\x4', '\x3', 
		'\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', '\x5', '\x4', 
		'\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', '\t', '\b', 
		'\x4', '\t', '\t', '\t', '\x4', '\n', '\t', '\n', '\x4', '\v', '\t', '\v', 
		'\x4', '\f', '\t', '\f', '\x4', '\r', '\t', '\r', '\x4', '\xE', '\t', 
		'\xE', '\x4', '\xF', '\t', '\xF', '\x4', '\x10', '\t', '\x10', '\x3', 
		'\x2', '\a', '\x2', '\"', '\n', '\x2', '\f', '\x2', '\xE', '\x2', '%', 
		'\v', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x3', '\x3', '\x3', '\x3', 
		'\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x3', '\x5', 
		'\x3', '\x30', '\n', '\x3', '\x3', '\x4', '\x3', '\x4', '\a', '\x4', '\x34', 
		'\n', '\x4', '\f', '\x4', '\xE', '\x4', '\x37', '\v', '\x4', '\x3', '\x4', 
		'\x3', '\x4', '\x3', '\x5', '\x3', '\x5', '\a', '\x5', '=', '\n', '\x5', 
		'\f', '\x5', '\xE', '\x5', '@', '\v', '\x5', '\x3', '\x5', '\x3', '\x5', 
		'\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\a', '\x6', 'H', 
		'\n', '\x6', '\f', '\x6', '\xE', '\x6', 'K', '\v', '\x6', '\x3', '\x6', 
		'\x3', '\x6', '\x3', '\a', '\x3', '\a', '\a', '\a', 'Q', '\n', '\a', '\f', 
		'\a', '\xE', '\a', 'T', '\v', '\a', '\x3', '\a', '\x3', '\a', '\x3', '\b', 
		'\x3', '\b', '\a', '\b', 'Z', '\n', '\b', '\f', '\b', '\xE', '\b', ']', 
		'\v', '\b', '\x3', '\b', '\x3', '\b', '\x3', '\t', '\x3', '\t', '\x3', 
		'\t', '\x3', '\t', '\x3', '\t', '\x3', '\t', '\x3', '\n', '\x3', '\n', 
		'\x3', '\n', '\x3', '\n', '\x3', '\n', '\a', '\n', 'l', '\n', '\n', '\f', 
		'\n', '\xE', '\n', 'o', '\v', '\n', '\x3', '\n', '\x3', '\n', '\x3', '\v', 
		'\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\v', '\a', 
		'\v', 'y', '\n', '\v', '\f', '\v', '\xE', '\v', '|', '\v', '\v', '\x3', 
		'\v', '\x3', '\v', '\a', '\v', '\x80', '\n', '\v', '\f', '\v', '\xE', 
		'\v', '\x83', '\v', '\v', '\x3', '\v', '\x3', '\v', '\x3', '\f', '\x3', 
		'\f', '\x3', '\f', '\x3', '\f', '\x5', '\f', '\x8B', '\n', '\f', '\x3', 
		'\r', '\x3', '\r', '\x3', '\xE', '\x3', '\xE', '\x3', '\xF', '\x3', '\xF', 
		'\x3', '\x10', '\x3', '\x10', '\x3', '\x10', '\x2', '\x2', '\x11', '\x2', 
		'\x4', '\x6', '\b', '\n', '\f', '\xE', '\x10', '\x12', '\x14', '\x16', 
		'\x18', '\x1A', '\x1C', '\x1E', '\x2', '\x4', '\x3', '\x2', '\xE', '\xF', 
		'\x3', '\x2', '\x11', '\x12', '\x2', '\x97', '\x2', '#', '\x3', '\x2', 
		'\x2', '\x2', '\x4', '/', '\x3', '\x2', '\x2', '\x2', '\x6', '\x31', '\x3', 
		'\x2', '\x2', '\x2', '\b', ':', '\x3', '\x2', '\x2', '\x2', '\n', '\x43', 
		'\x3', '\x2', '\x2', '\x2', '\f', 'N', '\x3', '\x2', '\x2', '\x2', '\xE', 
		'W', '\x3', '\x2', '\x2', '\x2', '\x10', '`', '\x3', '\x2', '\x2', '\x2', 
		'\x12', '\x66', '\x3', '\x2', '\x2', '\x2', '\x14', 'r', '\x3', '\x2', 
		'\x2', '\x2', '\x16', '\x8A', '\x3', '\x2', '\x2', '\x2', '\x18', '\x8C', 
		'\x3', '\x2', '\x2', '\x2', '\x1A', '\x8E', '\x3', '\x2', '\x2', '\x2', 
		'\x1C', '\x90', '\x3', '\x2', '\x2', '\x2', '\x1E', '\x92', '\x3', '\x2', 
		'\x2', '\x2', ' ', '\"', '\x5', '\x4', '\x3', '\x2', '!', ' ', '\x3', 
		'\x2', '\x2', '\x2', '\"', '%', '\x3', '\x2', '\x2', '\x2', '#', '!', 
		'\x3', '\x2', '\x2', '\x2', '#', '$', '\x3', '\x2', '\x2', '\x2', '$', 
		'&', '\x3', '\x2', '\x2', '\x2', '%', '#', '\x3', '\x2', '\x2', '\x2', 
		'&', '\'', '\a', '\x2', '\x2', '\x3', '\'', '\x3', '\x3', '\x2', '\x2', 
		'\x2', '(', '\x30', '\x5', '\x16', '\f', '\x2', ')', '\x30', '\x5', '\x6', 
		'\x4', '\x2', '*', '\x30', '\x5', '\x10', '\t', '\x2', '+', '\x30', '\x5', 
		'\x12', '\n', '\x2', ',', '\x30', '\x5', '\x14', '\v', '\x2', '-', '\x30', 
		'\x5', '\b', '\x5', '\x2', '.', '\x30', '\x5', '\n', '\x6', '\x2', '/', 
		'(', '\x3', '\x2', '\x2', '\x2', '/', ')', '\x3', '\x2', '\x2', '\x2', 
		'/', '*', '\x3', '\x2', '\x2', '\x2', '/', '+', '\x3', '\x2', '\x2', '\x2', 
		'/', ',', '\x3', '\x2', '\x2', '\x2', '/', '-', '\x3', '\x2', '\x2', '\x2', 
		'/', '.', '\x3', '\x2', '\x2', '\x2', '\x30', '\x5', '\x3', '\x2', '\x2', 
		'\x2', '\x31', '\x35', '\a', '\x3', '\x2', '\x2', '\x32', '\x34', '\x5', 
		'\x4', '\x3', '\x2', '\x33', '\x32', '\x3', '\x2', '\x2', '\x2', '\x34', 
		'\x37', '\x3', '\x2', '\x2', '\x2', '\x35', '\x33', '\x3', '\x2', '\x2', 
		'\x2', '\x35', '\x36', '\x3', '\x2', '\x2', '\x2', '\x36', '\x38', '\x3', 
		'\x2', '\x2', '\x2', '\x37', '\x35', '\x3', '\x2', '\x2', '\x2', '\x38', 
		'\x39', '\a', '\x4', '\x2', '\x2', '\x39', '\a', '\x3', '\x2', '\x2', 
		'\x2', ':', '>', '\a', '\x5', '\x2', '\x2', ';', '=', '\x5', '\x4', '\x3', 
		'\x2', '<', ';', '\x3', '\x2', '\x2', '\x2', '=', '@', '\x3', '\x2', '\x2', 
		'\x2', '>', '<', '\x3', '\x2', '\x2', '\x2', '>', '?', '\x3', '\x2', '\x2', 
		'\x2', '?', '\x41', '\x3', '\x2', '\x2', '\x2', '@', '>', '\x3', '\x2', 
		'\x2', '\x2', '\x41', '\x42', '\a', '\x6', '\x2', '\x2', '\x42', '\t', 
		'\x3', '\x2', '\x2', '\x2', '\x43', 'I', '\a', '\a', '\x2', '\x2', '\x44', 
		'\x45', '\x5', '\x4', '\x3', '\x2', '\x45', '\x46', '\x5', '\x4', '\x3', 
		'\x2', '\x46', 'H', '\x3', '\x2', '\x2', '\x2', 'G', '\x44', '\x3', '\x2', 
		'\x2', '\x2', 'H', 'K', '\x3', '\x2', '\x2', '\x2', 'I', 'G', '\x3', '\x2', 
		'\x2', '\x2', 'I', 'J', '\x3', '\x2', '\x2', '\x2', 'J', 'L', '\x3', '\x2', 
		'\x2', '\x2', 'K', 'I', '\x3', '\x2', '\x2', '\x2', 'L', 'M', '\a', '\b', 
		'\x2', '\x2', 'M', '\v', '\x3', '\x2', '\x2', '\x2', 'N', 'R', '\a', '\t', 
		'\x2', '\x2', 'O', 'Q', '\x5', '\x4', '\x3', '\x2', 'P', 'O', '\x3', '\x2', 
		'\x2', '\x2', 'Q', 'T', '\x3', '\x2', '\x2', '\x2', 'R', 'P', '\x3', '\x2', 
		'\x2', '\x2', 'R', 'S', '\x3', '\x2', '\x2', '\x2', 'S', 'U', '\x3', '\x2', 
		'\x2', '\x2', 'T', 'R', '\x3', '\x2', '\x2', '\x2', 'U', 'V', '\a', '\t', 
		'\x2', '\x2', 'V', '\r', '\x3', '\x2', '\x2', '\x2', 'W', '[', '\a', '\x5', 
		'\x2', '\x2', 'X', 'Z', '\x5', '\x1E', '\x10', '\x2', 'Y', 'X', '\x3', 
		'\x2', '\x2', '\x2', 'Z', ']', '\x3', '\x2', '\x2', '\x2', '[', 'Y', '\x3', 
		'\x2', '\x2', '\x2', '[', '\\', '\x3', '\x2', '\x2', '\x2', '\\', '^', 
		'\x3', '\x2', '\x2', '\x2', ']', '[', '\x3', '\x2', '\x2', '\x2', '^', 
		'_', '\a', '\x6', '\x2', '\x2', '_', '\xF', '\x3', '\x2', '\x2', '\x2', 
		'`', '\x61', '\a', '\x3', '\x2', '\x2', '\x61', '\x62', '\a', '\n', '\x2', 
		'\x2', '\x62', '\x63', '\x5', '\x1E', '\x10', '\x2', '\x63', '\x64', '\x5', 
		'\x4', '\x3', '\x2', '\x64', '\x65', '\a', '\x4', '\x2', '\x2', '\x65', 
		'\x11', '\x3', '\x2', '\x2', '\x2', '\x66', 'g', '\a', '\x3', '\x2', '\x2', 
		'g', 'h', '\a', '\v', '\x2', '\x2', 'h', 'i', '\x5', '\x1E', '\x10', '\x2', 
		'i', 'm', '\x5', '\xE', '\b', '\x2', 'j', 'l', '\x5', '\x4', '\x3', '\x2', 
		'k', 'j', '\x3', '\x2', '\x2', '\x2', 'l', 'o', '\x3', '\x2', '\x2', '\x2', 
		'm', 'k', '\x3', '\x2', '\x2', '\x2', 'm', 'n', '\x3', '\x2', '\x2', '\x2', 
		'n', 'p', '\x3', '\x2', '\x2', '\x2', 'o', 'm', '\x3', '\x2', '\x2', '\x2', 
		'p', 'q', '\a', '\x4', '\x2', '\x2', 'q', '\x13', '\x3', '\x2', '\x2', 
		'\x2', 'r', 's', '\a', '\x3', '\x2', '\x2', 's', 't', '\a', '\f', '\x2', 
		'\x2', 't', 'z', '\a', '\x5', '\x2', '\x2', 'u', 'v', '\x5', '\x1E', '\x10', 
		'\x2', 'v', 'w', '\x5', '\x4', '\x3', '\x2', 'w', 'y', '\x3', '\x2', '\x2', 
		'\x2', 'x', 'u', '\x3', '\x2', '\x2', '\x2', 'y', '|', '\x3', '\x2', '\x2', 
		'\x2', 'z', 'x', '\x3', '\x2', '\x2', '\x2', 'z', '{', '\x3', '\x2', '\x2', 
		'\x2', '{', '}', '\x3', '\x2', '\x2', '\x2', '|', 'z', '\x3', '\x2', '\x2', 
		'\x2', '}', '\x81', '\a', '\x6', '\x2', '\x2', '~', '\x80', '\x5', '\x4', 
		'\x3', '\x2', '\x7F', '~', '\x3', '\x2', '\x2', '\x2', '\x80', '\x83', 
		'\x3', '\x2', '\x2', '\x2', '\x81', '\x7F', '\x3', '\x2', '\x2', '\x2', 
		'\x81', '\x82', '\x3', '\x2', '\x2', '\x2', '\x82', '\x84', '\x3', '\x2', 
		'\x2', '\x2', '\x83', '\x81', '\x3', '\x2', '\x2', '\x2', '\x84', '\x85', 
		'\a', '\x4', '\x2', '\x2', '\x85', '\x15', '\x3', '\x2', '\x2', '\x2', 
		'\x86', '\x8B', '\x5', '\x18', '\r', '\x2', '\x87', '\x8B', '\x5', '\x1A', 
		'\xE', '\x2', '\x88', '\x8B', '\x5', '\x1C', '\xF', '\x2', '\x89', '\x8B', 
		'\x5', '\x1E', '\x10', '\x2', '\x8A', '\x86', '\x3', '\x2', '\x2', '\x2', 
		'\x8A', '\x87', '\x3', '\x2', '\x2', '\x2', '\x8A', '\x88', '\x3', '\x2', 
		'\x2', '\x2', '\x8A', '\x89', '\x3', '\x2', '\x2', '\x2', '\x8B', '\x17', 
		'\x3', '\x2', '\x2', '\x2', '\x8C', '\x8D', '\a', '\r', '\x2', '\x2', 
		'\x8D', '\x19', '\x3', '\x2', '\x2', '\x2', '\x8E', '\x8F', '\a', '\x10', 
		'\x2', '\x2', '\x8F', '\x1B', '\x3', '\x2', '\x2', '\x2', '\x90', '\x91', 
		'\t', '\x2', '\x2', '\x2', '\x91', '\x1D', '\x3', '\x2', '\x2', '\x2', 
		'\x92', '\x93', '\t', '\x3', '\x2', '\x2', '\x93', '\x1F', '\x3', '\x2', 
		'\x2', '\x2', '\r', '#', '/', '\x35', '>', 'I', 'R', '[', 'm', 'z', '\x81', 
		'\x8A',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace Donatello.Parser.Generated