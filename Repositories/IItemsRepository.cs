using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Entities;

namespace Catalog.Repositories
{
    public interface IItemsRepository
    {
        Task<Item> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task CreateItemAsync(Item item); // returns nothing, just receives the items that need to be created in repository
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Guid id);
    }
}