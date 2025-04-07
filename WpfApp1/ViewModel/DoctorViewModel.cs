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
    internal class DoctorViewModel : ObservableObject
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly MySQLManager _dbManager;

        public ICommand GoBackCommand { get; }

        // 환자 목록 및 선택된 환자 속성 추가
        private IList<Doctor> _doctors;
        public IList<Doctor> Doctors
        {
            get => _doctors;
            set => SetProperty(ref _doctors, value);
        }

        private Doctor _selectedDoctor;
        public Doctor SelectedDoctor
        {
            get => _selectedDoctor;
            set => SetProperty(ref _selectedDoctor, value);
        }

        public DoctorViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _dbManager = new MySQLManager();
            GoBackCommand = new RelayCommand(GoBack);

            // 의사 데이터 로드
            LoadDoctorData();
        }

        private void LoadDoctorData()
        {
            // 데이터베이스에서 환자 정보 가져오기
            Doctors = _dbManager.GetDoctors();
        }

        private void GoBack()
        {
            // MainWindow로 돌아가기
            _mainWindowViewModel.CurrentView = _mainWindowViewModel.DefaultView;
        }
    }
}
