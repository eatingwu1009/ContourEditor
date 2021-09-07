using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V1
{
    public class DeleteStructure
    {
        public string StructureId { get; set; }
        public bool IsSelected { get; set; }

        public DeleteStructure(string structureId, bool isSelected = false)
        {
            StructureId = structureId;
            IsSelected = isSelected;
        }
    }
}
