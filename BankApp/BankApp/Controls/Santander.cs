using BankApp.Data;
using BankApp.Models;

namespace BankApp.Control
{
    public class Santander : Bank
    {
        public Santander()
        {
            name = "Santander";
        }

        public override bool checkTerms(LoanApplication application)
        {
            DataAccess db = new DataAccess();

            if (application.client.yearsExp > 5 && application.loanAmount / application.numberOfEMI < 0.7 * application.client.monthlyIncome)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
