﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieLibrary.Models
{
    public class DetailedMovie
    {
        public string id { get; set; }
        public string title { get; set; }
        public string year { get; set; }
        public string[] genres { get; set; }
        public int[] ratings { get; set; }
        public string poster { get; set; }
        public string contentRating { get; set; }
        public string duration { get; set; }
        public string releaseDate { get; set; }
        public int averageRating { get; set; }
        public string originalTitle { get; set; }
        public string storyline { get; set; }
        public string[] actors { get; set; }
        public double imdbRating { get; set; }
        public string posterurl { get; set; }




    }
}
