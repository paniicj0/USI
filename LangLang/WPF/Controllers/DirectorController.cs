using LangLang.Model;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Controllers
{
    public class DirectorController
    {
        private DirectorService directorService;

        public DirectorController()
        {
            directorService = DirectorService.GetInstance();
        }

        public List<Director> GetAll()
        {
            return directorService.GetAll();
        }

        public Director GetById(int id)
        {
            return directorService.GetById(id);
        }

        public Director Create(Director director)
        {
            return directorService.Create(director);
        }

        public void Update(Director director)
        {
            directorService.Update(director);
        }

        public void Delete(int id)
        {
            directorService.Delete(id);
        }

        public List<Director> LoadFromFile()
        {
            return directorService.LoadFromFile();
        }

        public Director ConvertToDirector(object userObject)
        {
            return directorService.ConvertToDirector(userObject);
        }
    }
}
