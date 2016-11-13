using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class User
    {
        [BsonId]
        public ObjectId UserId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime Date { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return String.Format("{0}\n phone number: {1}\n e-mail addres:{2}\n", Name, PhoneNumber, EmailAddress);
        }
    }
}
