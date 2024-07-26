﻿namespace Test.API.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
