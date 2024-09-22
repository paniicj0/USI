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

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for StudentsCourseOngoingWindow.xaml
    /// </summary>
    public partial class StudentsCourseOngoingWindow : Window
    {
        private CourseController courseController = new CourseController();
        private StudentController studentController = new StudentController();
        private StudentCourseRequestController requestController = new StudentCourseRequestController();
        private Tutor tutor;
        List<Course> courses;
        List<Student> students;
        List<StudentCourseRequest> requests;
        private Student student;
        
        public int selectedCourseId;
        public int selectedId;
        public StatusEnum.Status selectedStatus;

        public StudentsCourseOngoingWindow(int selectedCourseId, Tutor tutor)
        {
            InitializeComponent();
            this.tutor = tutor;
            this.selectedCourseId = selectedCourseId;
            
        }

        private void StudentsCourseOngoingWindow_Loaded(object sender, RoutedEventArgs e)
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
                    dataTable.Columns.Add("Penalty points", typeof(int));

                    dataTable.Rows.Clear();
                    foreach (StudentCourseRequest request in requests)
                    {
                        if (request.IdCourse == selectedCourseId)
                        {
                            foreach (Student student in students)
                            {
                                if (request.IdStudent == student.Id && request.Status == StatusEnum.Status.Accepted)
                                {
                                    if (IsValidStudent(student))
                                    {
                                        dataTable.Rows.Add(
                                            student.Id,
                                            student.Name,
                                            student.Surname,
                                            student.Gender.ToString(),
                                            student.Birthday.ToString(),
                                            student.Email,
                                            student.Profession.ToString(),
                                            request.Status.ToString(),
                                            student.PenaltyPoints);
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

        private void btnPenaltyPoint_Click(object sender, RoutedEventArgs e)
        {
            if (dgvStudents.SelectedItem != null)
            {
                
                Student student = studentController.GetById(selectedId);

                GivePenaltyPoint givePenaltyPointWindow = new GivePenaltyPoint(student, selectedCourseId);
                givePenaltyPointWindow.ShowDialog();
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
