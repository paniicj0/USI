using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class Course
    {

        private int id;
        private LanguageEnum.Language language;
        private LanguageLevelEnum.LanguageLevel languageLevel;
        private int duration;
        private List<DaysEnum.Days> days;
        private DateTime start;
        private RealizationEnum.Realization realization;
        private int maxStudents;
        private int tutorId;
        private int numberOfStudents;
        public Course(int id, LanguageEnum.Language language, LanguageLevelEnum.LanguageLevel languageLevel, int duration, List<DaysEnum.Days> days, DateTime start,
            RealizationEnum.Realization realization, int maxStudents, int tutorId, int numberOfStudents)
        {
            this.id = id;
            this.language = language;
            this.languageLevel = languageLevel;
            this.duration = duration;
            this.start = start;
            this.days = days;
            this.realization = realization;
            this.maxStudents = maxStudents;
            this.tutorId = tutorId;
            this.numberOfStudents = numberOfStudents;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public LanguageEnum.Language Language
        {
            get { return language; }
            set { language = value; }

        }

        public LanguageLevelEnum.LanguageLevel LanguageLevel
        {
            get { return languageLevel; }
            set { languageLevel = value; }
        }

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public List<DaysEnum.Days> Days
        {
            get { return days; }
            set { days = value; }
        }

        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }

        public RealizationEnum.Realization Realization
        {
            get { return realization; }
            set { realization = value; }
        }

        public int MaxStudents
        {
            get { return maxStudents; }
            set { maxStudents = value; }
        }

        public int TutorId
        {
            get { return tutorId; }
            set { tutorId = value; }
        }

        public int NumberOfStudents
        {
            get { return numberOfStudents; }
            set { numberOfStudents = value; }
        }

        public String StringToCsv()
        {
            string daysString = string.Join(",", days.Select(d => d.ToString()));
            return $"{id}|{language}|{languageLevel}|{duration}|{daysString}|{start.ToString("yyyy-MM-dd")}|{realization}|{maxStudents}|{tutorId}|{numberOfStudents}";
        }
    }
}