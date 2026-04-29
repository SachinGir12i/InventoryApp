using InventoryApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryApp.Domain.Entities
{
    public class PartyCategory : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Party> Parties { get; set; } = new List<Party>();
        public Enums.PartyType Type { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
