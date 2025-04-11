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

        public ICommand EditDoctorCommand { get; }
        public ICommand DeleteDoctorCommand { get; }

        public DoctorViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _dbManager = new MySQLManager();
            GoBackCommand = new RelayCommand(GoBack);

            // 의사 데이터 로드
            LoadDoctorData();

            // Command 초기화
            EditDoctorCommand = new RelayCommand<Doctor>(EditDoctor);
            DeleteDoctorCommand = new RelayCommand<Doctor>(DeleteDoctor);
        }

        public void LoadDoctorData()
        {
            // 데이터베이스에서 의사 정보 다시 로드
            Doctors = _dbManager.GetDoctors();
        }

        private void EditDoctor(Doctor doctor)
        {
            if (doctor == null) return;

            // 수정 창 열기
            AddDoctorWindow editWindow = new AddDoctorWindow();

            // 기존 데이터 설정
            editWindow.SetInitialData(doctor);

            if (editWindow.ShowDialog() == true)
            {
                // 데이터 저장 후 목록 새로고침
                LoadDoctorData();
            }
        }

        private void DeleteDoctor(Doctor doctor)
        {
            if (doctor == null) return;

            // 확인 메시지 표시
            var result = MessageBox.Show($"'{doctor.Name}' 의사를 삭제하시겠습니까?", "삭제 확인", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool success = _dbManager.DeleteDoctor(doctor.Id);

                if (success)
                {
                    MessageBox.Show("의사가 성공적으로 삭제되었습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDoctorData(); // 목록 새로고침
                }
                else
                {
                    MessageBox.Show("의사 삭제에 실패했습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
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
