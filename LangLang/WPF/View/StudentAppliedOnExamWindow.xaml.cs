using LangLang.Controllers;
using LangLang.Model;
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
    /// Interaction logic for StudentAppliedOnExamWindow.xaml
    /// </summary>
    public partial class StudentAppliedOnExamWindow : Window
    {
        StudentController studentController;
        private TutorRepository tutorRepository;
        private StudentRepository studentRepository;
        public int selectedId;
        public string selectedName;
        public string selectedSurname;
        public string selectedEmail;
        public string selectedPenaltyPoints;
        private Tutor tutor;
        private Student student;
        private List<Student> students;
        private CourseRepository course;

        public StudentAppliedOnExamWindow(Tutor tutor)
        {
            InitializeComponent();
            this.students = students;
            this.tutor = tutor;
            studentRepository = StudentRepository.GetInstance();
            //studentRepository.PenaltyPointsAdded += UpdateChangedPoints;
            tutorRepository= TutorRepository.GetInstance();
            course= CourseRepository.GetInstance();
        }
        private void UpdateChangedPoints(object sender, EventArgs e)
        {
            LoadDataFromCSV();
        }

        private void StudentAppliedOnExamWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //ExamRepository examRepository = ExamRepository.GetInstance();
            //seting possibility to sort collumns 
            foreach (DataGridColumn column in dgvAppStudents.Columns)
            {
                column.CanUserSort = true;
            }
            LoadDataFromCSV();
        }

        private void LoadDataFromCSV()
        {
            
            if (tutor == null)
            {
                return;
            }

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Surname", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("PenaltyPoints", typeof(int));

            studentController = new StudentController();
            students = studentController.GetAll();

            foreach (Student student in students)
            {
                dataTable.Rows.Add(
                    student.Id,
                    student.Name,
                    student.Surname,
                    student.Email,
                    student.PenaltyPoints
                );
            }

            dgvAppStudents.ItemsSource = dataTable.DefaultView;
            dgvAppStudents.UnselectAllCells();
        }

        private void dgvAppStudents_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvAppStudents.SelectedItem == null || dgvAppStudents.SelectedCells.Count == 0)
            {
                dgvAppStudents.UnselectAll();
                return;
            }

            if (dgvAppStudents.SelectedItem is DataRowView selectedRow)
            {
                selectedRow = (DataRowView)dgvAppStudents.SelectedItem;
                selectedId = (int)selectedRow["Id"];
                selectedName = selectedRow["Name"].ToString();
                selectedSurname = selectedRow["Surname"].ToString();
                selectedEmail = selectedRow["Email"].ToString();
                selectedPenaltyPoints = selectedRow["PenaltyPoints"].ToString();
            }
        }

        
    }
}
