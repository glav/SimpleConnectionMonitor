using System;
using System.Timers;

namespace Connectionmonitor
{
    public class Monitor : IDisposable
    {
        private AuditLogger _logger;
        private EndpointTester _tester;

        private Timer _timer = null;
        private ConnectionState _currentState;
        private int _intervalInSeconds = 5;

        public Monitor(AuditLogger logger, EndpointTester tester, int intervalInSeconds)
        {
            _logger = logger;
            _tester = tester;
            _intervalInSeconds = intervalInSeconds;
        }

        public void StartMonitoring()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            _logger.LogStart();
            _timer = new Timer(_intervalInSeconds * 1000);
            _timer.Elapsed += TimerFired;
            _timer.Start();
        }

        private void TimerFired(object sender, ElapsedEventArgs e)
        {
            var testedState = _tester.IsEndpointAvailable() ? ConnectionState.Connected : ConnectionState.Disconnected;
            if (testedState != _currentState)
            {
                _currentState = testedState;
                _logger.LogState(_currentState);
                Console.WriteLine(_currentState.ToString());
            }

        }

        public void StopMonitoring()
        {
            _timer.Elapsed -= TimerFired;
            _timer.Stop();
        }

        public void Dispose()
        {
            if (_timer == null)
            {
                return;
            }

            _timer.Dispose();
            _timer = null;
        }
    }
}