using System.Collections.Generic;
using AP.Data;
using AP.Models;

namespace AP.Business
{
    public class UserBusiness
    {
        private readonly UserRepository _repo = new UserRepository();

        public List<User> GetUsers()
        {
            return _repo.GetUsers();
        }

        public void CreateUser(User model)
        {
            _repo.CreateUser(model);
        }

        public User ValidateLogin(string email, string password)
        {
            return _repo.GetByEmailAndPassword(email, password);
        }

        public User GetUserById(int id)
        {
            return _repo.GetUserById(id);
        }

        public void UpdateUser(User model)
        {
            _repo.UpdateUser(model);
        }
    }
}