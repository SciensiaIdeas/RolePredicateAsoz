using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;


class AsozReadRoles
{
    public JObject asozRes;
    public IList<Dictionary<string, string>> assistData;

    // Чтение JSON с файла
    public int ReadJson(string filepath)
    {
        int error = 0;

        try
        {
            using StreamReader r = new StreamReader(filepath);
            string json = r.ReadToEnd();

            // Экранируем кавычки внутри ролей
            json = Regex.Replace(json, @"
            (?<![:,{][\s]*)   # не после : или , { (с возможными пробелами)
            ""               # сама кавычка
            (?![\s]*[:,}])    # не перед : или } или ,
        ", "\\\"", RegexOptions.IgnorePatternWhitespace);

            asozRes = JObject.Parse(json);
            string errorCode = asozRes["assistErrorCode"]?.ToString();

            // Проверяем отсутствие ошибки Asoz API
            if (errorCode == "0")
            {
                assistData = asozRes["assistData"]?.ToObject<IList<Dictionary<string, string>>>();
                error = 0;
            }
            else
            {
                error = -1;
            }
        }
        catch (Exception)
        {
            // Файл не существует или имеет неверную структуру
            error = -2;
        }

        return error;
    }

    // Метод получение столбца ИС-ИР (кратко)
    public string[] GetRoles()
    {
        return assistData?.Select(column => column["assistIsDescShort"]).ToArray() ?? Array.Empty<string>();
    }
}