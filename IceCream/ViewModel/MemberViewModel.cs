using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IceCream.Models;
using PagedList;

namespace IceCream.ViewModel
{
    public class MemberViewModel
    {
        public IPagedList<User> Users { get; set; }
        public string Name { get; set; }
        public int Role { get; set; }
    }

    public class UpdateMemberViewModel
    {
        public User User { get; set; }
    }

}