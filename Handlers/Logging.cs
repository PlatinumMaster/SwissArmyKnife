using System;
using System.IO;

namespace SwissArmyKnife.Avalonia.Handlers; 

public static class Logging {
    private static TextWriter loggerStream;
    private static string logFileName;

    private static void Log(string Message) {
        using (loggerStream = File.AppendText(logFileName)) {
            loggerStream.WriteLine($"[{DateTime.Now}] {Message}");
            loggerStream.Flush();
            loggerStream.Close();
        }
    }

    public static void LogWarning(string Message) => Log($"<WARNING>: {Message}"); 
    public static void LogError(string Message) => Log($"<ERROR>: {Message}"); 
    public static void LogStandard(string Message) => Log($"{Message}"); 

    public static void StopLogger() => loggerStream.Close();

    public static void InitializeLogger() {
        string baseName = $"SAKLog-{DateTime.Today.Year}{DateTime.Today.Month}{DateTime.Today.Day}";
        int sessionNumber = 0;
        
        Directory.CreateDirectory("Logs");
        while (File.Exists(logFileName = Path.Combine("Logs", $"{baseName}-{sessionNumber}.log"))) {
            sessionNumber++;
        }
        Log($"# SwissArmyKnife - (Commit Version Here) - Log File");
    }
    
}