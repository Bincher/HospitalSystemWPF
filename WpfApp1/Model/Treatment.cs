using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Treatment : ObservableObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _doctorId;
        public int DoctorId
        {
            get => _doctorId;
            set => SetProperty(ref _doctorId, value);
        }

        private int _patientId;
        public int PatientId
        {
            get => _patientId;
            set => SetProperty(ref _patientId, value);
        }

        private string? _date;
        public string? Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private string? _doctorDepartment;
        public string? DoctorDepartment
        {
            get => _doctorDepartment;
            set => SetProperty(ref _doctorDepartment, value);
        }

        private string? _doctorName;
        public string? DoctorName
        {
            get => _doctorName;
            set => SetProperty(ref _doctorName, value);
        }

        private string? _patientName;
        public string? PatientName
        {
            get => _patientName;
            set => SetProperty(ref _patientName, value);
        }

        private bool _complete;
        public bool Complete
        {
            get => _complete;
            set => SetProperty(ref _complete, value);
        }
    }

}
