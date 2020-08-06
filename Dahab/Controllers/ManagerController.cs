using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dahab.Models;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

#region Author
/*
    *[Auth : Ata Sabri Ata Ahmed
    * Version : V_001
    * Info : Api Controller For Manage All Requests & Responses From Wahag APP 
    * E-Mail : ata.sabry@rooyadev.com]
 */
#endregion

namespace Dahab.Controllers
{
    public class ManagerController : ApiController
    {
        // DataBase Context 
        DB db = new DB();



        // ___________________________________ Authentication && Profile __________________________
        [HttpPost]
        public IHttpActionResult Register([]string Lan)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Lan);
                #region Params
                string Name = HttpContext.Current.Request.Form["Name"];
                string FirstName = HttpContext.Current.Request.Form["FirstName"];
                string LastName = HttpContext.Current.Request.Form["LastName"];
                string Email = HttpContext.Current.Request.Form["Email"];
                string Password = HttpContext.Current.Request.Form["Password"];
                string Phone = HttpContext.Current.Request.Form["Phone"];
                string Facebook_ID = HttpContext.Current.Request.Form["Facebook_ID"];
                bool AllowNews = Convert.ToBoolean(HttpContext.Current.Request.Form["AllowNews"]);
                #endregion

                //Encrypt Email To Take Token
                string Token = Methods.encrypt(Email);
                //Create New User
                User user = new User
                {
                    Name = Name,
                    Token = Token,
                    FirstName = FirstName,
                    LastName = LastName,
                    Phone = Phone,
                    Email = Email,
                    AllowNews = AllowNews,
                    HasOrders=false
                };
                // Check Is Use Facebook Or Custom Register
                if (!string.IsNullOrEmpty(Facebook_ID))
                {
                    var facebookuser = Methods.FindFaceBookUser(Facebook_ID);
                    if (facebookuser!=null)
                    {
                        return Ok(new { Key = true, Message = APIResource.FacebookUserFound, User = facebookuser });
                    }
                    user.Password = null;
                    user.Facebook_ID = Facebook_ID;
                }
                else
                {
                    user.Password = Password;
                    user.Facebook_ID = null;
                }
                //Validation
                Validate(user);
                if (!ModelState.IsValid)
                {
                    return Ok(new { key = false, Message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).FirstOrDefault() });
                }
                //check If Email Is Used
                if (db.Users.Any(x => x.Email == Email))
                {
                     return Ok(new { Key = false, Message = APIResource.UniqueEmail });
                }
                //Create The User Finally In DB 
                db.Users.Add(user);
                db.SaveChanges();
                return Ok(new { Key = true, Message = APIResource.RegisterDone, User = user });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Login(string Lan)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Lan);
                #region Params          
                string Email = HttpContext.Current.Request.Form["Email"];
                string Password = HttpContext.Current.Request.Form["Password"];
                string Facebook_ID = HttpContext.Current.Request.Form["Facebook_ID"];
                #endregion

                object user;
                if (string.IsNullOrEmpty(Facebook_ID))
                {
                    //Find Normal User Using Email && Password
                    user = db.Users.Where(x => x.Email == Email && x.Password == Password).Select(x => new
                    {
                        x.ID,
                        x.Name,
                        x.Token,
                        x.FirstName,
                        x.LastName,
                        x.Phone,
                        x.Email,
                        x.Password,
                        x.Facebook_ID,
                        x.Address,
                        x.AllowNews,
                        x.BillAddress,
                    }).FirstOrDefault();

                }
                else
                {
                    //Find FaceBook User
                    user = Methods.FindFaceBookUser(Facebook_ID);
                }
                if (user != null)
                {
                    return Ok(new { Key = true, Message = APIResource.LoginDone, User = user });
                }
                return Ok(new { Key = false, Message = APIResource.LoginError });
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Edit_Profile(string Lan)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Lan);
                #region Params
                string Token = HttpContext.Current.Request.Form["Token"];
                string Name = HttpContext.Current.Request.Form["Name"];
                string FirstName = HttpContext.Current.Request.Form["FirstName"];
                string LastName = HttpContext.Current.Request.Form["LastName"];
                string Email = HttpContext.Current.Request.Form["Email"];
                string Password = HttpContext.Current.Request.Form["Password"];
                string Phone = HttpContext.Current.Request.Form["Phone"];
                string Address = HttpContext.Current.Request.Form["Address"];
                string BillAddress = HttpContext.Current.Request.Form["BillAddress"];
                #endregion
                User user = db.Users.SingleOrDefault(x => x.Token == Token);
                //Check User If Found
                if (user == null)
                {
                    return Ok(new { Key = false, Message = APIResource.UserNotFound });
                }
                if (!string.IsNullOrEmpty(Name))
                    user.Name = Name;
                if (!string.IsNullOrEmpty(FirstName))
                    user.FirstName = FirstName;
                if (!string.IsNullOrEmpty(LastName))
                    user.LastName = LastName;
                if (!string.IsNullOrEmpty(Phone))
                    user.Phone = Phone;
                if (!string.IsNullOrEmpty(Email))
                {
                    //Check If Email Used
                    if (db.Users.Where(x => x.ID != user.ID).Any(x => x.Email == Email))
                    {
                        return Ok(new { Key = false, Message = APIResource.UniqueEmail });
                    }
                    user.Email = Email;
                }
                //Check If User Is FaceBook User
                if (!string.IsNullOrEmpty(user.Facebook_ID) && !string.IsNullOrEmpty(Password))
                    user.Password = Password;

                user.Address = Address;
                user.BillAddress = BillAddress;

                //Validation
                Validate(user);
                if (!ModelState.IsValid)
                {
                    return Ok(new { key = false, Message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).FirstOrDefault() });
                }

                //Edit In DB
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(new { Key = true, Message = APIResource.EditProfileDone });

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IHttpActionResult User_Orders()
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            User user = db.Users.SingleOrDefault(x => x.Token == Token);
            //Orders Data
            var Data = user.Orders.Select(x => new
            {
                x.ID,
                x.Name,
                x.City,
                x.Place,
                x.Phone,
                x.Date,
                x.Discount,
                x.DiscountType,
                x.TotalPrice,
                x.FinalPrice,
                x.IsArriveToUser,
                x.Lat,
                x.Log,
                x.UseVisa,
                Details = x.Order_Details.Select(d => new
                {
                    d.Product_ID,
                    d.Product.Name,
                    d.Price,
                    d.Count,
                    d.Message,
                })
            });
            return Ok(Data);
        }




        // ___________________________________ Home Page __________________________________________
        [HttpGet]
        public IHttpActionResult MostOrdered(int Skip,int Take)
        {
            var Data = db.Products.OrderByDescending(x => x.Order_Details.Count).Select(x => new
            {
                x.ID,
                x.Name,
                x.Name_EN,
                x.Model,
                x.Price,
                x.TotalPrice,
                x.Discount,
                Photo = Methods.Domain + "/Uploads/Products/" + x.ID+".jpg"
            }).Skip(Skip).Take(Take);
            return Ok(Data);
        }
        [HttpGet]
        public IHttpActionResult NewProducts(int Skip, int Take)
        {
            var Data = db.Products.OrderByDescending(x => x.ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Name_EN,
                x.Model,
                x.Price,
                x.TotalPrice,
                x.Discount,
                Photo = Methods.Domain + "/Uploads/Products/" + x.ID +".jpg"
            }).Skip(Skip).Take(Take);
            return Ok(Data);
        }
        [HttpGet]
        public IHttpActionResult DiscountProducts(int Skip, int Take)
        {
            var Data = db.Products.OrderByDescending(x => x.Discount).Select(x => new
            {
                x.ID,
                x.Name,
                x.Name_EN,
                x.Model,
                x.Price,
                x.TotalPrice,
                x.Discount,
                Photo = Methods.Domain + "/Uploads/Products/" + x.ID +".jpg"
            }).Skip(Skip).Take(Take);
            return Ok(Data);
        }







        // ___________________________________ Categories && Products ______________________________
        [HttpGet]
        public IHttpActionResult Categories()
        {
            var Data = db.Categories.Select(x => new
            {
                x.ID,
                x.Name,
                x.Name_EN,
                Photo=Methods.Domain+"/Uploads/Categories/"+x.ID+".jpg"
            });
            return Ok(Data);
        }
        [HttpGet]
        public IHttpActionResult Products(int Category_ID)
        {
            var Data = db.Products.Where(x => x.Cat_ID == Category_ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Name_EN,
                x.Model,
                x.Price,
                x.TotalPrice,
                x.Discount,
                Photo = Methods.Domain + "/Uploads/Products/" + x.ID + ".jpg"
            });
            return Ok(Data);
        }
        [HttpGet]
        public IHttpActionResult Product(string Lan, int Product_ID)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Lan);
            var Data = db.Products.Where(x=>x.ID==Product_ID).Select(x => new
            {
                x.ID,
                x.Name,
                x.Name_EN,
                x.Model,
                x.Price,
                x.TotalPrice,
                x.Discount,
                x.Description,
                x.Description_EN,
                Photo = Methods.Domain + "/Uploads/Products/" + x.ID + ".jpg"
            }).SingleOrDefault();
            if(Data==null)
            {
                return Ok(new { Key = false, Message = APIResource.ProductNotFound });
            }
            return Ok(new { Key = true, Data = Data });
        }
        [HttpGet]
        public IHttpActionResult Sizes(int Category_ID)
        {
            var Data = db.Sizes.Where(x => x.Cat_ID == Category_ID).Select(x => new
            {
                x.ID,
                x.Size1
            });
            return Ok(Data);
        }





        //____________________________________ Favorites ___________________________________________
        [HttpPost]
        public IHttpActionResult Favorites()
        {
            string Token = HttpContext.Current.Request.Form["Token"];
            User user = db.Users.SingleOrDefault(x => x.Token == Token);
            if(user==null)
            {
                return Ok(new { Key = false, Message = APIResource.UserNotFound });
            }
            var Data = user.Favorites.Select(x => new
            {
                x.ID,
                x.Product_ID,
                x.Product.Name,
                x.Product.Name_EN,
                x.Product.Price,
                x.Product.Discount,
                x.Product.TotalPrice,
                Photo=Methods.Domain+"/Uploads/Products/"+ x.Product.ID +".jpg"
            });
            return Ok(Data);
        }
        [HttpPost]
        public IHttpActionResult Add_Favorite(string Lan,int Product_ID)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Lan);
                string Token = HttpContext.Current.Request.Form["Token"];
                //Find User && Check If Found
                User user = db.Users.SingleOrDefault(x => x.Token == Token);
                if (user == null)
                {
                    return Ok(new { Key = false, Message = APIResource.UserNotFound });
                }
                //You Already Like This Product
                if(user.Favorites.Any(x=>x.Product_ID==Product_ID))
                {
                    return Ok(new { Key = false, Message = APIResource.IsLiked });
                }
                //Add Favorite Using User_ID && Product_ID And Add To DB
                Favorite fav = new Favorite { Product_ID = Product_ID, User_ID = user.ID };
                db.Favorites.Add(fav);
                db.SaveChanges();
                return Ok(new { Key = true, Message = APIResource.AddFaveDone });

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IHttpActionResult Delete_Favorite(string Lan, int Favorite_ID)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Lan);
                string Token = HttpContext.Current.Request.Form["Token"];
                Favorite fav = db.Favorites.SingleOrDefault(x => x.ID == Favorite_ID && x.User.Token == Token);
                //Check Favorite Is Found
                if (fav == null)
                {
                    return Ok(new { Key = false, Message = APIResource.DeleteFavError });
                }
                //Delete Favorite From DB
                db.Favorites.Remove(fav);
                db.SaveChanges();
                return Ok(new { Key = true, Message = APIResource.DeleteFavDone });
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






        //____________________________________ Search && Order _____________________________________
        [HttpPost]
        public IHttpActionResult Search()
        {
            string Key = HttpContext.Current.Request.Form["Key"];
            var Data = db.Products.Where(x => x.Name.Contains(Key) || x.Name_EN.Contains(Key)).Select(x => new
            {
                x.ID,
                x.Name,
                x.Name_EN,
                x.Model,
                x.Price,
                x.TotalPrice,
                x.Discount,
                Photo = Methods.Domain + "/Uploads/Products/" + x.ID + ".jpg"
            });
            return Ok(Data);
        }
        // Order
        [HttpPost]
        public IHttpActionResult Order(string Lan)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Lan);
                #region Params
                string Token = HttpContext.Current.Request.Form["Token"];
                string Name = HttpContext.Current.Request.Form["Name"];
                string Phone = HttpContext.Current.Request.Form["Phone"];
                string City = HttpContext.Current.Request.Form["City"];
                string Place = HttpContext.Current.Request.Form["Place"];
                string Lat = HttpContext.Current.Request.Form["Lat"];
                string Log = HttpContext.Current.Request.Form["Log"];
                bool UseVisa = Convert.ToBoolean(HttpContext.Current.Request.Form["UseVisa"]);
                string Code = HttpContext.Current.Request.Form["Code"];

                string[] Products = HttpContext.Current.Request.Form.GetValues("Products");
                string[] Products_Quantities = HttpContext.Current.Request.Form.GetValues("Products_Quantities");
                string[] Products_Messages = HttpContext.Current.Request.Form.GetValues("Products_Messages");
                #endregion

                //Find User && Check If Found
                User user = db.Users.SingleOrDefault(x => x.Token == Token);
                if (user == null)
                {
                    return Ok(new { Key = false, Message = APIResource.UserNotFound });
                }
                //Create New Order
                Order order = new Order
                {
                    City = City,
                    Date = DateTime.Now,
                    User_ID = user.ID,
                    Name = Name,
                    Phone = Phone,
                    Place = Place,
                    UseVisa = UseVisa,
                    IsArriveToUser = false,
                };
                if (!string.IsNullOrEmpty(Lat))
                    order.Lat = Convert.ToDouble(Lat);
                if (!string.IsNullOrEmpty(Log))
                    order.Log = Convert.ToDouble(Log);

                //Check validation
                Validate(order);
                if (!ModelState.IsValid)
                {
                    return Ok(new { key = false, Message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).FirstOrDefault() });
                }
                //Check Code Is Found And Discount Of It
                #region Check Code Validation
                if (!string.IsNullOrEmpty(Code))
                {
                    int c = int.Parse(Code);
                    Code CodeDiscount = db.Codes.SingleOrDefault(x => x.Code1 == c);
                    if (CodeDiscount != null)
                    {
                        order.Discount = CodeDiscount.Discount;
                        order.DiscountType = "Code Discount";
                        db.Codes.Remove(CodeDiscount);
                    }
                    else
                    {
                        return Ok(new { Key = false, Message = APIResource.InvalidCode });
                    }
                }
                #endregion

                //Check Products Validation
                #region Add Order Details Data
                if (Products.Length != Products_Quantities.Length ||
                    Products.Length != Products_Messages.Length ||
                    Products_Quantities.Length != Products_Messages.Length ||
                    string.IsNullOrEmpty(Products[0]))
                {
                    return Ok(new { Key = false, Message = APIResource.ProductsValid });
                }
                //Add Products Data To Order
                for (int i = 0; i < Products.Length; i++)
                {
                    int Product_ID = int.Parse(Products[i]);
                    Product product = db.Products.Find(Product_ID);
                    Order_Details order_details = new Order_Details
                    {
                        Product_ID = product.ID,
                        Count = int.Parse(Products_Quantities[i]),
                        Message = Products_Messages[i],
                        Price = product.TotalPrice
                    };
                    order.Order_Details.Add(order_details);
                }
                #endregion

                order.TotalPrice = order.Order_Details.Sum(x => x.Price);

                //Check First Order For Discount
                #region First Order Discount
                if (!user.HasOrders&&user.Orders.Count==0)
                {

                    order.Discount = order.Discount.HasValue ? order.Discount.Value + 10 : 10;
                    order.DiscountType += "First Order Discount";
                    user.HasOrders = true;
                    db.Entry(user).State = EntityState.Modified;
                }
                #endregion

                //Calculate The Final Price And Pass To Order
                if (order.Discount.HasValue)
                {
                    double Discount = (order.Discount.Value / 100) * order.TotalPrice;
                    order.FinalPrice = order.TotalPrice - Discount;
                }
                else
                {
                    order.FinalPrice = order.TotalPrice;
                }
                //Add Order To DB 
                db.Orders.Add(order);
                db.SaveChanges();

                return Ok(new { key = true, Message = APIResource.OrderDone });

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}
