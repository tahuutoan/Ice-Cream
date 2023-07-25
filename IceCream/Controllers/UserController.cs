using IceCream.DAL;
using IceCream.Filters;
using IceCream.Models;
using IceCream.ViewModel;
using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Helpers;

namespace IceCream.Controllers
{
    [RoutePrefix("account"), MemberFilter]
    public class UserController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private string Email => WebConfigurationManager.AppSettings["email"];
        private string PasswordEmail => WebConfigurationManager.AppSettings["password"];
        public ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["ConfigSite"];

        private List<User> ListUser = new List<User>();
        private new User User => _unitOfWork.UserRepository.Get(a => a.Username == HttpContext.User.Identity.Name)
            .SingleOrDefault();

        [OverrideActionFilters]
        [Route("dang-ky")]
        public ActionResult Register(string result ="")
        {
            ViewBag.Result = result;
            return View();
        }

        [OverrideActionFilters]
        [Route("dang-ky")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;
                var email = _unitOfWork.UserRepository.GetQuery(l => l.Email.Equals(model.Email)).FirstOrDefault();
                var username = _unitOfWork.UserRepository.GetQuery(l => l.Username.Equals(model.Username)).FirstOrDefault();
                if (username != null)
                {
                    ModelState.AddModelError("", @"Tên đăng nhập đã được sử dụng, vui lòng chọn tên đăng nhập khác !!");
                    isPost = false;
                }
                else if (email != null)
                {
                    ModelState.AddModelError("", @"Email đã được sử dụng, vui lòng chọn 1 email khác !!");
                    isPost = false;
                }
                if (isPost)
                {
                    var _username = RemoveUnicodeAndSpace(model.Username);
                    var hashPass = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                    _unitOfWork.UserRepository.Insert(new User
                    {
                        Fullname = model.Fullname,
                        Password = hashPass,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        Username = _username,
                        Active = true
                    }); 
                    _unitOfWork.Save();
                    //VerifyEmail(model); 
                     
                    return RedirectToAction("Register", "User", new { Result = "register" });
                }
            }
            return View(model);
        }

        public RedirectToRouteResult LogOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session["CartId"] = new Guid();
            return RedirectToAction("Index", "Home");
        }

        [OverrideActionFilters]
        [Route("dang-nhap")]
        public ActionResult Login()
        {
            return View();
        }

        [OverrideActionFilters]
        [Route("dang-nhap")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.UserRepository.GetQuery(a => a.Username == model.Username && a.Active).SingleOrDefault();

                if (user != null && HtmlHelpers.VerifyHash(model.Password, "SHA256", user.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    //var cart = new ShoppingCart();
                    //cart.MigrateCart(model.Email); 
                    HttpContext.Session["CartId"] = model.Username;

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", @"Thông tin đăng nhập không hợp lệ");
            }
            return View(model);
        }

        [OverrideActionFilters]
        public JsonResult CheckEmail(string email)
        {
            var user = _unitOfWork.UserRepository.GetQuery(a => a.Email == email).FirstOrDefault();

            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json("Email đã được sử dụng, vui lòng chọn thông tin khác", JsonRequestBehavior.AllowGet);
        }

        [OverrideActionFilters]
        public JsonResult CheckUsername(string username)
        {
            var user = _unitOfWork.UserRepository.GetQuery(a => a.Username == username).FirstOrDefault();

            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json("Tên đăng nhập đã được sử dụng, vui lòng chọn thông tin khác", JsonRequestBehavior.AllowGet);
        }

        [OverrideActionFilters]
        [Route("quen-mat-khau")]
        public ActionResult ForgotPassword(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }

        [OverrideActionFilters]
        [Route("quen-mat-khau"), HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exits = _unitOfWork.UserRepository.GetQuery(a => a.Email == model.Email).SingleOrDefault();
                if (exits == null)
                {
                    ModelState.AddModelError("", @"Email không chính xác.");
                    return View(model);
                }

                var password = HtmlHelpers.RandomCode(6);
                exits.Password = HtmlHelpers.ComputeHash(password, "SHA256", null);
                _unitOfWork.Save();

                var body = new StringBuilder();
                body.Append("<p>Xin chào, " + exits.Fullname + "</p>");
                body.Append("<p>Bạn đã gửi yêu cầu lấy lại mật khẩu từ website " + Request.Url?.Host + "</p>");
                body.Append("<p>Mật khẩu mới: " + password + "</p>");
                body.Append("<p>Bạn vui lòng đăng nhập và thay đổi mật khẩu mới để sử dụng</p>");
                body.Append("<p>Đây là email tự động, vui lòng không phản hồi lại email này.</p>");

                Task.Run(() => HtmlHelpers.SendEmail("gmail", "Yêu cầu lấy lại mật khẩu từ " + Request.Url?.Host,
                    body.ToString(), exits.Email, Email, Email, PasswordEmail, "ICECREAM Parlor Shop"));

                return RedirectToAction("ForgotPassword", new { result = 1 });
            }
            return View(model);
        }

        [Route("thong-tin-tai-khoan")]
        public ActionResult ProfileInfo(int? page)
        {
            var listUser = _unitOfWork.UserRepository.GetQuery
                (p => p.Active, c => c.OrderByDescending(l => l.CreateDate));

            var pageNumber = page ?? 1;
            var model = new ListUserIntroViewModel
            {
                Users = listUser.ToPagedList(pageNumber, 20),
                User = User,
                ListUserIntro = ListUser
            };
            return View(model);
        }


        [Route("doi-mat-khau")]
        public ActionResult ChangePassword(int result = 0)
        {
            ViewBag.Result = result;
            return View();
        }

        [Route("doi-mat-khau"), HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!HtmlHelpers.VerifyHash(model.OldPassword, "SHA256", User.Password))
                {
                    ModelState.AddModelError("", @"Mật khẩu hiện tại không chính xác");
                    return View(model);
                }
                User.Password = HtmlHelpers.ComputeHash(model.Password, "SHA256", null);
                _unitOfWork.Save();
                return RedirectToAction("ChangePassword", new { result = 1 });
            }
            return View(model);
        }

        [Route("cap-nhat-thong-tin-chung")]
        public ActionResult UpdateInfo(int result = 0)
        {
            var model = new UpdateInfoViewModel
            {
                Email = User.Email,
                Address = User.Address,
                PhoneNumber = User.PhoneNumber,
                Fullname = User.Fullname
            };
            ViewBag.Result = result;
            return View(model);
        }

        [Route("cap-nhat-thong-tin-chung"), HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdateInfo(UpdateInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isPost = true;

                var file = Request.Files["Avatar"];
                if (file != null && file.ContentLength > 0)
                {
                    if (!HtmlHelpers.CheckFileExt(file.FileName, "jpg|jpeg|png|gif|svg"))
                    {
                        ModelState.AddModelError("", @"Chỉ chấp nhận định dạng jpg, png, gif, jpeg, svg");
                        isPost = false;
                    }
                    else
                    {
                        if (file.ContentLength > 4000 * 1024)
                        {
                            ModelState.AddModelError("", @"Dung lượng lớn hơn 4MB. Hãy thử lại");
                            isPost = false;
                        }
                        else
                        {
                            var imgPath = "/images/users/" + DateTime.Now.ToString("yyyy/MM/dd");
                            HtmlHelpers.CreateFolder(Server.MapPath(imgPath));
                            var imgFileName = DateTime.Now.ToFileTimeUtc() + Path.GetExtension(file.FileName);
                            if (System.IO.File.Exists(Server.MapPath("/images/products/" + User.Avatar)))
                            {
                                System.IO.File.Delete(Server.MapPath("/images/products/" + User.Avatar));
                            }
                            User.Avatar = DateTime.Now.ToString("yyyy/MM/dd") + "/" + imgFileName;
                            file.SaveAs(Server.MapPath(Path.Combine(imgPath, imgFileName)));
                        }
                    }
                }
                if (isPost)
                {
                    User.Fullname = model.Fullname;
                    User.PhoneNumber = model.PhoneNumber;
                    User.Address = model.Address;
                    _unitOfWork.Save();
                    return RedirectToAction("UpdateInfo", new { result = 1 });
                }

            }
            return View(model);
        }

        [OverrideActionFilters]
        [HttpPost, Route("yeu-thich")]
        public JsonResult AddWishlist(int proId = 0, string action = "add")
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Json(new { status = 1, msg = "Bạn cần đăng nhập để sử dụng chức năng này" });
            }
            var product = _unitOfWork.ProductRepository.GetById(proId);
            if (product == null || !product.Active)
            {
                return Json(new { status = 1, msg = "Sản phẩm không chính xác. Vui lòng kiểm tra lại" });
            }

            var exits = User.Products.SingleOrDefault(a => a.Id == proId);

            if (action == "remove")
            {
                if (exits == null)
                {
                    return Json(new { status = 1, msg = "Sản phẩm không có trong danh sách yêu thích của bạn." });
                }

                product.Users.Remove(User);
                _unitOfWork.Save();
                return Json(new { status = 0, msg = "Xóa sản phẩm trong danh sách yêu thích thành công.", count = User.Products.Count });
            }

            if (exits != null)
            {
                return Json(new { status = 1, msg = "Sản phẩm đã có trong danh sách yêu thích của bạn." });
            }

            product.Users.Add(User);
            _unitOfWork.Save();
            return Json(new { status = 0, msg = "Thêm sản phẩm vào danh sách yêu thích thành công.", count = User.Products.Count });
        }

        [ChildActionOnly]
        public PartialViewResult UserNav()
        {
            return PartialView(User);
        }


        [ChildActionOnly]
        public PartialViewResult ListUserIntro(int? page)
        {
            var listUser = _unitOfWork.UserRepository.GetQuery(p => p.Active, c => c.OrderByDescending(l => l.CreateDate));
            var pageNumber = page ?? 1;
            var model = new ListUserIntroViewModel
            {
                Users = listUser.ToPagedList(pageNumber, 1),
                User = User
            };
            return PartialView(model);
        }


        public static string RemoveUnicodeAndSpace(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
            "d",
            "e","e","e","e","e","e","e","e","e","e","e",
            "i","i","i","i","i",
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
            "u","u","u","u","u","u","u","u","u","u","u",
            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            text = Regex.Replace(text, @"\s", "");
            text = text.ToLower();
            return text;
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}