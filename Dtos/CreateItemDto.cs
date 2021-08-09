using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Dtos
{
    public record CreateItemDto
    {
        [Required]
        public string Name { get; init; }
        [Required]
        [Range(1,1000)] // only accept range value from 1 to 1000 (no negative or 0)
        public decimal Price {get; init; }
        
    }
}