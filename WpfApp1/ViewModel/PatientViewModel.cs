using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DB_Linkage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Models;

namespace WpfApp1.ViewModel
{
    public class PatientViewModel : ObservableObject
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly MySQLManager _dbManager;

        public ICommand GoBackCommand { get; }

        // 환자 목록 및 선택된 환자 속성 추가
        private IList<Patient> _patients;
        public IList<Patient> Patients
        {
            get => _patients;
            set => SetProperty(ref _patients, value);
        }

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set => SetProperty(ref _selectedPatient, value);
        }

        public PatientViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _dbManager = new MySQLManager();
            GoBackCommand = new RelayCommand(GoBack);

            // 환자 데이터 로드
            LoadPatientData();
        }

        public void LoadPatientData()
        {
            // 데이터베이스에서 환자 정보 가져오기
            Patients = _dbManager.GetPatients();
        }

        private void GoBack()
        {
            // MainWindow로 돌아가기
            _mainWindowViewModel.CurrentView = _mainWindowViewModel.DefaultView;
        }
    }
}
