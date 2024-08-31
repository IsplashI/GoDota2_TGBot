using LibreHardwareMonitor.Hardware;
using System.Diagnostics;
using System.Threading;

public class PowerUsageMonitor : IDisposable
{
    private Stopwatch _stopwatch;
    private readonly Computer _computer;
    private static DateTime _startTime;
    private static double _totalEnergyUsed; // To track the total energy used
    private Dictionary<ISensor, SensorData> _sensorData;

    private static PowerUsageMonitor _instance;
    public static PowerUsageMonitor Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PowerUsageMonitor();
            }
            return _instance;
        }
    }

    public PowerUsageMonitor()
    {
        _computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = true,
            IsBatteryEnabled = true,
            IsNetworkEnabled = true,
            IsStorageEnabled = true,
            IsPsuEnabled = true

        };
        _computer.Open();
        _stopwatch = new Stopwatch();
        _totalEnergyUsed = 0.0;
        _sensorData = new Dictionary<ISensor, SensorData>();

        if (!HasPowerSensors())
        {
            Console.WriteLine("No power sensors available. Exiting...");
            return; // Завершуємо роботу програми, якщо немає датчиків
        }
    }

    public void StartMonitoring()
    {
        _startTime = DateTime.Now;
        _stopwatch.Restart();
        _totalEnergyUsed = 0.0;
    }

    public string GetCurrentPowerUsage()
    {
        var totalPower = GetTotalPowerUsage();
        return $"Current power usage: {totalPower:F2} watts";
    }
    private bool HasPowerSensors()
    {
        foreach (var hardware in _computer.Hardware)
        {
            hardware.Update();
            foreach (var sensor in hardware.Sensors)
            {
                if (sensor.SensorType == SensorType.Power && sensor.Value.HasValue)
                {
                    return true; // Знайдено принаймні один датчик потужності
                }
            }
        }
        return false; // Не знайдено жодного датчика потужності
    }
    public void UpdatePowerUsage()
    {
        // Update the total energy used since the last call
        var currentPower = GetTotalPowerUsage(); // Get the current power usage in watts
        var elapsedTimeInSeconds = (DateTime.Now - _startTime).TotalSeconds;

        // Calculate energy used in watt-seconds and convert to watt-hours
        _totalEnergyUsed += currentPower * elapsedTimeInSeconds / 3600.0; // Update total energy used

        // Reset start time for next interval
        _startTime = DateTime.Now;
    }
    public string GetTotalEnergyUsed()
    {
        return $"Total energy used: {_totalEnergyUsed:F4} watt-hours";
    }
    private double GetTotalPowerUsage()
    {
        double totalPower = 0.0;

        foreach (var hardware in _computer.Hardware)
        {
            hardware.Update();
            foreach (var sensor in hardware.Sensors)
            {
                if (sensor.SensorType == SensorType.Power)
                {
                    if (sensor.Value.HasValue && sensor.Value.Value != 0)
                    {
                        if (!_sensorData.ContainsKey(sensor))
                        {
                            _sensorData[sensor] = new SensorData();
                        }

                        // Оновлення даних сенсора
                        _sensorData[sensor].Sum += sensor.Value.Value;
                        _sensorData[sensor].Count++;

                        totalPower += sensor.Value.Value;
                    }
                    else if (_sensorData.TryGetValue(sensor, out SensorData data) && data.Count > 0 && sensor.Value.Value == 0)
                    {
                        totalPower += data.Sum / data.Count;
                    }
                }
            }
        }

        return totalPower;
    }
    public List<string> GetSensorInformation()
    {
        var sensorInfo = new List<string>();

        foreach (var hardware in _computer.Hardware)
        {
            hardware.Update();
            foreach (var sensor in hardware.Sensors)
            {
                if (sensor.SensorType == SensorType.Power && sensor.Value.HasValue)
                {
                    string info = $"Hardware: {hardware.HardwareType}, Sensor: {sensor.Name}, Value: {sensor.Value:F2}W";
                    sensorInfo.Add(info);
                }
            }
        }

        return sensorInfo;
    }

    public void Dispose()
    {
        _computer.Close();
    }

    static void RunPowerUsageMonitor()
    {
        
        Instance.StartMonitoring(); // Початок моніторингу

        // Виводимо інформацію про датчики, з яких вдалося зчитати дані
        var sensorInfo = Instance.GetSensorInformation();
        Console.WriteLine("Detected Sensors:");
        foreach (var info in sensorInfo)
        {
            Console.WriteLine(info);
        }
        while (true)
        {
            Thread.Sleep(10000); // Затримка 10 секунди

            Instance.UpdatePowerUsage(); // Оновлення даних про споживання

            string currentUsage = Instance.GetCurrentPowerUsage();
            Console.WriteLine(currentUsage);
            string totalEnergyUsed = Instance.GetTotalEnergyUsed();
            Console.WriteLine(totalEnergyUsed);            
        }        
    }
    public static async Task RunPowerMonitors()
    {
        // Запускаємо метод `RunPowerUsageMonitor` асинхронно в окремому потоці
        Task powerMonitorTask = Task.Run(() => RunPowerUsageMonitor());
        await Task.CompletedTask;
    }
    public static string GetPowerUsageString()
    {
        
        var sensorInfo = Instance.GetSensorInformation();
        string message = "Detected Sensors:\n";
        foreach (var info in sensorInfo)
        {
            message += info + "\n";
        }

        // Now you can print or use the message variable
        Console.WriteLine(message);

        Instance.UpdatePowerUsage();
        return $"{message}\n{Instance.GetCurrentPowerUsage()}\n{Instance.GetTotalEnergyUsed()}";
    }
    private class SensorData
    {
        public double Sum { get; set; } = 0.0;
        public int Count { get; set; } = 0;
    }
}
