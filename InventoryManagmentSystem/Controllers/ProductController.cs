using InventoryManagmentSystem.Models;
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


        [HttpPost]
        public ActionResult AddNewProduct(ProdactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            var newProduct = new Product
            {
                ProductName = model.prductName,
                ProductDescription = model.ProductDescription,
                Price = model.unitPrice,
                SupplierID = model.supplierId,
                BrandId = model.brandId
            };
            _DbContext.Products.Add(newProduct);
            _DbContext.SaveChanges();
            return Content("Successfully added a new Brand");
        }


    }
}