using Microsoft.Data.SqlClient;
using System.Data;

namespace ConsoleApp20
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using SqlConnection connection = new SqlConnection("Server=DESKTOP-SNIONM0\\SQLEXPRESS;Database=PizzaMizzaDB;Trusted_Connection=True;TrustServerCertificate=true"); 
            connection.Open();

            while (true)
            {
                Console.WriteLine("PIZZAMIZZA MENYU");
                Console.WriteLine("1. Pizzalara bax");
                Console.WriteLine("2. Pizza elave et");
                Console.WriteLine("3. Pizza sil");

            Start:
                Console.Write("Secim et: ");
                string? choice = Console.ReadLine();
                Console.Clear();
                switch (choice)
                {
                    case "1":
                        {
                            SqlCommand GetAllcommdand = new SqlCommand("SELECT * FROM Pizzas", connection);
                            SqlDataAdapter adapter = new SqlDataAdapter(GetAllcommdand);
                            DataSet dataSet = new ();
                            adapter.Fill(dataSet);
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                Console.WriteLine($"{row["Id"]}.{row["Name"]} {row["Price"]} {row["IngredientCount"]}");
                            }
                            break;
                        }

                    case "2":
                        {
                            Console.Write("Pizzanin adini daxil et: ");
                            string? name = Console.ReadLine();
                        priceInput:
                            Console.Write("Qiymeti daxil et: ");
                            string? priceInput = Console.ReadLine();
                            var isParsedPrice = decimal.TryParse(priceInput, out decimal price);
                            if (!isParsedPrice)
                            {
                                Console.WriteLine("Qiymet duzgun formatda deyil");
                                goto priceInput;
                            }


                            Console.Write("Ingredient sayı: ");
                            int count = int.Parse(Console.ReadLine());

                            SqlCommand Createcommand = new SqlCommand($"INSERT INTO Pizzas  VALUES ({name},{price},{count})", connection);

                            var insertResult=Createcommand.ExecuteNonQuery();
                            if (insertResult == 0)
                            {
                                Console.WriteLine("Pizza elave olunmadi");
                            }
                            else
                            {
                                Console.WriteLine("Pizza ugurla elave olundu ");
                            }
                             
                            break;
                        }

                    case "3":
                        {
                            Console.Write("Silinecek pizza ID: ");
                            int id = int.Parse(Console.ReadLine());
                            SqlCommand Deletecommand = new SqlCommand("DELETE FROM Pizzas WHERE Id=@id", connection);
                            Deletecommand.Parameters.AddWithValue("@id", id);

                            int result = Deletecommand.ExecuteNonQuery();

                            if (result == 0)

                                Console.WriteLine("Pizza tapılmadı ");
                            else
                                Console.WriteLine("Pizza silindi ");
                            break;
                        }
                    default:
                        Console.WriteLine("Yanlis secim");
                        break;
                }
                goto Start;
            }
            connection.Close();
        }
    }
}
