using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using V1;

// TODO: Replace the following version attributes by creating AssemblyInfo.cs. You can do this in the properties of the Visual Studio project.
[assembly: AssemblyVersion("1.0.0.2")]
[assembly: AssemblyFileVersion("1.0.0.2")]
[assembly: AssemblyInformationalVersion("1.0")]

// TODO: Uncomment the following line if the script requires write access.
[assembly: ESAPIScript(IsWriteable = true)]

namespace VMS.TPS
{
    public class Script
    {
        public Script()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Execute(ScriptContext scriptContext, Window window)
        {
            // TODO : Add here the code that is called when the script is launched from Eclipse.
            UserControl1 userControl = new UserControl1(scriptContext);
            window.Content = userControl;
            window.Title = "ContourEditor";
            window.Height = 480;
            window.Width = 420;
        }

        public static void Main()
        {
            System.Windows.MessageBox.Show("This is what happens when you switch to a Windows application!");
            // We will create our UserControl1 class so that you can create it in two ways:
            // 1. Using ScriptContext (ESAPI gives this to you)
            // 2. Using an object you create yourself (e.g. in the code)
        }
    }
}
