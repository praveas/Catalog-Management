using System;

namespace Catalog.Api.Entities
{
    public class Item // 1
    {
        public Guid Id { get; set;}
        public string Name { get; set; }
        public string Description {get; set;}
        public decimal Price {get; set; }
        public DateTimeOffset CreatedDate {get; set;}
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

/* --- Record
 public record Item // 1
    {
        public Guid Id { get; init;}
        public string Name { get; init; }
        public decimal Price {get; init; }
        public DateTimeOffset CreatedDate {get; init;}
    }

*/