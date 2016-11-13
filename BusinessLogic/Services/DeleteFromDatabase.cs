using BusinessLogic.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class DeleteFromDatabase
    {
        public static bool DeleteFoundAnimal(string id)
        {
            return Database.Instance.DeleteFoundAnimal(id);
        }

        public static bool DeleteWantedAnimal(string id)
        {
            return Database.Instance.DeleteWantedAnimal(id);
        }

        public static bool DeleteUser(string id)
        {
            return Database.Instance.DeleteUser(id);
        }
    }
}
