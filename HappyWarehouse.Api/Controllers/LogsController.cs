using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LogsController : BaseApiController
    {
        private readonly string _logsDir;

        public LogsController(IWebHostEnvironment env)
        {
            _logsDir = Path.Combine(env.ContentRootPath, "Logs");
            Directory.CreateDirectory(_logsDir);
        }

        [HttpGet]
        public IActionResult GetLogs([FromQuery] int last = 200)
        {
            var file = Directory.EnumerateFiles(_logsDir, "log-*.txt")
                                .OrderByDescending(x => x)
                                .FirstOrDefault();

            if (string.IsNullOrEmpty(file))
                return Ok(new { lines = Array.Empty<string>() });

            string[] lines = Array.Empty<string>();

            for (int attempt = 0; attempt < 3; attempt++)
            {
                try
                {
                    using var fs = new FileStream(file, FileMode.Open, FileAccess.Read,
                                                  FileShare.ReadWrite | FileShare.Delete);
                    using var sr = new StreamReader(fs);
                    var all = new List<string>();
                    string? line;
                    while ((line = sr.ReadLine()) != null) all.Add(line);
                    lines = all.TakeLast(Math.Max(1, last)).ToArray();
                    break;
                }
                catch (IOException)
                {
                    Thread.Sleep(50);
                }
            }

            return Ok(new { lines });
        }
    }
}
