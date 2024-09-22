using LangLang.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repository
{
    internal class GradeTutorRepository
    {
        private static GradeTutorRepository instance = null;
        private List<GradeTutor> grades;

        public event EventHandler GradeUpdate;
  
        public GradeTutorRepository()
        {
            grades = new List<GradeTutor>();
            grades = LoadFromFile();
        }

        public static GradeTutorRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new GradeTutorRepository();
            }
            return instance;
        }

        public List<GradeTutor> GetAll()
        {
            return new List<GradeTutor>(grades);
        }

        public GradeTutor GetById(int id)
        {
            foreach (GradeTutor grade in grades)
            {
                if (grade.Id == id)
                {
                    return grade;
                }
            }
            return null;
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (GradeTutor grade in grades)
            {
                if (grade.Id > maxId)
                {
                    maxId = grade.Id;
                }
            }
            return maxId + 1;
        }

        public void Save()
        {
            try
            {
                StreamWriter file = new StreamWriter("../../../WPF/Data/GradeTutorFile.csv", false);

                foreach (GradeTutor grade in grades)
                {
                    file.WriteLine(grade.StringToCsv());
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
            GradeTutor grade = GetById(id);
            if (grade == null)
            {
                return;
            }
            grades.Remove(grade);
            Save();
        }

        public void Update(GradeTutor grade)
        {
            GradeTutor oldGrade = GetById(grade.Id);

            if (oldGrade != null)
            {
                oldGrade.StudentId = grade.StudentId;
                oldGrade.TutorId = grade.TutorId;
                oldGrade.CourseId = grade.CourseId;
                oldGrade.Grade = grade.Grade;
                Save();
                GradeUpdate?.Invoke(this, EventArgs.Empty);
            }
        }

        public GradeTutor Create(GradeTutor grade)
        {
            grade.Id = GenerateId();
            grades.Add(grade);
            Save();
            return grade;
        }

        public List<GradeTutor> LoadFromFile()
        {
            string filename = "../../../WPF/Data/GradeTutorFile.csv";

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');

                    if (tokens.Length < 5)
                    {
                        continue;
                    }

                    GradeTutor grade = new GradeTutor(
                        id: Int32.Parse(tokens[0]),
                        studentId: Int32.Parse(tokens[1]),
                        tutorId: Int32.Parse(tokens[2]),
                        courseId: Int32.Parse(tokens[3]),
                        grade: Int32.Parse(tokens[4])
                    );
                    grades.Add(grade);
                }
            }
            return grades;
        }

        public int CalculateAverageTutorsGrade(int id)
        {
            int totalGrades = 0;
            int count = 0;

            foreach (GradeTutor g in grades)
            {
                if (g.TutorId == id)
                {
                    totalGrades += g.Grade;
                    count++;
                }
            }

            if (count == 0)
            {
               return 0;
            }

            int averageGrade = totalGrades / count;

            return averageGrade;
        }
    }
}
