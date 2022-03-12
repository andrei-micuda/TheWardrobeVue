using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TheWardrobe.API.Controllers
{

  //[Authorize]
  [Route("api/[controller]")]
  public class ClothesClassificationController : ControllerBase
  {
    protected readonly Serilog.ILogger _log = Serilog.Log.ForContext<ClothesClassificationController>();

    [HttpPost]
    public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
    {
      long size = files.Sum(f => f.Length);

      foreach (var formFile in files)
      {
        if (formFile.Length > 0)
        {
          var filePath = Path.GetTempFileName();

          _log.Information(filePath);

          using (var stream = System.IO.File.Create(filePath))
          {
            await formFile.CopyToAsync(stream);
          }
        }
      }

      // Process uploaded files
      // Don't rely on or trust the FileName property without validation.
      return Ok(new { count = files.Count, size });
    }
  }
}