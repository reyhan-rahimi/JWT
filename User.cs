using JWT.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWT
{
    public class User
    {
        
        public UserModel search(UserModel userModel)
        {
            IList<UserModel> users = new List<UserModel>
            {
                new UserModel() { Username = "reyhan", EmailAddress = "12345" },
                new UserModel() { Username = "hamid", EmailAddress = "1345" },
                new UserModel() { Username = "rahim", EmailAddress = "12" },
                new UserModel() { Username = "ramin", EmailAddress = "45" },
            };
            var search = from ch in users
                         where ch.Username == userModel.Username && ch.EmailAddress == userModel.EmailAddress
                         select ch;

            return search.First();
        }
    }
}
