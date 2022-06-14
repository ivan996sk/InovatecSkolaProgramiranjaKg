
namespace BankApp.Models
{
    public abstract class Bank
    {
        public Bank()
        {
        }

        public Bank(string name)
        {
            this.name = name;
        }

        public string name { get; set; }

        public abstract bool checkTerms(LoanApplication application);

    }
}