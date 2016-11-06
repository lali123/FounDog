using BusinessLogic.DataAccessLayer;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UpdateDatabase
    {
        public static bool UpdateFoundAnimal(Animal animal)
        {
            return Database.Instance.UpdateAnimal(animal);
        }
    }
}
