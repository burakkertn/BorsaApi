using BorsaApi.Concrete;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BorsaApi.DataAccessLayer
{
        public class UserService : IUserService
        {
            private readonly IMongoCollection<User> _users;

            public UserService(IBorsaApiDatabaseSettings settings, IMongoClient mongoClient)
            {
                var database = mongoClient.GetDatabase(settings.DatabaseName);
                _users = database.GetCollection<User>(settings.UserCollectionName);
            }

            public User Create(User user)
            {
                user.Id = ObjectId.GenerateNewId().ToString();
                _users.InsertOne(user);
                return user;
            }

            public List<User> Get()
            {
                return _users.Find(user => true).ToList();
            }

            public User Get(string id)
            {
                return _users.Find(user => user.Id == id).FirstOrDefault();
            }

           

            public void Remove(string id)
            {
                _users.DeleteOne(user => user.Id == id);
            }

            public void Update(string id, User user)
            {
                _users.ReplaceOne(user => user.Id == id, user);
            }
        }
    }