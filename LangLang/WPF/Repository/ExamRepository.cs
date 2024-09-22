using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LangLang.ModelEnum.LanguageEnum;

namespace LangLang.Repository
{
    internal class ExamRepository
    {
        private static ExamRepository instance = null;
        private List<Exam> exams;
        public event EventHandler ExamAdded;
        private TutorController tutorController = new TutorController();
        //private ExamController examController = new ExamController();
        private GradeTutorRepository gradeTutorRepository = new GradeTutorRepository();
        private List<Tutor> tutors;


        private ExamRepository()
        {
            exams = new List<Exam>();
            exams = Load();
        }

        public static ExamRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new ExamRepository();
            }
            return instance;
        }

        public List<Exam> GetAll()
        {
            return new List<Exam>(exams);
        }

        public Exam GetById(int id)
        {
            foreach (Exam exam in exams)
            {
                if (exam.Id == id)
                {
                    return exam;
                }
            }

            return null;
        }

        public Exam GetByTutor(int id)
        {
            foreach (Exam exam in exams)
            {
                if (exam.TutorId == id)
                {
                    return exam;
                }
            }

            return null;
        }

        private int GenerateId()
        {
            //Console.WriteLine($"number of exams: {exams.Count}");

            int maxId = 0;
            foreach (Exam exam in exams)
            {
                if (exam.Id > maxId)
                {
                    maxId = exam.Id;
                }
            }

            return maxId + 1;
        }

        public void Save()
        {
            try
            {

                StreamWriter file = new StreamWriter("../../../WPF/Data/ExamFile.csv", false);


                foreach (Exam exam in exams)
                {
                    file.WriteLine(exam.StringToCsv());
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
            Exam exam = GetById(id);
            if (exam == null)
            {
                return;
            }

            exams.Remove(exam);
            Save();
        }

        public void DeleteTutorsExams(int id)
        {
            Exam exam = GetByTutor(id);
            if (exam == null)
            {
                return;
            }

            exams.Remove(exam);
            Save();
        }

        public void Update(Exam exam)
        {
           
            Console.WriteLine(exam.Id);
            Exam oldExam = GetById(exam.Id);
            if (oldExam == null)
            {
                return;
            }

            if (!oldExam.Confirmed)
            {
                oldExam.Language = exam.Language;
                oldExam.LanguageLevel = exam.LanguageLevel;
                oldExam.NumOfStudents = exam.NumOfStudents;
                oldExam.ExamDate = exam.ExamDate;
                oldExam.TutorId = exam.TutorId;
                oldExam.NumberOfAppliedStudents = exam.NumberOfAppliedStudents;
                oldExam.Confirmed = exam.Confirmed;
                Save();
            }
            
        }

        public Exam Create(Exam exam)
        {
            exam.Id = GenerateId();
            exams.Add(exam);
            Save();
            ExamAdded?.Invoke(this, EventArgs.Empty);
            return exam;
        }

        public List<Exam> Load()
        {

            string filename = "../../../WPF/Data/ExamFile.csv";


            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');

                    if (tokens.Length < 10)
                    {
                        continue;
                    }
                    Exam exam = new Exam(
                        id: Int32.Parse(tokens[0]),
                        language: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), tokens[1]),
                        languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), tokens[2]),
                        numOfStudents: Int32.Parse(tokens[3]),
                        examDate: DateTime.Parse(tokens[4]),
                        examTime: TimeSpan.Parse(tokens[5]),
                        examDuration: Int32.Parse(tokens[6]),
                        tutorId: Int32.Parse(tokens[7]),
                        numberOfAppliedStudents: Int32.Parse(tokens[8]),
                        confirmed: bool.Parse(tokens[9])
                    );
                    exams.Add(exam);
                }
            }
            return exams;
        }

        public int SmartChoiceTutorForExam(LanguageEnum.Language language, LanguageLevelEnum.LanguageLevel level, DateTime? dateExam, TimeSpan? time)
        {
            List<Tutor> tutors = tutorController.GetAll();
            List<Exam> exams = this.exams;

            List<Tutor> filterTutors = new List<Tutor>();
            TimeSpan examDuration = TimeSpan.FromHours(4);

            foreach (Tutor tutor in tutors)
            {
                if (tutor.Languages == language && tutor.LanguageLevel == level)
                {
                    bool isAvailable = true;
                    foreach (Exam exam in exams)
                    {
                        if (dateExam == exam.ExamDate && tutor.Id == exam.TutorId &&
                     (time == exam.ExamTime || (time >= exam.ExamTime && time < exam.ExamTime.Add(examDuration))))
                        {
                            isAvailable = false;
                            break;
                        }
                    }
                    if (isAvailable)
                    {
                        filterTutors.Add(tutor);
                    }
                }
            }

            Dictionary<int, int> TutorsGrade = new Dictionary<int, int>();

            foreach (Tutor tutor in filterTutors)
            {
                int averageGrade = gradeTutorRepository.CalculateAverageTutorsGrade(tutor.Id);
                TutorsGrade.Add(tutor.Id, averageGrade);
            }

            int maxGradeTutorId = -1;
            int maxAverageGrade = 0;

            foreach (var entry in TutorsGrade)
            {
                if (entry.Value > maxAverageGrade)
                {
                    maxAverageGrade = entry.Value;
                    maxGradeTutorId = entry.Key;
                }
            }

            return maxGradeTutorId;
        }


    }
}
