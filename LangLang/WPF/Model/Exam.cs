using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LangLang.ModelEnum.LanguageEnum;
using static LangLang.ModelEnum.LanguageLevelEnum;

namespace LangLang.Model
{
    public class Exam
    {
        private int id;
        public Language language;
        public LanguageLevel languageLevel;
        private int numOfStudents;
        private DateTime examDate;
        private TimeSpan examTime;
        private int examDuration;
        private int tutorId;
        private int numberOfAppliedStudents;
        bool confirmed;


        public Exam(int id, LanguageEnum.Language language, LanguageLevelEnum.LanguageLevel languageLevel,
                int numOfStudents, DateTime examDate, TimeSpan examTime, int examDuration, int tutorId, int numberOfAppliedStudents, bool confirmed=false)
        {
            this.id = id;
            this.language = language;
            this.languageLevel = languageLevel;
            this.numOfStudents = numOfStudents;
            this.examDate = examDate;
            this.examTime = examTime;
            this.examDuration = examDuration;
            this.tutorId = tutorId;
            this.numberOfAppliedStudents = numberOfAppliedStudents;
            this.confirmed = confirmed;
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

        public int NumOfStudents
        {
            get { return numOfStudents; }
            set { numOfStudents = value; }

        }

        public DateTime ExamDate
        {
            get { return examDate; }
            set { examDate = value; }
        }

        public int ExamDuration
        {
            get { return examDuration; }
            set { examDuration = value; }
        }


        public int TutorId 
        { 
            get { return tutorId; }
            set {  tutorId = value; }
        }

        public int NumberOfAppliedStudents
        {
            get { return numberOfAppliedStudents; }
            set { numberOfAppliedStudents = value; }
        }
        
        public bool Confirmed
        {
            get { return confirmed; }
            set { confirmed = value; }
        }
        public TimeSpan ExamTime
        {
            get { return examTime; }
            set { examTime = value; }

        }

        public String StringToCsv()
        {
            return id + "|" + language + "|" + languageLevel + "|" + numOfStudents + "|" +
                examDate.ToString("yyyy-MM-dd") + "|" + examTime.ToString(@"hh\:mm\:ss") + "|" + 4 +"|"+ tutorId + "|" + numberOfAppliedStudents + "|" + confirmed ;
        }
    }
}