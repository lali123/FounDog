using BusinessLogic.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DataAccessLayer
{
    internal sealed class Database
    {
        private static IMongoClient _client;
        private static IMongoDatabase _database;

        public IMongoDatabase AnimalsDatabase
        {
            get { return _database; }
        }

        public IMongoCollection<User> UserCollection
        {
            get { return _database.GetCollection<User>("users"); }
        }

        public IMongoCollection<Animal> FoundAnimalCollection
        {
            get { return _database.GetCollection<Animal>("foundanimals"); }
        }

        public IMongoCollection<Animal> WantedAnimalCollection
        {
            get { return _database.GetCollection<Animal>("wantedanimals"); }
        }

        #region Singleton

        private static volatile Database instance;
        private static object syncRoot = new Object();

        public static Database Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Database();
                    }
                }

                return instance;
            }
        }

        public Database()
        {
            string connectionString = "mongodb://localhost:27017";
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("FindAnimalsDatabase");
        }

        #endregion

        public List<User> FindUsers(Func<User, bool> func)
        {
            return UserCollection.AsQueryable().Where(func).ToList();
        }

        public List<Animal> FindFoundAnimals(Func<Animal, bool> func)
        {
            return FoundAnimalCollection.AsQueryable().Where(func).ToList();
        }

        public List<Animal> FindWantedAnimals(Func<Animal, bool> func)
        {
            return WantedAnimalCollection.AsQueryable().Where(func).ToList();
        }

        public void AddUsers(User user)
        {
            UserCollection.InsertOne(user);
        }

        public void AddWantedAnimal(Animal animal)
        {
            WantedAnimalCollection.InsertOne(animal);
        }

        public void AddFoundAnimal(Animal animal)
        {
            FoundAnimalCollection.InsertOne(animal);
        }
    }
}
