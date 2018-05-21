using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Donatello.Ast;
using Sigil.NonGeneric;
using Donatello.TypeInference;
using System.Collections.Generic;

namespace Donatello.Emitter
{
    class ClassEmitter : IVisitor
    {
        static MethodAttributes PublicStatic = MethodAttributes.HideBySig | MethodAttributes.Static | MethodAttributes.Public;

        readonly AssemblyBuilder AssemblyBuilder;
        readonly ModuleBuilder ModuleBuilder;
        readonly TypeBuilder TypeBuilder;

        IDictionary<string, Emit> Functions = new Dictionary<string, Emit>();
        IDictionary<string, FieldInfo> Fields = new Dictionary<string, FieldInfo>();
        IDictionary<string, ITypedExpression> Constants = new Dictionary<string, ITypedExpression>();
        IDictionary<string, Type> Types = new Dictionary<string, Type>();

        // Emit API is stateful, so this field mutates as the AST is traversed.
        Emit Emitter;

        public ClassEmitter(AssemblyBuilder assemblyBuilder, ModuleBuilder moduleBuilder, string className)
        {
            this.AssemblyBuilder = assemblyBuilder;
            this.ModuleBuilder = moduleBuilder;
            this.TypeBuilder = moduleBuilder.DefineType(className);
        }

        public void Visit(ITypedExpression elem)
        {
            elem.Accept(this);
        }

        public void Visit(FileExpression file)
        {
            var fields = file.Statements.OfType<DefExpression>().ToList();
            Emitter = Emit.BuildTypeInitializer(TypeBuilder);
            foreach (var field in fields)
            {
                Visit(field);
            }
            Emitter.Return();
            Emitter.CreateTypeInitializer();

            var functions = file.Statements.OfType<FunctionExpression>().ToList();
            foreach (var function in functions)
            {
                Visit(function);
            }

            Emitter = Emit.BuildStaticMethod(typeof(void), new[] { typeof(string[]) }, TypeBuilder, "Main", PublicStatic);
            foreach (var elem in file.Statements.Except(fields).Except(functions))
            {
                this.Visit(elem);
            }
            Emitter.Return();
            var method = Emitter.CreateMethod();

            TypeBuilder.CreateType();
            AssemblyBuilder.SetEntryPoint(method);
        }

        public void Visit(FloatExpression expr)
        {
            Emitter.LoadConstant(expr.Value);
        }

        public void Visit(LongExpression expr)
        {
            Emitter.LoadConstant(expr.Value);
        }

        public void Visit(SymbolExpression expr)
        {
            //TODO: the hard stuff

            if(Fields.TryGetValue(expr.Name, out var field))
            {
                Emitter.LoadField(field);
            }
            else if(Constants.TryGetValue(expr.Name, out var constantExpression))
            {
                Visit(constantExpression);
            }
        }

        public void Visit(StringExpression expr)
        {
            Emitter.LoadConstant(expr.Value);
        }

        public void Visit(ListExpression list)
        {
            var function = list.Elements[0] as SymbolExpression;
            var arguments = list.Elements.Skip(1).ToList();

            // find method and emit 'call' instruction
            if(function.Name == "new")
            {
                var typeName = arguments[0];
                var constructorParameters = arguments.Skip(1).ToArray();
                foreach (var param in constructorParameters)
                {
                    this.Visit(param);
                }

                var typeToInstantiate = Types[typeName.ToString()];
                var constructorParameterTypes = arguments.Select(arg => ConcreteType(arg.Type)).ToArray();
                var constructor = typeToInstantiate.GetConstructor(constructorParameterTypes);
                Emitter.NewObject(constructor);
            }
            else if(Functions.TryGetValue(function.Name, out var localMethod))
            {
                foreach (var elem in arguments)
                {
                    this.Visit(elem);
                }
                Emitter.Call(localMethod);
            }
            else
            {
                foreach (var elem in arguments)
                {
                    this.Visit(elem);
                }
                var externalMethod = GetExternalMethod(function, arguments);
                Emitter.Call(externalMethod);
            }
        }

        private static MethodInfo GetExternalMethod(SymbolExpression function, List<ITypedExpression> arguments)
        {
            var functionNameIndex = function.Name.LastIndexOf('.');
            return Type
                .GetType(function.Name.Substring(0, functionNameIndex))
                .GetMethod(
                    function.Name.Substring(functionNameIndex + 1),
                    arguments.Select(ResolveType).ToArray()
                );
        }

        private static Type ResolveType(ITypedExpression arg)
        {
            return arg.Type is TypeVariable t ? throw new InvalidOperationException("unresolved type") :
                   arg.Type is ConcreteType c ? c.Type :
                   throw new ArgumentException("unknown class " + arg.Type.GetType().Name);
        }

        public void Visit(BooleanExpression expr)
        {
            throw new NotImplementedException();
        }

        public void Visit(VectorExpression expr)
        {
            throw new NotImplementedException();
        }

        public void Visit(SetExpression expr)
        {
            throw new NotImplementedException();
        }

        public void Visit(DefExpression expr)
        {
            var type = ResolveType(expr.Symbol);
            // literal == const, initonly == readonly
            var literalOrInitOnly = expr.Body is ILiteralExpression ? FieldAttributes.Literal : FieldAttributes.InitOnly;
            var fieldBuilder = TypeBuilder.DefineField(expr.Symbol.Name, type,
                FieldAttributes.Public | FieldAttributes.Static | literalOrInitOnly);

            if(expr.Body is ILiteralExpression literal)
            {
                fieldBuilder.SetConstant(literal.Value);
                Constants.Add(expr.Symbol.Name, expr.Body);
            }
            else
            {
                Visit(expr.Body);
                Emitter.StoreField(fieldBuilder);
                Fields.Add(expr.Symbol.Name, fieldBuilder);
            }
        }

        public void Visit(FunctionExpression expr)
        {
            Emitter = Emit.BuildStaticMethod(
                ConcreteType(expr.Type.ReturnType),
                expr.Type.ArgumentTypes.Select(ConcreteType).ToArray(),
                TypeBuilder,
                expr.Symbol.Name,
                PublicStatic);

            foreach (var elem in expr.Body)
            {
                this.Visit(elem);
            }

            Emitter.Return();
            var method = Emitter.CreateMethod();
            Functions.Add(expr.Symbol.Name, Emitter);
        }

        public Type ConcreteType(IType type)
        {
            return ((ConcreteType)type).Type;
        }

        public void Visit(DefTypeExpression expr)
        {
            var type = Record.CreateRecord(ModuleBuilder, expr);
            Types.Add(expr.Identifier, type);
        }
    }
}