using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWardrobe.API.Repositories;

namespace TheWardrobe.API.Controllers
{
  [ApiController]
  [Route("/public/api/[controller]")]
  public class ImageUploadController : ControllerBase
  {
    private readonly IImageUploadRepository _imageUploadRepository;

    public ImageUploadController(IImageUploadRepository imageUploadRepository)
    {
      _imageUploadRepository = imageUploadRepository;
    }

    [HttpPost]
    public IActionResult OnPost()
    {
      var file = Request.Form.Files[0];
      string uploadName = Request.Form["uploadName"];
      var resourceUrl = _imageUploadRepository.UploadImageToS3(file.OpenReadStream(), uploadName);
      return Ok(resourceUrl);
    }
  }
}