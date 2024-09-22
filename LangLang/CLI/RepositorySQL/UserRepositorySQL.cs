using System;
using System.Collections.Generic;
using LangLang.Model;
using Oracle.ManagedDataAccess.Client;

namespace LangLang.RepositorySQL
{
    public class UserRepositorySQL
    {
        private static UserRepositorySQL instance = null;
        private StudentRepositorySQL studentRepository;
        private TutorRepositorySQL tutorRepository;
        private DirectorRepositorySQL directorRepository;

        private List<Student> students;
        private List<Tutor> tutors;
        private List<Director> directors;

        private UserRepositorySQL()
        {
            studentRepository = StudentRepositorySQL.GetInstance();
            tutorRepository = TutorRepositorySQL.GetInstance();
            directorRepository = DirectorRepositorySQL.GetInstance();

            students = studentRepository.GetAll();
            tutors = tutorRepository.GetAll();
            directors = directorRepository.GetAll();
        }

        public static UserRepositorySQL GetInstance()
        {
            if (instance == null)
            {
                instance = new UserRepositorySQL();
            }
            return instance;
        }

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
    }
}
