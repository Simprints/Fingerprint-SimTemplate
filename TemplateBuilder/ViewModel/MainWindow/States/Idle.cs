﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateBuilder.Model;

namespace TemplateBuilder.ViewModel.MainWindow
{
    public partial class TemplateBuilderViewModel
    {
        public class Idle : TemplateBuilderBaseState
        {
            public Idle(TemplateBuilderViewModel outer) : base(outer)
            { }

            public override void LoadFile()
            {
                TransitionTo(typeof(Loading));
            }

            public override void SetScannerType(ScannerType type)
            {
                // Ignore.
            }
        }
    }
}
