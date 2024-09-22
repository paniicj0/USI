using LangLang.Controllers;
using LangLang.Model;
using LangLang.Repository;
using LangLang.WPF.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Service
{
    public class UserService
    {
        private static UserService instance = null;
        private UserRepository userRepository;
        private List<User> users;
        public StudentController studentController;
        public TutorController tutorController;
        public DirectorController directorController;

        private UserService()
        {
            userRepository = UserRepository.GetInstance();
            this.users = userRepository.GetAll();
            studentController = new StudentController();
            tutorController = new TutorController();
            directorController = new DirectorController();
        }

        public static UserService GetInstance()
        {
            if (instance == null)
            {
                instance = new UserService();
            }
            return instance;
        }

        public User GetById(int id)
        {
            return userRepository.GetById(id);
        }

        public List<User> GetAll()
        {
            return userRepository.GetAll();
        }

        public List<User> Create(User user)
        {
            return userRepository.Create(user);
        }

        public void Update(User user)
        {
            userRepository.Update(user);
        }

        public void Delete(int id)
        {
            userRepository.Delete(id);
        }

        public List<User> LoadFromFile()
        {

            return userRepository.LoadFromFile();

        }

        /*
        public List<Student> GetStudents()
        {
            return userRepository.GetStudents();
        }

        public List<Tutor> GetTutors()
        {
            return userRepository.GetTutors();
        }

        public List<Director> GetDirectors()
        {
            return userRepository.GetDirectors();
        }
        */

        public (object, string) IsLoggedIn(string email, string password)
        {

            foreach (User user in users)
            {
                if(user.Email == email && user.Password == password)
                {
                    return (user, user.Role.ToString());
                }
            }

            /*
            List<Student> students = studentRepository.GetAll();
            List<Tutor> tutors = tutorRepository.GetAll();
            List<Director> directors = directorRepository.GetAll();

            foreach (Student student in students)
            {
                if (student.Email == email && student.Password == password)
                {
                    return (student, "student");
                }
            }

            foreach (Tutor tutor in tutors)
            {
                if (tutor.Email == email && tutor.Password == password)
                {
                    return (tutor, "tutor");

                }
            }

            foreach (Director director in directors)
            {
                if (director.Email == email && director.Password == password)
                {

                    return (director, "director");
                }
            }
            */
            return (null, "");
        }

        public T ConvertToUserType<T>(object userObject)
        {
            try
            {
                // gets type based on its name
                Type targetType = typeof(T);

                Type userType = userObject.GetType();

                if (targetType.IsAssignableFrom(userType))  // checks if object corresponds to T
                {
                    return (T)userObject;   // if userObject is already instance of T
                }
                User user = (User)userObject;

                // conversion
                switch (user.Role)
                {
                    case RoleEnum.Role.Student:
                        if (typeof(T) == typeof(Student))
                        {
                            return (T)(object)studentController.GetById(user.RoleId); 
                        }
                        break;
                    case RoleEnum.Role.Tutor:
                        if (typeof(T) == typeof(Tutor))
                        {
                            return (T)(object)tutorController.GetById(user.RoleId); 
                        }
                        break;
                    case RoleEnum.Role.Director:
                        if (typeof(T) == typeof(Director))
                        {
                            return (T)(object)directorController.GetById(user.RoleId); 
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during conversion: {ex.Message}");
            }

            return default(T);
        }

    }
}
