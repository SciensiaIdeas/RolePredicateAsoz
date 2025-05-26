using System.Data;


public class RoleExpressionEvaluator
{
    private string[] rolesSet;
    private string[] tokens;
    private int pos;
    private const string terminals = "&|~()";

    public bool Evaluate(string[] tokens, string[] userRoles)
    {
        rolesSet = userRoles;
        this.tokens = tokens;
        pos = 0;
        bool res = parseExpression();
        return res;
    }

    private bool parseExpression()
    {
        // Внешнее выражение или внутри скобок (дизъюнкция)
        bool result = parseTerm();
        while (match("|"))
        {
            if (result)
            {
                skipTo("|");
                break;
            }
            result = result | parseTerm();
        }
        return result;
    }

    private bool parseTerm()
    {
        // конъюнкция
        bool result = parseFactor();
        while (match("&"))
        {
            if (!result)
            {
                skipTo("&");
                break;
            }
            result = result & parseFactor();
        }
        return result;
    }

    private bool parseFactor()
    {
       // отрицание или сама роль
        if (match("~"))
        {
            return !parseFactor();
        }
        else if (match("("))
        {
            bool result = parseExpression();
            if (!match(")"))
            {
                throw new SyntaxErrorException("Expected ')' in pos: " + pos.ToString());
            }
            return result;
        }
        else
        {
            return checkRole(expectIdentifier());
        }
    }

    private string expectIdentifier()
    {
        if (pos >= tokens.Length || tokens[pos] == "" || terminals.Contains(tokens[pos][0]))
        {
            throw new SyntaxErrorException("Expected role name at position " + pos.ToString());
        }
        return tokens[pos++];
    }

    private bool match(string expected)
    {
        if (pos < tokens.Length && tokens[pos] == expected)
        {
            ++pos;
            return true;
        }
        return false;
    }

    private void skipTo(string expected)
    {
        int n = tokens.Length - 1;
        while (pos < n && tokens[pos] != expected)
        {
            if (tokens[pos] == ")")
            {
                return;
            }
            ++pos;
        }
        ++pos;
    }

    private bool checkRole(string role)
    {
        // Проверка, что действующие роли содержат заданное ключевое слово
        foreach (string Role in rolesSet)
        {
            if (Role.Contains(role))
            {
                return true;
            }
        }
        return false;
    }
}