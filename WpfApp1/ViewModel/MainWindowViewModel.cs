using CommunityToolkit.Mvvm.Input;
using DB_Linkage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Models;
using WpfApp1.View;
using WpfApp1.ViewModel.Bases;

namespace WpfApp1.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand ShowPatientCommand { get; }
        public ICommand ShowTreatmentCommand { get; }
        public ICommand AddCommand { get; }

        // DefaultView 속성 추가
        public object DefaultView { get; private set; }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

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

        private void ShowPatient()
        {
            System.Diagnostics.Debug.WriteLine("환자 버튼 클릭됨");
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

        private void ShowTreatment()
        {
            System.Diagnostics.Debug.WriteLine("진료 버튼 클릭됨");
        }

        private void AddNewItem()
        {
            System.Diagnostics.Debug.WriteLine("+ 버튼 클릭됨");
        }

        private readonly MySQLManager _dbManager;

        public MainWindowViewModel()
        {
            _dbManager = new MySQLManager();
            Doctor = _dbManager.GetDoctors(); // 데이터베이스에서 의사 정보를 가져옴

            // DefaultView 초기화
            DefaultView = this; // 메인 화면을 기본 뷰로 설정
            CurrentView = DefaultView;

            ShowPatientCommand = new RelayCommand(ShowPatient);
            ShowTreatmentCommand = new RelayCommand(ShowTreatment);
            AddCommand = new RelayCommand(AddNewItem);
        }
    }
}
