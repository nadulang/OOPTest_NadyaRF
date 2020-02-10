using System;
using System.IO;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Linq;


using System.Collections.Generic;
using static OOPTest_NadyaRF.LogClass;
using OOPTest_NadyaRF;

namespace OOP_Test_NadyaRF
{
    class Program
    {
        static void Main(string[] args)
        {
            Hash.md5("secret");
            Hash.sha1("secret");
            Hash.sha256("secret");
            Hash.sha512("secret");


            string message = Cipher.encrypt("ini tulisan rahasia", "p4$$w0rd");
            Console.WriteLine("Anyone without password can't read this message");
            Console.WriteLine("");

            Console.WriteLine("Your decrypted string is:");
            string decryptedstring = Cipher.decrypt(message, "p4$$w0rd", "p4$$w0rd");
            Console.WriteLine(decryptedstring);

            Authentication.login("root", "secret");
            Authentication.validate("root", "secret");
            Authentication._user();
            Authentication.check();
            Authentication.guest();
            Authentication.lastLogin();
            Authentication.logout();
            Authentication.guest();

            Console.WriteLine("Authentication is done.");
            Console.WriteLine("");

            LogClass.Log_1.SaveAllLog();
            Console.WriteLine("Log is done. See file App.Log to know the result");
            Console.WriteLine("");


            Cart cartbaru = new Cart();
            // Do some chainings
            cartbaru.AddItem(2, 10000, 2)
                    .AddItem(3, 10000, 3)
                    .AddItem(4, 10000, 4)
                    .RemoveItem(2)
                    .AddDiscount(50);
             Console.WriteLine($"Total Items: {Cart.TotalItems()}");
             Console.WriteLine($"Total Quantity: {Cart.TotalQuantity()}");
             Console.WriteLine($"Total Price: {Cart.TotalPrice()}");
             Cart.ShowAllItems();
             Cart.Checkout(); 







        }


    }

    class Hash
    {
        public static void md5(string source)
        {

            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GenerateMd5Hash(md5Hash, source);

                Console.WriteLine($"Hash.md5 : {hash}");

            }
        }

        static string GenerateMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static void sha1(string source)
        {
            var data = Encoding.ASCII.GetBytes(source);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("x2");
            }
            Console.WriteLine($"Hash.sha1 : {hash}");
        }

        public static void sha256(string source)
        {

            string hashedData = ComputeSha256Hash(source);
            Console.WriteLine($"Hash.sha256 : {hashedData}");
        }

        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }

        public static void sha512(string source)
        {

            string hashedData = ComputeSha512Hash(source);
            Console.WriteLine($"Hash.sha512 : {hashedData}");
        }

        static string ComputeSha512Hash(string rawData)
        {
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }

        }


    }



    class Cipher
    {


        private const string initVector = "pemgail9uzpgzl88";

        private const int keysize = 256;

        public static string encrypt(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string decrypt(string cipherText, string passPhrase, string passWord)
        {
            if (passPhrase == passWord)
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(keysize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            else
            {
                return "Anyone without password can't read this message";
            }

        }



    }

    class Authentication
    {
		public static string user = "root";
		public static string password = "secret";
		public static string id = "rx-178";
		public static int condition = 0;
		public static DateTime loginTime = new DateTime();



		public static void login(string User, string Password)
		{
			if (User == user && Password == password)
			{
				Console.WriteLine("Logged in");
				Log_1.PopulateLog("Logged in");
				condition = 1;
				loginTime = DateTime.Now;
			}
			else
			{
				Console.WriteLine("Wrong password or username");
				Log_1.PopulateLog("unknown tried to log in");
			}
		}

		public static void validate(string User, string Password)
		{
			if (User == user && Password == password && condition == 1)
			{
				Console.WriteLine("Already Logged in");
				Log_1.PopulateLog("user validated");
			}
			else
			{
				Console.WriteLine("Log in first");
				Log_1.PopulateLog("unknown tried to validate");
			}
		}

		public static void ID()
		{
			if (condition == 1)
			{
				Console.WriteLine(id);
				Log_1.PopulateLog("user request id");
			}
			else
			{
				Console.WriteLine("log in first");
				Log_1.PopulateLog("unknown tried to request id");
			}
		}

		public static void logout()
		{
			condition = 0;
			Console.WriteLine("logged out");
			Log_1.PopulateLog($"{user} logged out");
		}

		public static void _user()
		{
			if (condition == 1)
			{
				Console.WriteLine(user[0]);
				Log_1.PopulateLog($"{user} shows username");
			}
			else
			{
				Console.WriteLine("log in first");
			}
		}

		public static void check()
		{
			if (condition == 1)
			{
				Console.WriteLine(true);
				Log_1.PopulateLog($"{user} is logged in");
			}
			else
			{
				Console.WriteLine(false);
				Log_1.PopulateLog("unknown tried to check");
			}
		}
		public static void guest()
		{
			if (condition == 0)
			{
				Console.WriteLine(true);
				Log_1.PopulateLog("guest is user");
			}
			else
			{
				Console.WriteLine(false);
				Log_1.PopulateLog($"{user} is logged in");
			}
		}
		public static void lastLogin()
		{
			Console.WriteLine(loginTime);
			Log_1.PopulateLog($"{user} last log in");
		}
	}


    


    public interface ICart
    {
        int Item_id { get; set; }
        int Price { get; set; }
        int Quantity { get; set; }
    }
    public class Cart : ICart
    {
        static string cartPath = @"/Users/gigaming/Documents/Nadya RF/Bootcamp_Refactory/OOPTest_NadyaRF/OOPTest_NadyaRF/carts.txt";
        public int Item_id { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        static List<ICart> itemCart = new List<ICart>();
        public Cart AddItem(int id, int price, int qty = 1)
        {
            var obj = new Cart();
            obj.Item_id = id;
            obj.Price = price;
            obj.Quantity = qty;
            itemCart.Add(obj);
            return this;
        }
        public Cart RemoveItem(int id)
        {
            var obj = itemCart;
            var newObj = new List<ICart>();
            foreach (var x in obj)
            {
                if (x.Item_id != id)
                {
                    newObj.Add(x);
                }
            }
            itemCart = newObj;
            return this;
        }
        public Cart AddDiscount(int id)
        {
            var obj = itemCart;
            foreach (var x in obj)
            {
                x.Price = x.Price * id / 100;
            }
            itemCart = obj;
            return this;
        }
        public static int TotalItems()
        {
            return itemCart.Count();
        }
        public static int TotalQuantity()
        {
            int totalQty = 0;
            foreach (var x in itemCart)
            {
                totalQty += x.Quantity;
            }
            return totalQty;
        }
        public static int TotalPrice()
        {
            int totalPrice = 0;
            foreach (var x in itemCart)
            {
                totalPrice += x.Price * x.Quantity;
            }
            return totalPrice;
        }
        public static string ShowAllItems()
        {
            var allItems = new List<string>();
            foreach (var x in itemCart)
            {
                allItems.Add(x.Item_id.ToString());
            }
            allItems.Distinct();
            return String.Join(',', allItems);
        }
        public static void Checkout()
        {
            List<string> lines = new List<string>();
            lines.Add("Item_id,Price,Qty");
            foreach (var x in itemCart)
            {
                lines.Add($"{x.Item_id},{x.Price},{x.Quantity}");
            }
            File.WriteAllLines(cartPath, lines);
        }
    }

}
