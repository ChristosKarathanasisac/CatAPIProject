﻿namespace NatechAPI.Models.ViewModels
{
    public class CatVM
    {
        public List<BreedVM> breeds { get; set; }
        public string id { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
