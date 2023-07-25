using IceCream.DAL;
using IceCream.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IceCream.Controllers
{
    [Authorize] 
    public class OrderController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public ActionResult ListOrder(int? page, string madonhang, string fromdate, string todate, string customerName, string customerEmail, string customerMobile, int status = -1, int payment = 0, int pageSize = 50)
        {
            var pageNumber = page ?? 1; 
            var orders = _unitOfWork.OrderRepository.GetQuery(orderBy: q => q.OrderByDescending(a => a.Id));

            if (!string.IsNullOrEmpty(madonhang))
            {
                orders = orders.Where(a => a.MaDonHang.Contains(madonhang));
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                orders = orders.Where(a => a.CustomerInfo.Fullname.ToLower().Contains(customerName.ToLower()));
            }

            if (!string.IsNullOrEmpty(customerEmail))
            {
                orders = orders.Where(a => a.CustomerInfo.Email.ToLower().Contains(customerEmail.ToLower()));
            }

            if (!string.IsNullOrEmpty(customerMobile))
            {
                orders = orders.Where(a => a.CustomerInfo.Mobile.Contains(customerMobile));
            }

            if (DateTime.TryParse(fromdate, new CultureInfo("vi-VN"), DateTimeStyles.None, out var fd))
            {
                orders = orders.Where(a => DbFunctions.TruncateTime(a.CreateDate) >= DbFunctions.TruncateTime(fd));
            }
            if (DateTime.TryParse(todate, new CultureInfo("vi-VN"), DateTimeStyles.None, out var td))
            {
                orders = orders.Where(a => DbFunctions.TruncateTime(a.CreateDate) <= DbFunctions.TruncateTime(td));
            }
            if (status >= 0)
            {
                orders = orders.Where(a => a.Status == status);
            }
            if (payment > 0)
            {
                switch (payment)
                {
                    case 1:
                        orders = orders.Where(a => !a.Payment);
                        break;
                    case 2:
                        orders = orders.Where(a => a.Payment);
                        break;
                }
            }

            var model = new ListOrderViewModel
            {
                Orders = orders.ToPagedList(pageNumber, pageSize),
                MaDonhang = madonhang,
                Status = status,
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                CustomerMobile = customerMobile,
                FromDate = fromdate,
                ToDate = todate,
                PageSize = pageSize,
                Payment = payment
            };

            return View(model);
        }
        public ActionResult LoadOrder(int orderId = 0)
        {
            var order = _unitOfWork.OrderRepository.GetById(orderId);
            var orderdetails = _unitOfWork.OrderDetailRepository.Get(a => a.OrderId == orderId);

            var orderdetailproduct = orderdetails.Select(orderdetail => new OrderDetailProduct
            {
                Product = _unitOfWork.ProductRepository.GetById(orderdetail.ProductId),
                Price = orderdetail.Price,
                Quantity = orderdetail.Quantity
            }).ToList();

            var model = new OrderViewModel
            {
                Order = order,
                OrderDetailProduct = orderdetailproduct
            };
            return PartialView(model);
        }
        [HttpPost] 
        public bool UpdateOrder(string notice, int payment = 0, int status = 0, int orderId = 0)
        {
            var order = _unitOfWork.OrderRepository.GetById(orderId);
            if (order == null)
            {
                return false;
            }
            if (status >= 0)
            {
                order.Status = status;
            }
            if (payment > 0)
            {
                switch (payment)
                {
                    case 1:
                        order.Payment = false;
                        break;
                    case 2:
                        order.Payment = true;
                        break;
                }
            }
            


            
            _unitOfWork.Save();
            return true;
        }
        [HttpPost]
        public bool UpdateOrderNotice(string notice, int thanhtoantruoc = 0, int orderId = 0)
        {
            var order = _unitOfWork.OrderRepository.GetById(orderId);
            if (order == null)
            {
                return false;
            }
            order.ThanhToanTruoc = thanhtoantruoc;
            order.CustomerInfo.Body = notice;
            _unitOfWork.Save();
            return true;
        }
        [HttpPost]
        public bool DeleteOrder(int orderId = 0)
        {
            var order = _unitOfWork.OrderRepository.GetById(orderId);
            if (order == null)
            {
                return false;
            }

            order.Status = 3;
            _unitOfWork.Save();
            return true;
        }
    }
}