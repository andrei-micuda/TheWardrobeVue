using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TheWardrobe.API.Entities;

namespace TheWardrobe.API.Models
{
  public class ReviewRequest
  {
    [Range(1, 5, ErrorMessage = "Please enter a value between 1 and 5")]
    public int Rating { get; set; }
  }
}