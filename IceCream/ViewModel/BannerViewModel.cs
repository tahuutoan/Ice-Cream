﻿using IceCream.Models;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Web;
using System.Web.Mvc; 

namespace IceCream.ViewModel
{
    public class BannerViewModel
    {
        public Banner Banner { get; set; }
        public SelectList SelectGroup { get; set; }
        public BannerViewModel()
        {
            var listgroup = new Dictionary<int, string> { { 1, "Banner Slide (1900 x 525)" }, { 2, "Banner đối tác (150 x 100)" } };
            SelectGroup = new SelectList(listgroup, "Key", "Value");
        }
    }
    public class ListBannerViewModel
    {
        public IEnumerable<Banner> Banners { get; set; }
        public int? GroupId { get; set; }
        public SelectList SelectGroup { get; set; }
        public ListBannerViewModel()
        {
            var listgroup = new Dictionary<int, string> { { 1, "Banner Slide (1900 x 525)" }, { 2, "Banner đối tác (150 x 100)" } };
            SelectGroup = new SelectList(listgroup, "Key", "Value");
        }
    }
}