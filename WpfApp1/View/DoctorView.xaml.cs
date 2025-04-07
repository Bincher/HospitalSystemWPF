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
    }
}
