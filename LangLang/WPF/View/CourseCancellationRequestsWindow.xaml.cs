using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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
    public partial class CourseCancellationRequestsWindow : Window
    {
        Tutor tutor;
        private CourseController courseController = new CourseController();
        private StudentCourseCancellationController cancellationController = new StudentCourseCancellationController();
        private StudentController studentController = new StudentController();
        List<Course> courses;
        List<StudentCourseCancellation> cancellationRequests;
        List<Student> students;

        public int selectedId;

        public CourseCancellationRequestsWindow(Tutor tutor)
        {
            InitializeComponent();
            this.tutor = tutor;
        }

        private void CourseCancellationRequestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (DataGridColumn column in dgvRequest.Columns)
            {
                column.CanUserSort = true;
            }

            courses = courseController.GetAll();
            cancellationRequests = cancellationController.GetAll();
            students = studentController.GetAll();
            LoadDataFromCSV(courses, cancellationRequests, students);

        }
        private void LoadDataFromCSV(List<Course> courses, List<StudentCourseCancellation> cancellationRequests, List<Student> students)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Id", typeof(int));
                dataTable.Columns.Add("Language", typeof(string));
                dataTable.Columns.Add("Level", typeof(string));
                dataTable.Columns.Add("Message", typeof(string));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("Penalty points", typeof(int));

                foreach (StudentCourseCancellation request in cancellationRequests)
                {
                    if(request.Status == StatusEnum.Status.Pending) 
                    {
                        foreach (Course course in courses)
                        {
                            foreach (Student student in students)
                            {
                                if (request.IdCourse == course.Id && request.Status == StatusEnum.Status.Pending && course.TutorId == tutor.Id && request.IdStudent == student.Id)
                                {
                                    TimeSpan timeUntilCourse = course.Start - DateTime.Today;
                                    if (timeUntilCourse.TotalDays <= 0)
                                    {
                                        dataTable.Rows.Add(
                                            request.Id,
                                            course.Language.ToString(),
                                            course.LanguageLevel.ToString(),
                                            request.Message,
                                            request.Status.ToString(),
                                            student.PenaltyPoints);
                                    }
                                }
                            }
                        }
                    }
                }

                dgvRequest.ItemsSource = dataTable.DefaultView;
                dgvRequest.UnselectAllCells();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgvRequest_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvRequest.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgvRequest.SelectedItem;
                selectedId = (int)selectedRow["Id"];

            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            CourseAcceptCancellationWindow accept = new CourseAcceptCancellationWindow(tutor, selectedId);
            accept.Show();
            courses = courseController.GetAll();
            cancellationRequests = cancellationController.GetAll();
            students = studentController.GetAll();
            LoadDataFromCSV(courses, cancellationRequests, students);

        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            CourseDeclineCancellationWindow decline = new CourseDeclineCancellationWindow(tutor, selectedId);
            decline.Show();
            courses = courseController.GetAll();
            cancellationRequests = cancellationController.GetAll();
            students = studentController.GetAll();
            LoadDataFromCSV(courses, cancellationRequests, students);
        }
    }
}
