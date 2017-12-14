using Donatello.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Donatello.TypeInference
{
    public interface IType { }
    public class TypeVariable : IType
    {
        public TypeVariable(string name)
        {
            Name = name;
        }

        // todo: use guid?
        public static int CurrentName = 'a' - 1;
        public static TypeVariable Next()
        {
            char name = (char)Interlocked.Increment(ref CurrentName);
            return new TypeVariable(name.ToString());
        }

        public string Name { get; }

        public override string ToString() => "'" + Name;

        public override bool Equals(object obj)
        {
            var variable = obj as TypeVariable;
            return variable != null &&
                   Name == variable.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
    public class ConcreteType : IType
    {
        public ConcreteType(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
        public string Name => Type.Name;

        public override bool Equals(object obj)
        {
            var type = obj as ConcreteType;
            return type != null &&
                   EqualityComparer<Type>.Default.Equals(Type, type.Type);
        }

        public override int GetHashCode()
        {
            return 2049151605 + EqualityComparer<Type>.Default.GetHashCode(Type);
        }

        public override string ToString() => Name;
    }

    public class FunctionType : IType
    {
        public FunctionType(IEnumerable<IType> argTypes, IType returnType)
        {
            ArgumentTypes = argTypes.ToArray();
            ReturnType = returnType;
        }

        public IType[] ArgumentTypes { get; }
        public IType ReturnType { get; }

        public override bool Equals(object obj)
        {
            var type = obj as FunctionType;
            return type != null &&
                   ArgumentTypes.SequenceEqual(type.ArgumentTypes, EqualityComparer<IType>.Default) &&
                   EqualityComparer<IType>.Default.Equals(ReturnType, type.ReturnType);
        }

        public override int GetHashCode()
        {
            var hashCode = -1349845170;
            hashCode = hashCode * -1521134295 + 
                ArgumentTypes.Aggregate(-17, (hash, type) => hash * -17 + type.GetHashCode());
            hashCode = hashCode * -1521134295 + EqualityComparer<IType>.Default.GetHashCode(ReturnType);
            return hashCode;
        }

        public override string ToString() =>
            string.Join(" → ", ArgumentTypes.Select(t => t.ToString()).Append(ReturnType.ToString()));
    }

    class TypeEnvironment
    {
        public static Dictionary<string, IType> Current = new Dictionary<string, IType>();


        public static IType GetTypeForName(string name)
        {
            if(!Current.TryGetValue(name, out IType type))
            {
                type = TypeVariable.Next();
                Current[name] = type;
            }
            return type;
        }
    }
}
