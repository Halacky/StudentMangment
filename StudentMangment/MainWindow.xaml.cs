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
        public MainWindow()
        {
            InitializeComponent();
            CheckCountOfSelectedItems();
            UniversityManagmentDBEntities1 dBEntities = new UniversityManagmentDBEntities1();
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

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)//add
        {
            WindowForAddAndEdit windowForAddAndEdit = new WindowForAddAndEdit("Add");
            windowForAddAndEdit.Show();
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)//refresh
        {
            Refresh();
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)//delete
        {
            UniversityManagmentDBEntities1 dBEntities = new UniversityManagmentDBEntities1();
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
        
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)//edit
        {
            if (gridWithStudent.SelectedIndex >= 0)
            {
                if (gridWithStudent.SelectedItems.Count >= 0)
                {
                    if (gridWithStudent.SelectedItems[0].GetType() == typeof(Student))
                    {
                        var student = (Student)gridWithStudent.SelectedItems[0];
                        WindowForAddAndEdit windowForAddAndEdit = new WindowForAddAndEdit("Update", student);
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
            UniversityManagmentDBEntities1 dBEntities = new UniversityManagmentDBEntities1();
            gridWithStudent.ItemsSource = dBEntities.Students.ToList();
        }

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
        }

        private void gridWithStudent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckCountOfSelectedItems();
        }
    }
}
