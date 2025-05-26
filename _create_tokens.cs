
// Используйте этот код в Robin (tokens - массив строк, expr - строка-выражение)
void tokenize(string expr)
{
     const string terminals = "&|~()";

    // Разбиваем выражение на токены (роли и терминалы)
    List<string> result = new List<string>();
    int i = 0;
    while (i < expr.Length)
    {
        if (char.IsWhiteSpace(expr[i]))
        {
            ++i;
        }
        else if (terminals.Contains(expr[i]))
        {
            result.Add(expr[i].ToString());
            ++i;
        }
        else
        {
            int start = i;
            while (i < expr.Length && !terminals.Contains(expr[i]))
            {
                ++i;
            }
            string word = expr.Substring(start, i - start);
            result.Add(string.Format("\"{0}\"", word.TrimEnd()));
        }
    }
    // _a_tokens = result;
}
