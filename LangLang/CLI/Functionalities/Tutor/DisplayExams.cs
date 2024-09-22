using LangLang.Controllers;
using LangLang.Model;
using LangLang.WPF.RepositorySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.CLI.Functionalities.Tutor
{
    public class DisplayExams
    {
        private ExamController examController;
        private TutorController tutorController;
        private CreateExamTutor createExam;
        private UpdateExam updateExam;
        private DeleteExam deleteExam;

        public DisplayExams()
        {
            examController = new ExamController();
            tutorController = new TutorController();
            createExam = new CreateExamTutor();
            updateExam = new UpdateExam();
            deleteExam = new DeleteExam();
        }

        public void Run(int tutorId)
        {
            while (true)
            {
                DisplayExamsTable(tutorId);
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Create exam");
                Console.WriteLine("2. Update exam");
                Console.WriteLine("3. Delete exam");
                Console.WriteLine("4. Exit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateExam(tutorId);
                        break;
                    case "2":
                        UpdateExam();
                        break;
                    case "3":
                        DeleteExam();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        private void DisplayExamsTable(int tutorId)
        {
            
            List<Exam> exams = examController.GetAll(); 
            if (exams.Count == 0)
            {
                Console.WriteLine("The list of exams is empty.");
                return;
            }
            Console.WriteLine("Exams:");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("ID    | Language  | Language Level | Num of Students | Exam Date   | Exam Time  | Duration | Applied | Confirmed |");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------");

            foreach (Exam exam in exams)
            {
                if (exam.TutorId == tutorId)
                {
                    string confirmedString = exam.Confirmed ? "Yes" : "No";
                    Console.WriteLine($"{exam.Id,-5} | {exam.Language,-9} | {exam.LanguageLevel,-14} | {exam.NumOfStudents,-15} | {exam.ExamDate:yyyy-MM-dd} | {exam.ExamTime:hh\\:mm\\:ss} | {exam.ExamDuration,-8} | {exam.NumberOfAppliedStudents,-7} | {confirmedString,-9} |");
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------------");
                }
            }
        }

        private void CreateExam(int tutorId)
        {
            createExam.Run(tutorId);
        }

        private void UpdateExam()
        {
            Console.Write("Enter the ID of the exam to update: ");
            if (int.TryParse(Console.ReadLine(), out int examId))
            {
                Exam selectedExam = examController.GetById(examId);
                if (selectedExam != null)
                {
                    updateExam.Run(selectedExam.TutorId, selectedExam.Id);
                }
                else
                {
                    Console.WriteLine("Exam not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        private void DeleteExam()
        {
            Console.Write("Enter the ID of the exam to delete: ");
            if (int.TryParse(Console.ReadLine(), out int examId))
            {
                deleteExam.Run(examId);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }


    }
}
