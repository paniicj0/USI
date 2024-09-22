using LangLang.Controllers;
using LangLang.Model;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public partial class DirectorSendThankYouMail : Window
    {
        private CourseRepository courseRepository;
        private CourseController courseController;
        public List<Course> courses;
        public DirectorSendThankYouMail()
        {
            InitializeComponent();
            courseRepository = CourseRepository.GetInstance();
            this.courses = courseRepository.GetAll();
        }

        public void DirectorSendThankYouMail_Load(object sender, RoutedEventArgs e)
        {
            LoadDataFromCSV();
        }

        private void LoadDataFromCSV()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("CourseID", typeof(int));
            dataTable.Columns.Add("Language", typeof(string));
            dataTable.Columns.Add("Language Level", typeof(string));

            courseController = new CourseController();

            foreach (Course course in courses)
            {
                if (((DateTime.Now - course.Start).TotalDays)/7 > course.Duration)
                {
                    string language = course.Language.ToString();
                    string languageLevel = course.LanguageLevel.ToString();
                    dataTable.Rows.Add(
                        course.Id,
                        language,
                        languageLevel
                    );
                }
            }
            dgThankYouMail.ItemsSource = dataTable.DefaultView;
            dgThankYouMail.UnselectAllCells();
            dgThankYouMail.IsReadOnly = true;
        }

        private void ThankYouMailBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dgThankYouMail.SelectedItem == null)
            {
                MessageBox.Show("Select a row before clicking 'Send thank you mails' button!");
                return;
            }

            DataRowView row = (DataRowView)dgThankYouMail.SelectedItem;

            int courseId = (int)row["CourseID"];

            DirectorChoosePriorityForMail directorChoosePriorityForMail = new DirectorChoosePriorityForMail(courseId);
            directorChoosePriorityForMail.Show();
        }
    }
}
