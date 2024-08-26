using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32;

namespace GoDota2_Bot
{
    public class SoftwareInfo
    {
        /// <summary>
        /// Retrieves a list of currently running processes with their memory usage.
        /// </summary>
        /// <returns>A formatted string containing the names and memory usage of running processes.</returns>
        public static string GetRunningProcesses()
        {
            var sb = new StringBuilder();
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                try
                {
                    sb.AppendLine($"Process: {process.ProcessName}, Memory Usage: {(process.WorkingSet64 / (1024 * 1024)).ToString("F2")} MB");
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Process: {process.ProcessName}, Error: {ex.Message}");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Retrieves a list of installed applications by reading from the Windows registry.
        /// </summary>
        /// <returns>A formatted string containing the names of installed applications.</returns>
        public static string GetInstalledApplications()
        {
            var sb = new StringBuilder();
            var uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var baseKey = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                if (baseKey != null)
                {
                    foreach (var subKey in baseKey.GetSubKeyNames())
                    {
                        using (var appKey = baseKey.OpenSubKey(subKey))
                        {
                            var displayName = appKey?.GetValue("DisplayName") as string;
                            if (!string.IsNullOrEmpty(displayName))
                            {
                                sb.AppendLine(displayName);
                            }
                        }
                    }
                }
            }
            return sb.ToString();
        }
    }
}
