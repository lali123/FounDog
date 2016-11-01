using BusinessLogic.DataAccessLayer;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class WriteToDatabase
    {
        public static void WriteUserToDatabase(User user)
        {
            Database.Instance.AddUsers(user);
        }

        public static void WriteFoundAnimalToDatabase(Animal animal)
        {
            Database.Instance.AddFoundAnimal(animal);
        }

        public static void WriteWantedAnimalToDatabase(Animal animal)
        {
            Database.Instance.AddWantedAnimal(animal);
        }
    }
}
