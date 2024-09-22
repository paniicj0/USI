using LangLang.Model;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Controllers
{
    public class UserController
    {
        private UserService userService;

        public UserController()
        {
            userService = UserService.GetInstance();
        }

        public List<User> GetAll()
        {
            return userService.GetAll();
        }

        public User GetById(int id)
        {
            return userService.GetById(id);
        }

        public List<User> Create(User user)
        {
            return userService.Create(user);
        }

        public void Update(User user)
        {
            userService.Update(user);
        }

        public void Delete(int id)
        {
            userService.Delete(id);
        }

        public List<User> LoadFromFile()
        {
            return userService.LoadFromFile();
        }

        public (object, string) IsLoggedIn(string email, string password)
        {
            return userService.IsLoggedIn(email, password);
        }

        public T ConvertToUserType<T>(object userObject)
        {
            return userService.ConvertToUserType<T>(userObject);
        }

        /*
        public List<Student> GetStudents()
        {
            return userService.GetStudents();
        }

        public List<Tutor> GetTutors()
        {
            return userService.GetTutors();
        }


        public (object, string) IsLoggedIn(string email, string password)
        {
            return userService.IsLoggedIn(email, password);
        }
        */
    }
}
