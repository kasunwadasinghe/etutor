using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETutor.ViewModel
{
    public class StudentTutorViewModel
    {
        public StudentTutorViewModel()
        {
            selectedStudents = new List<DropdownViewModel>();
            tutors = new List<DropdownViewModel>();
            students = new List<DropdownViewModel>();
        }
        public DropdownViewModel tutorId { get; set; }
        public List<DropdownViewModel> selectedStudents { get; set; }
        public List<DropdownViewModel> tutors { get; set; }
        public List<DropdownViewModel> students { get; set; }
        public List<DropdownViewModel> users { get; set; }
    }
}