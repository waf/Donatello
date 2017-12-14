using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Sigil;
using Donatello.Ast;
using Sigil.NonGeneric;
using Donatello.TypeInference;

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
            // TODO: now, we only support one method per file, since we don't actually have a way to declare methods
            Emitter = Emit.BuildStaticMethod(typeof(void), new[] { typeof(string[]) }, TypeBuilder, "Main", PublicStatic);

            foreach (var elem in file.Statements)
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
            var function = list.Elements[0] as SymbolUntypedExpression;
            var arguments = list.Elements.Skip(1).ToList();

            foreach (var elem in arguments)
            {
                this.Visit(elem);
            }

            // find method and emit 'call' instruction
            var functionNameIndex = function.Name.LastIndexOf('.');
            var method = Type
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
            Emitter.Call(method);
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
            throw new NotImplementedException();
        }
    }
}