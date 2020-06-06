using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackUpSoftware.ViewModel
{
    public class FolderViewModel
    {
        public User User { get; set; }
        public string Path { get; set; }
        public string[]  Folders{ get; set; }
        public string[] Files { get; set; }

    }
}