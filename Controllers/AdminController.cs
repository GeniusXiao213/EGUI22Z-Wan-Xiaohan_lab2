using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using static LibrarySystem.Data_Access.Data;
using static LibrarySystem.Data_Access.Methods;

namespace LibrarySystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LogOut()
        {
            CurrentUser = "";
            return RedirectToAction("Index", "Home");
        }

        

        [HttpPost]
        public IActionResult SearchBook(string searching)
        {
            
            List<BookModel> books = new List<BookModel>();
            ReadFromTableBook(books);
            List<BookModel> searchlist = new List<BookModel>();

            searching.ToLower();
            foreach (var item in books)
            {
                string title = item.Title.ToLower();
                string publisher=item.Publisher.ToLower();
                string author=item.Author.ToLower();
                if (title.Contains(searching) || publisher.Contains(searching) || author.Contains(searching))
                {
                    
                    searchlist.Add(item);
                }
            }

            CreateBookList newlist = new CreateBookList(searchlist);
            return View(newlist);
        }
        public IActionResult ViewBook()
        {
            if(CurrentUser=="")
            {
                return RedirectToAction("error", "Home");
            }
            List<BookModel> books = new List<BookModel>();
            ReadFromTableBook(books);
            CreateBookList newlist = new CreateBookList(books);
            return View(newlist);
        }

        public IActionResult ViewUsers()
        {
            if (CurrentUser == "")
            {
                return RedirectToAction("error", "Home");
            }
            List<UserModel> ListUsers = new List<UserModel>();
            ReadFromTable(ListUsers);
            CreateList newList = new CreateList(ListUsers);
            return View(newList);
        }

        public IActionResult CancelReservation(string id, string reserved,string user)
        {

            if (reserved !=null&&user!=null)
            {
                List<UserModel> ListUsers = new List<UserModel>();
                ReadFromTable(ListUsers);
                foreach(var item in ListUsers)
                {
                    if(item.Name==user)
                    {
                        MinusIsReserved(item);
                    }
                }
                try
                {
                    string connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";
                    

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "UPDATE books3 " +
                            "SET username='',reserved='' " +
                            "WHERE id=@id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);


                            command.ExecuteNonQuery();
                            TempData["cancel"] = "Cancelled Reservation Successfully!";
                            return RedirectToAction("ViewBook", "Admin");

                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["cancel"] = "Cancelled Reservation Failed!";
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
            TempData["cancel"] = "Cancelled Reservation Failed!";
            return RedirectToAction("ViewBook", "Admin");
        }

        public IActionResult BorrowBook(string id, string user)
        {

            if (user == null)
            {
                List<UserModel> ListUsers = new List<UserModel>();
                ReadFromTable(ListUsers);
                foreach (var item in ListUsers)
                {
                    if (item.Name == CurrentUser)
                    {
                        UpdateIsBorrowed(item);
                    }
                }
                try
                {
                    String connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";
                    DateTime now = DateTime.Now;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "UPDATE books3 " +
                            "SET username=@username,leased=@leased " +
                            "WHERE id=@id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            command.Parameters.AddWithValue("@username", CurrentUser);
                            command.Parameters.AddWithValue("@leased", now.ToString());



                            command.ExecuteNonQuery();
                            TempData["borrow"] = "Borrowed Book Successfully!";
                            return RedirectToAction("ViewBook", "Admin");

                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["borrow"] = "Borrowed Book Failed!";
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
            TempData["borrow"] = "Borrowed Book Failed!";
            return RedirectToAction("ViewBook", "Admin");
        }

        //public IActionResult EditUser()
        //{

        //    return RedirectToAction("ViewUsers", "Admin");
        //}

        public IActionResult DeleteUser(string name,int isborrowed, int isreserved)
        {
            int borrowBookNo = 0;
            int reserveBookNo = 0;
            List<UserModel> ListUsers = new List<UserModel>();
            ReadFromTable(ListUsers);
            foreach (var item in ListUsers)
            {
                if (item.Name == name)
                {
                    borrowBookNo = item.IsBorrowed;
                    reserveBookNo = item.IsReserved;
                }
            }
            
                if (name!="librarian" && borrowBookNo == 0 && reserveBookNo == 0)
            {
                try
                {
                    string connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";


                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "DELETE FROM users2 WHERE name=@name";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@name", name);
                            command.ExecuteNonQuery();
                            TempData["delete"] = "Delete User Successfully!";
                            return RedirectToAction("ViewUsers", "Admin");

                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["delete"] = "Delete User Failed! He/She is ACTIVE!";

                    Console.WriteLine("ERROR: " + ex.Message);
                    return RedirectToAction("ViewUsers", "Admin");
                }

                
            }
            else
            {
                TempData["delete"] = "Delete User Failed! He/She is ACTIVE!";

                return RedirectToAction("ViewUsers", "Admin");
            }
        }

        public IActionResult ReturnBook(string id, string leased,string user)
        {
            if (leased != null && user!=null)
            {
                List<UserModel> ListUsers = new List<UserModel>();
                ReadFromTable(ListUsers);
                foreach (var item in ListUsers)
                {
                    if (item.Name == user)
                    {
                        MinusIsBorrowed(item);
                    }
                }

                try
                {
                    string connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";


                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "UPDATE books3 " +
                            "SET username='',leased='' " +
                            "WHERE id=@id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);


                            command.ExecuteNonQuery();
                            TempData["return"] = "Return Book Successfully!";
                            return RedirectToAction("ViewBook", "Admin");

                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["return"] = "Return Failed!";

                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
            TempData["return"] = "Return Failed!";

            return RedirectToAction("ViewBook", "Admin");
        }

        public IActionResult ReserveBook(string id, string user)
        {

            if (user == null)
            {
                List<UserModel> ListUsers = new List<UserModel>();
                ReadFromTable(ListUsers);
                foreach (var item in ListUsers)
                {
                    if (item.Name == CurrentUser)
                    {
                        UpdateIsReserved(item);
                    }
                }
                try
                {
                    string connectionString = "Data Source=XIAOHAN\\SQLEXPRESS;Initial Catalog=FinalLibrary;Integrated Security=True";
                    DateTime now = DateTime.Now;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "UPDATE books3 " +
                            "SET username=@username,reserved=@reserved " +
                            "WHERE id=@id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@username", CurrentUser);
                            command.Parameters.AddWithValue("@reserved", now.ToString());
                            command.Parameters.AddWithValue("@id", id);


                            command.ExecuteNonQuery();
                            TempData["reserve"] = "Reserved Book Successfully!";
                            return RedirectToAction("ViewBook", "Admin");

                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["reserve"] = "Reserved Book Failed!";

                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
            TempData["reserve"] = "Reserved Book Failed!";

            return RedirectToAction("ViewBook", "Admin");

        }
    }



}

