using System;

namespace LegacyApp
{ 
    public class UserService
    {
        private static int _minAge = 21;

        private static int _creditLimit = 500;
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsFirstNameCorrect(firstName) || !IsLastNameCorrect(lastName)||!IsEmailCorrect(email)||!IsAgeTooLow(dateOfBirth))
            {
                return false;
            }


            var client = GetClientById(clientId);
            
            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);
            
            SetUserCreditDetails(user,client);
            

            if (!IsValidUserCreditLimit(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        public bool IsValidUserCreditLimit(User user)
        {
            return !user.HasCreditLimit || user.CreditLimit >= _creditLimit;
        }

        public static Client GetClientById(int clientId)
        {
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);
            return client;
            
        }

        public void SetUserCreditDetails(User user, Client client)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else 
            {
                user.HasCreditLimit = true;

                var userCreditService = new UserCreditService();
                
                int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    
                if (client.Type == "ImportantClient")
                {
                    creditLimit = creditLimit * 2;
                }
                    
                user.CreditLimit = creditLimit;
                
            }
            
        }
        private static User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth,
            Client client)
        {
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
        }
        

        private static int CalulateAgeUsingBirthDate(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            
            var isMonthAfterBirth = now.Month < dateOfBirth.Month;
            var isCurrMonth = now.Month == dateOfBirth.Month;
            var isDayAfterBirth = now.Day < dateOfBirth.Day;
            
            if (isMonthAfterBirth || (isCurrMonth && isDayAfterBirth)) age--;
            return age;
            
        }

        private static bool IsAgeTooLow(DateTime dateOfBirth)
        {
            var age = CalulateAgeUsingBirthDate(dateOfBirth);
            return age >= _minAge;

        }

        private static bool IsEmailCorrect(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private static bool IsLastNameCorrect(string lastName)
        {
            return !string.IsNullOrEmpty(lastName);
        }

        private static bool IsFirstNameCorrect(string firstName)
        {
            return !string.IsNullOrEmpty(firstName);
        }
    }
}
