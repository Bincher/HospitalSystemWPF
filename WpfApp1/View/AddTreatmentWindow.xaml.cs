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

        public AddTreatmentWindow()
        {
            InitializeComponent();
            _dbManager = new MySQLManager();

            // 의사와 환자 목록 로드
            LoadDoctors();
            LoadPatients();
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
        Date = treatmentDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
        DoctorId = (int)cbDoctors.SelectedValue,
        PatientId = (int)cbPatients.SelectedValue,
        Complete = chkComplete.IsChecked ?? false
    };

            // 데이터베이스에 저장
            try
            {
                bool success = _dbManager.AddTreatment(newTreatment);

                if (success)
                {
                    MessageBox.Show("진료 정보가 성공적으로 저장되었습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("진료 정보 저장에 실패했습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
