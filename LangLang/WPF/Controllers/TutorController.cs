using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Controllers
{
    public class TutorController
    {
        private TutorService tutorService;

        public TutorController()
        {
            tutorService = TutorService.GetInstance();
        }

        public List<Tutor> GetAll()
        {
            return tutorService.GetAll();
        }

        public Tutor GetById(int id)
        {
            return tutorService.GetById(id);
        }

        public Tutor Create(Tutor tutor)
        {
            return tutorService.Create(tutor);
        }

        public void Update(Tutor tutor)
        {
            tutorService.Update(tutor);
        }

        public void Delete(int id)
        {
            tutorService.Delete(id);
        }

        public List<Tutor> LoadFromFile()
        {
            return tutorService.LoadFromFile();
        }

        public List<Tutor> SearchTutors(string language, string languageLevel, DateTime dateAdded)
        {
            return tutorService.SearchTutors(language, languageLevel, dateAdded);
        }

        public List<Tutor> SearchTutorsByLanguageAndLevel(string language, string languageLevel)
        {
            return tutorService.SearchTutorsByLanguageAndLevel(language, languageLevel);
        }

        public List<Tutor> SearchTutorsByLanguage(string language)
        {
            return tutorService.SearchTutorsByLanguage(language);
        }

        public List<Tutor> SearchTutorsByLevel(string languageLevel)
        {
            return tutorService.SearchTutorsByLevel(languageLevel);
        }

        public List<Tutor> SearchTutorsByDateAdded(DateTime dateAdded)
        {
            return tutorService.SearchTutorsByDateAdded(dateAdded);
        }

        public Tutor ConvertToTutor(object userObject)
        {
            return tutorService.ConvertToTutor(userObject);
        }

        
    }
}
