using LangLang.Controllers;
using LangLang.Model;
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
    /// Interaction logic for DisplayTutorWindow.xaml
    /// </summary>
    public partial class DirectorDisplayTutorWindow : Window
    {
        private TutorController tutorController = new TutorController();

        public DirectorDisplayTutorWindow()
        {
            InitializeComponent();

        }

        private void LoadDataFromCSV(object sender, EventArgs e)
        {

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Surname", typeof(string));
            dataTable.Columns.Add("Gender", typeof(string));
            dataTable.Columns.Add("Birthday", typeof(DateTime));
            dataTable.Columns.Add("Phone number", typeof(int));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("Language", typeof(string));
            dataTable.Columns.Add("Language level", typeof(string));
            dataTable.Columns.Add("Date of tutor of creation", typeof(DateTime));

            tutorController = new TutorController();
            List<Tutor> tutors = tutorController.GetAll();
            if (tutors.Count == 0)
            {
                MessageBox.Show("The list of tutors is empty.", "Empty list", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            foreach (Tutor tutor in tutors)
            {
                string gender = tutor.Gender.ToString();
                string language = tutor.Languages.ToString();
                string languageLevel = tutor.LanguageLevel.ToString();
                dataTable.Rows.Add(
                    tutor.Id,
                    tutor.Name,
                    tutor.Surname,
                    gender,
                    tutor.Birthday,
                    tutor.PhoneNumber,
                    tutor.Email,
                    tutor.Password,
                    language,
                    languageLevel,
                    tutor.TutorCreationDate
                );

            }

            dgvTutor.ItemsSource = dataTable.DefaultView;
            dgvTutor.UnselectAllCells();
            dgvTutor.IsReadOnly = true;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgvTutor.SelectedItem == null)
            {
                MessageBox.Show("Please select a tutor for updating.", "No selection.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRowView selectedRow = dgvTutor.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                int tutorId = (int)selectedRow["Id"];
                Tutor selectedTutor = tutorController.GetById(tutorId);
                if (selectedTutor != null)
                {
                    TutorUpdateWindow updateWindow = new TutorUpdateWindow(selectedTutor);

                    updateWindow.TutorUpdated += (s, updatedTutor) =>
                    {
                        selectedRow["Name"] = updatedTutor.Name;
                        selectedRow["Surname"] = updatedTutor.Surname;
                        selectedRow["Gender"] = updatedTutor.Gender.ToString();
                        selectedRow["Birthday"] = updatedTutor.Birthday;
                        selectedRow["Phone number"] = updatedTutor.PhoneNumber;
                        selectedRow["Email"] = updatedTutor.Email;
                        selectedRow["Password"] = updatedTutor.Password;

                        dgvTutor.Items.Refresh();
                    };

                    updateWindow.ShowDialog();

                }
            }
        }


        private void dgvTutor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvTutor.SelectedItem == null)
            {
                MessageBox.Show("Please select a tutor.", "No selection.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataRowView selectedRow = dgvTutor.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                int tutorId = (int)selectedRow["Id"];
                Tutor selectedTutor = tutorController.GetById(tutorId);
                if (selectedTutor != null)
                {
                    TutorDeleteWindow deleteWindow = new TutorDeleteWindow(selectedTutor);
                    deleteWindow.TutorDeleted += (sender, deletedTutor) =>
                    {
                        // Osvežite listu tutora nakon brisanja
                        LoadDataFromCSV(sender, null);
                    };
                    deleteWindow.Show();


                }
            }
        }

    }




}