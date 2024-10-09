using RstInvent.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RstInvent.ViewModel
{
    internal class AppViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NomenclatureEntity> Nomenclatures { get; set; }

        private List<NomenclatureGroup> _receiverGroups = new List<NomenclatureGroup>();
        private List<NomenclatureGroup> _shipmentGroups = new List<NomenclatureGroup>();

        public List<NomenclatureGroup> ReceiverGroups
        {
            get => _receiverGroups;
            set
            {
                _receiverGroups = value;
                OnPropertyChanged(nameof(ReceiverGroups));
            }
        }

        public List<NomenclatureGroup> ShipmentGroups
        {
            get => _shipmentGroups;
            set
            {
                _shipmentGroups = value;
                OnPropertyChanged(nameof(ShipmentGroups));
            }
        }

        private Dictionary<string, int> _receiverNomenclatureCountDict { get; set; } = new Dictionary<string, int>();
        private Dictionary<string, int> _shipmentNomenclatureCountDict { get; set; } = new Dictionary<string, int>();

        RelayCommand _addNum;

        public AppViewModel()
        {
            var entites = CsvUtil.GetNomenclatureEntities();
            Nomenclatures = entites != null
                ? new ObservableCollection<NomenclatureEntity>(entites)
                : new ObservableCollection<NomenclatureEntity>();
        }

        public RelayCommand AddNum => _addNum ?? (_addNum = new RelayCommand(obj =>
        {
            var name = obj as string;
            if (string.IsNullOrWhiteSpace(name))
                return;

            string id = GeneratorId.Generate();
            Nomenclatures.Add(new NomenclatureEntity() { Id = id, Name = name });
            CsvUtil.SaveNomenclatureEntities(Nomenclatures.ToList());
        }));

        public RelayCommand GetReceiverNum => new RelayCommand(obj =>
        {
            string searchString = obj as string;
            if (string.IsNullOrWhiteSpace(searchString)) return;

            UpdateTables(searchString, EntityType.Receiver);
        });

        public RelayCommand GetShipmentNum => new RelayCommand(obj =>
        {
            string searchString = obj as string;
            if (string.IsNullOrWhiteSpace(searchString)) return;

            UpdateTables(searchString, EntityType.Shipment);
        });

        private void UpdateTables(string search, EntityType entityType)
        {
            string[] ids = search.Split(' ').Select(id => id.ToLower()).ToArray();
            Dictionary<string, int> current;
            Dictionary<string, int> other;

            if (entityType == EntityType.Receiver)
            {
                current = _receiverNomenclatureCountDict;
                other = _shipmentNomenclatureCountDict;
            }
            else
            {
                current = _shipmentNomenclatureCountDict;
                other = _receiverNomenclatureCountDict;
            }

            foreach (var entity in Nomenclatures.Where(n => ids.Contains(n.Id.ToLower())))
            {
                if (other.ContainsKey(entity.Name))
                {
                    other[entity.Name]--;
                    if (other[entity.Name] == 0)
                        other.Remove(entity.Name);
                }

                if (current.ContainsKey(entity.Name))
                    current[entity.Name]++;

                else
                    current.Add(entity.Name, 1);
            }

            ReceiverGroups =
                _receiverNomenclatureCountDict.Select(pair => new NomenclatureGroup(pair.Key, pair.Value)).ToList();
            ShipmentGroups =
                _shipmentNomenclatureCountDict.Select(pair => new NomenclatureGroup(pair.Key, pair.Value)).ToList();
        }

        public RelayCommand Clear => new RelayCommand(obj =>
        {
            _receiverNomenclatureCountDict.Clear();
            _shipmentNomenclatureCountDict.Clear();
            ShipmentGroups = null;
            ReceiverGroups = null;
        });

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}