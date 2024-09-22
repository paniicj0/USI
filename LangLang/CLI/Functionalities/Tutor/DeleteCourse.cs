using LangLang.Controllers;
using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.CLI.Functionalities.Tutor
{
    public class DeleteCourse
    {
        CourseController courseController;

        public DeleteCourse() 
        {
            courseController = new CourseController();
        }

        public void Run(int id)
        {
            Course selectedCourse = courseController.GetById(id);
            if (selectedCourse != null)
            {
                courseController.Delete(id);
                Console.WriteLine("Successfully deleted the course.");
            }
            else
            {
                Console.WriteLine("Course not found.");
            }
        }
    }
}
