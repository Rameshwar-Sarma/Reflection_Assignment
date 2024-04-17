using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using MVVMAssignment.Models;
using GalaSoft.MvvmLight.Command;
using MVVMAssignment.Data;

namespace MVVMAssignment.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeContext _context;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Employee> Employees { get; set; }

        public ICommand LoadDataCommand { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand UpdateEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public EmployeeViewModel(EmployeeContext context)
        {
            _context = context;

            LoadDataCommand = new RelayCommand(LoadData);
            AddEmployeeCommand = new RelayCommand(AddEmployee);
            UpdateEmployeeCommand = new RelayCommand(UpdateEmployee);
            DeleteEmployeeCommand = new RelayCommand(DeleteEmployee);

            Employees = new ObservableCollection<Employee>();
            LoadData();
        }

        private void LoadData()
        {
            Employees.Clear();
            var employees = _context.Employees.ToList();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }

        private void AddEmployee()
        {
            var newEmployee = new Employee { Name = "New Employee", Email = "newemployee@example.com" };
            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
            LoadData();
        }

        private void UpdateEmployee()
        {
            if (SelectedEmployee != null)
            {
                _context.Entry(SelectedEmployee).State = EntityState.Modified;
                _context.SaveChanges();
                LoadData();
            }
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee != null)
            {
                _context.Employees.Remove(SelectedEmployee);
                _context.SaveChanges();
                LoadData();
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
