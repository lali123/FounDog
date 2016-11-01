using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class Animal
    {
        [BsonId]
        public ObjectId AnimalId { get; set; }
        public ObjectId UserId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Breed { get; set; }
        public byte[] Image { get; set; }

        public override string ToString()
        {
            return String.Format("{0} x:{1} y:{2}\n{3}", Date.ToString(),Latitude, Longitude, Description);
        }
    }
}
