namespace RstInvent.Model
{
    internal class NomenclatureGroup
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public NomenclatureGroup(string name, int count)
        {
            Name = name;
            Count = count;
        }
    }

    internal enum EntityType
    {
        Receiver,
        Shipment
    }
}