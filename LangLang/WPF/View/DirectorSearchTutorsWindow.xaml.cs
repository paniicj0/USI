using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for DirectorSearchTutorsWindow.xaml
    /// </summary>
    public partial class DirectorSearchTutorsWindow : Window
    {
        private TutorController tutorController = new TutorController();

        public DirectorSearchTutorsWindow()
        {
            InitializeComponent();
            FillComboBoxWithLanguages();
            FillComboBoxWithLanguageLevel();
        }
        private void FillComboBoxWithLanguages()
        {
            Array languages = Enum.GetValues(typeof(LanguageEnum.Language));

            foreach (var language in languages)
            {
                cbLanguage.Items.Add(language);
            }
        }

        private void FillComboBoxWithLanguageLevel()
        {
            Array languageLevel = Enum.GetValues(typeof(LanguageLevelEnum.LanguageLevel));

            foreach (var languagelevel in languageLevel)
            {
                cbLanguageLevel.Items.Add(languagelevel);
            }
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string language = cbLanguage.SelectedItem?.ToString();
            string languageLevel = cbLanguageLevel.SelectedItem?.ToString();
            DateTime? dateAdded = dpDateAdded.SelectedDate;

            List<Tutor> searchedTutors = new List<Tutor>();

            if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(languageLevel) && dateAdded.HasValue)
            {
                searchedTutors = tutorController.SearchTutors(language, languageLevel, dateAdded.Value);
            }
            else if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(languageLevel))
            {
                searchedTutors = tutorController.SearchTutorsByLanguageAndLevel(language, languageLevel);
            }
            else if (!string.IsNullOrEmpty(language))
            {
                searchedTutors = tutorController.SearchTutorsByLanguage(language);
            }
            else if (!string.IsNullOrEmpty(languageLevel))
            {
                searchedTutors = tutorController.SearchTutorsByLevel(languageLevel);
            }
            else if (dateAdded.HasValue)
            {
                searchedTutors = tutorController.SearchTutorsByDateAdded(dateAdded.Value);
            }
            else
            {
                MessageBox.Show("Please enter search criteria.");
                return;
            }

            dgvSearchedTutors.ItemsSource = searchedTutors;


        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cbLanguage.SelectedItem = null;
            cbLanguageLevel.SelectedItem = null;


            // Clear date picker
            dpDateAdded.SelectedDate = null;
        }
    }
}

