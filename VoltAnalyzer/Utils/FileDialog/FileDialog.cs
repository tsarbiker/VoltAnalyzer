using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoltAnalyzer.Utils.FileDialog
{
    public sealed class FileDialog
    {
        public Stream OpenFile(string defaultExtension, string filter)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = defaultExtension;
            fd.Filter = filter;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                return fd.OpenFile();
            }

            return null;
        }

        public Stream SaveFile(string defaultExtension, string filter)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.DefaultExt = defaultExtension;
            fd.Filter = filter;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                return fd.OpenFile();
            }

            return null;
        }

        public string BrowseFolder()
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                return fd.SelectedPath;
            }

            return "";
        }
    }
}
