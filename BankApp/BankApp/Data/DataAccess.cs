using BankApp.Models;
using MongoDB.Driver;

namespace BankApp.Data
{
    internal class DataAccess
    {
        private const string ConnectionString = "mongodb+srv://si_user:si_user@sidatabase.twtfm.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
        private const string DatabaseName = "InovatecInternship";
        private const string BankCollection = "Bank";
        private const string ClientCollection = "Client";
        private const string AcceptedLoanCollection = "AcceptedLoans";

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T>(collection);
        }

        public async Task<List<LoanApplication>> GetAllLoans()
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);
            var results = await loanCollection.FindAsync(_ => true);
            return results.ToList();
        }

        public void AddLoan(LoanApplication loan)
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);
            loanCollection.InsertOne(loan);
        }

        public void UpdateLoan(LoanApplication loan)
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);
            var filter = Builders<LoanApplication>.Filter.Eq("_id", loan._id);
            loanCollection.ReplaceOne(filter, loan, new ReplaceOptions { IsUpsert = true });
        }

        public void DeleteLoan(LoanApplication loan)
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);
            loanCollection.DeleteOne(c => c._id == loan._id);
        }

        public List<LoanApplication> GetClientLoans(string clientId, string bankName)
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);
            
            List<LoanApplication> loans = null;
            loans = loanCollection.Find(loan => (loan.bank == bankName) && loan.client.idNumber == clientId).ToList();

            if (loans == null)
                return null;
            else
                return loans;
        }

        public List<LoanApplication> GetClientLoans(string clientId)
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);

            List<LoanApplication> loans = null;
            loans = loanCollection.Find(loan => loan.client.idNumber == clientId).ToList();

            return loans;
        }

        public Client GetClient(string clientId)
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);
            LoanApplication loan= loanCollection.Find(la => la.client.idNumber == clientId).FirstOrDefault();
            return loan.client;
        }

        public void PayLoan(string id)
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);
            LoanApplication la = loanCollection.Find(loan => loan._id == id).FirstOrDefault();
            la.loanAmount -= la.loanAmount / la.numberOfEMI;
            --la.numberOfEMI;

            if (la.numberOfEMI == 0)
                DeleteLoan(la);
            else
                UpdateLoan(la);
        }

        public bool ClientExists(string id)
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);
            LoanApplication loan = loanCollection.Find(lc => lc.client.idNumber == id).FirstOrDefault();
            if(loan == null)
                return false;
            return true;
        }
        public bool BankExists(string name)
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);
            LoanApplication loan = loanCollection.Find(lc => lc.bank == name).FirstOrDefault();
            if (loan == null)
                return false;
            return true;
        }

        public bool GetClientLoan(string clientId, string bankName)
        {
            var loanCollection = ConnectToMongo<LoanApplication>(AcceptedLoanCollection);
            LoanApplication loan = loanCollection.Find(lc => lc.bank == bankName && lc.client.idNumber == clientId).FirstOrDefault();
            if (loan == null)
                return false;
            else
                return true;
        }
    }
}
