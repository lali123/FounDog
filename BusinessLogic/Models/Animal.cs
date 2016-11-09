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
        public string UserId { get; set; }
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

        public string AnimalIdToString()
        {
            return AnimalId.ToString();
        }

        public void AnimalIdToObjectId(string AnimalId)
        {
            var objId = new ObjectId();
            ObjectId.TryParse(AnimalId,out objId);
         
            this.AnimalId = objId;
        }
    }
}
