using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public interface TransactionLogger
    {
        void Log(string message);
    }
    public class BankAccount
    {
        private readonly TransactionLogger logg;
        private float bal;
        public string AccountNumber { get; }
        // Конструктор, который принимает номер счёта (accountNumber) и начальный баланс (balance, по умолчанию 0)
        public BankAccount(string accountNumber, float balance = 0.0f, TransactionLogger logger = null)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                throw new ArgumentException("Account number can't be empty", "accountNumber");
            }

            if (balance < 0)
            {
                throw new ArgumentException("Initial balance can't be negative", "balance");
            }

            AccountNumber = accountNumber;
            bal = balance;
            logg = logger;
        }
        public void Deposit(float amount) // Метод для пополнения баланса
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive", "amount");
            }

            bal += amount;

            if (logg != null)
            {
                logg.Log("Deposited " + amount + " to " + AccountNumber + ". New balance: " + bal);
            }
        }
        public void Withdraw(float amount) // Метод для снятия денег со счёта
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdraw amount must be positive", "amount");
            }

            if (amount > bal)
            {
                throw new InvalidOperationException("Insufficient funds");
            }

            bal -= amount;

            if (logg != null)
            {
                logg.Log("Withdrew " + amount + " from " + AccountNumber + ". New balance: " + bal);
            }
        }
        public float GetBalance() // Метод для получения текущего баланса
        {
            return bal;
        }
    }
}
