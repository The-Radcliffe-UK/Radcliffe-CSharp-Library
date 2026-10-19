using Radcliffe.Core.FileSystem.Files;
using System.Collections.Generic;
using System.IO;

namespace Radcliffe.Core.FileSystem.Directories {
    public class Directory {

        public string name { get { return _name; } }

        private string _name = "";
        private DirectoryInfo? _fsInfo = null;
        private Directory? _parent = null;
        private string _fsPath = "";
        private List<Directory> _subDirectories = new List<Directory>();
        private List<BaseFile> _files = new List<BaseFile>();

        public Directory(string name, string parentPath, out bool success, bool createIfNotExist = false)
        {
            _name = name;
            if (parentPath.EndsWith("/") || parentPath.EndsWith(@"\"))
            {
                _fsPath = parentPath + name;
            }
            else
            {
                _fsPath = parentPath + "/" + name;
            }
            if (!System.IO.Directory.Exists(_fsPath))
            {
                if (createIfNotExist)
                {
                    System.IO.Directory.CreateDirectory(_fsPath);
                }
                else
                {
                    success = false;
                    return;
                }
            }
            _fsInfo = new DirectoryInfo(_fsPath);
            success = true;
        }
    }
}
