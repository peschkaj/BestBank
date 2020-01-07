using System;
using System.Collections.Generic;

namespace BestBank
{
    internal class Bank
    {
        private const string DefaultFileName = "bank.json";
        private string _fileName;

        public Bank() : this(DefaultFileName) { }

        public Bank(string fileName) {
            _fileName = fileName;
            Accounts = new List<Account>();
        }

        public List<Account> Accounts { get; }

        public bool CreateAccount(string username, string password) {
            var a = new Account(username, password);
            if (Accounts.Contains(a)) {
                return false;
            }

            Accounts.Add(a);
            return true;
        }

        public static Bank LoadFromFile() {
            throw new NotImplementedException();
        }

        // TODO save to file
        public void SaveToFile() {
            throw new NotImplementedException();
        }
    }
}