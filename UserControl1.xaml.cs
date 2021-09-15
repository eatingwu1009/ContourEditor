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
        public ObservableCollection<DeleteStructure> DeleteStructures { get; set; }
        public List<String> TypeList { get; set; }
        public List<String> TargetName { get; set; }
        public List<String> TargetType { get; set; }
        public List<String> Common { get; set; }
        public int SelectedDeleteStructureIndex { get; set; }
        public int SelectedAddStructureIndex { get; set; }
        public string DefaultPath { get; set; }
        public StructureSet ss { get; set; }
        public UserControl1(ScriptContext scriptContext)
        {
            NewStructures = new ObservableCollection<NewStructure>();
            DeleteStructures = new ObservableCollection<DeleteStructure>();
            foreach (var structure in scriptContext.StructureSet.Structures)
            {
                DeleteStructures.Add(new DeleteStructure(structure.Id));
            }
            //Common = new List<String>();
            //foreach (DeleteStructure deleteStructure in DeleteStructures)
            //{
            //    if (deleteStructure.IsSelected) Common.Add(deleteStructure.StructureId);
            //}

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

            string DefaultPath = "\\Desktop\\" + scriptContext.Patient.Name + "_" + scriptContext.Course.Id + ".csv";

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

            string msg = string.Format("The following DELETE Structures are TARGET, Still Continue? {0}", TargetName);
            MessageBoxResult Result = MessageBox.Show(msg, "DoubleCheck", MessageBoxButton.OKCancel);
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
            }
        }
        private void Button_Click_SAVE(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Produced by Eatingwu©");
            
            //FileInfo fi = new FileInfo(DefaultPath);
            //if (!fi.Directory.Exists)
            //{
            //    fi.Directory.Create();
            //}
            //FileStream fs = new FileStream(DefaultPath, FileMode.Create);
            //StreamWriter sw = new StreamWriter(fs);
            //string data = string.Empty; 

            //for (i = 0; i < numberofitem; i++)
            //{
            //    data = label[i].Content + "," + Box[i].SelectedItem + "," + TypeBox[i].SelectedItem +
            //    "," + emptyboxdose[i].Text + "," + emptyboxcriteria[i].Text + "," + result[i].Content + "\n";
            //    sw.Write(data);
            //}
            //sw.Close();
            //fs.Close();
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
            MessageBox.Show("❤️ To Be Continued ❤️");
            //string filename = @DefaultPath;

            ////Load template
            //if (File.Exists(filename))
            //{
            //    //Clear the main window before load the new template
            //    DELETEListBox.Items.Clear();
            //    ADDListBox.Items.Clear();

            //    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            //    StreamReader sr = new StreamReader(fs);
            //    string strLine = string.Empty;
            //    string[] aryLine = null;
            //    int j = 0;
            //    while ((strLine = sr.ReadLine()) != null)
            //    {
            //        aryLine = strLine.Split(',');
            //        int numberofitem = j + 1;
            //        updatelookupadd();
            //        Box[j].SelectedItem = aryLine[1];
            //        TypeBox[j].SelectedItem = aryLine[2];
            //        emptyboxdose[j].Text = aryLine[3];
            //        emptyboxcriteria[j].Text = aryLine[4];

            //        bool existornot = CheckStructureIsExist(Convert.ToString(aryLine[1]));
            //        if (existornot == false)
            //        {
            //            Box[j].Items.Add(Convert.ToString(aryLine[1]));

            //        }
            //        if (existornot == false && Convert.ToString(aryLine[1]) != "")
            //        {
            //            Box[j].Foreground = Brushes.Red;
            //        }
            //        j++;

            //    }
            //    sr.Close();
            //    fs.Close();
            //    scroll.Maximum = numberofitem;
            //    CheckDoseBoxIsEditable();
            //}
        }
    }
}