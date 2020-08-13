using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO.Packaging;
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

namespace StudentMangment
{
    public partial class WindowForAddAndEdit: Window,IWorkDb, ICreatableItem
    {
        public WindowForAddAndEdit(string nameOfBtn, UniversityManagmentDBEntities1 dBEntities1)
        {
            InitializeComponent();
            EnabledBtn();

            universityManagmentDB = dBEntities1;

            tbName.MaxLength = 40;
            tbLastname.MaxLength = 40;
            tbLastname.MaxLength = 40;

            NameOfBtn = nameOfBtn;
            btnOnSubWindow.Content = nameOfBtn;
        }
        public WindowForAddAndEdit(string nameOfBtn, Student student, UniversityManagmentDBEntities1 dBEntities1)
        {
            InitializeComponent();
            EnabledBtn();

            tbName.MaxLength = 40;
            tbLastname.MaxLength = 40;
            tbLastname.MaxLength = 40;

            NameOfBtn = nameOfBtn;
            btnOnSubWindow.Content = nameOfBtn;
            universityManagmentDB = dBEntities1;

            cbGender.Text = student.Gender.Trim();
            tbName.Text = student.Name.Trim();
            tbSurname.Text = student.Surname.Trim();
            tbLastname.Text = student.Lastname.Trim();
            updatingStudentId = student.Id;
        }
        private string NameOfBtn { get; set; }
        public int updatingStudentId { get; set; }
        private UniversityManagmentDBEntities1 universityManagmentDB{ get; set; }


        private void EnabledBtn()
        {
            if (cbGender.SelectedItem == null || tbName.Text.Trim().Length == 0 || tbSurname.Text.Trim().Length == 0)
            {
                btnOnSubWindow.IsEnabled = false;
            }
            else
            {
                btnOnSubWindow.IsEnabled = true;
            }
        }
        private void ButtonAddOrUpdate_Click(object sender, RoutedEventArgs e)
        { 
            if (IsDublicate(universityManagmentDB) && NameOfBtn == "Add")
            {
                MessageBox.Show("Данный студент уже есть в БД.");
            }
            else if(NameOfBtn == "Add")
            {
                AddItem();
            }else
            {
                UpdateItem();
            }
            SaveChanges(universityManagmentDB);
            this.Close();
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

        public void AddItem()
        {
            Student student = new Student();
            if (tbLastname.Text.Trim().Length==0)
            {
                student.Gender = cbGender.Text;
                student.Name = tbName.Text;
                student.Surname = tbSurname.Text;
                student.Lastname = "Нет";
            }
            else
            {
                student.Gender = cbGender.Text;
                student.Name = tbName.Text;
                student.Surname = tbSurname.Text;
                student.Lastname = tbLastname.Text;
            }

            universityManagmentDB.Students.Add(student);           
        }

        public void UpdateItem()
        {
            var student = (from st in universityManagmentDB.Students
                           where st.Id == updatingStudentId
                           select st).SingleOrDefault();

            if (student != null)
            {
                student.Gender = cbGender.Text;
                student.Name = tbName.Text;
                student.Surname = tbSurname.Text;

                if (tbLastname.Text.Trim().Length==0)
                {
                    student.Lastname = "Нет";
                }
                else
                {
                    student.Lastname = tbLastname.Text;
                }
                
            }
        }

        public bool IsDublicate(UniversityManagmentDBEntities1 universityManagmentDB)
        {
            return universityManagmentDB.Students.Any(item => item.Name == tbName.Text &&
                                                              item.Surname == tbSurname.Text &&
                                                              item.Lastname == tbLastname.Text);
        }
    
        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnabledBtn();
        }

        private void cbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnabledBtn();
        }
    }    
}
