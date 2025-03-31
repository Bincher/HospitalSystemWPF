using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfApp1.Models
{
    public class Doctor : ObservableObject
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        private string? _name;
        public string? Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        private string? _department;
        public string? Department
        {
            get { return _department; }
            set
            {
                SetProperty(ref _department, value);
            }
        }

        private DateTime? _birth;
        public DateTime? Birth
        {
            get { return _birth; }
            set
            {
                SetProperty(ref _birth, value);
            }
        }

        private int _gender;
        public int Gender
        {
            get { return _gender; }
            set
            {
                SetProperty(ref _gender, value);
            }
        }

        private string? _profileImage;
        public string? ProfileImage
        {
            get { return _profileImage; }
            set { SetProperty(ref _profileImage, value); }
        }
    }
}
