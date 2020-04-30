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

        public Monitor(AuditLogger logger, EndpointTester tester)
        {
            _logger = logger;
            _tester = tester;
        }

        public void StartMonitoring()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            _logger.LogStart();
            _timer = new Timer(5000);
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