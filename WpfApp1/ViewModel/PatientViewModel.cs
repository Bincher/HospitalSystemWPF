using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DB_Linkage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.View;

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

        public ICommand EditPatientCommand { get; }
        public ICommand DeletePatientCommand { get; }

        public PatientViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _dbManager = new MySQLManager();
            GoBackCommand = new RelayCommand(GoBack);

            // 환자 데이터 로드
            LoadPatientData();

            // Command 초기화
            EditPatientCommand = new RelayCommand<Patient>(EditPatient);
            DeletePatientCommand = new RelayCommand<Patient>(DeletePatient);
        }

        public void LoadPatientData()
        {
            // 데이터베이스에서 환자 정보 가져오기
            Patients = _dbManager.GetPatients();
        }

        private void EditPatient(Patient patient)
        {
            if (patient == null) return;

            // 수정 창 열기
            AddPatientWindow editWindow = new AddPatientWindow();

            // 기존 데이터 설정
            editWindow.SetInitialData(patient);

            if (editWindow.ShowDialog() == true)
            {
                // 데이터 저장 후 목록 새로고침
                LoadPatientData();
            }
        }

        private void DeletePatient(Patient patient)
        {
            if (patient == null) return;

            // 확인 메시지 표시
            var result = MessageBox.Show($"'{patient.Name}' 환자를 삭제하시겠습니까?", "삭제 확인", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool success = _dbManager.DeletePatient(patient.Id);

                if (success)
                {
                    MessageBox.Show("환자가 성공적으로 삭제되었습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadPatientData(); // 목록 새로고침
                }
                else
                {
                    MessageBox.Show("환자 삭제에 실패했습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void GoBack()
        {
            // MainWindow로 돌아가기
            _mainWindowViewModel.CurrentView = _mainWindowViewModel.DefaultView;
        }
    }
}
