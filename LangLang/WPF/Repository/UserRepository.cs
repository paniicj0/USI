using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.WPF.ModelEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repository
{
    internal class UserRepository
    {
        private static UserRepository instance = null;
        private List<User> users;

        private UserRepository()
        {
            users = LoadFromFile();
        }

        public static UserRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new UserRepository();
            }
            return instance;
        }

        public User GetById(int id)
        {
            foreach (User user in users)
            {
                if (user.Id == id)
                {
                    return user;
                }
            }
            return null;
        }

        public List<User> GetAll()
        {
            return users;
        }

        /*
        public List<Student> GetStudents()
        {
            return students;
        }

        public List<Tutor> GetTutors()
        {
            return tutors;
        }

        public List<Director> GetDirectors()
        {
            return directors;
        }
        */

        private int GenerateId()
        {
            int maxId = 0;
            foreach (User user in users)
            {
                if (user.Id > maxId)
                {
                    maxId = user.Id;
                }
            }

            return maxId + 1;
        }

        public void Save()
        {
            try
            {
                StreamWriter file = new StreamWriter("../../../WPF/Data/UserFile.csv", false);

                foreach (User user in users)
                {
                    file.WriteLine(user.StringToCsv());
                }
                file.Flush();
                file.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Delete(int id)
        {
            User user = GetById(id);
            if (user == null)
            {
                return;
            }
            users.Remove(user);
            Save();
        }

        public void Update(User user)
        {
            User oldUser = GetById(user.Id);

            if (oldUser != null)
            {
                oldUser.Email = user.Email;
                oldUser.Password = user.Password;
                Save();
            }
        }

        public List<User> Create(User user)
        {
            user.Id = GenerateId();
            users.Add(user);
            Save();
            //students = LoadFromFile();
            return users;
        }

        public List<User> LoadFromFile()
        {

            List<User> users = new List<User>();

            string filename = "../../../WPF/Data/UserFile.csv";

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');

                    if (tokens.Length < 3)
                    {
                        continue;
                    }

                    User user = new User(
                        id: Int32.Parse(tokens[0]),
                        email: tokens[1],
                        password: tokens[2],
                        role: (RoleEnum.Role)Enum.Parse(typeof(RoleEnum.Role), tokens[3]),
                        roleId: Int32.Parse(tokens[4])
                    );
                    users.Add(user);
                }
            }
            return users;
        }
    }
}

