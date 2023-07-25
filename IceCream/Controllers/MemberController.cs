using IceCream.DAL;
using IceCream.Models;
using IceCream.ViewModel;
using Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IceCream.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult ListMember(int? page, string name, int role = -1)
        {
            var pageNumber = page ?? 1;
            const int pageSize = 15;
            var members = _unitOfWork.UserRepository.GetQuery(orderBy: l => l.OrderBy(a => a.Fullname));
            if (!string.IsNullOrEmpty(name))
            {
                members = members.Where(l => l.Fullname.ToLower().Contains(name.ToLower()));
            } 

            var model = new MemberViewModel
            {
                Users = members.ToPagedList(pageNumber, pageSize),
                Name = name,
                Role = role
            };
            return View(model);
        }

        public ActionResult UpdateMember(int memberId = 0)
        {
            var member = _unitOfWork.UserRepository.GetById(memberId);
            if (member == null) 
            { 
                return RedirectToAction("ListMember");
            }

            var model = new UpdateMemberViewModel
            {
                User = member
            }; 
            return View(model);
        }
         
        [HttpPost] 
        public ActionResult UpdateMember(UpdateMemberViewModel model, FormCollection fc)
        {
            var member = _unitOfWork.UserRepository.GetById(model.User.Id);
            if(member == null)
            {
                return RedirectToAction("ListMember");
            }
            var isPost = true;
            var diseaseIds = fc.GetValues("DiseaseId");
            if (diseaseIds == null)
            {
                ModelState.AddModelError("", @"Bạn chưa thêm bệnh cho người dùng");
                isPost = false;
            }

            if (isPost)
            {
                member.Active = model.User.Active;
                _unitOfWork.Save();

                using (var entities = new DataEntities())
                { 
                    foreach (var item in diseaseIds)
                    {
                        var diseaseId = Convert.ToInt32(item); 
                        entities.Database.ExecuteSqlCommand(
                            "INSERT INTO DiseaseUsers (Disease_Id, User_Id) VALUES (@p0, @p1)", diseaseId, model.User.Id);
                    }
                }

                return RedirectToAction("ListMember", new { result = "update" });
            }

            return View(model);
        }

        [HttpPost]
        public bool DeleteMember(int memberId = 0)
        {
            var model = _unitOfWork.UserRepository.GetById(memberId);
            if (model == null)
            {
                return false; 
            } 
            _unitOfWork.UserRepository.Delete(model);
            _unitOfWork.Save();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}