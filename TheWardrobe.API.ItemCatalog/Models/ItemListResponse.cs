using System.Collections.Generic;
using System.Linq;

namespace TheWardrobe.API.Models
{
  public class ItemListResponse
  {
    public IEnumerable<ItemRequestResponse> Items { get; set; }
    public int NumItems { get; set; }
    public float? MinPrice { get; set; }
    public float? MaxPrice { get; set; }
  }
}