using Microsoft.AspNetCore.Mvc;
using play_catalog.Entities;
using play_catalog.Repositories;
using static play_catalog.Dtos.Dtos;

namespace play_catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsRepository itemsRepository = new();

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> Get()
        {
            var items = (await itemsRepository.GetAllAsync()).Select(items => items.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(Guid id) {
            var item = await itemsRepository.GetAsync(id);
            if (item == null) { 
                return NotFound(); 
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Store(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };


            await itemsRepository.CreateAsync(item);

            return CreatedAtAction(
                nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateItemDto updateItemDto) {
            var existingItem = await itemsRepository.GetAsync(id);
            if (existingItem == null) 
            { 
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Destroy(Guid id) {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await itemsRepository.RemoveAsync(item.Id);

            return NoContent();
        }
    }
}
