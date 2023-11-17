using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using ICal;

namespace ReflectDemo
{
    public partial class FrmCal : Form
    {
        public FrmCal()
        {
            InitializeComponent();
        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            //ICalculator objCal = Assembly.LoadFrom("CalDLL.dll").CreateInstance("");
        }
    }
}
