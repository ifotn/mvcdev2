using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using mvc_dev_2017.Models;
using mvc_dev_2017.ViewModels;

namespace mvc_dev_2017.Controllers
{
    public class ShoppingCartController : Controller
    {
        MusicStoreModel db = new MusicStoreModel();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // set up viewmodel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewModel);
        }

        // GET: AddToCart
        public ActionResult AddToCart(int AlbumId)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var album = db.Albums.SingleOrDefault(a => a.AlbumId == AlbumId);

            cart.AddToCart(album);

            return RedirectToAction("Index");
        }

        // POST: RemoveFromCart
        public ActionResult RemoveFromCart(int AlbumId)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var album = db.Albums.SingleOrDefault(a => a.AlbumId == AlbumId);

            int itemCount = cart.RemoveFromCart(AlbumId);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = "Your Cart has been Updated",
                CartTotal = cart.GetTotal(),
                ItemCount = itemCount,
                DeleteId = AlbumId
            };

            return Json(results);
           
        }
    }
}