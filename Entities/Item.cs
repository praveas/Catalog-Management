using System;

namespace Catalog.Entities
{
    public record Item // 1
    {
        public Guid Id { get; init;}
        public string Name { get; init; }
        public decimal Price {get; init; }
        public DateTimeOffset CreatedDate {get; init;}
    }
}


// 1 Record Types
        // intead of public class Item use 
        // public record item
        // Use for immutable objects
        // with-expressions support
        // value - based equality support
        // record types are pretty handy

// init : insetead of using set; using init- only properties
// creating mutable property (nomore private set;)