using InventoryManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFramework.Extensions;
using System.Web.Mvc;
using System.Web.Configuration;

namespace InventoryManagmentSystem.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly InventorySystemEntities1 _DbContext;
        public OrderItemController(InventorySystemEntities1 dbContext)
        {
            _DbContext = dbContext;
        }

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

        [HttpPost]
        public ActionResult SaveOrder(int customerid, List<OrderItems> orders)
        {
            if (customerid <= 0)
            {
                return new HttpStatusCodeResult(400, "Bad Request: Invalid customerid.");
            }
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Bad Request: ModelState is not valid.");
            }
            var orderItemsList = new List<OrderItem>();
            foreach (var order in orders)
            {
                int ProductId = order.ProductVariantId;
                int Quantity = order.Quantity;
                var productVariant = _DbContext.ProductVariantes.FirstOrDefault(x => x.ProductVariantId == ProductId);
                if (productVariant != null)
                {
                    if (Quantity <= productVariant.StockQuantity)
                    {
                        productVariant.StockQuantity -= Quantity;
                        _DbContext.SaveChanges();
                    }
                    else
                    {
                        return new HttpStatusCodeResult(400, "Bad Request");
                    }
                }
                else
                {
                    return new HttpStatusCodeResult(400, "Bad Request");
                }
                var orderItem = new OrderItem
                {
                    BrandId = order.BrandId,
                    ProductId = order.ProductId,
                    ProductVariantId = order.ProductVariantId,
                    ItemPrice = order.ItemPrice,
                    Quantity = order.Quantity,
                    CreateDate = DateTime.Now,
                    CustomerId = customerid
                };
                orderItemsList.Add(orderItem);
            }
            _DbContext.OrderItems.AddRange(orderItemsList); 
            _DbContext.SaveChanges();
            return new HttpStatusCodeResult(200, "Orders saved successfully.");
        }

        [HttpGet]
        public ActionResult GetPaymentMethod()
        {
            var paymentMethod = _DbContext.PayMentMethods.ToList();
            var paymentMethods = paymentMethod.Select(u => new PaymentMethodViewModel
            {
                PaymentMethodId = u.PaymentMethodsId,
                PaymentMethod = u.PaymentMethod1

            }).ToList();
            return Json(paymentMethods, JsonRequestBehavior.AllowGet);
        }

    }
}