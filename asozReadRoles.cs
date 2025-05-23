using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;


class AsozReadRoles
{
    public JObject asozRes;
    public IList<Dictionary<string, string>> assistData;

    // ������ JSON � �����
    public int ReadJson(string filepath)
    {
        int error = 0;

        try
        {
            using StreamReader r = new StreamReader(filepath);
            string json = r.ReadToEnd();

            // ���������� ������� ������ �����
            json = Regex.Replace(json, @"
            (?<![:,{][\s]*)   # �� ����� : ��� , { (� ���������� ���������)
            ""               # ���� �������
            (?![\s]*[:,}])    # �� ����� : ��� } ��� ,
        ", "\\\"", RegexOptions.IgnorePatternWhitespace);

            asozRes = JObject.Parse(json);
            string errorCode = asozRes["assistErrorCode"]?.ToString();

            // ��������� ���������� ������ Asoz API
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
            // ���� �� ���������� ��� ����� �������� ���������
            error = -2;
        }

        return error;
    }

    // ����� ��������� ������� ��-�� (������)
    public string[] GetRoles()
    {
        return assistData?.Select(column => column["assistIsDescShort"]).ToArray() ?? Array.Empty<string>();
    }
}