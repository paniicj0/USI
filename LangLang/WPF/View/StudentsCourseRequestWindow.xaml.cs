using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
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
using USIProject.View;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for StudentsCourseRequestWindow.xaml
    /// </summary>
    public partial class StudentsCourseRequestWindow : Window
    {
        private CourseController courseController = new CourseController();
        private StudentController studentController = new StudentController();
        private StudentCourseRequestController requestController = new StudentCourseRequestController();
        private Tutor tutor;
        List<Course> courses;
        List<Student> students;
        List<StudentCourseRequest> requests;

        public int selectedCourseId;
        public int selectedId;
        public StatusEnum.Status selectedStatus;

        public StudentsCourseRequestWindow(int selectedCourseId, Tutor tutor)
        {
            InitializeComponent();
            this.tutor = tutor;
            this.selectedCourseId = selectedCourseId;
        }

        private void StudentsCourseRequestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            foreach (DataGridColumn column in dgvStudents.Columns)
            {
                column.CanUserSort = true;
            }
            students = studentController.GetAll();
            courses = courseController.GetAll();
            requests = requestController.GetAll();
            LoadDataFromCSV(students, courses, requests);

        }

        private void LoadDataFromCSV(List<Student> students, List<Course> courses, List<StudentCourseRequest> requests)
        {
            try
            {
                using (DataTable dataTable = new DataTable())
                {
                    dataTable.Columns.Clear();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Name", typeof(string));
                    dataTable.Columns.Add("Surname", typeof(string));
                    dataTable.Columns.Add("Gender", typeof(string));
                    dataTable.Columns.Add("Birthday", typeof(string));
                    dataTable.Columns.Add("Email", typeof(string));
                    dataTable.Columns.Add("Proffesion", typeof(string));
                    dataTable.Columns.Add("Status", typeof(string));

                    dataTable.Rows.Clear();
                    foreach (StudentCourseRequest request in requests)
                    {
                        if (request.IdCourse == selectedCourseId)
                        {
                            foreach (Student student in students)
                            {
                                if (request.IdStudent == student.Id)
                                {
                                    if (IsValidStudent(student))
                                    {
                                        dataTable.Rows.Add(
                                            request.Id,
                                            student.Name,
                                            student.Surname,
                                            student.Gender.ToString(),
                                            student.Birthday.ToString(),
                                            student.Email,
                                            student.Profession.ToString(),
                                            request.Status.ToString());
                                    }

                                }

                            }
                        }

                    }      

                    dgvStudents.ItemsSource = dataTable.DefaultView;
                    dgvStudents.UnselectAllCells();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidStudent(Student student)
        {

            return student != null;
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (dgvStudents.SelectedItem != null)
            {
                if (selectedStatus == StatusEnum.Status.Accepted || selectedStatus == StatusEnum.Status.Declined)
                {
                    MessageBox.Show("You have already processed this request. You cannot change the status again.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                StudentCourseRequest request = requestController.GetById(selectedId);

                StudentAcceptWindow acceptWindow = new StudentAcceptWindow(request);
                acceptWindow.ShowDialog();
                students = studentController.GetAll();
                courses = courseController.GetAll();
                requests = requestController.GetAll();
                LoadDataFromCSV(students, courses, requests);
            }
            else
            {
                MessageBox.Show("Please select the row you want to update first.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDecline_Click(object sender, RoutedEventArgs e)
        {
            if (dgvStudents.SelectedItem != null)
            {
                if (selectedStatus == StatusEnum.Status.Accepted || selectedStatus == StatusEnum.Status.Declined)
                {
                    MessageBox.Show("You have already processed this request. You cannot change the status again.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                StudentCourseRequest request = requestController.GetById(selectedId);

                StudentDeclineWindow declineWindow = new StudentDeclineWindow(request);
                declineWindow.ShowDialog();
                students = studentController.GetAll();
                courses = courseController.GetAll();
                requests = requestController.GetAll();
                LoadDataFromCSV(students, courses, requests);
            }
            else
            {
                MessageBox.Show("Please select the row you want to update first.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void dgvStudents_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvStudents.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgvStudents.SelectedItem;
                selectedId = (int)selectedRow["Id"];
                selectedStatus = (StatusEnum.Status)Enum.Parse(typeof(StatusEnum.Status), selectedRow["Status"].ToString());
         
            }
        }
    }
}
