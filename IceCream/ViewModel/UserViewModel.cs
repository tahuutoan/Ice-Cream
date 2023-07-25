using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IceCream.Models;
using PagedList;

namespace IceCream.ViewModel 
{
    public class ListUserViewModel
    {
        public PagedList.IPagedList<User> Users { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy điền tên đăng nhập"), StringLength(50, MinimumLength = 6,ErrorMessage = "Tối đa 50 ký tự")]
        public string Username { get; set; }
        [DisplayName("Mật khẩu"),  StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 - 20 ký tự")]
        public string Password { get; set; }
        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Hãy nhập số điện thoại"), Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), Display(Name = "Địa chỉ email"), EmailAddress(ErrorMessage = "Email không chính xác"), Required(ErrorMessage = "Bạn vui lòng nhập email")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ"), Required(ErrorMessage = "Hãy nhập địa chỉ"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự")]
        public string Address { get; set; }
        public string Avatar { get; set; }
        [Display(Name = "Ngày sinh")]
        public string Birthday { get; set; }
        public DateTime CreateDate { get; set; }
    }
     
    public class UserLoginModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy nhập tên đăng nhập của bạn")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Hãy nhập mật khẩu")]
        public string Password { get; set; }
    }

    public class UserDashboardViewModel
    {
        public User User { get; set; }
        //public IEnumerable<Order> Orders { get; set; } 
    }

    public class ListUserIntroViewModel
    { 
        public IPagedList<User> Users { get; set; }
        public User User { get; set; } 
        public IEnumerable<User> ListUserIntro { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Display(Name = "Email"), EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ"), Required(ErrorMessage = "Hãy nhập email của bạn")]
        public string Email { get; set; }
    }
    public class ChangePasswordViewModel
    {
        [DisplayName("Mật khẩu hiện tại"), Required(ErrorMessage = "Bạn vui lòng nhập mật khẩu hiện tại"), StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 - 20 ký tự")]
        public string OldPassword { get; set; }
        [DisplayName("Mật khẩu"), StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 - 20 ký tự"), Required(ErrorMessage = "Bạn vui lòng nhập mật mới")]
        public string Password { get; set; }
        [DisplayName("Nhập lại mật khẩu"), Compare("Password", ErrorMessage = "Nhập lại mật khẩu không chính xác"), StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 - 20 ký tự"), Required(ErrorMessage = "Bạn vui lòng nhập lại mật khẩu mới"),]
        public string ConfirmPassword { get; set; }
    }

    public class UpdateInfoViewModel
    {
        [Display(Name = "Họ và tên"), Required(ErrorMessage = "Hãy nhập họ và tên"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự")] 
        public string Fullname { get; set; }
        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Hãy nhập số điện thoại"), Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), Display(Name = "Địa chỉ email"), EmailAddress(ErrorMessage = "Email không chính xác"), Required(ErrorMessage = "Bạn vui lòng nhập email")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ"), Required(ErrorMessage = "Hãy nhập địa chỉ"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự")]
        public string Address { get; set; }



        [Display(Name = "Những bệnh đang mắc phải"), DataType(DataType.MultilineText), StringLength(4000)]
        public string ListDisease { get; set; }

        [StringLength(500)]
        [Display(Name = "Ảnh đại diện")]
        public string Avatar { get; set; }


        [Display(Name = "Ngày sinh")]
        public string Birthday { get; set; }

    }
}