using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using OpenHardwareMonitor.Hardware;

namespace GoDota2_Bot
{
    public class SystemMetrics
    {
        public static string CheckSystemMetrics()
        {
            var sb = new StringBuilder();

            // Check OS details
            sb.AppendLine($"Operating System: {GetOperatingSystem()}");
            sb.AppendLine($"OS Architecture: {GetOsArchitecture()}");

            // Check system information
            sb.AppendLine($"Computer Name: {Environment.MachineName}");
            sb.AppendLine($"User Name: {Environment.UserName}");
            sb.AppendLine($"Domain Name: {Environment.UserDomainName}");

            // Check CPU usage
            var cpuUsage = GetCpuUsage();
            sb.AppendLine($"CPU Usage: {cpuUsage.ToString("F2")}%");

            // Check memory usage
            var memoryUsage = GetMemoryUsage();
            sb.AppendLine($"Memory Usage: {memoryUsage.ToString("F2")}%");

            // Check disk usage
            var diskUsage = GetDiskUsage();
            sb.AppendLine($"Disk Usage: {diskUsage.ToString("F2")}%");

            // Check system uptime
            var uptime = GetSystemUptime();
            sb.AppendLine($"System Uptime: {uptime}");

            // Check network usage
            var (sentBytes, receivedBytes) = GetNetworkUsage();
            sb.AppendLine($"Network Sent: {(sentBytes / 1024.0 / 1024.0).ToString("F2")} MB");
            sb.AppendLine($"Network Received: {(receivedBytes / 1024.0 / 1024.0).ToString("F2")} MB");

            // Check CPU temperature (if supported)
            var cpuTemp = GetCpuTemperature();
            sb.AppendLine($"CPU Temperature: {cpuTemp.ToString("F2")} °C");

            // Check network bandwidth utilization
            var (uploadSpeed, downloadSpeed) = GetNetworkBandwidthUsage();
            sb.AppendLine($"Network Upload Speed: {uploadSpeed.ToString("F2")} KB/s");
            sb.AppendLine($"Network Download Speed: {downloadSpeed.ToString("F2")} KB/s");

            // Get number of processes and threads
            var processCount = GetProcessCount();
            var threadCount = GetThreadCount();
            sb.AppendLine($"Number of Processes: {processCount}");
            sb.AppendLine($"Number of Threads: {threadCount}");

            // Check drive-specific information
            GetDetailedDriveInfo(sb);

            // Check battery information (if applicable)
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var batteryInfo = GetBatteryInfo();
                sb.AppendLine($"Battery Status: {batteryInfo.Item1}");
                sb.AppendLine($"Battery Percentage: {batteryInfo.Item2}%");
            }

            // Logical and Physical CPU Cores
            sb.AppendLine($"Logical CPU Cores: {Environment.ProcessorCount}");
            sb.AppendLine($"Physical CPU Cores: {GetPhysicalCpuCores()}");

            // BIOS Information
            sb.AppendLine($"BIOS Version: {GetBiosVersion()}");
            sb.AppendLine($"BIOS Manufacturer: {GetBiosManufacturer()}");

            // Graphics Card Information
            GetGraphicsCardInfo(sb);

            // GPU Usage
            sb.AppendLine($"GPU Usage: {GetGpuUsage().ToString("F2")}%");

            // Sound Device Information
            GetSoundDeviceInfo(sb);

            // Windows Update Status
            sb.AppendLine($"Last Windows Update: {GetLastWindowsUpdate()}");

            // Firewall Status
            sb.AppendLine($"Firewall Enabled: {IsFirewallEnabled()}");

            // Network Adapter Details
            GetNetworkAdapterDetails(sb);

            // CPU Core Utilization
            GetCpuCoreUtilization(sb);

            // Network ping to a specific host
            var pingTime = GetNetworkPing("godota2.com");
            sb.AppendLine($"Ping to godota2.com: {pingTime} ms");

            // Add resource usage of the current process
            var (processCpuUsage, processMemoryUsage) = GetProcessResourceUsage();
            sb.AppendLine($"Process CPU Usage: {processCpuUsage.ToString("F2")}%");
            sb.AppendLine($"Process Memory Usage: {(processMemoryUsage / (1024.0 * 1024.0)).ToString("F2")} MB");

            return sb.ToString();
        }

        public static string GetOperatingSystem()
        {
            return Environment.OSVersion.VersionString;
        }

        public static string GetOsArchitecture()
        {
            return RuntimeInformation.ProcessArchitecture.ToString();
        }

        public static float GetCpuUsage()
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            Thread.Sleep(1000); // Wait a second to get a real value
            return cpuCounter.NextValue();
        }

        public static string GetBiosVersion()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                foreach (var obj in searcher.Get())
                {
                    return obj["SMBIOSBIOSVersion"].ToString();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or unsupported scenarios
            }

            return "Unknown";
        }

        public static string GetBiosManufacturer()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                foreach (var obj in searcher.Get())
                {
                    return obj["Manufacturer"].ToString();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or unsupported scenarios
            }

            return "Unknown";
        }

        public static void GetGraphicsCardInfo(StringBuilder sb)
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (var obj in searcher.Get())
                {
                    sb.AppendLine($"Graphics Card: {obj["Name"]}");
                    sb.AppendLine($"  Driver Version: {obj["DriverVersion"]}");
                    sb.AppendLine($"  Status: {obj["Status"]}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or unsupported scenarios
            }
        }

        public static float GetGpuUsage()
        {
            try
            {
                var hardwareMonitor = new Computer
                {
                    GPUEnabled = true
                };
                hardwareMonitor.Open();

                foreach (var hardware in hardwareMonitor.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAti)
                    {
                        hardware.Update();

                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core")
                            {
                                return sensor.Value.GetValueOrDefault();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or unsupported scenarios
            }

            return 0.0f;
        }

        public static float GetMemoryUsage()
        {
            var availableMemoryMb = new PerformanceCounter("Memory", "Available MBytes").NextValue();
            var committedMemoryBytes = new PerformanceCounter("Memory", "Committed Bytes").NextValue();
            var totalMemoryMb = availableMemoryMb + (committedMemoryBytes / (1024 * 1024)); // Convert to megabytes

            if (totalMemoryMb == 0)
            {
                return 0; // Avoid division by zero
            }

            var usedMemoryMb = committedMemoryBytes / (1024 * 1024);
            return (float)(usedMemoryMb / totalMemoryMb * 100);
        }

        public static void GetSoundDeviceInfo(StringBuilder sb)
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SoundDevice");
                foreach (var obj in searcher.Get())
                {
                    sb.AppendLine($"Sound Device: {obj["Name"]}");
                    sb.AppendLine($"  Status: {obj["Status"]}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or unsupported scenarios
            }
        }

        public static float GetDiskUsage()
        {
            var drives = DriveInfo.GetDrives();
            float totalUsage = 0;
            int driveCount = 0;

            foreach (var drive in drives)
            {
                if (drive.IsReady)
                {
                    var totalSpace = drive.TotalSize;
                    var usedSpace = totalSpace - drive.AvailableFreeSpace;
                    totalUsage += (float)(usedSpace / (double)totalSpace * 100);
                    driveCount++;
                }
            }

            return driveCount > 0 ? totalUsage / driveCount : 0;
        }

        public static TimeSpan GetSystemUptime()
        {
            return TimeSpan.FromMilliseconds(Environment.TickCount64);
        }

        public static (long sentBytes, long receivedBytes) GetNetworkUsage()
        {
            long totalSentBytes = 0;
            long totalReceivedBytes = 0;

            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                var stats = nic.GetIPv4Statistics();
                totalSentBytes += stats.BytesSent;
                totalReceivedBytes += stats.BytesReceived;
            }

            return (totalSentBytes, totalReceivedBytes);
        }

        public static float GetCpuTemperature()
        {
            try
            {
                var searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                var temperatureObjects = searcher.Get();

                foreach (var obj in temperatureObjects)
                {
                    // Temperature is returned in tenths of a Kelvin
                    var temperature = Convert.ToDouble(obj["CurrentTemperature"].ToString());
                    // Convert Kelvin to Celsius
                    temperature = (temperature / 10.0) - 273.15;
                    return (float)temperature;
                }
            }
            catch (Exception ex)
            {
                return float.NaN; // Return NaN if not supported
            }

            return float.NaN; // Return NaN if not supported
        }

        public static (float uploadSpeed, float downloadSpeed) GetNetworkBandwidthUsage()
        {
            // Placeholder implementation for real-time bandwidth usage
            // Would require capturing network interface statistics over time to compute actual speed
            return (0.0f, 0.0f);
        }

        public static int GetProcessCount()
        {
            return Process.GetProcesses().Length;
        }

        public static int GetThreadCount()
        {
            return Process.GetCurrentProcess().Threads.Count;
        }

        public static void GetDetailedDriveInfo(StringBuilder sb)
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    sb.AppendLine($"Drive {drive.Name}");
                    sb.AppendLine($"  Total Size: {(drive.TotalSize / (1024.0 * 1024.0 * 1024.0)).ToString("F2")} GB");
                    sb.AppendLine($"  Free Space: {(drive.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0)).ToString("F2")} GB");
                }
            }
        }

        public static long GetNetworkPing(string host)
        {
            try
            {
                var ping = new Ping();
                var reply = ping.Send(host);
                return reply.Status == IPStatus.Success ? reply.RoundtripTime : -1;
            }
            catch (Exception ex)
            {
                return -1; // Return -1 on failure
            }
        }

        public static (string Status, int Percentage) GetBatteryInfo()
        {
            var batteryStatus = "Unknown";
            int batteryPercentage = -1;

            try
            {
                var searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_Battery");
                var batteryObjects = searcher.Get();

                foreach (var obj in batteryObjects)
                {
                    batteryStatus = obj["BatteryStatus"] switch
                    {
                        1 => "Charging",
                        2 => "Discharging",
                        3 => "Fully Charged",
                        4 => "Low",
                        5 => "Critical",
                        6 => "Charging and Low",
                        7 => "Charging and Critical",
                        8 => "Undefined",
                        _ => batteryStatus
                    };

                    batteryPercentage = Convert.ToInt32(obj["EstimatedChargeRemaining"]);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or unsupported scenarios
            }

            return (batteryStatus, batteryPercentage);
        }

        public static int GetPhysicalCpuCores()
        {
            try
            {
                var searcher = new ManagementObjectSearcher(@"root\CIMV2", "SELECT * FROM Win32_Processor");
                var processors = searcher.Get();

                int physicalCores = 0;
                foreach (var processor in processors)
                {
                    physicalCores += Convert.ToInt32(processor["NumberOfCores"]);
                }

                return physicalCores;
            }
            catch (Exception ex)
            {
                return 0; // Return 0 if unable to determine physical cores
            }
        }
        public static string GetLastWindowsUpdate()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_QuickFixEngineering");
                var updates = searcher.Get();

                foreach (var obj in updates)
                {
                    return obj["InstalledOn"]?.ToString() ?? "Unknown";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or unsupported scenarios
            }

            return "Unknown";
        }

        public static bool IsFirewallEnabled()
        {
            try
            {
                var searcher = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM FirewallProduct");
                foreach (var obj in searcher.Get())
                {
                    return Convert.ToBoolean(obj["Enabled"]);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions or unsupported scenarios
            }

            return false; // Default to false if not supported
        }

        public static void GetNetworkAdapterDetails(StringBuilder sb)
        {
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                sb.AppendLine($"Network Adapter: {nic.Description}");
                sb.AppendLine($"  MAC Address: {nic.GetPhysicalAddress()}");
                sb.AppendLine($"  Speed: {nic.Speed / (1024 * 1024)} Mbps");
                sb.AppendLine($"  Status: {nic.OperationalStatus}");
                var ipProps = nic.GetIPProperties();
                foreach (var ip in ipProps.UnicastAddresses)
                {
                    sb.AppendLine($"  IP Address: {ip.Address}");
                }
            }
        }

        public static void GetCpuCoreUtilization(StringBuilder sb)
        {
            try
            {
                int coreCount = Environment.ProcessorCount;
                for (int i = 0; i < coreCount; i++)
                {
                    var coreUsage = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                    coreUsage.NextValue();
                    Thread.Sleep(1000);
                    sb.AppendLine($"Core {i} Usage: {coreUsage.NextValue().ToString("F2")}%");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine("CPU Core Utilization not available.");
            }
        }
        public static (float CpuUsage, long MemoryUsage) GetProcessResourceUsage()
        {
            var process = Process.GetCurrentProcess();
            var cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
            cpuCounter.NextValue();
            Thread.Sleep(1000); // Wait a second to get a real value
            float cpuUsage = cpuCounter.NextValue();

            long memoryUsage = process.WorkingSet64; // Memory usage in bytes

            return (cpuUsage, memoryUsage);
        }
    }
}

