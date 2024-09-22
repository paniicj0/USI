using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Controllers
{
    internal class ExamController
    {
        private ExamService examService;

        public ExamController()
        {
            examService = ExamService.GetInstance();
        }

        public List<Exam> GetAll()
        {
            return examService.GetAll();
        }

        public Exam GetById(int id)
        {
            return examService.GetById(id);
        }

        public Exam Create(Exam exam)
        {
            return examService.Create(exam);
        }

        public void Update(Exam exam)
        {
            examService.Update(exam);
        }

        public void Delete(int id)
        {
            examService.Delete(id);
        }

        public void DeleteTutorsExams(int id)
        {
            examService.DeleteTutorsExams(id);
        }
        
        public int SmartChoiceTutorForExam(LanguageEnum.Language language, LanguageLevelEnum.LanguageLevel level, DateTime? dateExam, TimeSpan? time)
        {
            return examService.SmartChoiceTutorForExam(language, level, dateExam, time);
        }
    }
}
