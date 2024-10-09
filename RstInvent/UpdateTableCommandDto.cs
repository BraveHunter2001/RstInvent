using RstInvent.Model;

namespace RstInvent
{
    internal class UpdateTableCommandDto
    {
        public string SearchField { get; set; }
        public EntityType Type { get; set; }
    }
}