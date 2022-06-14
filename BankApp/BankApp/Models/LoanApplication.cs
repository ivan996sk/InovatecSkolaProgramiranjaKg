using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankApp.Models
{
    public class LoanApplication
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string bank { get; set; }
        public Client client { get; set; }
        public double loanAmount { get; set; }
        public int numberOfEMI { get; set; }
    }
}
