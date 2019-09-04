using System;

namespace OperationLibrary
{
    public enum Type { Prefix, Infix, Postfix }

    public enum Group { Geometric, Arithmetic }

    public abstract class Operation : IComparable<Operation>
    {
        public int Priority { get; protected set; }
        public int Arity { get; protected set; }
        public Type Type { get; protected set; }
        public bool LeftAssociativity { get; protected set; } = false;
        public bool Commutativity { get; protected set; }
        public string Symbol { get; protected set; }
        public Group Group { get; protected set; }

        public int CompareTo(Operation other)
        {
            int result = Group.CompareTo(other.Group);
            if (result == 0) result = Priority.CompareTo(other.Priority);
            return result;
        }

        public abstract decimal Execute(params decimal[] operands);

        public override string ToString()
        {
            return string.Format(
                "Операция \"{0}\"\t" +
                "символ: \"{1}\"\t" +
                "{2}-арная\t" +
                "{3}", this.GetType().Name, Symbol, Arity, Type);
        }
    }
}
