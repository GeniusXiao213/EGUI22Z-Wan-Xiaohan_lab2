using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using static LibrarySystem.Data_Access.Data;

namespace LibrarySystem.Data_Access
{
    public static class Methods
    {
        public static void UpdateIsReserved(UserModel thisuser) //+1
        {
            
            try
            {
                String connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";
                int isreserved = thisuser.IsReserved + 1;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE users2 " +
                        "SET isreserved=@isreserved " +
                        "WHERE name=@name";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@name", thisuser.Name);
                        command.Parameters.AddWithValue("@isreserved", isreserved);



                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        public static void MinusIsReserved(UserModel thisuser) //-1
        {
            
            try
            {
                String connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";
                int isreserved=thisuser.IsReserved -1;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE users2 " +
                        "SET isreserved=@isreserved " +
                        "WHERE name=@name";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@name", thisuser.Name);
                        command.Parameters.AddWithValue("@isreserved", isreserved);



                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        public static void UpdateIsBorrowed(UserModel thisuser)
        {
            
            try
            {
                String connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";
                int isborrowed = thisuser.IsBorrowed + 1;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE users2 " +
                        "SET isborrowed=@isborrowed " +
                        "WHERE name=@name";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@name", thisuser.Name);
                        command.Parameters.AddWithValue("@isborrowed", isborrowed);



                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        public static void MinusIsBorrowed(UserModel thisuser)
        {
            try
            {
                String connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";
                int isborrowed=thisuser.IsBorrowed - 1;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE users2 " +
                        "SET isborrowed=@isborrowed " +
                        "WHERE name=@name";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@name", thisuser.Name);
                        command.Parameters.AddWithValue("@isborrowed", isborrowed);



                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        public static void ReadFromTable(List<UserModel> ListUsers)  //from table users2
        {

            try
            {
                string connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM users2";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserModel model = new UserModel();
                                model.Name = reader.GetString(0);
                                //model.Email = reader.GetString(1);
                                model.Password = reader.GetString(2);
                                model.BorrowedBook = reader.GetString(3);
                                model.ReservedBook = reader.GetString(4);
                                model.CreatedAt = reader.GetDateTime(5).ToString();
                                model.IsBorrowed = reader.GetInt32(6);
                                model.IsReserved = reader.GetInt32(7);
                                ListUsers.Add(model);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        public static void ReadFromTableBook(List<BookModel> ListBooks)  //from table book3
        {

            try
            {
                string connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM books3";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookModel model = new BookModel();
                                model.Id = reader.GetInt32(0);
                                model.Author = reader.GetString(1);
                                model.Title = reader.GetString(2);
                                model.Publisher = reader.GetString(3);
                                model.Date = reader.GetInt32(4);
                                model.User = reader.GetString(5);
                                model.Reserved = reader.GetString(6);
                                model.Leased = reader.GetString(7);

                                ListBooks.Add(model);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }


        
    }
}
