using LangLang.Controllers;
using LangLang.Model;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Service
{
    class AppliedExamService
    {
        private static AppliedExamService instance = null;
        private AppliedExamRepository appliedExamRepository;
        private List<AppliedExam> appliedExams;

        private StudentRepository studentRepository;
        private StudentController studentController;
        public List<Student> students;

        private AppliedExamService()
        {
            appliedExamRepository = AppliedExamRepository.GetInstance();
            this.appliedExams = appliedExamRepository.GetAll();
            studentRepository = StudentRepository.GetInstance();
            this.students = studentRepository.GetAll();
        }

        public static AppliedExamService GetInstance()
        {
            if (instance == null)
            {
                instance = new AppliedExamService();
            }
            return instance;
        }

        public AppliedExam GetById(int id)
        {
            return appliedExamRepository.GetById(id);
        }

        public AppliedExam GetByExamId(int id)
        {
            return appliedExamRepository.GetByExamId(id);
        }

        public AppliedExam GetByStudentId(int id)
        {
            return appliedExamRepository.GetByStudentId(id);
        }

        public List<AppliedExam> GetAll()
        {
            return appliedExamRepository.GetAll();
        }

        public AppliedExam Create(AppliedExam appliedExam)
        {
            return appliedExamRepository.Create(appliedExam);
        }

        public void Update(AppliedExam appliedExam)
        {
           appliedExamRepository.Update(appliedExam);
        }

        public void Delete(int id)
        {
            appliedExamRepository.Delete(id);
        }

        public List<AppliedExam> LoadFromFile()
        {
            return appliedExamRepository.LoadFromFile();
        }

        public bool BanStudent(int studentId)
        {
            return appliedExamRepository.BanStudent(studentId);
        }

        // function for getting students with grade 10 for knowledge
        public List<Student> GetBestStudentsKnowledge(int courseId)
        {
            List<Student> bestStudents = new List<Student>();

            foreach (AppliedExam appliedExam in appliedExams)
            {
                if (appliedExam.IdCours == courseId && appliedExam.Grade == 10)
                {
                    studentController = new StudentController();
                    Student bestStudent = studentController.GetById(appliedExam.IdStudent);
                    bestStudents.Add(bestStudent);
                }
            }
            return bestStudents;
        }

        // function for getting students with grade 10 for activity
        public List<Student> GetBestStudentsActivity(int courseId)
        {
            List<Student> bestStudents = new List<Student>();

            foreach (AppliedExam appliedExam in appliedExams)
            {
                if (appliedExam.IdCours == courseId && appliedExam.GradeActivity == 10)
                {
                    studentController = new StudentController();
                    Student bestStudent = studentController.GetById(appliedExam.IdStudent);
                    bestStudents.Add(bestStudent);
                }
            }
            return bestStudents;
        }
    }
}