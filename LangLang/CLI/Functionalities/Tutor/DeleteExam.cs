using LangLang.Controllers;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.CLI.Functionalities.Tutor
{
    public class DeleteExam
    {
        ExamController examController;

        public DeleteExam()
        {
            examController = new ExamController();
        }

        public void Run(int id)
        {
            Exam selectedExam = examController.GetById(id);
            if (selectedExam != null)
            {
                examController.Delete(id);
                Console.WriteLine("Successfully deleted the exam.");
            }
            else
            {
                Console.WriteLine("Exam not found.");
            }
        }
    }
}
