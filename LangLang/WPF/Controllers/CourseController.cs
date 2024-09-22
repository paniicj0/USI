using LangLang.Model;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Controllers
{
    public class CourseController
    {
        private CourseService courseService;

        public CourseController()
        {
            courseService = CourseService.GetInstance();
        }

        public List<Course> GetAll()
        {
            return courseService.GetAll();
        }

        public Course GetById(int id)
        {
            return courseService.GetById(id);
        }

        public Course Create(Course course)
        {
            return courseService.Create(course);
        }

        public void Update(Course course)
        {
            courseService.Update(course);
        }

        public void Delete(int id)
        {
            courseService.Delete(id);
        }

        public void DeleteTutorsCourses(int id)
        {
            courseService.DeleteTutorsCourses(id);
        }

        public List<Course> LoadFromFile()
        {
            return courseService.LoadFromFile();
        }
    }
}
