﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TAMS_V2.Modules
{
    public partial class ucChecckDoc : UserControl
    {
        private static ucChecckDoc _instance;
        public static ucChecckDoc Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new ucChecckDoc();
                }
                return _instance;
            }
        }
        public ucChecckDoc()
        {
            InitializeComponent();
        }
    }
}
