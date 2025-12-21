using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YourAppNamespace.Pages
{
    public class StressModel : PageModel
    {
        public IActionResult OnGet()
        {
            var startTime = DateTime.UtcNow;
            var duration = TimeSpan.FromSeconds(5);
            
            // CPU-intensive calculation
            while (DateTime.UtcNow - startTime < duration)
            {
                double result = 0;
                for (int i = 0; i < 100000; i++)
                {
                    result += Math.Sqrt(i) * Math.Sin(i) * Math.Cos(i);
                }
            }
            
            return new JsonResult(new 
            { 
                message = "CPU intensive task completed",
                duration = (DateTime.UtcNow - startTime).TotalSeconds,
                timestamp = DateTime.UtcNow
            });
        }
    }
}