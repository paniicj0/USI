using LangLang.Model;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Service
{
    internal class GradeTutorService
    {
        private static GradeTutorService instance = null;
        private GradeTutorRepository gradeTutorRepository;

        private GradeTutorService()
        {
            gradeTutorRepository = GradeTutorRepository.GetInstance();  
        }

        public static GradeTutorService GetInstance()
        {
            if (instance == null)
            {
                instance = new GradeTutorService();
            }
            return instance;
        }

        public GradeTutor GetById(int id)
        {
            return gradeTutorRepository.GetById(id);
        }

        public List<GradeTutor> GetAll()
        {
            return gradeTutorRepository.GetAll();
        }

        public GradeTutor Create(GradeTutor grade)
        {
            return gradeTutorRepository.Create(grade);
        }

        public void Update(GradeTutor grade)
        {
            gradeTutorRepository.Update(grade);
        }

        public void Delete(int id)
        {
            gradeTutorRepository.Delete(id);
        }

        public List<GradeTutor> LoadFromFile()
        {

            return gradeTutorRepository.LoadFromFile();

        }

        public List<GradeTutor> GetByCourseTutorId(int courseId, int tutorId)
        {
            List<GradeTutor> grades = gradeTutorRepository.GetAll();
            List<GradeTutor> filterGrades = new List<GradeTutor>();
            foreach (GradeTutor grade in grades)
            {
                if (grade.CourseId == courseId && grade.TutorId == tutorId)
                {
                    filterGrades.Add(grade);
                }
            }
            return filterGrades;
        }
    }
}
