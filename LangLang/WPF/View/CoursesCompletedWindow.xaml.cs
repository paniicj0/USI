using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangLang.View
{
    public partial class CoursesCompletedWindow : Window
    {
        public Student student;
        public List<GradeTutor> grades;
        private GradeTutorRepository gradeTutorRepository;
        private GradeTutorController gradeTutorController;
        public List<StudentCourseRequest> requests;
        public StudentCourseRequestRepository studentCourseRequestRepository;
        public List<Course> courses;
        public CourseRepository courseRepository;
        public List<Tutor> tutors;
        public TutorRepository tutorRepository;

        public StudentWindow studentWindow;

        public CoursesCompletedWindow(Student student)
        {
            InitializeComponent();
            gradeTutorRepository = GradeTutorRepository.GetInstance();
            this.grades = gradeTutorRepository.GetAll();
            studentCourseRequestRepository = StudentCourseRequestRepository.GetInstance();
            this.requests = studentCourseRequestRepository.GetAll();
            courseRepository = CourseRepository.GetInstance();
            this.courses = courseRepository.GetAll();
            tutorRepository = TutorRepository.GetInstance();
            this.tutors = tutorRepository.GetAll();
            this.student = student;
            studentWindow = new StudentWindow(student);
        }

        public void CoursesCompletedWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadDataFromCSV(grades);
        }

        private void LoadDataFromCSV(List<GradeTutor> grades)
        {
            DataTable dataTabe = new DataTable();

            dataTabe.Columns.Add("CourseID", typeof(int));
            dataTabe.Columns.Add("Language", typeof(string));
            dataTabe.Columns.Add("Language Level", typeof(string));
            dataTabe.Columns.Add("TutorID", typeof(int));
            dataTabe.Columns.Add("Tutor's name", typeof(string));
            dataTabe.Columns.Add("Tutor's surname", typeof(string));
            // dataTabe.Columns.Add("Your grade", typeof(int));

            gradeTutorController = new GradeTutorController();

            foreach (StudentCourseRequest request in requests)
            {
                if (student.Id == request.IdStudent)
                {
                    int courseId = request.IdCourse;
                    foreach (Course course in courses)
                    {
                        if (course.Id == courseId && ((((DateTime.Now - course.Start).TotalDays)) / 7) >= course.Duration)
                        {
                            foreach (Tutor tutor in tutors)
                            {
                                if (course.TutorId == tutor.Id)
                                {
                                    string language = course.Language.ToString();
                                    string languageLevel = course.LanguageLevel.ToString();
                                    dataTabe.Rows.Add(
                                        course.Id,
                                        language,
                                        languageLevel,
                                        course.TutorId,
                                        tutor.Name, 
                                        tutor.Surname
                                    );
                                }
                            }
                        }
                    }
                }
            }

            dgCompletedCourses.ItemsSource = dataTabe.DefaultView;
            dgCompletedCourses.UnselectAllCells();
            dgCompletedCourses.IsReadOnly = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel && this.IsVisible)
            {
                e.Cancel = true;
                this.Hide();
                studentWindow.Show();
            }
        }

        private void btnGradeTutor_Click(object sender, RoutedEventArgs e)
        {
            if (dgCompletedCourses.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgCompletedCourses.SelectedItem;

                int studentId = student.Id;
                int tutorId = (int)row["TutorID"];
                int courseId = (int)row["CourseID"];

                grades = gradeTutorRepository.GetAll();

                foreach (GradeTutor grade in grades)
                {
                    if (grade.StudentId == studentId && grade.TutorId == tutorId && grade.CourseId == courseId)
                    {
                        MessageBox.Show("You already graded teacher for this course!");
                        return;
                    }
                }

                foreach (Course course in courses)
                {
                    if (course.Id == courseId && student.Id == studentId && course.TutorId == tutorId)
                    {
                        GradeTutorWindow gradeTutorWindow = new GradeTutorWindow(student, tutorId, courseId);
                        gradeTutorWindow.Show();
                        LoadDataFromCSV(grades);
                    }
                }
            } else
            {
                MessageBox.Show("Select a row before clicking 'Grade tutor' button!");
            }
        }
    }
}
