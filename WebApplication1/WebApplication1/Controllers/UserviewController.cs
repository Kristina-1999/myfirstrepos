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
    public class UserviewController : Controller
    {
        UserDatabase db = new UserDatabase();
        GroupDatabase cdb = new GroupDatabase();
        ProductDatabase pdb = new ProductDatabase();
        CartDatabase cartdb = new CartDatabase();
        OrderDatabase odb = new OrderDatabase();
        List<Order> OrdersList;
        List<Cart> CartsList;
        List<User> UsersList;
        List<Group> GroupsList;
        List<Product> ProductsList;
        [HttpGet]
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult UserPge(String RGB, String LNB, String LN, String LP, String RN, String RP, String RM)
        {
            User LoggedUser = db.Users.FirstOrDefault(x => x.IsLogged > 0);
            if (LoggedUser == null)
            {
                if (LNB != null)
                {
                    User myUser = db.Users.Where(x => x.Username == LN && x.Password == LP).FirstOrDefault();
                    if (myUser != null)
                    {
                        if (myUser.IsAdmin == 1)
                        {
                            myUser.IsLogged = 1;
                            db.SaveChanges();
                            return Redirect("/Userview/LKA");
                        }
                        else
                        {
                            myUser.IsLogged = 1;
                            db.SaveChanges();
                            return Redirect("/Userview/LK");
                        }

                    }
                    else
                    {
                        myUser = db.Users.Where(x => x.Email == LN && x.Password == LP).FirstOrDefault();
                        if (myUser != null)
                        {
                            if (myUser.IsAdmin == 1)
                            {
                                myUser.IsLogged = 1;
                                db.SaveChanges();
                                return Redirect("/Userview/LKA");
                            }
                            else
                            {
                                myUser.IsLogged = 1;
                                db.SaveChanges();
                                return Redirect("/Userview/LK");
                            }

                        }

                        return Redirect("/Userview/UserPge");
                    }

                }
                else if (RGB != null)
                {
                    User a = new User();
                    a.Username = RN;
                    a.IsAdmin = 0;
                    a.Password = RP;
                    a.Email = RM;
                    a.IsLogged = 1;
                    db.Entry(a).State = EntityState.Added;
                    db.SaveChanges();
                    return Redirect("/Userview/LK");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                if (LoggedUser.IsAdmin == 1)
                {
                    return Redirect("/Userview/LKA");
                }
                else
                {
                    return Redirect("/Userview/LK");
                }
            }
        }
        [HttpGet]
        public ActionResult LK(int? page)
        {
            OrdersList = new List<Order>();
            User currentuser = db.Users.Where(x => x.IsLogged == 1).FirstOrDefault();
            foreach (var item in odb.Orders)
            {
                if (currentuser.id == item.userid)
                {
                    OrdersList.Add(item);
                }
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(OrdersList.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult LK(String ex)
        {
            return View(odb.Orders);
        }
        [HttpGet]
        public ActionResult LKA(int? page)
        {
            UsersList = new List<User>();
            foreach (var item in db.Users)
            {
                UsersList.Add(item);
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(UsersList.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult LKA(String ex)
        {
            return View(db.Users);
        }
        public ActionResult LoggingOff()
        {
            User myUser = db.Users.Where(x => x.IsLogged == 1).FirstOrDefault();
            myUser.IsLogged = 0;
            db.SaveChanges();
            return Redirect("/Userview/UserPge");
        }
        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            User r = db.Users.Find(id);
            if (r == null)
            {
                return HttpNotFound();
            }
            return View(r);
        }
        [HttpPost, ActionName("DeleteUser")]
        public ActionResult DeleteConfirmed(int id)
        {
            User r = db.Users.Find(id);
            if (r == null)
            {
                return HttpNotFound();
            }
            db.Users.Remove(r);
            db.SaveChanges();
            return RedirectToAction("LKA");
        }
        [HttpGet]
        public ActionResult Groups()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GroupsPage(int? page)
        {
            GroupsList = new List<Group>();
            foreach (var item in cdb.Groups)
            {
                GroupsList.Add(item);
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(GroupsList.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult DeleteGroup(int id)
        {
            Group r = cdb.Groups.Find(id);
            if (r == null)
            {
                return HttpNotFound();
            }
            return View(r);
        }
        [HttpPost, ActionName("DeleteGroup")]
        public ActionResult DeleteGroupConfirmed(int id)
        {
            Group r = cdb.Groups.Find(id);
            if (r == null)
            {
                return HttpNotFound();
            }
            Product myProduct = pdb.Products.Where(x => x.Category == r.Name).FirstOrDefault();
            while (myProduct != null)
            {
                pdb.Products.Remove(myProduct);
                pdb.SaveChanges();
                myProduct = pdb.Products.Where(x => x.Category == r.Name).FirstOrDefault();
            }
            cdb.Groups.Remove(r);
            cdb.SaveChanges();

            return RedirectToAction("GroupsPage");
        }
        [HttpGet]
        public ActionResult AddGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGroup(Group nr)
        {
            if (nr.Name != null)
            {
                cdb.Entry(nr).State = EntityState.Added;
                cdb.SaveChanges();

                return RedirectToAction("GroupsPage");
            }
            else
            {
                return RedirectToAction("AddGroup");
            }
        }

        [HttpGet]
        public ActionResult Products()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ProductsPage(int? page)
        {
            ProductsList = new List<Product>();
            foreach (var item in pdb.Products)
            {
                ProductsList.Add(item);
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(ProductsList.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            Product p = pdb.Products.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }
        [HttpPost, ActionName("DeleteProduct")]
        public ActionResult DeleteProductConfirmed(int id)
        {
            Product p = pdb.Products.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            pdb.Products.Remove(p);
            pdb.SaveChanges();
            return RedirectToAction("ProductsPage");
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product np)
        {
            if (np.Image != null & np.Name != null & np.Price != null & np.Description != null & np.Category != null)
            {
                pdb.Entry(np).State = EntityState.Added;
                pdb.SaveChanges();
                Group myGroup = cdb.Groups.Where(x => x.Name == np.Category).FirstOrDefault();
                if (myGroup == null)
                {
                    Group NewGroup = new Group();
                    NewGroup.Name = np.Category;
                    cdb.Entry(NewGroup).State = EntityState.Added;
                    cdb.SaveChanges();
                }
                return RedirectToAction("ProductsPage");
            }
            else
            {
                return RedirectToAction("AddProduct");
            }
        }
        public ActionResult CartIn()
        {
            Cart usercart = cartdb.Carts.Where(x => x.id > 0).FirstOrDefault();
            if (usercart == null)
            {
                return Redirect("/Home/Main");
            }
            return View(cartdb.Carts);
        }
        public ActionResult CartOut()
        {
            Cart usercart = cartdb.Carts.Where(x => x.id > 0).FirstOrDefault();
            Order neworder = new Order();
            User orderuser = db.Users.Where(x => x.IsLogged > 0).FirstOrDefault();
            neworder.OrderDate = DateTime.Now.ToString();
            int price = 0;
            string goodsids = "";
            while (usercart != null)
            {
                price += usercart.Price;
                Product TempProduct = pdb.Products.Where(x => x.Name == usercart.Name).FirstOrDefault();
                goodsids = goodsids + " " + TempProduct.id.ToString();
                cartdb.Carts.Remove(usercart);
                cartdb.SaveChanges();
                usercart = cartdb.Carts.Where(x => x.id > 0).FirstOrDefault();
            }
            neworder.TotalPrice = price;
            neworder.Goods = goodsids;
            neworder.userid = orderuser.id;
            odb.Entry(neworder).State = EntityState.Added;
            odb.SaveChanges();
            int orderid = neworder.id;
            if (orderuser.Orders == "0")
            {
                orderuser.Orders = orderid.ToString();
            }
            else
            {
                orderuser.Orders = orderuser.Orders + " " + orderid.ToString();
            }
            return Redirect("/Home/Main");
        }

        [HttpGet]
        public ActionResult OrdersPage(int? page)
        {
            OrdersList = new List<Order>();
            foreach (var item in odb.Orders)
            {
                OrdersList.Add(item);
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(OrdersList.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult ConfirmOrder(int id)
        {
            Order o = odb.Orders.Find(id);
            if (o == null)
            {
                return HttpNotFound();
            }
            return View(o);
        }
        [HttpPost, ActionName("ConfirmOrder")]
        public ActionResult OrderConfirmed(int id)
        {
            Order o = odb.Orders.Find(id);
            if (o == null)
            {
                return HttpNotFound();
            }
            o.Status = 1;
            odb.Entry(o).State = EntityState.Modified;
            odb.SaveChanges();
            return RedirectToAction("OrdersPage");
        }

        [HttpGet]
        public ActionResult CancelOrder(int id)
        {
            Order o = odb.Orders.Find(id);
            if (o == null)
            {
                return HttpNotFound();
            }
            return View(o);
        }
        [HttpPost, ActionName("CancelOrder")]
        public ActionResult CancelConfirmed(int id)
        {
            Order o = odb.Orders.Find(id);
            if (o == null)
            {
                return HttpNotFound();
            }
            o.Status = 2;
            odb.Entry(o).State = EntityState.Modified;
            odb.SaveChanges();
            return RedirectToAction("OrdersPage");
        }

        public ActionResult EditGroup(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }
            Group r = cdb.Groups.Find(id);
            if (r != null)
            {
                TempData["OldGroup"] = r.Name;
                return View(r);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult EditGroup(Group r)
        {
            if (r.Name != null)
            {
                cdb.Entry(r).State = EntityState.Modified;
                cdb.SaveChanges();
                string oldname = TempData["OldGroup"].ToString();
                Product myProduct = pdb.Products.Where(x => x.Category == oldname).FirstOrDefault();
                while (myProduct != null)
                {
                    myProduct.Category = r.Name;
                    pdb.Entry(myProduct).State = EntityState.Modified;
                    pdb.SaveChanges();
                    myProduct = pdb.Products.Where(x => x.Category == oldname).FirstOrDefault();
                }

                return RedirectToAction("GroupsPage");
            }
            else
            {
                return RedirectToAction("EditGroup");
            }

        }
        public ActionResult EditProduct(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }
            Product r = pdb.Products.Find(id);
            if (r != null)
            {
                return View(r);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult EditProduct(Product r)
        {
            if (r.Image != null & r.Name != null & r.Price != null & r.Description != null & r.Category != null)
            {
                pdb.Entry(r).State = EntityState.Modified;
                pdb.SaveChanges();
                return RedirectToAction("ProductsPage");
            }
            else
            {
                return RedirectToAction("EditProduct");
            }
        }
    }
}