using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Codex 与当前已打开 Unity 实例的轻量命令桥。
/// 通过写入 Temp/CodexCommands/command.txt 触发编辑器内方法执行。
/// </summary>
[InitializeOnLoad]
public static class CodexUnityCommandBridge
{
    private const string CommandFolder = "Temp/CodexCommands";
    private const string CommandFileName = "command.txt";
    private const string ResultFileName = "result.txt";
    private const string ReadyFileName = "ready.txt";
    private const string GenerateInventoryCommand = "generate_inventory_prefab";

    private static bool _isExecuting;
    private static double _nextPollTime;
    private static DateTime _lastCommandWriteTimeUtc = DateTime.MinValue;

    static CodexUnityCommandBridge()
    {
        WriteReadyFile();
        EditorApplication.update += PollCommandFile;
    }

    [MenuItem("Tools/UI/Codex/写入生成命令(Inventory)")]
    public static void WriteGenerateInventoryCommand()
    {
        string commandPath = GetCommandPath();
        Directory.CreateDirectory(Path.GetDirectoryName(commandPath) ?? CommandFolder);
        File.WriteAllText(commandPath, GenerateInventoryCommand);
        Debug.Log($"[CodexUnityCommandBridge] 已写入命令: {GenerateInventoryCommand}");
    }

    private static void PollCommandFile()
    {
        if (_isExecuting)
        {
            return;
        }

        if (EditorApplication.timeSinceStartup < _nextPollTime)
        {
            return;
        }

        _nextPollTime = EditorApplication.timeSinceStartup + 1.0d;

        if (EditorApplication.isCompiling || EditorApplication.isUpdating)
        {
            return;
        }

        string commandPath = GetCommandPath();
        if (!File.Exists(commandPath))
        {
            return;
        }

        DateTime writeTime = File.GetLastWriteTimeUtc(commandPath);
        if (writeTime <= _lastCommandWriteTimeUtc)
        {
            return;
        }

        string command = ReadTextSafe(commandPath).Trim();
        if (string.IsNullOrEmpty(command))
        {
            return;
        }

        _lastCommandWriteTimeUtc = writeTime;
        _isExecuting = true;
        EditorApplication.delayCall += () => ExecuteCommand(command);
    }

    private static void ExecuteCommand(string command)
    {
        string resultPath = GetResultPath();
        Directory.CreateDirectory(Path.GetDirectoryName(resultPath) ?? CommandFolder);

        try
        {
            if (command == GenerateInventoryCommand)
            {
                UGUIInventoryPrefabGenerator.GenerateFromBatch();
                File.WriteAllText(resultPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | OK | {command}");
                Debug.Log($"[CodexUnityCommandBridge] 执行成功: {command}");
            }
            else
            {
                File.WriteAllText(resultPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | UNKNOWN | {command}");
                Debug.LogWarning($"[CodexUnityCommandBridge] 未知命令: {command}");
            }
        }
        catch (Exception ex)
        {
            File.WriteAllText(resultPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | ERROR | {command} | {ex.Message}");
            Debug.LogError($"[CodexUnityCommandBridge] 执行失败: {command}\n{ex}");
        }
        finally
        {
            _isExecuting = false;
        }
    }

    private static string GetCommandPath()
    {
        return Path.Combine(GetProjectRoot(), "Temp", "CodexCommands", CommandFileName);
    }

    private static string GetResultPath()
    {
        return Path.Combine(GetProjectRoot(), "Temp", "CodexCommands", ResultFileName);
    }

    private static string GetReadyPath()
    {
        return Path.Combine(GetProjectRoot(), "Temp", "CodexCommands", ReadyFileName);
    }

    private static string GetProjectRoot()
    {
        return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
    }

    private static string ReadTextSafe(string filePath)
    {
        try
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        catch
        {
            return string.Empty;
        }
    }

    private static void WriteReadyFile()
    {
        try
        {
            string readyPath = GetReadyPath();
            Directory.CreateDirectory(Path.GetDirectoryName(readyPath) ?? CommandFolder);
            File.WriteAllText(readyPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | READY");
            Debug.Log("[CodexUnityCommandBridge] 桥接已激活，等待命令。");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"[CodexUnityCommandBridge] 写入 ready 文件失败: {ex.Message}");
        }
    }
}
