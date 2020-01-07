#nullable enable
using System;
using System.Diagnostics;

namespace BestBank
{
    internal enum UserAction
    {
        Exit = 0,
        CreateAccount,
        Login,
        Deposit,
        Withdraw,
        CheckBalance,
        TransactionHistory,
        Logout
    }

    internal class Program
    {
        private static void Main() {
            var exit = false;

            // TODO read any configuration and application data here
            var bank = new Bank();
            Account? currentAccount = null;

            do {
                PrintMenu(bank, currentAccount);

                switch (ReadMenuOption()) {
                    case UserAction.Exit:
                        exit = true;
                        break;
                    case UserAction.CreateAccount:
                        CreateAccount(bank);
                        break;
                    case UserAction.Login:
                        if (currentAccount != null) {
                            Console.WriteLine(
                                "\nYou are currently logged in.\nLog out before attempting to log in again.\n");
                        } else {
                            currentAccount = Login(bank);

                            if (currentAccount == null)
                                Console.WriteLine(
                                    "Unable to log you in. Please check your credentials and try again.\n");
                        }

                        break;
                    case UserAction.Logout:
                        currentAccount = null;
                        break;
                    case UserAction.Deposit:
                        Debug.Assert(currentAccount != null, nameof(currentAccount) + " != null");
                        DepositFunds(currentAccount);
                        break;
                    case UserAction.Withdraw:
                        Debug.Assert(currentAccount != null, nameof(currentAccount) + " != null");
                        WithdrawFunds(currentAccount);
                        break;
                    case UserAction.CheckBalance:
                        Debug.Assert(currentAccount != null, nameof(currentAccount) + " != null");
                        CheckBalance(currentAccount);
                        break;
                    case UserAction.TransactionHistory:
                        Debug.Assert(currentAccount != null, nameof(currentAccount) + " != null");
                        ViewTransactionHistory(currentAccount);
                        break;
                    default:
                        Console.WriteLine("That is not a valid option. Please select an option from the menu.\n");
                        break;
                }
            } while (!exit);
        }

        private static void CheckBalance(Account currentAccount) {
            Console.WriteLine($"\nBalance is {currentAccount.CheckBalance()}.\n");
        }

        private static int GetIntFromUser(string prompt) {
            int withdrawalAmount;

            Console.Write(prompt);
            var input = Console.ReadLine();
            while (!int.TryParse(input, out withdrawalAmount)) {
                Console.Write(prompt);
                input = Console.ReadLine();
            }

            return withdrawalAmount;
        }

        private static void WithdrawFunds(Account currentAccount) {
            if (currentAccount == null) throw new ArgumentNullException(nameof(currentAccount));

            var withdrawalAmount = GetIntFromUser("Withdrawal amount: ");
            currentAccount.WithdrawFunds((uint) withdrawalAmount);
        }

        private static void DepositFunds(Account currentAccount) {
            if (currentAccount == null) throw new ArgumentNullException(nameof(currentAccount));

            var depositAmount = GetIntFromUser("Deposit Amount");
            currentAccount.DepositFunds((uint) depositAmount);
        }

        private static void ViewTransactionHistory(Account currentAccount) {
            var transactions = currentAccount.TransactionHistory();

            Console.WriteLine($"Transactions for account '{currentAccount.Username}'");

            if (transactions.Count == 0) {
                Console.WriteLine("No transactions.");
                return;
            }

            var currentBalance = 0;

            Console.WriteLine("Txn Date\t\tAmount\tBalance");
            Console.WriteLine("------------------\t------\t-------");

#pragma warning disable 8509
            foreach (var txn in transactions) {
                var txnAmount = txn.Type switch {
                    TransactionType.Deposit => $"{txn.Amount}",
                    TransactionType.Withdrawal => $"({txn.Amount})"
                };

                currentBalance += txn.Type switch {
                    TransactionType.Deposit => (int) txn.Amount,
                    TransactionType.Withdrawal => (int) (0 - txn.Amount)
                };
#pragma warning restore 8509

                Console.WriteLine($"{txn.Time:yyyy-MM-dd HH:mm}\t{txnAmount}\t{currentBalance}");
            }
        }

        private static Account? Login(Bank bank) {
            if (bank == null) throw new ArgumentNullException(nameof(bank));

            var (username, password) = ReadUserNameAndPassword();

            return bank.Accounts.Find(a => a.Username == username && a.Password == password);
        }

        private static (string, string) ReadUserNameAndPassword() {
            Console.Write("Username: ");
            var username = Console.ReadLine();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            return (username, password);
        }

        private static void CreateAccount(Bank bank) {
            bool accountCreated;

            do {
                var (username, password) = ReadUserNameAndPassword();
                accountCreated = bank.CreateAccount(username, password);

                if (accountCreated == false) Console.WriteLine("Unable to create your account. Please try again.\n");
            } while (!accountCreated);
        }

        private static UserAction ReadMenuOption() {
            Console.Write(MenuPrompt);
            var input = Console.ReadLine();

            while (true) {
                int intInput;
                while (!int.TryParse(input, out intInput)) {
                    Console.Write(MenuPrompt);
                    input = Console.ReadLine();
                }

                try {
                    var action = (UserAction) intInput;
                    return action;
                } catch (Exception) {
                    Console.WriteLine("That is not a valid option. Please select an option from the menu.\n");
                }
            }
        }

        private static void PrintMenu(Bank bank, Account? currentAccount) {
            if (bank.Accounts.Count == 0) {
                Console.Write(NoAccountsMenu);
                return;
            }

            if (currentAccount == null) {
                Console.Write(NoLoginMenu);
                return;
            }

            Console.WriteLine($"Logged in as {currentAccount.Username}\n");

            Console.Write(FullMenu);
        }

#region strings

        private const string NoAccountsMenu = @"
Best Bank v1.0
==================================================
1. Create Account
0. Exit
";

        private const string NoLoginMenu = @"
Best Bank v1.0
==================================================
1. Create Account
2. Login
0. Exit
";

        private const string FullMenu = @"
Best Bank v1.0
==================================================
1. Create Account
2. Login
3. Deposit Funds
4. Withdraw Funds
5. Check Balance
6. View Transaction History
7. Log out
0. Exit
";

        private const string MenuPrompt = "> ";

#endregion
    }
}