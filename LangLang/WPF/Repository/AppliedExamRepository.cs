using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Repository
{
    class AppliedExamRepository
    {

        private static AppliedExamRepository instance = null;
        private List<AppliedExam> appliedExams;

        private AppliedExamRepository()
        {
            appliedExams = new List<AppliedExam>();
            appliedExams = LoadFromFile();

        }

        public static AppliedExamRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new AppliedExamRepository();
            }
            return instance;
        }

        public List<AppliedExam> GetAll()
        {
            return new List<AppliedExam>(appliedExams);
        }

        public AppliedExam GetById(int id)
        {
            foreach (AppliedExam appliedExam in appliedExams)
            {
                if (appliedExam.Id == id)
                {
                    return appliedExam;
                }
            }

            return null;
        }

        public AppliedExam GetByExamId(int id)
        {
            foreach (AppliedExam appliedExam in appliedExams)
            {
                if (appliedExam.IdExam == id)
                {
                    return appliedExam;
                }
            }

            return null;
        }

        public AppliedExam GetByStudentId(int id)
        {
            foreach (AppliedExam appliedExam in appliedExams)
            {
                if (appliedExam.IdStudent == id)
                {
                    return appliedExam;
                }
            }

            return null;
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (AppliedExam appliedExam in appliedExams)
            {
                if (appliedExam.Id > maxId)
                {
                    maxId = appliedExam.Id;
                }
            }

            return maxId + 1;
        }

        public void Save()
        {
            try
            {
                StreamWriter file = new StreamWriter("../../../WPF/Data/AppliedExamFile.csv", false);

                foreach (AppliedExam appliedExam in appliedExams)
                {
                    file.WriteLine(appliedExam.StringToCSV());
                }

                file.Flush();
                file.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Update(AppliedExam exam)
        {
            AppliedExam oldExam = GetById(exam.Id);
            if (oldExam == null)
            {
                return;
            }

            oldExam.Grade = exam.Grade;
            oldExam.GradeActivity = exam.GradeActivity;
            oldExam.Reading = exam.Reading;
            oldExam.Speaking = exam.Speaking;
            oldExam.Listening = exam.Listening;
            oldExam.Banned = exam.Banned;
            oldExam.Passed = exam.Passed;
            oldExam.Writting = exam.Writting;
            Save();
        }

        public void Delete(int id)
        {
            AppliedExam appliedExam = GetById(id);
            if (appliedExam == null)
            {
                return;
            }

            appliedExams.Remove(appliedExam);
            Save();
        }

       

        public AppliedExam Create(AppliedExam appliedExam)
        {
            appliedExam.Id = GenerateId();
            appliedExams.Add(appliedExam);
            Save();

            return appliedExam;
        }

        public List<AppliedExam> LoadFromFile()
        {

            string filename = "../../../WPF/Data/AppliedExamFile.csv";

            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');
                    if (tokens.Length < 12)
                    {
                        continue;
                    }

                    AppliedExam appliedExam = new AppliedExam(
                        id: Int32.Parse(tokens[0]),
                        idCourse: Int32.Parse(tokens[1]),
                        idStudent: Int32.Parse(tokens[2]),
                        idExam: Int32.Parse(tokens[3]),
                        banned: Boolean.Parse(tokens[4]),
                        passed: Boolean.Parse(tokens[5]),
                        reading: Int32.Parse(tokens[6]),
                        writting: Int32.Parse(tokens[7]),
                        listening: Int32.Parse(tokens[8]),
                        speaking: Int32.Parse(tokens[9]),
                        grade: Int32.Parse(tokens[10]),
                        gradeActivity: Int32.Parse(tokens[11])

                        );

                    appliedExams.Add(appliedExam);
                }
            }
            return appliedExams;


        }

        
        public bool BanStudent(int studentId)
        {
            try
            {              
                AppliedExam appliedExam = appliedExams.FirstOrDefault(ae => ae.IdStudent == studentId);

                if (appliedExam != null)
                {
                    
                    appliedExam.Banned = true;

                    Save();
                    StudentRepository.GetInstance().Delete(studentId);
                   
                    return true;
                }
                else
                {
                    return false; 
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine("Error banning student: " + ex.Message);
                return false;
            }
        }

        

    }
}
