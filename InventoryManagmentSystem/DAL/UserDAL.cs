using InventoryManagmentSystem.Models;
using System.Data.SqlClient;
using System.Linq;

namespace InventoryManagmentSystem.DAL
{
    public class UserDAL
    {
        private readonly InventorySystemEntities1 _DbContext;

        public UserDAL(InventorySystemEntities1 inventorySystemEntities)
        {
            this._DbContext = inventorySystemEntities;
        }


        public int GetUserIdBySessionKey(string session)
        {
            var userIds = _DbContext.getSessionSp(session).ToList();

            if (userIds.Count > 0)
            {
                int? userIdNullable = userIds[0];

                if (userIdNullable.HasValue)
                {
                    int userId = userIdNullable.Value;
                    return userId;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }

    }
}