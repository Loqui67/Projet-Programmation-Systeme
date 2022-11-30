﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Projet_Programmation_Système.developpement
{
    public static class JsonFileManager
    {
        private const string backupJobFileName = "SaveBackupJob.json";
        private static JsonSerializerOptions optionsWriteIndented = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private static void CreateJsonBackupJobFileIfNotExist()
        {
            if (!File.Exists(backupJobFileName)) 
            {
                List<BackupJob> backupJobs = new List<BackupJob>();
                using (FileStream fs = File.Create(backupJobFileName))
                {
                    for (int i = 1; i < 6; i++)
                    {
                        backupJobs.Add(new BackupJob { id = i.ToString(), name = "" });
                    }
                }
                WriteBackupJobToFile(backupJobs);
            }
        }
        
        public static void WriteBackupJobToFile(List<BackupJob> backupJobs) {
            if (File.Exists(backupJobFileName))
            {
                using (FileStream fs = File.OpenWrite(backupJobFileName))
                {
                    string jsonString = JsonSerializer.Serialize(backupJobs, optionsWriteIndented);
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
                    fs.Write(jsonBytes, 0, jsonBytes.Length);
                }
            }
            //File.WriteAllText(backupJobFileName, JsonSerializer.Serialize(backupJobs));
        }
        
        public static List<BackupJob>? ReadBackupJobFile()
        {
            CreateJsonBackupJobFileIfNotExist();
            using (FileStream fs = File.OpenRead(backupJobFileName))
            {
                byte[] jsonBytes = new byte[fs.Length];
                fs.Read(jsonBytes, 0, jsonBytes.Length);
                string jsonString = Encoding.UTF8.GetString(jsonBytes);
                return JsonSerializer.Deserialize<List<BackupJob>>(jsonString);
            }
            //return JsonSerializer.Deserialize<List<BackupJob>>(File.ReadAllText(backupJobFileName));
        }

        public static void WriteDailyLogToFile(Log dailyLog)
        {
            if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");

            List<Log>? logs;
            logs = ReadDailyLogFile();
            logs.Add(dailyLog);
            
            using (FileStream fs = File.Create(GetDailyFileName()))
            {

                string jsonString = JsonSerializer.Serialize(logs, optionsWriteIndented);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
                fs.Write(jsonBytes, 0, jsonBytes.Length);
                //File.WriteAllText(GetDailyFileName(), JsonSerializer.Serialize(logs));
            }
        }

        public static List<Log>? ReadDailyLogFile()
        {
            if (File.Exists(GetDailyFileName())) {
                using (FileStream fs = File.OpenRead(GetDailyFileName()))
                {
                    byte[] jsonBytes = new byte[fs.Length];
                    fs.Read(jsonBytes, 0, jsonBytes.Length);
                    string jsonString = Encoding.UTF8.GetString(jsonBytes);
                    return JsonSerializer.Deserialize<List<Log>>(jsonString);
                    //return JsonSerializer.Deserialize<List<Log>>(File.ReadAllText(GetDailyFileName()));
                }
            }
            return new List<Log>();

        }

        private static string GetDailyFileName()
        {
            return "logs/" + DateTime.Now.ToString("yyyy-MM-dd") + "-log.json";
        }
    }
}