using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;
using WpfApp1.ViewModel.Bases;

namespace WpfApp1.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IList<Doctor>? _doctor;
        public IList<Doctor>? Doctor
        {
            get => _doctor;
            set => SetProperty(ref _doctor, value);
        }

        private Doctor? _selectedPerson;
        public Doctor? SelectedPerson
        {
            get => _selectedPerson;
            set => SetProperty(ref _selectedPerson, value);
        }

        public MainWindowViewModel()
        {
            Doctor = new List<Doctor>
            {
                new Doctor{Id = 111111, Name = "김지찬", Department = "정형외과", Age = 31},
                new Doctor{Id = 111112, Name = "이재현", Department = "정형외과", Age = 30},
                new Doctor{Id = 111113, Name = "김영웅", Department = "성형외과", Age = 35},
                new Doctor{Id = 112111, Name = "박병호", Department = "성형외과", Age = 44},
                new Doctor{Id = 112112, Name = "류지혁", Department = "피부과", Age = 52},
            };
        }

    }
}
