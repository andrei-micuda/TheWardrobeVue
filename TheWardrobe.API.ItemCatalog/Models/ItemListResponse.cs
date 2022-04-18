using System.Collections.Generic;
using System.Linq;

namespace TheWardrobe.API.ItemCatalog.Models
{
  public class ItemListResponse
  {
    public IEnumerable<ItemRequestResponse> Items { get; set; }
    public int NumItems { get; set; }
    public float? MinPrice
    {
      get
      {
        if (Items.Any())
          return Items.Select(i => i.Price).Min(i => i);

        return null;
      }
    }
    public float? MaxPrice
    {
      get
      {
        if (Items.Any())
          return Items.Select(i => i.Price).Max(i => i);

        return null;
      }
    }
  }
}