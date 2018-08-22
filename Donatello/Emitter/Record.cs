using Donatello.Ast;
using Sigil.NonGeneric;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Donatello.Emitter
{
    static class Record
    {
        private static CustomAttributeBuilder compilerGeneratedAttribute;
        private static CustomAttributeBuilder debuggerBrowsableAttribute;

        static Record()
        {
            compilerGeneratedAttribute = new CustomAttributeBuilder(
                typeof(CompilerGeneratedAttribute).GetConstructor(Type.EmptyTypes),
                new object[0]
            );
            debuggerBrowsableAttribute = new CustomAttributeBuilder(
                typeof(DebuggerBrowsableAttribute).GetConstructor(new[] { typeof(DebuggerBrowsableState) }),
                new object[] { DebuggerBrowsableState.Never }
            );
        }

        public static Type CreateRecord(ModuleBuilder moduleBuilder, DefTypeExpression expr)
        {
            TypeBuilder typeBuilder = BuildRecordType(moduleBuilder, expr);

            var constructor = DefineConstructor(typeBuilder, expr);
            var constructorIl = constructor.GetILGenerator();

            // call base class constructor
            constructorIl.Emit(OpCodes.Ldarg, (short)0); // push 'this' on the stack
            constructorIl.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));

            // create properties, and initialize them in the constructor
            for (int i = 0; i < expr.Properties.Count; i++)
            {
                var propertyDefinition = expr.Properties[i];
                FieldBuilder field = BuildFieldAndProperty(typeBuilder, propertyDefinition);
                InitializeFieldInConstructor(constructor, constructorIl, i, field, propertyDefinition.Identifier);
            }

            constructorIl.Emit(OpCodes.Ret);
            return typeBuilder.CreateType();
        }

        private static TypeBuilder BuildRecordType(ModuleBuilder moduleBuilder, DefTypeExpression expr)
        {
            return moduleBuilder.DefineType(expr.Identifier,
                TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed
                 | TypeAttributes.AutoClass | TypeAttributes.BeforeFieldInit | TypeAttributes.Serializable
                 | TypeAttributes.AutoLayout,
                typeof(object)
            );
        }

        public static ConstructorBuilder DefineConstructor(TypeBuilder typeBuilder, DefTypeExpression expr)
        {
            var constructorArguments = expr.Properties
                .Select(prop => Type.GetType("System." + prop.Type)) // this will need rework to refer to custom types
                .ToArray();
            var constructor = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.HasThis,
                constructorArguments
            );
            return constructor;
        }

        private static FieldBuilder BuildFieldAndProperty(TypeBuilder typeBuilder, Property prop)
        {
            Type propType = Type.GetType("System." + prop.Type);
            var field = typeBuilder.DefineField($"<{prop.Identifier}>k__BackingField", propType, FieldAttributes.Private | FieldAttributes.InitOnly);
            field.SetCustomAttribute(compilerGeneratedAttribute);
            field.SetCustomAttribute(debuggerBrowsableAttribute);

            var property = typeBuilder.DefineProperty(prop.Identifier,
                PropertyAttributes.HasDefault,
                propType,
                Type.EmptyTypes);

            // create property getter
            var getterImplementation = Emit.BuildMethod(propType, Type.EmptyTypes, typeBuilder, "get_" + prop.Identifier,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                CallingConventions.HasThis);
            getterImplementation.LoadArgument(0);  // Push "this" on the stack
            getterImplementation.LoadField(field); // Load the private field for this property
            getterImplementation.Return();
            var getter = getterImplementation.CreateMethod();
            getter.SetCustomAttribute(compilerGeneratedAttribute);
            property.SetGetMethod(getter);
            return field;
        }

        private static void InitializeFieldInConstructor(
            ConstructorBuilder constructor, ILGenerator constructorIl,
            int fieldIndex, FieldInfo field, string parameterName)
        {
            short parameterIndex = (short)(fieldIndex + 1);    // 0th parameter is `this`

            // assign constructor parameter to the field
            constructorIl.Emit(OpCodes.Ldarg, (short)0);       // push 'this' on the stack
            constructorIl.Emit(OpCodes.Ldarg, parameterIndex); // Push constructor parameter for this field
            constructorIl.Emit(OpCodes.Stfld, field);          // Set the private field to constructor argument

            // specify name of parameter
            constructor.DefineParameter(parameterIndex, ParameterAttributes.None, parameterName);
        }
    }
}
