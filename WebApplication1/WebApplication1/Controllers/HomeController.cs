using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.Entity;
using PagedList.Mvc;
using PagedList;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        CartDatabase cdb = new CartDatabase();
        List<Cart> CartsList;
        List<Product> ProductsList;
        ProductDatabase db = new ProductDatabase();
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult Main(String id)
        {

            return View(db.Products);
        }

        public ActionResult AddToCart(String iid)
        {
            int id = Convert.ToInt32(iid);
            if (id > 0)
            {
                Product p = db.Products.Find(id);
                if (p == null)
                {
                    return HttpNotFound();
                }
                Cart good = new Cart();
                good.Name = p.Name;
                good.Price = p.Price;
                cdb.Entry(good).State = EntityState.Added;
                cdb.SaveChanges();
            }
            return Redirect("/Home/Main");
        }
        public ActionResult Search(int? page)
        {
            ProductsList = new List<Product>();
            string searchstring = TempData["Search"].ToString();
            foreach (var item in db.Products)
            {
                if (item.Name.Contains(searchstring))
                {
                    ProductsList.Add(item);
                }

            }
            TempData["Search"] = searchstring;
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(ProductsList.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SearchPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchPage(Search nr)
        {
            TempData["Search"] = nr.Name.ToString();
            return RedirectToAction("Search");
        }

    }
}