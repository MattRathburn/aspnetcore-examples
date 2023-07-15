using health_check.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace health_check
{
    public abstract class HttpStatusCodeHealthCheck : IHealthCheck
    {
        readonly HttpClient _httpClient;
        readonly IServer _server;
        readonly short _statusCode;

        public HttpStatusCodeHealthCheck(HttpClient httpClient, IServer server, short statusCode)
        {
            _httpClient = httpClient;
            _server = server;
            _statusCode = statusCode;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var serverAddress = _server.Features.Get<IServerAddressesFeature>();
                var localServer = serverAddress.Addresses.First();

                //var result = await _httpClient.GetAsync(localServer + $"/home/fakestatus/?statuscode={_statusCode}");
                var result = await _httpClient.GetAsync(localServer + $"/home");


                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return HealthCheckResult.Healthy("Healthy");
                }
                else if(result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    context.Registration.FailureStatus = HealthStatus.Degraded;
                    return HealthCheckResult.Degraded($"Degraded: Http Status returns {result.StatusCode}");
                }
                else
                {
                    return HealthCheckResult.Unhealthy($"Fails: Http Status returns {result.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Exception {ex.Message} : {ex.StackTrace}");
            }
        }
    }

    public class OKHttpStatusCodeHealthCheck : HttpStatusCodeHealthCheck
    {
        public OKHttpStatusCodeHealthCheck(HttpClient client, IServer server, StatusOK status) : base(client, server, status.Status)
        { }
    }

    public class Error2HttpStatusCodeHealthCheck : HttpStatusCodeHealthCheck
    {
        public Error2HttpStatusCodeHealthCheck(HttpClient client, IServer server, StatusBadRequest status) : base(client, server, status.Status)
        { }
    }
}
