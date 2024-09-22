using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repository
{
    internal class DirectorRepository
    {
        private static DirectorRepository instance = null;
        private List<Director> directors;

        private DirectorRepository()
        {
            directors = LoadFromFile();
        }

        public static DirectorRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new DirectorRepository();
            }
            return instance;
        }
        public List<Director> GetAll()
        {
            return new List<Director>(directors);
        }

        public Director GetById(int id)
        {
            foreach (Director director in directors)
            {
                if (director.Id == id)
                {
                    return director;
                }
            }

            return null;
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (Director director in directors)
            {
                if (director.Id > maxId)
                {
                    maxId = director.Id;
                }
            }

            return maxId + 1;
        }

        public void Save()
        {
            try
            {

                StreamWriter file = new StreamWriter("../../../WPF/Data/DirectorFile.csv", true);

                foreach (Director director in directors)
                {
                    file.WriteLine(director.StringToCsv());
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
            Director director = GetById(id);
            if (director == null)
            {
                return;
            }
            directors.Remove(director);
            Save();
        }

        public void Update(Director director)
        {
            Director oldDirector = GetById(director.Id);
            if (oldDirector == null)
            {
                return;
            }
            oldDirector.Name = director.Name;
            oldDirector.Surname = director.Surname;
            oldDirector.PhoneNumber = director.PhoneNumber;
            oldDirector.Email = director.Email;
            oldDirector.Password = director.Password;
            Save();
        }

        public Director Create(Director director)
        {
            director.Id = GenerateId();
            directors.Add(director);
            Save();
            return director;
        }

        public List<Director> LoadFromFile()
        {

            List<Director> directors = new List<Director>();
            string filename = "../../../WPF/Data/DirectorFile.csv";


            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');
                    if (tokens.Length < 8) { continue; }

                    Director director = new Director(
                        id: Int32.Parse(tokens[0]),
                        name: tokens[1],
                        surname: tokens[2],
                        gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), tokens[3]),
                        birthday: DateTime.ParseExact(tokens[4], "yyyy-MM-dd", null),
                        phoneNumber: Convert.ToInt32(tokens[5]),
                        email: tokens[6],
                        password: tokens[7]);
                    directors.Add(director);
                }
            }
            return directors;


        }

    }
}
