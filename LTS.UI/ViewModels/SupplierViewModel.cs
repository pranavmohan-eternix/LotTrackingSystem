using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using LTS.Application.Services;
using LTS.Common.Models;
using LTS.UI.Helpers;

namespace LTS.UI.ViewModels
{
    public class SupplierViewModel :
        INotifyPropertyChanged
    {
        private readonly SupplierService _service;

        public event PropertyChangedEventHandler?
            PropertyChanged;

        public string SupplierName { get; set; }

        public string ContactPerson { get; set; }

        public string Email { get; set; }

        public ObservableCollection<Supplier>
            Suppliers
        { get; set; }

        public Supplier SelectedSupplier { get; set; }

        public ICommand SaveSupplierCommand { get; }

        public ICommand DeleteSupplierCommand { get; }

        public SupplierViewModel(
            SupplierService service)
        {
            _service = service;

            SaveSupplierCommand =
                new RelayCommand(SaveSupplier);

            DeleteSupplierCommand =
                new RelayCommand(DeleteSupplier);

            LoadSuppliers();
        }

        private void SaveSupplier()
        {
            try
            {
                var supplier = new Supplier
                {
                    SupplierName = SupplierName,
                    ContactPerson = ContactPerson,
                    Email = Email,
                    AddedDate =
                        DateTime.Now.ToString("dd-MM-yyyy")
                };

                _service.AddSupplier(supplier);

                MessageBox.Show(
                    "Supplier saved successfully");

                SupplierName = "";
                ContactPerson = "";
                Email = "";

                OnPropertyChanged(nameof(SupplierName));
                OnPropertyChanged(nameof(ContactPerson));
                OnPropertyChanged(nameof(Email));

                LoadSuppliers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteSupplier()
        {
            try
            {
                if (SelectedSupplier == null)
                {
                    MessageBox.Show(
                        "Select supplier first");

                    return;
                }

                var result = MessageBox.Show(
                    "Delete this supplier?",
                    "Confirm",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _service.DeleteSupplier(
                        SelectedSupplier.SupplierId);

                    LoadSuppliers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadSuppliers()
        {
            var suppliers =
                _service.GetSuppliers();

            Suppliers =
                new ObservableCollection<Supplier>(
                    suppliers);

            OnPropertyChanged(nameof(Suppliers));
        }

        private void OnPropertyChanged(
            [CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(name));
        }
    }
}