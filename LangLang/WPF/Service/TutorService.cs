using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Service
{
    internal class TutorService
    {
        private static TutorService instance = null;
        private TutorRepository tutorRepository;

        private TutorService()
        {
            tutorRepository = TutorRepository.GetInstance();

        }

        public static TutorService GetInstance()
        {
            if (instance == null)
            {
                instance = new TutorService();
            }
            return instance;
        }

        public Tutor GetById(int id)
        {
            return tutorRepository.GetById(id);
        }

        public List<Tutor> GetAll()
        {
            return tutorRepository.GetAll();
        }

        public Tutor Create(Tutor tutor)
        {
            return tutorRepository.Create(tutor);
        }

        public void Update(Tutor tutor)
        {
            tutorRepository.Update(tutor);
        }

        public void Delete(int id)
        {
            tutorRepository.Delete(id);
        }

        public List<Tutor> LoadFromFile()
        {
            return tutorRepository.LoadFromFile();
        }

        public List<Tutor> SearchTutors(string language, string languageLevel, DateTime dateAdded)
        {
            List<Tutor> tutors = tutorRepository.GetAll();
            List<Tutor> searchedTutors = new List<Tutor>();

            foreach (Tutor tutor in tutors)
            {
                if (tutor.Languages.ToString() == language &&
                tutor.LanguageLevel.ToString() == languageLevel &&
                tutor.TutorCreationDate.Date == dateAdded.Date)
                {
                    searchedTutors.Add(tutor);
                }
            }

            return searchedTutors;
        }

        public List<Tutor> SearchTutorsByLanguageAndLevel(string language, string languageLevel)
        {
            List<Tutor> tutors = tutorRepository.GetAll();
            List<Tutor> searchedTutors = new List<Tutor>();

            foreach (Tutor tutor in tutors)
            {
                if (tutor.Languages.ToString() == language && tutor.LanguageLevel.ToString() == languageLevel)
                {
                    searchedTutors.Add(tutor);
                }
            }

            return searchedTutors;
        }

        public List<Tutor> SearchTutorsByLanguage(string language)
        {
            List<Tutor> tutors = tutorRepository.GetAll();
            List<Tutor> searchedTutors = new List<Tutor>();

            foreach (Tutor tutor in tutors)
            {
                if (tutor.Languages.ToString() == language)
                {
                    searchedTutors.Add(tutor);
                }
            }

            return searchedTutors;
        }

        public List<Tutor> SearchTutorsByLevel(string languageLevel)
        {
            List<Tutor> tutors = tutorRepository.GetAll();
            List<Tutor> searchedTutors = new List<Tutor>();

            foreach (Tutor tutor in tutors)
            {
                if (tutor.LanguageLevel.ToString() == languageLevel)
                {
                    searchedTutors.Add(tutor);
                }
            }

            return searchedTutors;
        }

        public List<Tutor> SearchTutorsByDateAdded(DateTime dateAdded)
        {
            List<Tutor> tutors = tutorRepository.GetAll();
            List<Tutor> searchedTutors = new List<Tutor>();

            foreach (Tutor tutor in tutors)
            {
                if (tutor.TutorCreationDate.Date == dateAdded.Date)
                {
                    searchedTutors.Add(tutor);
                }
            }

            return searchedTutors;
        }

        public Tutor ConvertToTutor(object userObject)
        {
            User user = (User)userObject;

            if (user.Role.ToString() == "Tutor")
            {
                return GetById(user.RoleId);
            }
            return null;
        } 
    }
}
