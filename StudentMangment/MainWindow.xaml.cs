using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentMangment
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IWorkDb
    {
        private UniversityManagmentDBEntities1 dBEntities { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            dBEntities = new UniversityManagmentDBEntities1();
            CheckCountOfSelectedItems();
            gridWithStudent.ItemsSource = dBEntities.Students.ToList();
        }

        private void DG1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string headername = e.Column.Header.ToString();

            if (headername == "Id")
            {
                e.Cancel = true;
            }
        }//EditGrid

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)//addBtn
        {
            WindowForAddAndEdit windowForAddAndEdit = new WindowForAddAndEdit("Add",dBEntities);
            windowForAddAndEdit.Show();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)//refreshBtn
        {
            Refresh();
            tbName.Text = "";
            tbSurname.Text = "";
            tbLastname.Text = "";
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)//deleteBtn
        {

            var student = (Student)gridWithStudent.SelectedItems[0];

            var singleStudentObj = (from st in dBEntities.Students
                                    where st.Id == student.Id
                                    select st).SingleOrDefault();

            if (singleStudentObj != null)
            {
                dBEntities.Students.Remove(singleStudentObj);
                SaveChanges(dBEntities);
            }
            Refresh();
        }
        
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)//editBtn
        {
            if (gridWithStudent.SelectedIndex >= 0)
            {
                if (gridWithStudent.SelectedItems.Count >= 0)
                {
                    if (gridWithStudent.SelectedItems[0].GetType() == typeof(Student))
                    {
                        var student = (Student)gridWithStudent.SelectedItems[0];
                        WindowForAddAndEdit windowForAddAndEdit = new WindowForAddAndEdit("Update", student, dBEntities);
                        windowForAddAndEdit.Show();
                    }
                }
            }

        }

        private void CheckCountOfSelectedItems()
        {
            if (gridWithStudent.SelectedItems.Count<1)
            {
                EditBtn.IsEnabled = false;
                DeleteBtn.IsEnabled = false;
            }
            else
            {
                EditBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
        }

        private void Refresh()
        {
            
            gridWithStudent.ItemsSource = dBEntities.Students.ToList();
        }//methodForRefresh

        public void SaveChanges(UniversityManagmentDBEntities1 universityManagmentDB)
        {
            try
            {
                universityManagmentDB.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }//saveDbWithTryCatch

        private void gridWithStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckCountOfSelectedItems();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterBySymbols();
        }        

        private void FilterBySymbols()
        {
            var filteredDb = dBEntities.Students.Where(item => item.Name.StartsWith(tbName.Text)&& 
                                                       item.Surname.StartsWith(tbSurname.Text)&& 
                                                       item.Lastname.StartsWith(tbLastname.Text)).Select(item => item);
            gridWithStudent.ItemsSource = filteredDb.ToList();
        }//SearchInDb

        private void FilterBySymbols(string gender)
        {
            var filteredDb = dBEntities.Students.Where(item => item.Name.StartsWith(tbName.Text)&&
                                                       item.Surname.StartsWith(tbSurname.Text)&&
                                                       item.Lastname.StartsWith(tbLastname.Text)&&
                                                       item.Gender == gender).Select(item => item);
            gridWithStudent.ItemsSource = filteredDb.ToList();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)rbMale.IsChecked)
            {
                FilterBySymbols("Муж");                
            }
            else if((bool)rbFemale.IsChecked)
            {
                FilterBySymbols("Жен");                
            }
            else
            {
                FilterBySymbols();
            }
        }
    }
}
