using Microsoft.AspNetCore.Mvc;

namespace health_check.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new ContentResult
            {
                Content = @"
                    <html><body>
                    <h1>Health Check - Failed/Success check</h1>
                    <a href=""/HealthCheck"">Check Status</a>
                    </body></html>",
                ContentType = "text/html"
            };
        }
    }
}
