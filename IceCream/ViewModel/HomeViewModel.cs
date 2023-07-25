﻿using IceCream.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IceCream.ViewModel
{
    public class HomeViewModel
    {  
        public IEnumerable<Feedback> Feedbacks { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable<ItemHomeProduct> ItemHomeProducts { get; set; }
        public IEnumerable<Article> ExpIceCream { get; set; } 
        public IEnumerable<Article> ArticleHome { get; set; }
        public IEnumerable<Product> ProductHome { get; set; }
        public class ItemHomeProduct
        {
            public ProductCategory ProductCategory { get; set; }
            public IEnumerable<Product> Products { get; set; }
        }
    }
    public class AllArticle
    {
        public IEnumerable<ArticleCategory> ArticleCategory { get; set; }
        public IPagedList<Article> Articles { get; set; }
    }
    public class HeaderViewModel
    {
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
    public class FooterViewModel
    {
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }
    public class CategoryProductViewModel
    {
        public ProductCategory Category { get; set; }
        public IPagedList<Product> Products { get; set; }
        public IEnumerable<ProductCategory> Categories { get; set; }
        public int CatId { get; set; }
        public string Sort { get; set; }
    }
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; }
    } 
  
    public class CategoryArticleViewModel
    {
        public ArticleCategory Category { get; set; }
        public IPagedList<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> Categories { get; set; }
        public int CatId { get; set; }
    }
    public class MenuArticleViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
    }
    public class MenuProductCategoryViewModel
    {
        public IEnumerable<Product> ProductHot { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }


    public class AllProductViewModel
    {
        public ProductCategory Category { get; set; }
        public IPagedList<Product> Products { get; set; }
        public IEnumerable<ProductCategory> Categories { get; set; }
        public int CatId { get; set; }
        public string Sort { get; set; }
    }


    public class MenuProductViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
    public class ArticleDetailViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<Article> Articles { get; set; }
    }
    public class ProductSearchViewModel
    {
        public string Keywords { get; set; }
        public int? CategoryId { get; set; }
        public IPagedList<Product> Products { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
    public class ArticleSearchViewModel
    {
        public string Keywords { get; set; }
        public IPagedList<Article> Articles { get; set; }
    }
}