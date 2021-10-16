using System;
using Melanchall.DryWetMidi.Devices;
using System.Threading;
using System.Diagnostics;

public sealed class ThreadTickGenerator : TickGenerator
{
    private Thread _thread;
    private bool _isRunning;
    private bool _disposed;

    protected override void Start(TimeSpan interval)
    {
        if (_thread != null)
            return;

        _thread = new Thread(() =>
        {
            var stopwatch = new Stopwatch();
            var lastMs = 0L;

            stopwatch.Start();
            _isRunning = true;

            while (_isRunning)
            {
                var elapsedMs = stopwatch.ElapsedMilliseconds;
                if (elapsedMs - lastMs >= interval.TotalMilliseconds)
                {
                    GenerateTick();
                    lastMs = elapsedMs;
                }
            }
        });

        _thread.Start();
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _isRunning = false;
        }

        _disposed = true;
    }

    protected override void Stop()
    {
        throw new NotImplementedException();
    }
}