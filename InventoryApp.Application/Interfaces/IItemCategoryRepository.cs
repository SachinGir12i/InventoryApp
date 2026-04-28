using InventoryApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryApp.Application.Interfaces
{
    public interface IItemCategoryRepository
    {
        Task<ItemCategory> CreateAsync(ItemCategory category);
        Task<ItemCategory?> GetByIdAsync(int id);
        Task<List<ItemCategory>> GetAllAsync();
        Task<ItemCategory> UpdateAsync(ItemCategory category);
        Task<bool> DeleteAsync(int id);
    }
}
