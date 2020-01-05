using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoltAnalyzer.Utils.Controls
{
    public interface IWindow
    {
        void Close();
        void Minimize();
        void Expand();
        void Move();
    }
}
