﻿namespace CodeChallenging.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Category {  get; set; }
        public string Image { get; set; }

        public Product() { }
    }
}
