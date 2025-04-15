using DB_Linkage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Model;

namespace WpfApp1.View
{
    /// <summary>
    /// AddTreatmentWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddTreatmentWindow : Window
    {
        private readonly MySQLManager _dbManager;

        private bool _isEditMode = false;
        private int _treatmentId;

        public AddTreatmentWindow()
        {
            InitializeComponent();
            _dbManager = new MySQLManager();

            // 의사와 환자 목록 로드
            LoadDoctors();
            LoadPatients();
        }

        // 수정 모드 초기화
        public void SetInitialData(Treatment treatment)
        {
            _isEditMode = true;
            _treatmentId = treatment.Id;

            // 기존 데이터 설정
            dpDate.SelectedDate = DateTime.Parse(treatment.Date);
            cbDoctors.SelectedValue = treatment.DoctorId;
            cbPatients.SelectedValue = treatment.PatientId;
            chkComplete.IsChecked = treatment.Complete;
        }

        private void LoadDoctors()
        {
            cbDoctors.ItemsSource = _dbManager.GetDoctors();
        }

        private void LoadPatients()
        {
            cbPatients.ItemsSource = _dbManager.GetPatients();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 입력값 유효성 검사
            if (!dpDate.SelectedDate.HasValue)
            {
                MessageBox.Show("진료 날짜를 선택해주세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cbTime.SelectedItem == null)
            {
                MessageBox.Show("진료 시간을 선택해주세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cbDoctors.SelectedItem == null)
            {
                MessageBox.Show("담당 의사를 선택해주세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cbPatients.SelectedItem == null)
            {
                MessageBox.Show("환자를 선택해주세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 진료 날짜와 시간 결합
            DateTime selectedDate = dpDate.SelectedDate.Value;
            string selectedTime = (cbTime.SelectedItem as ComboBoxItem).Content.ToString();
            DateTime treatmentDateTime = DateTime.Parse($"{selectedDate.ToString("yyyy-MM-dd")} {selectedTime}");

            // 새 진료 데이터 생성
            Treatment newTreatment = new Treatment
            {
                Id = _treatmentId,
                Date = treatmentDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                DoctorId = (int)cbDoctors.SelectedValue,
                PatientId = (int)cbPatients.SelectedValue,
                Complete = chkComplete.IsChecked ?? false
            };

            // 데이터베이스에 저장
            try
            {
                bool success;

                if (_isEditMode)
                    success = _dbManager.UpdateTreatment(newTreatment); // 수정
                else
                    success = _dbManager.AddTreatment(newTreatment); // 추가

                if (success)
                {
                    MessageBox.Show("저장되었습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("저장에 실패했습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
