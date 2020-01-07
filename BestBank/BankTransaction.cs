using System;
using System.Collections.Generic;
using System.Text;

namespace BestBank
{
    public enum TransactionType
    {
        Deposit
        , Withdrawal
    }
    internal class BankTransaction
    {
        public DateTime Time { get;  }
        public TransactionType Type { get; }
        public uint Amount { get; }
        

        public BankTransaction(uint amount, TransactionType type) {
            Amount = amount;
            Type = type;
            Time = DateTime.Now;
        }

        public override string ToString() {
            var txnAmount = Type switch {
                TransactionType.Deposit => $"{Amount}",
                TransactionType.Withdrawal => $"({Amount})"
            };
            return $"{Time:yyyy-MM-dd HH:mm}\t{txnAmount}";
        }
    }
}
