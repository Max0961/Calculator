
namespace OperationLibrary
{
    public class Summation : Operation
    {
        public Summation()
        {
            Priority = 0;
            Arity = 2;
            Type = Type.Infix;
            Commutativity = true;
            Symbol = "+";
            Group = Group.Arithmetic;
        }

        public override decimal Execute(params decimal[] operands)
        {
            return operands[0] + operands[1];
        }
    }
}
