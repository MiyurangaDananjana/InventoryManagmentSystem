using InventoryManagmentSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace InventoryManagmentSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly InventorySystemEntities1 _DbContext;

        public ProductController(InventorySystemEntities1 DbContext)
        {
            this._DbContext = DbContext;
        }
        // Add New Brand
        [HttpPost]
        public ActionResult AddNewBrand(string sessionKey)
        {
            if (string.IsNullOrEmpty(sessionKey))
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            // Access the model data from the request manually
            var model = new BrandDetailsModel();
            TryUpdateModel(model);
            var isSessionKey = _DbContext.UserSessions.FirstOrDefault(x => x.SessionKey == sessionKey);
            if (isSessionKey != null)
            {
                var isBarnd = _DbContext.Brands.FirstOrDefault(x => x.BrandName == model.BrandName);
                if (isBarnd == null)
                {
                    var brandDetails = new Brand
                    {
                        BrandName = model.BrandName,
                        BrandUpdateBy = isSessionKey.UserId
                    };
                    // Assuming 'brandDetails' represents the brand information you want to add
                    _DbContext.Brands.Add(brandDetails);
                    _DbContext.SaveChanges();
                    return Json(new { success = true, message = "Successfully added a new Brand", brand = brandDetails });
                }
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            return new HttpStatusCodeResult(400, "Bad Request");
        }

        //Get Brand Details 
        [HttpGet]
        public ActionResult GetBrandDetails()
        {
            var brandDetails = (from brand in _DbContext.Brands
                                join user in _DbContext.UserRegisters
                                on brand.BrandUpdateBy equals user.Id
                                select new
                                {
                                    BrandId = brand.BrandId,
                                    BrandName = brand.BrandName,
                                    CreateBy = user.UserName
                                }).ToList();
            if (brandDetails != null)
            {
                return Json(brandDetails, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(400, "Bad Request");
        }

        [HttpGet]
        public ActionResult GetBrandDetailsById(int id)
        {
            var list = (from brand in _DbContext.Brands
                        where brand.BrandId == id
                        select new BrandDetailsModel
                        {
                            Id = brand.BrandId,
                            BrandName = brand.BrandName
                        }).FirstOrDefault();
            if (list != null)
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        // Update Brand Details
        [HttpPost]
        public ActionResult UpdateBrand(string sessionKey, BrandViewModel model)
        {
            if (string.IsNullOrEmpty(sessionKey))
            {
                return new HttpStatusCodeResult(400, "Bad Request: SessionKey is required.");
            }

            if (!ModelState.IsValid)
            {
                // Return validation errors along with a 400 Bad Request response
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return new HttpStatusCodeResult(400, string.Join("; ", errors));
            }
            var isSessionKey = _DbContext.UserSessions.FirstOrDefault(x => x.SessionKey == sessionKey);
            if (isSessionKey != null)
            {
                var existingBrand = _DbContext.Brands.FirstOrDefault(x => x.BrandId == model.Id);

                if (existingBrand != null)
                {
                    if (existingBrand.BrandName == model.BrandName)
                    {
                        return new HttpStatusCodeResult(400, "Bad Request: Brand exist.");
                    }
                    // Update the existing brand's properties
                    existingBrand.BrandName = model.BrandName;
                    existingBrand.BrandUpdateBy = isSessionKey.UserId;
                    _DbContext.SaveChanges();
                    return Content("Success");
                }
                return new HttpStatusCodeResult(400, "Bad Request: Brand does not exist.");
            }

            return new HttpStatusCodeResult(400, "Bad Request: SessionKey not found.");
        }

        //Delete Brand
        [HttpPost]
        public ActionResult DeleteBrandDetails(int barndId)
        {
            if (barndId <= 0)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            var isBarnd = _DbContext.Brands.FirstOrDefault(x => x.BrandId == barndId);
            if (isBarnd != null)
            {
                _DbContext.Brands.Remove(isBarnd);
                _DbContext.SaveChanges();
                return Content("Delete");
            }
            return Content("Fail");
        }

        [HttpPost]
        public ActionResult AddNewProduct(ProdactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            var newProduct = new Product
            {
                ProductName = model.ProductName,
                ProductDescription = model.ProductDescription,
                Price = model.UnitPrice,
                SupplierID = model.SupplierId,
                BrandId = model.BrandId
            };
            _DbContext.Products.Add(newProduct);
            _DbContext.SaveChanges();
            return Json(new { success = true, message = "Successfully added a new Brand" });
        }

        [HttpGet]
        public ActionResult BrandIdName()
        {
            var brandDetails = (from brand in _DbContext.Brands
                                orderby brand.BrandName ascending
                                select new
                                {
                                    BrandId = brand.BrandId,
                                    BrandName = brand.BrandName

                                }).ToList();
            if (brandDetails != null)
            {
                return Json(brandDetails, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(400, "Bad Request");
        }

        // add to the collection and return the data
        public ActionResult GetProduct()
        {
            List<ProductView> products = new List<ProductView>();

            var productL = (from product in _DbContext.Products
                            join brand in _DbContext.Brands on product.BrandId equals brand.BrandId
                            join supplier in _DbContext.Suppliers on product.SupplierID equals supplier.SupplierID
                            select new
                            {
                                ProductId = product.ProductId,
                                BrandName = brand.BrandName,
                                ProductName = product.ProductName,
                                ProductDescription = product.ProductDescription,
                                Price = product.Price,
                                SupplierName = supplier.SupplierName
                            });
            foreach (var product in productL)
            {
                ProductView view = new ProductView();
                view.ProductId = product.ProductId;
                view.BrandName = product.BrandName;
                view.ProductName = product.ProductName;
                view.ProductDescription = product.ProductDescription;
                view.UnitPrice = (decimal)product.Price;
                view.SupplierName = product.SupplierName;
                products.Add(view);
            }
            return Json(products, JsonRequestBehavior.AllowGet);

        }

        // Product details get by the id
        [HttpGet]
        public ActionResult GetProductById(int productId)
        {
            var productList = (from product in _DbContext.Products
                               where product.ProductId == productId
                               join brand in _DbContext.Brands on product.BrandId equals brand.BrandId
                               join supplier in _DbContext.Suppliers on product.SupplierID equals supplier.SupplierID
                               select new
                               {
                                   ProductId = product.ProductId,
                                   BrandName = brand.BrandName,
                                   ProductName = product.ProductName,
                                   ProductDescription = product.ProductDescription,
                                   Price = product.Price,
                                   SupplierName = supplier.SupplierName

                               }).FirstOrDefault();

            if (productList == null)
            {
                return new HttpStatusCodeResult(404, "Not Found");
            }
            return Json(productList, JsonRequestBehavior.AllowGet);
        }

        // Update Product
        [HttpPost]
        public ActionResult UpdateProduct(ProdactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            var existingProduct = _DbContext.Products.Find(model.ProductId);
            if (existingProduct == null)
            {
                return new HttpStatusCodeResult(404, "Not Found");
            }
            existingProduct.ProductName = model.ProductName;
            existingProduct.ProductDescription = model.ProductDescription;
            existingProduct.Price = model.UnitPrice;
            existingProduct.SupplierID = model.SupplierId;
            existingProduct.BrandId = model.BrandId;
            _DbContext.SaveChanges();
            return Json(new { success = true, message = "Successfully updated the product" });
        }

        [HttpPost]
        public ActionResult DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            var isProduct = _DbContext.Products.FirstOrDefault(x => x.ProductId == id);
            if (isProduct != null)
            {
                _DbContext.Products.Remove(isProduct);
                _DbContext.SaveChanges();
                return Content("Delete");
            }
            return Content("Fail");

        }

        // add new product variant details
        [HttpPost]
        public ActionResult AddProductVariant(ProductVariantViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            var newProductVariant = new ProductVariante
            {
               ProductsId = model.ProductId,
               Description = model.Description,
               StockQuantity = model.Quantity
            };
            _DbContext.ProductVariantes.Add(newProductVariant);
            _DbContext.SaveChanges();
            return Json(new { success = true, message = "Successfully added a new Brand" });
        }


        //Get Product variant details
        [HttpGet]
        public ActionResult GetProductVariants()
        {
            List<ProductView> products = new List<ProductView>();
            var list = (from product in _DbContext.Products
                        select new
                        {
                            ProductId = product.ProductId,
                            ProductName = product.ProductName
                        });
            foreach (var product in list)
            {
                ProductView view = new ProductView();
                view.ProductId = product.ProductId;
                view.ProductName = product.ProductName;
                products.Add(view);
            }
            return Json(products, JsonRequestBehavior.AllowGet);
        }

    }
}