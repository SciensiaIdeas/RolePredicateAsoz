using System.Diagnostics;

/*
void proccess()
{
    // Укажите путь к исполняемому файлу и аргументы
    // Подставьте ссылку
    // string[] args = _a_tokens.ToArray();
    string[] args = { "ex1", "ex2" };
    ProcessStartInfo startInfo = new ProcessStartInfo
    {
        FileName = "rolePredicate.Asoz.exe",
        Arguments = string.Format("http://example/ {0}", string.Join(' ', args)),
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    try
    {
        using Process process = new Process();
        process.StartInfo = startInfo;

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit(); // Ожидание завершения

        int exitCode = process.ExitCode;

        Console.WriteLine($"Exit Code: {exitCode}");
        Console.WriteLine("Standard Output:");
        Console.WriteLine(output);
        Console.WriteLine("Error Output:");
        Console.WriteLine(error);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Exception occurred while starting process:");
        Console.WriteLine(ex.Message);
    }
}

*/