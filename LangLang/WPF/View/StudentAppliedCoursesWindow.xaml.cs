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
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static LangLang.ModelEnum.StatusEnum;

namespace LangLang.View
{
    public partial class StudentAppliedCoursesWindow : Window
    {
        public Student student;
        public List<Course> courses;
        private Course selectedCourse;
        private List<StudentCourseRequest> requests;
        public ExamsDisplayForStudent examsDisplayForStudent;
        private CourseController courseController;
        private CourseRepository courseRepository;
        private StudentCourseRequestController studentCourseRequestController = new StudentCourseRequestController();
        private StudentCourseCancellationController studentCourseCancellationController = new StudentCourseCancellationController();
        private StudentCourseRequestRepository studentCourseRepository;
        public StudentWindow studentWindow;
        public StudentCourseCancellationRepository studentCourseCancellationRepository;
        public List<StudentCourseCancellation> cancellations;

        public StudentAppliedCoursesWindow(Student student)
        {
            InitializeComponent();
            courseRepository = CourseRepository.GetInstance();
            this.courses = courseRepository.GetAll();
            studentCourseRepository = StudentCourseRequestRepository.GetInstance();
            this.requests = studentCourseRepository.GetAll();
            this.student = student;
            studentWindow = new StudentWindow(student);
            studentCourseCancellationRepository = StudentCourseCancellationRepository.GetInstance();
            this.cancellations = studentCourseCancellationRepository.GetAll();
        }

        public void StudentAppliedCoursesWindow_Load(object sender, RoutedEventArgs e)
        {
            LoadDataFromCSV(courses);
        }

        private void LoadDataFromCSV(List<Course> courses)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("RequestID", typeof(int));
            dataTable.Columns.Add("CourseID", typeof(int));
            dataTable.Columns.Add("Language", typeof(string));
            dataTable.Columns.Add("Level", typeof(string));
            dataTable.Columns.Add("Duration", typeof(int));
            dataTable.Columns.Add("Start", typeof(DateTime));
            dataTable.Columns.Add("Realization", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));

            courseController = new CourseController();

            foreach (StudentCourseRequest request in requests)
            {
                if (request.IdStudent == student.Id)
                {
                    foreach (Course course in courses)
                    {
                        if (course.Id == request.IdCourse)
                        {
                            string language = course.Language.ToString();
                            string languageLevel = course.LanguageLevel.ToString();
                            string status = request.Status.ToString();
                            dataTable.Rows.Add(
                                request.Id,
                                course.Id,
                                language,
                                languageLevel,
                                course.Duration,
                                course.Start,
                                course.Realization,
                                status
                            );
                        }
                    }
                }
            }
            dgCourse.ItemsSource = dataTable.DefaultView;
            dgCourse.UnselectAllCells();
            dgCourse.IsReadOnly = true;
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

        // Function for cancellation of requests for entering course
        private void btnCancelRequest_Click(object sender, RoutedEventArgs e)
        {
            if (dgCourse.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgCourse.SelectedItem;

                int requestId = (int)row["RequestID"];
                int courseId = (int)row["CourseID"];
                string status = (string)row["Status"];

                string accepted = StatusEnum.Status.Accepted.ToString();
                string declined = StatusEnum.Status.Declined.ToString();
                string pending = StatusEnum.Status.Pending.ToString();
                string completed = StatusEnum.Status.Completed.ToString();
                string waiting = StatusEnum.Status.Waiting.ToString();

                foreach (Course course in courses)
                {
                    if (course.Id == courseId)
                    {
                        if (status == pending)
                        {
                            if ((course.Start - DateTime.Now).TotalDays >= 7)
                            {
                                StudentCourseRequest studentCourseRequest = new StudentCourseRequest(requestId, courseId, student.Id, StatusEnum.Status.Declined, " ");
                                studentCourseRequestController.Update(studentCourseRequest);

                                LoadDataFromCSV(courses);
                            }
                            else
                            {
                                MessageBox.Show("Can't cancel application for courses week before its start!");
                            }
                        } else 
                        {
                            MessageBox.Show("You can cancel a request only for courses that are pending!");
                        }
                    } 
                }
            }
        }

        // Function for dropping out from course 
        private void btnDropOut_Click(object sender, RoutedEventArgs e)
        {
            if (dgCourse.SelectedItem == null)
            {
                return;
            }
            DataRowView row = (DataRowView)dgCourse.SelectedItem;

            int requestId = (int)row["RequestID"];
            int courseId = (int)row["CourseID"];
            string status = (string)row["Status"];

            string accepted = StatusEnum.Status.Accepted.ToString();

            cancellations = studentCourseCancellationRepository.GetAll();

            bool isCancellationSent = studentCourseCancellationController.IsCancellationSent(courseId, student.Id);
            if (isCancellationSent)
            {
                MessageBox.Show("You already sent a request for dropping out from this course!");
                return;
            }

            foreach (Course course in courses)
            {
                if (course.Id == courseId)
                {
                    if (status != accepted)
                    {
                        MessageBox.Show("You can drop out only from courses to which you have been accepted!");
                        return;
                    }
                    if ((DateTime.Now - course.Start).TotalDays < 7)
                    {
                        MessageBox.Show("Can't drop out from the course in the first week or before the course has started!");
                        return;
                    }
                    CourseDropOutMessageWindow dropOutWindow = new CourseDropOutMessageWindow(requestId, courseId, student);
                    dropOutWindow.Show();
                    LoadDataFromCSV(courses);
                }
            }
        }
    }
}
