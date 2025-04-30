using System;

namespace ArbeitInventur
{
    /// <summary>
    /// Interface for file system watchers, providing a standardized way to monitor and react to file events.
    /// </summary>
    public interface IFileWatcher : IDisposable
    {
        event Action<string> FileEvent;
        void StartWatching();
        void StopWatching();
    }
}