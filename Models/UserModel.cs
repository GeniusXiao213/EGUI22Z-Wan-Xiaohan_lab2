using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LibrarySystem.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "You need to give us your name.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "You need to provide a long enough name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must have a password.")]
        [DataType(DataType.Password)]

        [StringLength(100, MinimumLength = 3, ErrorMessage = "You need to provide a long enough password.")]
        public string Password { get; set; }


        //[Required(ErrorMessage ="You need to provide us your email address")]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; };
        public string ReservedBook;
        public string BorrowedBook;


        public int IsBorrowed;
        public int IsReserved;

        //public string ErrorMessage;
        //public string SuccessMessage;

        public string CreatedAt;

        public UserModel()
        {

            //Email = null;

        }
        public UserModel(string name, string password, string email, string reservedBook, string borrowedBook, string createdAt)
        {
            Name = name;
            Password = password;
            //Email = email;
            ReservedBook = reservedBook;
            BorrowedBook = borrowedBook;
            CreatedAt = createdAt;

            IsBorrowed = 0;
            IsReserved = 0;
            //ErrorMessage = "";
            //SuccessMessage = "";
        }
    }

    public class SignUp:UserModel
    {
        [Required(ErrorMessage = "You must have a confirm password.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Your password and confirm password do not match.")]
        public string Confirmpassword { get; set; }
    }

    public class CreateList
    {
        public List<UserModel> Users;

        
        public CreateList(List<UserModel> listUsers)
        {
            Users = listUsers;
        }
    }

}
