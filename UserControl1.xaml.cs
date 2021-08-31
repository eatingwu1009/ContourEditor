using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
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
        public List<String> StructureList { get; set; }
        public List<String> TypeList { get; set; }
        public List<String> TargetName { get; set; }
        public List<String> TargetType { get; set; }
        public int SelectedDeleteStructureIndex { get; set; }
        public int SelectedAddStructureIndex { get; set; }
        public StructureSet ss { get; set; }
        public ObservableCollection<String> IsSelected { get; set; }
        public UserControl1(ScriptContext scriptContext)
        {
            NewStructures = new ObservableCollection<NewStructure>();
            StructureList = new List<String>();
            foreach (var structure in scriptContext.StructureSet.Structures)
            {
                StructureList.Add(structure.Id);
            }
            TypeList = new List<String>();
            TypeList.Add("GTV");
            TypeList.Add("CTV");
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
            TypeList.Add("SUPPORT");

            ss = scriptContext.StructureSet;

            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click_AddStructure(object sender, RoutedEventArgs e)
        {
            NewStructures.Add(new NewStructure() { StructureId = "" });
        }
        private void Button_Click_DeleteStructure(object sender, RoutedEventArgs e)
        {
            if (0 <= SelectedAddStructureIndex && SelectedAddStructureIndex < NewStructures.Count())
                NewStructures.Remove(NewStructures.Where(i => i.StructureId == NewStructures[SelectedAddStructureIndex].StructureId).FirstOrDefault());
        }
        private void Button_Click_NEXT(object sender, RoutedEventArgs e)
        {
            string TargetName = string.Empty;
            foreach (var structure in ss.Structures)
            {
                switch (structure.DicomType)
                {
                    case "GTV":
                    case "CTV":
                    case "PTV":
                        TargetName += ("\n" + structure.Id); TargetName += (" \t Type :  " + structure.DicomType);
                        break;
                }
            }

            string msg = string.Format("The following DELETE Structures are TARGET, Still Continue? {0}", TargetName);
            MessageBoxResult Result = MessageBox.Show(msg, "DoubleCheck", MessageBoxButton.OKCancel);
            if (Result == MessageBoxResult.OK)
            {
                ss.Patient.BeginModifications();
                ss.AddStructure("CONTROL", "PTV +5");
            }
        }

         
    }
}