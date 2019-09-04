
namespace OperationLibrary
{
    public class Division : Operation
    {
        public Division()
        {
            Priority = 2;
            Arity = 2;
            Type = Type.Infix;
            LeftAssociativity = true;
            Commutativity = false;
            Symbol = "/";
            Group = Group.Arithmetic;
        }

        public override decimal Execute(params decimal[] operands)
        {
            return operands[0] / operands[1];
        }
    }
}
