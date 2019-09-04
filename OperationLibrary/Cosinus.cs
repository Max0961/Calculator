using System;

namespace OperationLibrary
{
    class Cosinus : Operation
    {
        public Cosinus()
        {
            Priority = 2;
            Arity = 1;
            Type = Type.Prefix;
            Symbol = "cos";
            Group = Group.Geometric;
        }

        public override decimal Execute(params decimal[] operands)
        {
            return (decimal) Math.Cos((double) operands[0] * Math.PI / 180);
        }
    }
}
