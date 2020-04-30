using System;
using System.Net.Http;

namespace Connectionmonitor
{

    public class EndpointTester
    {
        private Uri _uriToMonitor;

        public EndpointTester(Uri uriToMonitor)
        {
            _uriToMonitor = uriToMonitor;
        }

        public bool IsEndpointAvailable()
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(4);
                var result = client.GetAsync(_uriToMonitor).Result;
                if ((int)result.StatusCode <= 400)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static EndpointTester CreateEndpointTester(string uriToMonitor)
        {
            try
            {
                var uri = new Uri(uriToMonitor);
                return new EndpointTester(uri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Bad Uri ({ex.Message})");
                return null;
            }
        }
    }
}
