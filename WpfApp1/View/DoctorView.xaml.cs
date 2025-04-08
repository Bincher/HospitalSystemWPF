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
    /// DoctorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DoctorView : Window
    {
        public DoctorView()
        {
            InitializeComponent();
        }
        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // 창 닫기
        }
        private void AddDoctorClick(object sender, RoutedEventArgs e)
        {
            AddDoctorWindow addDoctorWindow = new AddDoctorWindow();

            if (addDoctorWindow.ShowDialog() == true)
            {
                // 새 의사 추가 후 목록 새로고침
                LoadDoctors();
            }
        }

        private void LoadDoctors()
        {
            // DoctorViewModel이 있다면 해당 메서드 호출
            var viewModel = DataContext as DoctorViewModel;
            if (viewModel != null)
            {
                viewModel.LoadDoctorData();
            }
        }


    }
}
