using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace LangLang.Repository
{
    public class TutorRepository
    {
        private static TutorRepository instance = null;
        private List<Tutor> tutors;
        private CourseRepository courseRepository;
        private StudentRepository studentRepository;

        private TutorRepository()
        {
            tutors = LoadFromFile();
            courseRepository=CourseRepository.GetInstance();
            studentRepository=StudentRepository.GetInstance();
        }

        public static TutorRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new TutorRepository();
            }
            return instance;
        }
        public List<Tutor> GetAll()
        {
            return tutors;
        }

        public Tutor GetById(int id)
        {
            foreach (Tutor tutor in tutors)
            {
                if (tutor.Id == id)
                {
                    return tutor;
                }
            }

            return null;
        }

        private int GenerateId()
        {
            int maxId = 0;
            foreach (Tutor tutor in tutors)
            {
                if (tutor.Id > maxId)
                {
                    maxId = tutor.Id;
                }
            }

            return maxId + 1;
        }

        public void Save()
        {
            try
            {



                StreamWriter file = new StreamWriter("../../../WPF/Data/TutorFile.csv", false);


                foreach (Tutor tutor in tutors)
                {
                    file.WriteLine(tutor.StringToCsv());
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
            Tutor tutor = GetById(id);
            if (tutor == null)
            {
                return;
            }
            tutors.Remove(tutor);
            Save();
        }

        public void Update(Tutor tutor)
        {
            Tutor oldTutor = GetById(tutor.Id);
            if (oldTutor == null)
            {
                return;
            }
            oldTutor.Name = tutor.Name;
            oldTutor.Surname = tutor.Surname;
            oldTutor.PhoneNumber = tutor.PhoneNumber;
            oldTutor.Email = tutor.Email;
            oldTutor.Password = tutor.Password;
            oldTutor.Languages = tutor.Languages;
            oldTutor.LanguageLevel = tutor.LanguageLevel;
            oldTutor.TutorCreationDate = tutor.TutorCreationDate;
            Save();
        }

        public Tutor Create(Tutor tutor)
        {
            if (tutors.Any(t => t.Email == tutor.Email))
            {
                return null;

            }
            tutor.Id = GenerateId();
            tutors.Add(tutor);
            Save();
            return tutor;
        }

        public List<Tutor> LoadFromFile()
        {

            List<Tutor> tutors = new List<Tutor>();

            string filename = "../../../WPF/Data/TutorFile.csv";


            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split('|');
                    if (tokens.Length < 9) { continue; }

                    Tutor tutor = new Tutor(
                        id: Int32.Parse(tokens[0]),
                        name: tokens[1],
                        surname: tokens[2],
                        gender: (GenderEnum.Gender)Enum.Parse(typeof(GenderEnum.Gender), tokens[3]),
                        birthday: DateTime.ParseExact(tokens[4], "yyyy-MM-dd", null),
                        phoneNumber: Convert.ToInt32(tokens[5]),
                        email: tokens[6],
                        password: tokens[7],
                        languages: (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), tokens[8]),
                        languageLevel: (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), tokens[9]),
                        tutorCreationDate: DateTime.ParseExact(tokens[10], "yyyy-MM-dd", null));
                    tutors.Add(tutor);
                }
            }
            return tutors;
        }

        public Tutor GetBestInstructor(List<Tutor> instructors, List<Course> courses, LanguageEnum.Language language, LanguageLevelEnum.LanguageLevel level, List<DaysEnum.Days> requiredDays, DateTime startDate, int duration)
        {
            // Filtriraj predavače po jeziku i nivou
            var qualifiedInstructors = instructors.Where(i => i.Languages == language && i.LanguageLevel == level).ToList();
            if (!qualifiedInstructors.Any())
                throw new Exception("No qualified tutors available");

            // Filtriraj predavače po dostupnosti
            var availableInstructors = new List<Tutor>();
            foreach (var instructor in qualifiedInstructors)
            {
                bool isAvailable = true;
                var instructorCourses = courses.Where(c => c.TutorId == instructor.Id).ToList();

                foreach (var course in instructorCourses)
                {
                    foreach (var day in requiredDays)
                    {
                        // Proveri da li se dani i termini preklapaju
                        if (course.Days.Contains(day))
                        {
                            DateTime courseEndDate = course.Start.AddDays(course.Duration * 7);
                            DateTime newCourseEndDate = startDate.AddDays(duration * 7);

                            if ((startDate <= courseEndDate && startDate >= course.Start) ||
                                (newCourseEndDate <= courseEndDate && newCourseEndDate >= course.Start))
                            {
                                isAvailable = false;
                                break;
                            }
                        }
                    }

                    if (!isAvailable)
                        break;
                }

                if (isAvailable)
                {
                    availableInstructors.Add(instructor);
                }
            }

            if (!availableInstructors.Any())
                throw new Exception("No instructors available at the required times");

            // Rangiraj predavače po opterećenju (broju kurseva)
            return availableInstructors.OrderBy(i => courses.Count(c => c.TutorId == i.Id)).First();
        }
    }
}