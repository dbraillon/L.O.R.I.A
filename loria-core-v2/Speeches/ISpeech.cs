﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Core.Speeches
{
    public interface ISpeech
    {
        void Tell(string brainMessage);
    }
}
