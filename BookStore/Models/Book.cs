﻿namespace BookStore.Models
{
    public class Book
    {

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
        public int Stock { get; set; }

       


    }
}
