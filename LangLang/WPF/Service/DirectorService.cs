using LangLang.Model;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Service
{
    internal class DirectorService
    {
        private static DirectorService instance = null;
        private DirectorRepository directorRepository;

        private DirectorService()
        {
            directorRepository = DirectorRepository.GetInstance();
        }

        public static DirectorService GetInstance()
        {
            if (instance == null)
            {
                instance = new DirectorService();
            }
            return instance;
        }

        public Director GetById(int id)
        {
            return directorRepository.GetById(id);
        }

        public List<Director> GetAll()
        {
            return directorRepository.GetAll();
        }

        public Director Create(Director director)
        {
            return directorRepository.Create(director);
        }

        public void Update(Director director)
        {
            directorRepository.Update(director);
        }

        public void Delete(int id)
        {
            directorRepository.Delete(id);
        }

        public List<Director> LoadFromFile()
        {
            return directorRepository.LoadFromFile();
        }

        public Director ConvertToDirector(object userObject)
        {
            User user = (User)userObject;

            if (user.Role.ToString() == "Director")
            {
                return GetById(user.RoleId);
            }
            return null;
        }
    }
}
