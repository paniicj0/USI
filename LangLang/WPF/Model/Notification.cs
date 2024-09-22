using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LangLang.ModelEnum.DaysEnum;
using static LangLang.ModelEnum.LanguageEnum;
using static LangLang.ModelEnum.LanguageLevelEnum;
using static LangLang.ModelEnum.RealizationEnum;
using System.Windows;

namespace LangLang.Model
{
    public class Notification
    {
        int id;
        int whom;
        int idCourse;
        bool read;
        string message;

        public Notification(int id, int whom, int idCourse, bool read, string message)
        {
            this.id = id;
            this.whom = whom;
            this.idCourse = idCourse;
            this.read = read;
            this.message = message;
        }

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public int Whom
        {
            get { return whom; }
            set { whom = value; }
        }

        public int IdCourse
        {
            get { return idCourse; }
            set { idCourse = value; }
        }

        public bool Read
        {
            get { return read; }
            set { read = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public String StringToCsv()
        {
            return $"{id}|{whom}|{idCourse}|{read}|{message}";
        }
    }
}
