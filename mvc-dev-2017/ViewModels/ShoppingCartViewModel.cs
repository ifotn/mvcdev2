using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using mvc_dev_2017.Models;

namespace mvc_dev_2017.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}