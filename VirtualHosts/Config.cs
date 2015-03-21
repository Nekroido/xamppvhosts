using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualHosts
{
    public class Config
    {
        public static string serverPath = "D:\\Server\\";

        public static string homeDir = "home";

        public static string HomeDir
        {
            get
            {
                return serverPath + homeDir;
            }
        }

        public static string XamppDir
        {
            get
            {
                return serverPath;
            }
        }

        public static string VhostsFile
        {
            get
            {
                return serverPath + "\\apache\\conf\\vhosts.cnf";
            }
        }

        public static string HostsFile
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\System32\\drivers\\etc\\hosts";
            }
        }

        public static string defaultHostDir = "public_html";

        public static string defaultTemplate = "<VirtualHost 127.0.0.1:80>\n\tDocumentRoot\t\"{1}\"\n\tServerName\t\"{0}\"\n\tServerAlias\t\"{0}\" \"www.{0}\"\n</VirtualHost>";
    }
}
