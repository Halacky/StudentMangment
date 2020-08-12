using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StudentMangment
{
    interface IWorkDb
    {
        void SaveChanges(UniversityManagmentDBEntities1 universityManagmentDB);
    }
}
