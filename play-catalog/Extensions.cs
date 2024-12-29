using play_catalog.Entities;
using static play_catalog.Dtos.Dtos;

namespace play_catalog
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(
                item.Id,
                item.Name,
                item.Description,
                item.Price,
                item.CreatedDate
                );
        }
    }
}
