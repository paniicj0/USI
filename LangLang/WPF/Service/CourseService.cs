using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LangLang.Service
{
    internal class CourseService
    {
        private static CourseService instance = null;
        private CourseRepository courseRepository;
        public StudentCourseRequestRepository requestRepository;
        

        private CourseService()
        {
            courseRepository = CourseRepository.GetInstance();
        }

        public static CourseService GetInstance()
        {
            if (instance == null)
            {
                instance = new CourseService();
            }
            return instance;
        }

        public Course GetById(int id)
        {
            return courseRepository.GetById(id);
        }

        public List<Course> GetAll()
        {
            return courseRepository.GetAll();
        }

        public Course Create(Course course)
        {
            return courseRepository.Create(course);
        }

        public void Update(Course course)
        {
            requestRepository = new StudentCourseRequestRepository();
            List<StudentCourseRequest> requests = requestRepository.GetAll();
            foreach(StudentCourseRequest request in requests)
            {
                if(request.Id == course.Id)
                {
                    if(request.Status == ModelEnum.StatusEnum.Status.Accepted || request.Status == ModelEnum.StatusEnum.Status.Declined) 
                    {
                        MessageBox.Show("By sending notifications to students regarding their acceptance or rejection, the final list of enrolled students is confirmed. After this point, no further changes to the course can be made.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }
            courseRepository.Update(course);
        }

        public void Delete(int id)
        {
            courseRepository.Delete(id);
        }

        public void DeleteTutorsCourses(int id)
        {
            courseRepository.DeleteTutorsCourses(id);
        }

        public List<Course> LoadFromFile()
        {
            return courseRepository.LoadFromFile();
        }

    }
}
