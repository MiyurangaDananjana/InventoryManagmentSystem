﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace InventoryManagmentSystem.BLL
{
    public class AuthBLL
    {
        public class RandomStringGenerator
        {
            private static readonly Random random = new Random();
            private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            public static string GenerateRandomString(int length)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < length; i++)
                {
                    int index = random.Next(CHARS.Length);
                    sb.Append(CHARS[index]);
                }
                return sb.ToString();
            }

            public static string ComputeSHA256Hash(string input)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                    byte[] hashBytes = sha256.ComputeHash(inputBytes);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
        }
    }
}