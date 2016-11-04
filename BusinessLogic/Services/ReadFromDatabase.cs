using BusinessLogic.DataAccessLayer;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ReadFromDatabase
    {
        public static List<User> ReadUsersFromDatabase()
        {
            return Database.Instance.FindUsers((e) => { return true; });
        }

        public static List<User> ReadUsersFromDatabase(string name)
        {
            return Database.Instance.FindUsers((e) =>
            {
                if (string.Equals(e.Name, name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public static List<Animal> ReadAnimalFromDatabase()
        {
            return Database.Instance.FindFoundAnimals((e) => { return true; });
        }

        public static List<Animal> ReadAnimalFromDatabase(string userId)
        {
            return Database.Instance.FindFoundAnimals((e) =>
            {
                if (e.UserId.Equals(new Guid(userId)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }

        //public static List<Animal> ReadAnimalFromDatabaseByLocation(GeoCoordinate coordinate)
        //{
        //    return Database.Instance.FindFoundAnimals((e) => {
        //        if (e.Coordinates.GetDistanceTo(coordinate) <= 500)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    });
        //}

        public static List<Animal> ReadWantedAnimalFromDatabase()
        {
            return Database.Instance.FindWantedAnimals((e) => { return true; });
        }

        //public static List<Animal> ReadWantedAnimalFromDatabaseByLocation(GeoCoordinate coordinate)
        //{
        //    return Database.Instance.FindWantedAnimals((e) => {
        //        if (e.Coordinates.GetDistanceTo(coordinate) <= 500)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    });
        //}
    }
}
