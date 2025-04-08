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
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    /// <summary>
    /// PatientView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PatientView : Window
    {
        public PatientView()
        {
            InitializeComponent();
        }
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // 창 닫기
        }

        private void AddPatientClick(object sender, RoutedEventArgs e)
        {
            AddPatientWindow addPatientWindow = new AddPatientWindow();

            if (addPatientWindow.ShowDialog() == true)
            {
                // 새 환자 추가 후 목록 새로고침
                LoadPatients();
            }
        }

        private void LoadPatients()
        {
            // DoctorViewModel이 있다면 해당 메서드 호출
            var viewModel = DataContext as PatientViewModel;
            if (viewModel != null)
            {
                viewModel.LoadPatientData();
            }
        }
    }
}
