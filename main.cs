using System.Data;

public class RolePredicateAsoz
{
    public static int Main(string[] args)
    {
        var asozRoles = new AsozReadRoles();
        int error = asozRoles.ReadJson(".asozres");
        if (error == 0)
        {
            string[] roles = asozRoles.GetRoles();
            var rolePredicator = new RoleExpressionEvaluator();
            bool res;
            try
            {
                res = rolePredicator.Evaluate(args[0], roles);
            } catch (SyntaxErrorException)
            {
                return -3;
            }
            if (res)
            {
                return 1;
            }
            return 0;
        }
        else
        {
            return error;
        }
    }
}