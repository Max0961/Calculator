using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationLibrary
{
    class Sinus : Operation
    {
        public Sinus()
        {
            Priority = 1;
            Arity = 1;
            Type = Type.Prefix;
            Symbol = "sin";
            Group = Group.Geometric;
        }

        public override decimal Execute(params decimal[] operands)
        {
            return (decimal) Math.Sin((double) operands[0] * Math.PI / 180);
        }
    }
}
