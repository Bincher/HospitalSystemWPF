using DB_Linkage.Service;
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

        private readonly MySQLManager _dbManager;
        public MainWindowViewModel()
        {
            _dbManager = new MySQLManager();
            Doctor = _dbManager.GetDoctors(); // 데이터베이스에서 의사 정보를 가져옴
        }
    }
}
