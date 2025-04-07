﻿using CommunityToolkit.Mvvm.Input;
using DB_Linkage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ICommand AddCommand { get; }

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

        private void AddNewItem()
        {
            System.Diagnostics.Debug.WriteLine("+ 버튼 클릭됨");
        }

        private IList<Treatment> _treatments;
        public IList<Treatment> Treatments
        {
            get => _treatments;
            set => SetProperty(ref _treatments, value);
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
            AddCommand = new RelayCommand(AddNewItem);
        }
    }
}
