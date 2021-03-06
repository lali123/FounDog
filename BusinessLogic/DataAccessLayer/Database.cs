﻿using BusinessLogic.Models;
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


        public bool UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq("Id", user.Id);
            var update = Builders<User>.Update.Set("Name", user.Name)
                .Set("PhoneNumber", user.PhoneNumber)
                .Set("Date", DateTime.Now)
                .Set("EmailAddress", user.EmailAddress)
                .Set("Password", user.Password);
            var updateResult = UserCollection.UpdateOne(filter, update);

            return updateResult.IsAcknowledged;
        }

        public bool UpdateAnimal(Animal animal)
        {
            var filter = Builders<Animal>.Filter.Eq("AnimalId", animal.AnimalId);
            var update = Builders<Animal>.Update.Set("Breed", animal.Breed)
                .Set("Description", animal.Description)
                .Set("Date", DateTime.Now)
                .Set("Latitude", animal.Latitude)
                .Set("Longitude", animal.Longitude)
                .Set("Image", animal.Image);
            var updateResult = FoundAnimalCollection.UpdateOne(filter, update);
            
            return updateResult.IsAcknowledged;
        }
        
        public bool UpdateWantedAnimal(Animal animal)
        {
            var filter = Builders<Animal>.Filter.Eq("AnimalId", animal.AnimalId);
            var update = Builders<Animal>.Update.Set("Breed", animal.Breed)
                .Set("Description", animal.Description)
                .Set("Date", DateTime.Now)
                .Set("Latitude", animal.Latitude)
                .Set("Longitude", animal.Longitude)
                .Set("Image", animal.Image);
            var updateResult = WantedAnimalCollection.UpdateOne(filter, update);

            return updateResult.IsAcknowledged;
        }


        public bool DeleteUser(string id)
        {
            var deleteResult = UserCollection.DeleteOne(a => a.Id == id);
            return deleteResult.IsAcknowledged;
        }

        public bool DeleteFoundAnimal(String id)
        {
            var deleteResult = FoundAnimalCollection.DeleteOne(a=>a.AnimalId == new MongoDB.Bson.ObjectId(id));
            return deleteResult.IsAcknowledged;           
        }

        public bool DeleteWantedAnimal(String id)
        {
            var deleteResult = WantedAnimalCollection.DeleteOne(a => a.AnimalId == new MongoDB.Bson.ObjectId(id));
            return deleteResult.IsAcknowledged;
        }
    }
}
