using DB_Linkage.Service;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using WpfApp1.Models;

namespace WpfApp1.View
{
    /// <summary>
    /// AddPatientWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddPatientWindow : Window
    {
        private string selectedImagePath;
        private readonly MySQLManager _dbManager;
        private bool _isEditMode = false; // 수정 모드 여부
        private int _patientId; // 수정할 환자의 ID

        public Patient NewPatient { get; private set; }

        public AddPatientWindow()
        {
            InitializeComponent();
            _dbManager = new MySQLManager();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // 파일 선택 대화상자 표시
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "이미지 파일|*.jpg;*.jpeg;*.png;*.gif|모든 파일|*.*",
                Title = "프로필 이미지 선택"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                txtImagePath.Text = selectedImagePath;

                // 이미지 미리보기 표시
                try
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(selectedImagePath));
                    imgPreview.Source = bitmap;
                    borderPreview.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"이미지 로드 중 오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 입력값 유효성 검사
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("이름을 입력해주세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!dpBirth.SelectedDate.HasValue)
            {
                MessageBox.Show("생년월일을 선택해주세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 프로필 이미지 저장 경로 설정
            string destinationImagePath = null;
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                string fileName = System.IO.Path.GetFileName(selectedImagePath);
                string destFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                // 폴더가 없으면 생성
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                destinationImagePath = System.IO.Path.Combine(destFolder, fileName);

                // 이미지 파일 복사
                try
                {
                    File.Copy(selectedImagePath, destinationImagePath, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"이미지 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // 새 환자 객체 생성
            NewPatient = new Patient
            {
                Id = _patientId,
                Name = txtName.Text,
                Birth = dpBirth.SelectedDate.Value,
                Gender = rbMale.IsChecked == true ? 1 : 0,
                ProfileImage = destinationImagePath
            };

            // 데이터베이스에 저장
            try
            {
                bool success;

                if (_isEditMode)
                {
                    // 수정 모드: 데이터 업데이트
                    success = _dbManager.UpdatePatient(NewPatient);
                    if (success)
                        MessageBox.Show("환자 정보가 성공적으로 수정되었습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("환자 정보 수정에 실패했습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // 추가 모드: 데이터 삽입
                    success = _dbManager.AddPatient(NewPatient);
                    if (success)
                        MessageBox.Show("환자 정보가 성공적으로 추가되었습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("환자 정보 추가에 실패했습니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (success)
                    DialogResult = true;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 기존 데이터를 설정하는 메서드 (수정 모드 활성화)
        public void SetInitialData(Patient patient)
        {
            _isEditMode = true; // 수정 모드 활성화
            _patientId = patient.Id; // 수정할 환자의 ID 저장

            txtName.Text = patient.Name;
            dpBirth.SelectedDate = patient.Birth;

            if (patient.Gender == 1)
                rbMale.IsChecked = true;
            else
                rbFemale.IsChecked = true;

            txtImagePath.Text = patient.ProfileImage;

            if (!string.IsNullOrEmpty(patient.ProfileImage))
            {
                imgPreview.Source = new BitmapImage(new Uri(patient.ProfileImage));
                borderPreview.Visibility = Visibility.Visible;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
