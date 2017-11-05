using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc_dev_2017.Models
{
    public class ShoppingCart
    {
        // db conn
        MusicStoreModel db = new MusicStoreModel();

        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        //// helper method to simplify cart calls
        //public static ShoppingCart GetCart(Controller controller)
        //{
        //    return GetCart(controller.HttpContext);
        //}

        // add item
        public void AddToCart(Album album)
        {
            // is album already in cart?
            var cartItem = db.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.AlbumId == album.AlbumId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    AlbumId = album.AlbumId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            // commit changes
            db.SaveChanges();
        }

        // use httpcontextbase for cookie access
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // random cart Id as user is unknown
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(c => c.CartId == ShoppingCartId).ToList();
        }

        public decimal GetTotal()
        {
            decimal? total = (from c in db.Carts
                              where c.CartId == ShoppingCartId
                              select (int?)c.Count * c.Album.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int RemoveFromCart(int AlbumId)
        {
            var item = db.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId
            && c.RecordId == AlbumId);

            int itemCount = 0;

            if (item != null)
            {
                if (item.Count > 1)
                {
                    item.Count--;
                    itemCount = item.Count;
                }
                else
                {
                    db.Carts.Remove(item);
                }

                db.SaveChanges();
            }

            return itemCount;
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            db.SaveChanges();
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Album.Price);

                db.OrderDetails.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }
    }
}