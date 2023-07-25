using Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using IceCream.DAL;
using IceCream.Models;
using IceCream.ViewModel; 

namespace IceCream.Controllers 
{
    [RoutePrefix("gio-hang")]
    public class ShoppingCartController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
         
        public ConfigSite ConfigSite => (ConfigSite)HttpContext.Application["ConfigSite"];

        private static string Email => WebConfigurationManager.AppSettings["email"];
        private static string Password => WebConfigurationManager.AppSettings["password"];

        [Route("thong-tin")]
        public ActionResult Index(string returnUrl)
        {
            var cart = ShoppingCart.GetCart(HttpContext);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(viewModel);
        }
        [HttpPost, Route("thong-tin")]
        public ActionResult Index(FormCollection fc)
        {
            var records = fc.GetValues("RecordId");
            var quantities = fc.GetValues("Quantity");

            if (records == null || quantities == null)
            {
                return RedirectToActionPermanent("Index");
            }
            for (var i = 0; i < records.Length; i++)
            {
                var recordId = Convert.ToInt32(records[i]);
                var quantity = Convert.ToInt32(quantities[i]);

                var cartItem = _unitOfWork.CartRepository.GetById(recordId);
                if (cartItem == null || cartItem.Count == quantity || quantity < 1) continue;

                cartItem.Count = quantity;
                _unitOfWork.Save();
            }
            return RedirectToActionPermanent("Index");
        }
        [Route("thanh-toan")]
        public ActionResult CheckOut()
        {
            var cart = ShoppingCart.GetCart(HttpContext);
            if (!cart.GetCartItems().Any())
            {
                return RedirectToAction("Index");
            }
            var model = new CheckOutViewModel
            {
                Order = new Order(),
                Carts = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            if (User.Identity.IsAuthenticated)
            {
                var user = _unitOfWork.UserRepository.GetQuery().FirstOrDefault(l => l.Username == User.Identity.Name);
                if (user != null)
                {
                    model.Order.CustomerInfo = new CustomerInfo
                    {
                        Fullname = user.Fullname,
                        Address = user.Address,
                        Email = user.Email,
                        Mobile = user.PhoneNumber
                    };
                }
            }

            return View(model);
        }
        [Route("thanh-toan")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CheckOut(CheckOutViewModel model, int typePay, int transport)
        {
            if (ModelState.IsValid)
            {
                var carts = ShoppingCart.GetCart(HttpContext);
                var item = carts.GetCartItems();
           

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var user = _unitOfWork.UserRepository.GetQuery().FirstOrDefault(l => l.Username == User.Identity.Name);
                    if (user != null)
                    {
                        model.Order.OrderMemberId = user.Id;
                    }
                }

                model.Order.ThanhToanTruoc = (int)carts.GetTotal();
                _unitOfWork.OrderRepository.Insert(model.Order);
                _unitOfWork.Save();

                model.Order.MaDonHang = DateTime.Now.ToString("yyyyMMddHHmm") + "C" + model.Order.Id;

                foreach (var odetails in from cart1 in item
                                         let product = _unitOfWork.ProductRepository.GetById(cart1.ProductId)
                                         select new OrderDetail
                                         {
                                             OrderId = model.Order.Id,
                                             ProductId = product.Id,
                                             Quantity = cart1.Count,
                                             Price = cart1.Price
                                         })
                {
                    _unitOfWork.OrderDetailRepository.Insert(odetails);
                }
                _unitOfWork.Save();

                var typepay = "Thanh toán khi nhận hàng";
                switch (typePay)
                {
                    case 1:
                        typepay = "Tiền mặt";
                        break;
                    case 2:
                        typepay = "Chuyển khoản";
                        break;
                    case 3:
                        typepay = "Hình thức khác";
                        break;
                }
                var giaohang = "Đến địa chỉ người nhận";
                switch (transport)
                {
                    case 2:
                        giaohang = "Khách đến nhận hàng";
                        break;
                    case 3:
                        giaohang = "Qua bưu điện";
                        break;
                    case 4:
                        giaohang = "Hình thức khác";
                        break;
                }
                var sb = "<p style='font-size:16px'>Thông tin đơn hàng gửi từ website " + Request.Url?.Host + "</p>";
                sb += "<p>Mã đơn hàng: <strong>" + model.Order.MaDonHang + "</strong></p>";
                sb += "<p>Họ và tên: <strong>" + model.Order.CustomerInfo.Fullname + "</strong></p>";
                sb += "<p>Địa chỉ: <strong>" + model.Order.CustomerInfo.Address + "</strong></p>";
                sb += "<p>Email: <strong>" + model.Order.CustomerInfo.Email + "</strong></p>";
                sb += "<p>Điện thoại: <strong>" + model.Order.CustomerInfo.Mobile + "</strong></p>";
                sb += "<p>Yêu cầu thêm: <strong>" + model.Order.CustomerInfo.Body + "</strong></p>";
                sb += "<p>Ngày đặt hàng: <strong>" + model.Order.CreateDate.ToString("dd-MM-yyyy HH:ss") + "</strong></p>";
                sb += "<p>Hình thức giao hàng: <strong>" + giaohang + "</strong></p>";
                sb += "<p>Hình thức thanh toán: <strong>" + typepay + "</strong></p>";
                sb += "<p>Thông tin đơn hàng</p>";
                sb += "<table border='1' cellpadding='10' style='border:1px #ccc solid;border-collapse: collapse'>" +
                      "<tr>" +
                      "<th>Ảnh sản phẩm</th>" +
                      "<th>Tên sản phẩm</th>" +
                      "<th>Số lượng</th>" +
                      "<th>Giá tiền</th>" +
                      "<th>Thành tiền</th>" +
                      "</tr>";
                foreach (var odetails in model.Order.OrderDetails)
                {
                    var thanhtien = Convert.ToDecimal(odetails.Price * odetails.Quantity);

                    var img = "NO PICTURE";
                    if (odetails.Product.ListImage != null)
                    {
                        img = "<img src='https://" + Request.Url?.Host + "/images/products/" + odetails.Product.ListImage.Split(',')[0] + "?w=100' />";
                    }
                    sb += "<tr>" +
                          "<td>" + img + "</td>" +
                          "<td>" + odetails.Product.Name;

                    sb += "</td>" +
                          "<td style='text-align:center'>" + odetails.Quantity + "</td>" +
                          "<td style='text-align:center'>" + Convert.ToDecimal(odetails.Price).ToString("N0") + "</td>" +
                          "<td style='text-align:center'>" + thanhtien.ToString("N0") + " đ</td>" +
                          "</tr>";
                }

                sb += "<tr><td colspan='5' style='text-align:right'><strong>Tổng tiền: " + carts.GetTotal().ToString("N0") + " đ</strong></td></tr>";
                sb += "</table>";
                sb += "<p>Cảm ơn bạn đã tin tưởng và mua hàng của chúng tôi.</p>";

                Task.Run(() => HtmlHelpers.SendEmail("gmail", "[" + model.Order.MaDonHang + "] Đơn đặt hàng từ website Consmetic", sb, ConfigSite.Email, Email, Email, Password, "Đặt Hàng Online", model.Order.CustomerInfo.Email, ConfigSite.Email));

                return RedirectToAction("CheckOutComplete", new { orderId = model.Order.MaDonHang });
            }
            var cart = ShoppingCart.GetCart(HttpContext);
            model.CartTotal = cart.GetTotal();

            return View(model);
        }
        [Route("thanh-toan-thanh-cong")]
        public ActionResult CheckOutComplete(string orderId)
        {
            EmptyCart();
            ViewBag.OrderId = orderId;
            return View();
        }
        public ActionResult EmptyCart()
        {
            var cart = ShoppingCart.GetCart(HttpContext);
            cart.EmptyCart();
            return RedirectToAction("Index");
        }
        [Route("them-don-hang")]
        public ActionResult AddToCart(int productId, string returnUrl)
        {
            var addedProduct = _unitOfWork.ProductRepository.GetQuery(a => a.Id == productId).Single();
            var cart = ShoppingCart.GetCart(HttpContext);

            decimal? price = null;

            if (addedProduct.SaleOff != null)
            {
                price = addedProduct.SaleOff;
            }
            else if (addedProduct.Price != null)
            {
                price = addedProduct.Price;
            }
            cart.AddToCart(addedProduct, price);
            return RedirectToAction("Index", new { returnUrl });
            //return Content("<script> alert(\"Thêm vào giỏ hàng thành công!\"); window.location.reload()</script>");
        }
        [Route("them-vao-gio-hang")]
        [HttpPost]
        public JsonResult AddToCartAjax(int productId)
        {
            try
            {
                decimal? price = 0;
                var addedProduct = _unitOfWork.ProductRepository.GetQuery(a => a.Id == productId).Single();
                if (addedProduct == null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                if (addedProduct.SaleOff != null)
                {
                    price = addedProduct.SaleOff;
                }

                else if (addedProduct.Price != null)
                {
                    price = addedProduct.Price;
                }

                var cart = ShoppingCart.GetCart(HttpContext);
                cart.AddToCart(addedProduct, price, 1);

                var data = new
                {
                    result = 1,
                    count = cart.GetCount()
                };
                return Json(data);
            }
            catch (Exception)
            {
                var data = new
                {
                    result = 0,
                    count = 0
                };
                return Json(data);
            }
        }
        [HttpPost]
        public void AddProduct(int sid = 0, int pid = 0, int quantity = 0)
        {
            var product = _unitOfWork.ProductRepository.GetById(pid);
            if (product == null) return;
            var cart = _unitOfWork.CartRepository.GetById(sid);
            if (cart == null) return;
            cart.Count = quantity;
            _unitOfWork.Save();
        }
        [HttpPost] 
        public JsonResult RemoveFromCart(int productId)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(HttpContext);

            // Get the name of the album to display confirmation
            var productName = _unitOfWork.CartRepository.GetQuery(p => p.RecordId == productId).FirstOrDefault().Product.Name;

            // Remove from cart
            bool itemCount = cart.RemoveFromCart(productId);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = productName + " đã được xóa khỏi giỏ hàng của bạn.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                Status = itemCount, 
                DeleteId = productId
            };
            return Json(results);
        }
        public PartialViewResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(HttpContext);
            var model = new CartSummaryViewModel
            {
                Carts = cart.GetCartItems(),
                Count = cart.GetCount(),
                TotalMoney = cart.GetTotal()
            };
            return PartialView("CartSummary", model);
        }
    }
}