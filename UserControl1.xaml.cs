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
using System.Windows.Forms;
using VMS.TPS.Common.Model.API;
using System.Collections.Specialized;
using System.ComponentModel;

namespace V1
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : System.Windows.Controls.UserControl
    {
        public ObservableCollection<NewStructure> NewStructures { get; set; }
        public ObservableCollection<DeleteStructure> DeleteStructures { get; set; }
        public List<String> TypeList { get; set; }
        public List<String> TargetName { get; set; }
        public List<String> TargetType { get; set; }
        public List<String> Common { get; set; }
        public int SelectedDeleteStructureIndex { get; set; }
        public int SelectedAddStructureIndex { get; set; }
        public string DefaultPath { get; set; }
        public StructureSet ss { get; set; }
        private StructureSet _ss;
        public UserControl1(ScriptContext scriptContext)
        {
            ss = scriptContext.StructureSet;
            _ss = scriptContext.StructureSet;
            NewStructures = new ObservableCollection<NewStructure>();
            DeleteStructures = new ObservableCollection<DeleteStructure>();

            RefreshDeleteStructures();

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

            //string DefaultPath = "C:\\Users\\aria\\source\\repos\\test" + scriptContext.Patient.Name + "_" + scriptContext.Course.Id + ".csv";

            InitializeComponent();
            DataContext = this;
        }

        private void RefreshDeleteStructures()
        {
            DeleteStructures.Clear();
            foreach (var structure in _ss.Structures)
            {
                DeleteStructures.Add(new DeleteStructure(structure.Id));
            }
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
            //var item = StructureList.Intersect(deleteStructure.IsSelected);
            Common = new List<String>();
            foreach (DeleteStructure deleteStructure in DeleteStructures)
            {
                if (deleteStructure.IsSelected) Common.Add(deleteStructure.StructureId);
            }

            //foreach (Structure structure in ss.Structures.Where(s=>Common.Contains(s.Id)))
            var RStructures = ss.Structures.Where(s => Common.Contains(s.Id)).ToList();
            foreach (Structure structure in RStructures)
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

            string msg = string.Format(" The following DELETE Structures are TARGET, Still Continue? {0}", TargetName);
            MessageBoxResult Result = System.Windows.MessageBox.Show(msg, "DoubleCheck", MessageBoxButton.OKCancel, (MessageBoxImage)System.Windows.Forms.MessageBoxIcon.Warning);
            if (Result == MessageBoxResult.OK)
            {
                ss.Patient.BeginModifications();
                foreach (var i in NewStructures)
                {
                    ss.AddStructure(i.StructureType, i.StructureId);
                }
                foreach (Structure structure in RStructures)
                {
                    ss.RemoveStructure(structure);
                }
                NewStructures.Clear();
                RefreshDeleteStructures();
            }
        }
        private void Button_Click_SAVE(object sender, RoutedEventArgs e)
        {
            Common = new List<String>();
            foreach (DeleteStructure deleteStructure in DeleteStructures)
            {
                if (deleteStructure.IsSelected) Common.Add(deleteStructure.StructureId);
            }
            var RStructures = ss.Structures.Where(s => Common.Contains(s.Id)).ToList();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files(.csv)|*.csv";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                string DelData = string.Empty;
                string AddData = string.Empty;
                foreach (Structure i in RStructures)
                {
                    DelData += i.Id + ",";
                }
                DelData = "--------DeletedDataFromHere--------" + "\r\n" + DelData + "\r\n";
                foreach (var i in NewStructures)
                {
                    AddData += i.StructureId + "&" + i.StructureType + ",";
                }
                AddData = "----------AddDataFromHere----------" + "\r\n" + AddData;
                sw.Write(DelData);
                sw.Write(AddData);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }
        private bool CheckStructureIsExist(String StructureLoaded)
        {
            bool StructureIsExist = false;
            foreach (var StructureInPlan in ss.Structures)
            {
                if (String.Equals(StructureLoaded, StructureInPlan.Id))
                {
                    StructureIsExist = true;
                    break;
                }
            }
            return StructureIsExist;
        }
        private void Button_Click_LOAD(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog(); ;
            openFileDialog1.Filter = "txt files(.csv)|*.csv";
            openFileDialog1.RestoreDirectory = true;
            var filePath = string.Empty;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                string[] filelines = File.ReadAllLines(filePath);
                List<string> sourceDel = filelines[1].Trim().Split(',').Select(s => s.Trim()).ToList();
                foreach (DeleteStructure deleteStructure in DeleteStructures)
                {
                    if (sourceDel.Contains(deleteStructure.StructureId)) deleteStructure.IsSelected = true;
                }

                string sourceAdd = filelines[3].Trim();
                List<string> Add = (filelines[3].Trim()).Split(',').ToList();
                for (int a = 0; a < sourceAdd.Count(f => f == ','); a++)
                {
                    string[] b = Add[a].Split('&');
                    NewStructures.Add(new NewStructure() { StructureId = b[0], StructureType = b[1] });
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (DeleteStructure deleteStructure in DeleteStructures)
            {
                deleteStructure.IsSelected = true;
            }
        }
    }
}