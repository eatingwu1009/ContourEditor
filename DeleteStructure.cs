using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V1
{
    public class DeleteStructure : INotifyPropertyChanged
    {
        private string _structureId;
        public string StructureId
        {
            get => _structureId;
            set
            {
                _structureId = value;
                RaisePropertyChanged(nameof(StructureId));
            }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public DeleteStructure(string structureId, bool isSelected = false)
        {
            StructureId = structureId;
            IsSelected = isSelected;
        }

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
