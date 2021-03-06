﻿using Loria.Dal.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Skills
{
    public delegate void OnFirstLaunchHandler();
    public delegate void OnMainLaunchHandler();

    public class SkillAction : Loria.Dal.Entities.Action
    {
        public event OnFirstLaunchHandler OnFirstLaunch;
        public event OnFirstLaunchHandler OnMainLaunch;

        public IList<string> Stimulus { get; set; }
        public FileInfo ActionFile { get; set; }

        public void FirstLaunch()
        {
            if (OnFirstLaunch != null) OnFirstLaunch();
        }

        public void MainLaunch()
        {
            if (OnMainLaunch != null) OnMainLaunch();
        }
    }
}
