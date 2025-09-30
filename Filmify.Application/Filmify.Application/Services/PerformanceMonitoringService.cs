using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Filmify.Application.Services;

public interface IPerformanceMonitoringService
{
    IDisposable StartTimer(string operationName);
    void RecordMetric(string metricName, double value);
    void IncrementCounter(string counterName);
}

public class PerformanceMonitoringService : IPerformanceMonitoringService
{
    private readonly ILogger<PerformanceMonitoringService> _logger;
    private readonly Dictionary<string, List<double>> _metrics = new();
    private readonly Dictionary<string, int> _counters = new();

    public PerformanceMonitoringService(ILogger<PerformanceMonitoringService> logger)
    {
        _logger = logger;
    }

    public IDisposable StartTimer(string operationName)
    {
        return new PerformanceTimer(operationName, this, _logger);
    }

    public void RecordMetric(string metricName, double value)
    {
        lock (_metrics)
        {
            if (!_metrics.ContainsKey(metricName))
                _metrics[metricName] = new List<double>();
            
            _metrics[metricName].Add(value);
        }
    }

    public void IncrementCounter(string counterName)
    {
        lock (_counters)
        {
            if (!_counters.ContainsKey(counterName))
                _counters[counterName] = 0;
            
            _counters[counterName]++;
        }
    }

    private class PerformanceTimer : IDisposable
    {
        private readonly string _operationName;
        private readonly PerformanceMonitoringService _monitor;
        private readonly ILogger<PerformanceMonitoringService> _logger;
        private readonly Stopwatch _stopwatch;

        public PerformanceTimer(string operationName, PerformanceMonitoringService monitor, ILogger<PerformanceMonitoringService> logger)
        {
            _operationName = operationName;
            _monitor = monitor;
            _logger = logger;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            var duration = _stopwatch.ElapsedMilliseconds;
            
            _monitor.RecordMetric($"operation_duration_{_operationName}", duration);
            _logger.LogInformation("Operation {OperationName} completed in {Duration}ms", _operationName, duration);
        }
    }
}
