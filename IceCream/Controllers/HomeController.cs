using IceCream.DAL;
using IceCream.Models;
using IceCream.ViewModel;
using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace IceCream.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private static string Email => WebConfigurationManager.AppSettings["email"];
        private static string Password => WebConfigurationManager.AppSettings["password"];
        public ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["ConfigSite"];
        private IEnumerable<Banner> Banners => _unitOfWork.BannerRepository.Get(a => a.Active, q => q.OrderBy(a => a.Sort));
        private IEnumerable<ArticleCategory> ArticleCategories =>
            _unitOfWork.ArticleCategoryRepository.GetQuery(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));
        private IEnumerable<ProductCategory> ProductCategories =>
            _unitOfWork.ProCategoryRepository.GetQuery(a => a.CategoryActive, q => q.OrderBy(a => a.CategorySort));


        [ChildActionOnly]
        public PartialViewResult Header()
        {
            var model = new HeaderViewModel
            {
                ArticleCategories = ArticleCategories.Where(l => l.ShowMenu),
                ProductCategories = ProductCategories
            };
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var model = new FooterViewModel
            {
                ArticleCategories = ArticleCategories.Where(l => l.ShowMenu),
                ProductCategories = ProductCategories
            };
            return PartialView(model);
        }

        public ActionResult Index()
        {
            var model = new HomeViewModel
            {
                ProductHome = _unitOfWork.ProductRepository.GetQuery(p => p.Active && p.Home, c => c.OrderBy(l => l.Sort), 8),
                Banners = Banners,
                Feedbacks = _unitOfWork.FeedbackRepository.GetQuery(l => l.Active, a => a.OrderBy(c => c.Sort), 16),
                ArticleHome = _unitOfWork.ArticleRepository.GetQuery(p => p.Active && p.Home
                && p.ArticleCategory.TypePost == TypePost.Article, c => c.OrderByDescending(l => l.CreateDate), 6)
            };
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [ChildActionOnly]
        public PartialViewResult ContactForm()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ContactForm(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }
            _unitOfWork.ContactRepository.Insert(model);
            _unitOfWork.Save();
            var subject = "Email liên hệ từ website: " + Request.Url?.Host;
            var body = $"<p>Tên người liên hệ: {model.Fullname},</p>" +
                        $"<p>Địa chỉ email người liên hệ: {model.Email},</p>" +
                        $"<p>Số điện thoại: {model.Mobile},</p>" +
                        $"<p>Địa chỉ: {model.Address},</p>" +
                        $"<p>Nội dung:{model.Body}</p>" +
                        $"<p>Đây là hệ thống gửi email tự động, vui lòng không phản hồi lại email này.</p>";

            Task.Run(() => HtmlHelpers.SendEmail("gmail", subject, body, ConfigSite.Email, "email-send@vico.vn",
           "email-send@vico.vn", "send@123", "IceCream- CÔNG ĐỒNG CHĂM SÓC SỨC KHỎE CHỦ ĐỘNG"));

            return Json(new { status = true, msg = "Gửi liên hệ thành công.\nChúng tôi sẽ liên lạc lại với bạn sớm nhất có thể." });
        }


        public PartialViewResult QuickContact()
        {
            return PartialView();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult QuickContact(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = "Hãy điền đúng định dạng." });
            }
            if (string.IsNullOrEmpty(model.Body))
            {
                model.Body = "Tôi muốn nhận tư vấn";
            }
            _unitOfWork.ContactRepository.Insert(model);
            _unitOfWork.Save();

            var subject = "Email liên hệ từ website: " + Request.Url?.Host;
            var body = $"<p>Tên người liên hệ: {model.Fullname},</p>" +
                        $"<p>Email liên hệ: {model.Email},</p>" +
                        $"<p>Nội dung:{model.Body}</p>" +
                        $"<p>Đây là hệ thống gửi email tự động, vui lòng không phản hồi lại email này.</p>";

            Task.Run(() => HtmlHelpers.SendEmail("gmail", subject, body, ConfigSite.Email, "email-send@vico.vn",
           "email-send@vico.vn", "send@123", "IceCream- CÔNG ĐỒNG CHĂM SÓC SỨC KHỎE CHỦ ĐỘNG"));

            return Json(new { status = true, msg = "Gửi liên hệ thành công.\nChúng tôi sẽ liên lạc lại với bạn sớm nhất có thể." });
        }


        public ActionResult Search(int? page, int? categoryId, string keywords)
        {
            if (string.IsNullOrEmpty(keywords))
            {
                return RedirectToActionPermanent("Index");
            }

            var products = _unitOfWork.ProductRepository.GetQuery(l => l.Name.ToLower().Contains(keywords.ToLower()),
                q => q.OrderByDescending(a => a.Sort));

            if (categoryId.HasValue)
            {
                products = products.Where(a => a.ProductCategoryId == categoryId || a.ProductCategory.ParentId == categoryId);
            }
            var pageNumber = page ?? 1;

            var model = new ProductSearchViewModel
            {
                Keywords = keywords,
                Products = products.ToPagedList(pageNumber, 15),
                CategoryId = categoryId
            };
            return View(model);
        }


        [ChildActionOnly]
        public PartialViewResult MenuProductCategory()
        {
            var model = new MenuProductCategoryViewModel
            {
                ProductCategories = ProductCategories,
                ProductHot = _unitOfWork.ProductRepository.GetQuery(p => p.Active && p.Home, c => c.OrderBy(q => q.Sort), 6)

            };
            return PartialView(model);
        }

        [Route("ProductCategories/{url:regex(^(?!.*(vcms|article|banner|contact|productvcms|uploader)).*$)}", Order = 2)]
        public ActionResult ProductCategory(int? page, string url, string Sort = "name")
        {
            var category = ProductCategories.FirstOrDefault(a => a.Url == url);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            var products = _unitOfWork.ProductRepository.GetQuery(l => l.Active && l.ProductCategoryId == category.Id || l.ProductCategory.ParentId == category.Id, c => c.OrderByDescending(a => a.Id));
            switch (Sort)
            {
                case "name":
                    products = products.OrderBy(a => a.Name);
                    break;
                case "name-desc":
                    products = products.OrderByDescending(a => a.Name);
                    break;
                case "date-new":
                    products = products.OrderByDescending(a => a.CreateDate);
                    break;
                case "date-old":
                    products = products.OrderBy(a => a.CreateDate);
                    break;
                case "sort":
                    products = products.Where(p => p.Home).OrderBy(a => a.Sort);
                    break;
                case "price-des":
                    products = products.OrderByDescending(a => a.Price);
                    break;
                case "price-asc":
                    products = products.OrderBy(a => a.Price);
                    break;
                //case "sale":
                //    products = products.Where(p => p.SaleOff > 0).OrderByDescending(c => c.SaleOff);
                //    break;
                default:
                    products = products.OrderByDescending(a => a.Id);
                    break;
            }
            var pageNumber = page ?? 1;
            var model = new CategoryProductViewModel
            {
                Products = products.ToPagedList(pageNumber, 15),
                Category = category,
                Sort = Sort,
                Categories = ProductCategories
            };
            return View(model);
        }

        public ActionResult AllProduct(int? page, string Sort = "name")
        {
            var products = _unitOfWork.ProductRepository.GetQuery(l => l.Active);
            switch (Sort)
            {
                case "name":
                    products = products.OrderBy(a => a.Name);
                    break;
                case "name-desc":
                    products = products.OrderByDescending(a => a.Name);
                    break;
                case "date-new":
                    products = products.OrderByDescending(a => a.CreateDate);
                    break;
                case "date-old":
                    products = products.OrderBy(a => a.CreateDate);
                    break;
                case "sort":
                    products = products.Where(p => p.Home).OrderBy(a => a.Sort);
                    break;
                case "price-des":
                    products = products.OrderByDescending(a => a.Price);
                    break;
                case "price-asc":
                    products = products.OrderBy(a => a.Price);
                    break;
                //case "sale":
                //    products = products.Where(p => p.SaleOff > 0).OrderByDescending(c => c.SaleOff);
                //    break;
                default:
                    products = products.OrderByDescending(a => a.Id);
                    break;
            }

            var pageNumber = page ?? 1;
            var model = new AllProductViewModel
            {
                Products = products.ToPagedList(pageNumber, 15),
                Sort = Sort,
                Categories = ProductCategories
            };
            return View(model);
        }

        public ActionResult ProductDetail(string url)
        {
            var project = _unitOfWork.ProductRepository.GetQuery(l => l.Active && l.Url == url).FirstOrDefault();
            if (project == null)
            {
                return RedirectToAction("Index");
            }
            var projects = _unitOfWork.ProductRepository.GetQuery
                (l => l.Active && l.ProductCategoryId == project.ProductCategoryId && l.Id != project.Id, a => a.OrderBy(c => Guid.NewGuid()), 6);
            var model = new ProductDetailViewModel
            {
                Product = project,
                Products = projects
            };
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult MenuArticle()
        {
            var model = new MenuArticleViewModel
            {
                ArticleCategories = ArticleCategories.Where(p => p.ParentId == null),
                Articles = _unitOfWork.ArticleRepository.GetQuery(l => l.Active, a => a.OrderByDescending(c => c.CreateDate), 4)
            };
            return PartialView(model);
        }

        public ActionResult ArticleDetail(string url)
        {
            var article = _unitOfWork.ArticleRepository.GetQuery(a => a.Active && a.Url == url).FirstOrDefault();
            if (article == null)
            {
                return RedirectToAction("Index");
            }
            var articles = _unitOfWork.ArticleRepository.GetQuery(l => l.Active && l.ArticleCategoryId == article.ArticleCategoryId && l.Id != article.Id, c => c.OrderByDescending(a => Guid.NewGuid()), 3);
            var model = new ArticleDetailViewModel
            {
                Article = article,
                Articles = articles,
            };
            return View(model);
        }

        public ActionResult ArticleCategory(int? page, string url)
        {
            var category = ArticleCategories.FirstOrDefault(a => a.Url == url);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            var article = _unitOfWork.ArticleRepository.GetQuery(l => l.Active && (l.ArticleCategoryId == category.Id
                || l.ArticleCategory.ParentId == category.Id), c => c.OrderByDescending(a => a.CreateDate));
            var pageNumber = page ?? 1;
            if (article.Count() == 1)
            {
                var fi = article.First();
                return RedirectToAction("ArticleDetail", new { url = fi.Url });
            }
            var model = new CategoryArticleViewModel
            {
                Articles = article.ToPagedList(pageNumber, 12),
                Category = category,
                Categories = new List<ArticleCategory>()
            };

            if (category.ParentId == null)
            {
                model.Categories = ArticleCategories.Where(a => a.ParentId == category.Id);
            }
            return View(model);
        }

        public ActionResult AllArticle(int? page)
        {
            var projects = _unitOfWork.ArticleRepository.GetQuery(l => l.Active, c => c.OrderByDescending(a => a.Id));
            var pageNumber = page ?? 1;
            var model = new AllArticle
            {
                Articles = projects.ToPagedList(pageNumber, 12),
                ArticleCategory = ArticleCategories
            };
            return View(model);
        }



        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}