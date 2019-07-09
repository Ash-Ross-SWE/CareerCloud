using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CareerCloud.BusinessLogicLayer
{
    public class CompanyProfileLogic : BaseLogic<CompanyProfilePoco>
    {
        private const int saltLengthLimit = 10;

        public CompanyProfileLogic(IDataRepository<CompanyProfilePoco> repository) : base(repository)
        {
        }

        public bool Authenticate(string userName, string password)
        {
            CompanyProfilePoco poco = base.GetAll().Where(s => s.Login == userName).FirstOrDefault();
            if (null == poco)
            {
                return false;
            }
            return VerifyHash(password, poco.Password);
        }

        private bool VerifyHash(string password1, object password2)
        {
            throw new NotImplementedException();
        }

        public override void Add(CompanyProfilePoco[] pocos)
        {
            Verify(pocos);

            base.Add(pocos);
        }

        public override void Update(CompanyProfilePoco[] pocos)
        {
            Verify(pocos);
            base.Update(pocos);
        }

        protected override void Verify(CompanyProfilePoco[] pocos)
        {
            foreach (var poco in pocos)
            {
                List<ValidationException> exceptions = new List<ValidationException>();
                string[] requiredExtendedPasswordChars = new string[] { ".ca", ".com", ".biz" };
                if (string.IsNullOrEmpty(poco.CompanyWebsite))
                {
                    exceptions.Add(new ValidationException(700, $"Password for SecurityLogin {poco.Id} cannot be null"));
                }
                else if (!requiredExtendedPasswordChars.Any(t => poco.CompanyWebsite.Contains(t)))
                {
                    exceptions.Add(new ValidationException(600, $"Company website must contain an extended character of '.ca', '.com' or '.biz' ."));
                }


                //if (string.IsNullOrEmpty(poco.ContactPhone))
                //{
                //    exceptions.Add(new ValidationException(702, $"PhoneNumber {poco.Id} is required"));
                //}
                //else
                //{
                //string[] phoneComponents = poco.ContactPhone.Split('-');
                //if (phoneComponents.Length != 3)
                //{
                //    exceptions.Add(new ValidationException(601, $"PhoneNumber {poco.Id} is not in the required format."));
                //}
                //else
                //{
                //    if (phoneComponents[0].Length != 3)
                //    {
                //        exceptions.Add(new ValidationException(601, $"PhoneNumber {poco.Id} is not in the required format."));
                //    }
                //    else if (phoneComponents[1].Length != 3)
                //    {
                //        exceptions.Add(new ValidationException(601, $"PhoneNumber {poco.Id} is not in the required format."));
                //    }
                //    else if (phoneComponents[2].Length != 4)
                //    {
                //        exceptions.Add(new ValidationException(601, $"PhoneNumber {poco.Id} is not in the required format."));
                //    }
                //}

                //}
                //}
                if (string.IsNullOrEmpty(poco.ContactPhone) || !Regex.Match(poco.ContactPhone, @"^(\+[0-9]{9})$").Success)
                {
                    exceptions.Add(new ValidationException(601, $"PhoneNumber must correspond to a valid phone number "));
                }

                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
            }
        }

        private static byte[] GetSalt()
        {
            return GetSalt(saltLengthLimit);
        }

        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return salt;
        }

        private string ComputeHash(string plainText, byte[] saltBytes)
        {
            if (saltBytes == null)
            {
                saltBytes = GetSalt();
            }

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (int i = 0; i < saltBytes.Length; i++)
            {
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
            }

            HashAlgorithm hash = new SHA512Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashWithSaltBytes[i] = hashBytes[i];
            }
            for (int i = 0; i < saltBytes.Length; i++)
            {
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
            }


            return Convert.ToBase64String(hashWithSaltBytes);
        }
        private bool VerifyHash(string plainText, string hashValue)
        {
            const int hashSizeInBytes = 64;

            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;
            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];
            string expectedHashString = ComputeHash(plainText, saltBytes);
            return (hashValue == expectedHashString);
        }

    }
}
