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
    public class WaferViewModel :
        INotifyPropertyChanged
    {
        private readonly WaferService _service;

        public event PropertyChangedEventHandler?
            PropertyChanged;

        // ══════════════════════════════════════
        // COLLECTIONS
        // ══════════════════════════════════════

        public ObservableCollection<Wafer>
            Wafers
        { get; set; }

        public ObservableCollection<Supplier>
            Suppliers
        { get; set; }

        // ══════════════════════════════════════
        // FILTER
        // ══════════════════════════════════════

        private bool _showUnallocatedOnly;

        public bool ShowUnallocatedOnly
        {
            get => _showUnallocatedOnly;

            set
            {
                _showUnallocatedOnly = value;

                OnPropertyChanged();

                LoadWafers();
            }
        }

        // ══════════════════════════════════════
        // PREFIX
        // ══════════════════════════════════════

        private string _prefix = "";

        public string Prefix
        {
            get => _prefix;

            set
            {
                _prefix = value;

                OnPropertyChanged();
            }
        }

        // ══════════════════════════════════════
        // GENERATE COUNT
        // ══════════════════════════════════════

        private int _generateCount = 25;

        public int GenerateCount
        {
            get => _generateCount;

            set
            {
                _generateCount = value;

                OnPropertyChanged();
            }
        }

        // ══════════════════════════════════════
        // SELECTED SUPPLIER
        // ══════════════════════════════════════

        private Supplier? _selectedSupplier;

        public Supplier? SelectedSupplier
        {
            get => _selectedSupplier;

            set
            {
                _selectedSupplier = value;

                OnPropertyChanged();
            }
        }

        // ══════════════════════════════════════
        // SELECTED WAFER
        // ══════════════════════════════════════

        private Wafer? _selectedWafer;

        public Wafer? SelectedWafer
        {
            get => _selectedWafer;

            set
            {
                _selectedWafer = value;

                OnPropertyChanged();
            }
        }

        // ══════════════════════════════════════
        // COMMANDS
        // ══════════════════════════════════════

        public ICommand GenerateWafersCommand { get; }

        public ICommand DeleteWaferCommand { get; }

        // ══════════════════════════════════════
        // CONSTRUCTOR
        // ══════════════════════════════════════

        public WaferViewModel(
            WaferService service,
            List<Supplier> suppliers)
        {
            _service = service;

            Suppliers =
                new ObservableCollection<Supplier>(
                    suppliers);

            GenerateWafersCommand =
                new RelayCommand(GenerateWafers);

            DeleteWaferCommand =
                new RelayCommand(DeleteWafer);

            LoadWafers();
        }

        // ══════════════════════════════════════
        // LOAD WAFERS
        // ══════════════════════════════════════

        private void LoadWafers()
        {
            var wafers =
                _service.GetWafers();

            // FILTER

            if (ShowUnallocatedOnly)
            {
                wafers =
                    wafers
                    .Where(x =>
                        !x.LotId.HasValue)
                    .ToList();
            }

            Wafers =
                new ObservableCollection<Wafer>(
                    wafers);

            OnPropertyChanged(nameof(Wafers));
        }

        // ══════════════════════════════════════
        // GENERATE WAFERS
        // ══════════════════════════════════════

        private void GenerateWafers()
        {
            try
            {
                // VALIDATION

                if (SelectedSupplier == null)
                {
                    MessageBox.Show(
                        "Select supplier");

                    return;
                }

                if (string.IsNullOrWhiteSpace(Prefix))
                {
                    MessageBox.Show(
                        "Enter prefix");

                    return;
                }

                if (GenerateCount <= 0)
                {
                    MessageBox.Show(
                        "Invalid count");

                    return;
                }

                int createdCount = 0;

                // GENERATE LOOP

                for (int i = 1;
                     i <= GenerateCount;
                     i++)
                {
                    string serial =
                        $"{Prefix}-{i:D3}";

                    var wafer =
                        new Wafer
                        {
                            WaferSerialNo =
                                serial,

                            SupplierId =
                                SelectedSupplier.SupplierId
                        };

                    try
                    {
                        _service.AddWafer(wafer);

                        createdCount++;
                    }
                    catch
                    {
                        // SKIP DUPLICATES
                    }
                }

                MessageBox.Show(
                    $"{createdCount} wafers generated");

                LoadWafers();
                Prefix = "";

                GenerateCount = 0;

                SelectedSupplier = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        // ══════════════════════════════════════
        // DELETE WAFER
        // ══════════════════════════════════════

        private void DeleteWafer()
        {
            try
            {
                if (SelectedWafer == null)
                {
                    MessageBox.Show(
                        "Select wafer");

                    return;
                }

                _service.DeleteWafer(
                    SelectedWafer.WaferId);

                MessageBox.Show(
                    "Wafer deleted");

                LoadWafers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        // ══════════════════════════════════════
        // PROPERTY CHANGED
        // ══════════════════════════════════════

        private void OnPropertyChanged(
            [CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(name));
        }
    }
}