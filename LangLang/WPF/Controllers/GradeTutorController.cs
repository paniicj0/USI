using LangLang.Model;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Controllers
{
    public class GradeTutorController
    {
        private GradeTutorService gradeTutorService;

        public GradeTutorController()
        {
            gradeTutorService = GradeTutorService.GetInstance();
        }

        public List<GradeTutor> GetAll()
        {
            return gradeTutorService.GetAll();
        }

        public GradeTutor GetById(int id)
        {
            return gradeTutorService.GetById(id);
        }

        public GradeTutor Create(GradeTutor grade)
        {
            return gradeTutorService.Create(grade);
        }

        public void Update(GradeTutor grade)
        {
            gradeTutorService.Update(grade);
        }

        public void Delete(int id)
        {
            gradeTutorService.Delete(id);
        }

        public List<GradeTutor> LoadFromFile()
        {
            return gradeTutorService.LoadFromFile();
        }

        public List<GradeTutor> GetByCourseTutorId(int coursdeId, int tutorId)
        {
            return gradeTutorService.GetByCourseTutorId(coursdeId, tutorId);
        }
    }
}
