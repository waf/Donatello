using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Sigil;
using Donatello.Ast;
using Sigil.NonGeneric;
using Donatello.TypeInference;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Donatello.Emitter
{
    class ClassEmitter : IVisitor
    {
        static MethodAttributes PublicStatic = MethodAttributes.HideBySig | MethodAttributes.Static | MethodAttributes.Public;

        readonly AssemblyBuilder AssemblyBuilder;
        readonly ModuleBuilder ModuleBuilder;
        readonly TypeBuilder TypeBuilder;

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
            var functions = file.Statements.OfType<FunctionExpression>().ToList();
            foreach (var function in functions)
            {
                Visit(function);
            }

            // TODO: now, we only support one method per file, since we don't actually have a way to declare methods
            Emitter = Emit.BuildStaticMethod(typeof(void), new[] { typeof(string[]) }, TypeBuilder, "Main", PublicStatic);

            foreach (var elem in file.Statements.Except(functions))
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
            throw new NotImplementedException();
        }

        public void Visit(StringExpression expr)
        {
            Emitter.LoadConstant(expr.Value);
        }

        public void Visit(ListExpression list)
        {
            var function = list.Elements[0] as SymbolExpression;
            var arguments = list.Elements.Skip(1).ToList();

            foreach (var elem in arguments)
            {
                this.Visit(elem);
            }

            // find method and emit 'call' instruction
            if(Functions.TryGetValue(function.Name, out var localMethod))
            {
                Emitter.Call(localMethod);
            }
            else
            {
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
                    arguments.Select(arg =>
                        arg.Type is TypeVariable t ?
                            throw new InvalidOperationException("unresolved type") :
                        arg.Type is ConcreteType c ?
                            c.Type :
                        throw new ArgumentException("unknown class " + arg.Type.GetType().Name)
                    ).ToArray()
                );
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
            throw new NotImplementedException();
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
            var instrs = Emitter.Instructions();
            Functions.Add(expr.Symbol.Name, Emitter);
        }

        IDictionary<string, Emit> Functions = new Dictionary<string, Emit>();

        public Type ConcreteType(IType type)
        {
            return ((ConcreteType)type).Type;
        }

        public void Visit(DefTypeExpression expr)
        {
            var type = Record.CreateRecord(ModuleBuilder, expr);
        }

    }
}