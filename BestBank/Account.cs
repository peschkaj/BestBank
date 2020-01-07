#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace BestBank
{
    internal class Account
    {
        private readonly List<BankTransaction> _transactions;

        public Account(string username, string password) {
            Password = password;
            Username = username;
            _transactions = new List<BankTransaction>();
        }

        public string Username { get; }
        internal string Password { get; }

        public int CheckBalance() {
            return _transactions.Select(t => t.Type switch {
                TransactionType.Deposit => (int) t.Amount,
                TransactionType.Withdrawal => (int) (0 - t.Amount),
                _ => throw new Exception("Unknown transaction type found, stop editing files by hand.")
            }).Sum();
        }

        public void DepositFunds(uint amount) {
            _transactions.Add(new BankTransaction(amount, TransactionType.Deposit));
        }

        public void WithdrawFunds(uint amount) {
            _transactions.Add(new BankTransaction(amount, TransactionType.Withdrawal));
        }

        /// <summary>
        ///     Create a copy of the transaction history.
        /// </summary>
        /// <returns>A complete copy of the transaction history for the user.</returns>
        public List<BankTransaction> TransactionHistory() {
            return new List<BankTransaction>(_transactions);
        }

        public override bool Equals(object? obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Account) obj);
        }

        protected bool Equals(Account other) {
            return Username == other.Username;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Username, Password);
        }
    }
}