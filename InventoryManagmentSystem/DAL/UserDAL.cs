using InventoryManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagmentSystem.DAL
{
    public class UserDAL
    {
        private readonly InventorySystemEntities1 _DbContext;

        public UserDAL(InventorySystemEntities1 inventorySystemEntities)
        {
            this._DbContext = inventorySystemEntities;
        }
       


    }
}