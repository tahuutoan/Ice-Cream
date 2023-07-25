using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
 
namespace IceCream.Models
{ 
    public class User
    {
        public int Id { get; set; }
         
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy nhập tên đăng nhập")
            , StringLength(50, MinimumLength = 6,ErrorMessage = "Tên đăng nhập phải có ít nhất 6 đến 50 ký tự"), Remote("CheckUsername", "User")]
        public string Username { get; set; }
         
          
        [Display(Name = "Họ và tên"), Required(ErrorMessage = "Hãy nhập họ và tên"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string Fullname { get; set; }


         
        [DisplayName("Mật khẩu"), Required(ErrorMessage = "Hãy nhập mật khẩu"), StringLength(60, MinimumLength = 6
            ,ErrorMessage = "Mật khẩu phải có ít nhất 6 đến 50 ký tự")]
        public string Password { get; set; }


        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Hãy nhập số điện thoại"), Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }


        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), Display(Name = "Địa chỉ email"), EmailAddress(ErrorMessage = "Email không chính xác"), Required(ErrorMessage = "Bạn vui lòng nhập email"), Remote("CheckEmail", "User")]


        public string Email { get; set; }
         

        [Display(Name = "Địa chỉ"), Required(ErrorMessage = "Hãy nhập địa chỉ"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự")]
        public string Address { get; set; }


        [StringLength(500)]
        [Display(Name ="Ảnh đại diện")]
        public string Avatar { get; set; } 

        public DateTime CreateDate { get; set; }
         
         
        [Display(Name = "Hoạt động", Description = "Hoạt động")]
        public bool Active { get; set; }
         
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public User()
        {
            CreateDate = DateTime.Now;
            Active = false;
        }
    }
}