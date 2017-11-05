using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc_dev_2017.Models;

namespace mvc_dev_2017.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        MusicStoreModel db = new MusicStoreModel();
        const string PromoCode = "FREE";

        // GET: Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.Email = User.Identity.Name;

                     //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    order.Total = cart.GetTotal();
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    db.Orders.Add(order);
                    db.SaveChanges();
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete", new { id = order.OrderId });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = db.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}