using System;

namespace OperationLibrary
{
    class Power : Operation
    {
        public Power()
        {
            Priority = 3;
            Arity = 2;
            Type = Type.Infix;
            Commutativity = false;
            Symbol = "^";
            Group = Group.Arithmetic;
        }

        public override decimal Execute(params decimal[] operands)
        {
            return (decimal) Math.Pow((double) operands[0], (double)operands[1]);
        }
    }
}
