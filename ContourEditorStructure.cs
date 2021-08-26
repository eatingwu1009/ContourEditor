using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V1
{
    public class ContourEditorStructure
    {
        public string StructureId { get; set; }
        public string StructureType { get; set; }

        public ContourEditorStructure(string structureId)
        {
            StructureId = structureId;
        }
    }
}
