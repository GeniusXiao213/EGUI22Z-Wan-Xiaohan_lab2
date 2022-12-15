using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using static LibrarySystem.Data_Access.Data;
using static LibrarySystem.Data_Access.Methods;

namespace LibrarySystem.Controllers
{
    public class UserHomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewBook()
        {
            if (CurrentUser == "")
            {
                return RedirectToAction("error", "Home");
            }
            List<BookModel> books = new List<BookModel>();
            ReadFromTableBook(books);
            CreateBookList newlist = new CreateBookList(books);
            return View(newlist);
        }

        public IActionResult ViewReservation()
        {

            if (CurrentUser == "")
            {
                return RedirectToAction("error", "Home");
            }
            List<BookModel> books = new List<BookModel>();
            ReadFromTableBook(books);
            List<BookModel> mybook=new List<BookModel>();
            foreach(var item in books)
            {
                if(item.User==CurrentUser&&item.Reserved!="")
                {
                    mybook.Add(item);
                }
            }
            CreateBookList newlist=new CreateBookList(mybook);

            return View(newlist);
        }

        public IActionResult ViewRental()
        {
            if (CurrentUser == "")
            {
                return RedirectToAction("error", "Home");
            }
            List<BookModel> books = new List<BookModel>();
            ReadFromTableBook(books);
            List<BookModel> mybook = new List<BookModel>();
            foreach (var item in books)
            {
                if (item.User == CurrentUser&&item.Leased!="")
                {
                    mybook.Add(item);
                }
            }
            CreateBookList newlist = new CreateBookList(mybook);

            return View(newlist);
        }

        public IActionResult BorrowBook(string id,string user)
        {

            if (user==null)
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
                            TempData["borrowbook"] = "Borrowed Book Successfully!";
                            return RedirectToAction("ViewBook", "UserHome");

                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["borrowbook"] = "Borrowed Book Failed!";
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
            TempData["borrowbook"] = "Borrowed Book Failed!";
            return RedirectToAction("ViewBook", "UserHome");
        }


        public IActionResult SearchBook(string searching)
        {
            List<BookModel> books = new List<BookModel>();
            ReadFromTableBook(books);
            List<BookModel> searchlist = new List<BookModel>();

            searching.ToLower();
            foreach (var item in books)
            {
                string title = item.Title.ToLower();
                string publisher = item.Publisher.ToLower();
                string author = item.Author.ToLower();
                if (title.Contains(searching) || publisher.Contains(searching) || author.Contains(searching))
                {

                    searchlist.Add(item);
                }
            }

            CreateBookList newlist = new CreateBookList(searchlist);
            return View(newlist);
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
                            TempData["reservebook"] = "Reserved Book Successfully!";
                            return RedirectToAction("ViewBook", "UserHome");

                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["reservebook"] = "Reserved Book Failed!";
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
            TempData["reservebook"] = "Reserved Book Failed!";
            return RedirectToAction("ViewBook", "UserHome");

        }

        public IActionResult CancelReservation(string id, string reserved, string user)
        {
            if (user == CurrentUser && reserved!="")
            {

                List<UserModel> ListUsers = new List<UserModel>();
                ReadFromTable(ListUsers);
                foreach (var item in ListUsers)
                {
                    if (item.Name == CurrentUser)
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
                            TempData["cancelreservation"] = "Cancelled Reservation Successfully!";
                            return RedirectToAction("ViewReservation", "UserHome");

                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["cancelreservation"] = "Cancelled Reservation Failed!";
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
            TempData["cancelreservation"] = "Cancelled Reservation Failed!";
            return RedirectToAction("ViewReservation", "UserHome");
        }

        public IActionResult LogOut()
        {
            CurrentUser = "";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteAccount()
        {
            int borrowBookNo = 0;
            int reserveBookNo = 0;
            List<UserModel> ListUsers = new List<UserModel>();
            ReadFromTable(ListUsers);
            foreach (var item in ListUsers)
            {
                if (item.Name == CurrentUser)
                {
                    borrowBookNo = item.IsBorrowed;
                    reserveBookNo = item.IsReserved;
                }
            }
            if (borrowBookNo == 0 && reserveBookNo == 0)
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
                            command.Parameters.AddWithValue("@name", user.Name);
                            command.ExecuteNonQuery();
                            TempData["deleteacc"] = "Deleted Account Successfully!";
                            CurrentUser = "";
                            return RedirectToAction("Index", "Home");

                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["deleteacc"] = "Deleted Account Failed!";
                    return RedirectToAction("Index", "UserHome");
                }
                
            }
            else
            {
                TempData["deleteacc"] = "Deleted Account Failed!";
                return RedirectToAction("Index", "UserHome");
            }
        }
       
    }
}
