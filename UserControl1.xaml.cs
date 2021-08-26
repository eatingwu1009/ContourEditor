using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VMS.TPS.Common.Model.API;

namespace V1
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public ObservableCollection<NewStructure> NewStructures { get; set; }
        public List<string> StructureList { get; set; }
        public List<String> TypeList { get; set; }
        public string SelectedStructureId { get; set; }
        public UserControl1(ScriptContext scriptContext)
        {
            NewStructures = new ObservableCollection<NewStructure>();
            StructureList = new List<string>();
            foreach (var structure in scriptContext.StructureSet.Structures)
            {
                StructureList.Add(structure.Id);
            }
            TypeList = new List<String>();
            TypeList.Add("CTV");
            TypeList.Add("GTV");
            TypeList.Add("PTV");
            TypeList.Add("EXTERNAL");
            TypeList.Add("ORGAN");
            TypeList.Add("AVOIDANCE");
            TypeList.Add("CAVITY");
            TypeList.Add("CONTRAST_AGENT");
            TypeList.Add("IRRAD_VOLUME");
            TypeList.Add("TREATED_VOLUME");
            TypeList.Add("FIXATION");
            TypeList.Add("CONTROL");
            TypeList.Add("DOSE_REGION");
            TypeList.Add("SUPPORTt");

            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click_AddStructure(object sender, RoutedEventArgs e)
        {
            NewStructures.Add(new NewStructure() { StructureId = SelectedStructureId });
        }
    }
}