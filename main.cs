using System;
using System.Data;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;


class AsozData
{
    public string assistErrorCode { get; set; }
    public string assistErrorMessage { get; set; }
    public IList<Dictionary<string, string>> assistData { get; set; }
}

public class RolePredicateAsoz2
{
    static async Task<int> Main(string[] args)
    {
        string url = args[0];
        using HttpClient client = new HttpClient();
        AsozData asozRes;
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();     // Throw if not 200-299

            string json = await response.Content.ReadAsStringAsync();

            // Экранируем кавычки внутри ролей
            json = Regex.Replace(json, @"
    (?<![:,{][\s]*)   # не после : или , { (с возможными пробелами)
    ""               # сама кавычка
    (?![\s]*[:,}])    # не перед : или } или ,
", "\\\"", RegexOptions.IgnorePatternWhitespace);

            asozRes = JsonSerializer.Deserialize<AsozData>(json);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 1;   // Ошибка HTTP
        }

        // Проверяем наличие ошибки Asoz API
        if (asozRes.assistErrorCode != "0")
        {
            Console.Error.WriteLine(asozRes.assistErrorMessage);
            return 2;
        }

        var assistData = asozRes.assistData;
        string[] roles = assistData.Select(column => column["assistIsDescShort"]).ToArray() ?? Array.Empty<string>();

        var rolePredicator = new RoleExpressionEvaluator();
        try
        {
            bool res = rolePredicator.Evaluate(args.Skip(1).ToArray(), roles);
            Console.WriteLine(res);
        } catch (SyntaxErrorException ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 4;
        }
        return 0;
    }
}