using CommunityToolkit.Mvvm.Input;
using DB_Linkage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.Models;
using WpfApp1.View;
using WpfApp1.ViewModel.Bases;

namespace WpfApp1.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand ShowPatientCommand { get; }
        public ICommand ShowDoctorCommand { get; }
        public ICommand AddTreatmentCommand { get; }
        public ICommand EditTreatmentCommand { get; }
        public ICommand DeleteTreatmentCommand { get; }

        // DefaultView 속성 추가
        public object DefaultView { get; private set; }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }
        
        private void ShowPatient()
        {
            try
            {
                var patientView = new PatientView
                {
                    DataContext = new PatientViewModel(this) // ViewModel 전달
                };
                patientView.Show(); // Window 열기
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"에러: {ex.Message}");
            }
        }

        private void ShowDoctor()
        {
            try
            {
                var doctorView = new DoctorView
                {
                    DataContext = new DoctorViewModel(this) // ViewModel 전달
                };
                doctorView.Show(); // Window 열기
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"에러: {ex.Message}");
            }
        }

        private IList<Treatment> _treatments;
        public IList<Treatment> Treatments
        {
            get => _treatments;
            set => SetProperty(ref _treatments, value);
        }

        public void LoadTreatmentData()
        {
            Treatments = _dbManager.GetTreatment();
        }

        private void AddTreatment()
        {
            // AddTreatmentWindow 열기
            AddTreatmentWindow addTreatmentWindow = new AddTreatmentWindow();

            if (addTreatmentWindow.ShowDialog() == true)
            {
                // 새 진료 추가 후 목록 새로고침
                LoadTreatmentData();
            }
        }

        private void EditTreatment(Treatment treatment)
        {
            if (treatment == null) return;

            // 진료 수정 창 열기
            AddTreatmentWindow editWindow = new AddTreatmentWindow();
            editWindow.SetInitialData(treatment); // 기존 데이터 설정

            if (editWindow.ShowDialog() == true)
            {
                LoadTreatmentData(); // 목록 새로고침
            }
        }

        private void DeleteTreatment(Treatment treatment)
        {
            if (treatment == null) return;

            // 확인 메시지 표시
            var result = MessageBox.Show(
                $"진료 기록(ID: {treatment.Id})을 삭제하시겠습니까?",
                "삭제 확인",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                bool success = _dbManager.DeleteTreatment(treatment.Id);

                if (success)
                {
                    MessageBox.Show("진료 기록이 삭제되었습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadTreatmentData(); // 목록 새로고침
                }
                else
                {
                    MessageBox.Show("삭제에 실패했습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private readonly MySQLManager _dbManager;

        public MainWindowViewModel()
        {
            _dbManager = new MySQLManager();
            Treatments = _dbManager.GetTreatment();

            // DefaultView 초기화
            DefaultView = this; // 메인 화면을 기본 뷰로 설정
            CurrentView = DefaultView;

            ShowPatientCommand = new RelayCommand(ShowPatient);
            ShowDoctorCommand = new RelayCommand(ShowDoctor);
            AddTreatmentCommand = new RelayCommand(AddTreatment);
            EditTreatmentCommand = new RelayCommand<Treatment>(EditTreatment);
            DeleteTreatmentCommand = new RelayCommand<Treatment>(DeleteTreatment);
        }
    }
}
