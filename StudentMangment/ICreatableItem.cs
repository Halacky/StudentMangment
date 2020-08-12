using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMangment
{
    interface ICreatableItem
    {
        int updatingStudentId { get; set; }

        void AddItem();

        void UpdateItem();

        bool IsDublicate(UniversityManagmentDBEntities1 universityManagmentDB);
    }
}
