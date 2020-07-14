using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Kassa1.UtilCode
{
    public class ImgDeleter
    {
        public void Remove (string pathFolder)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathFolder);

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch
                {
                }
            }
        }
    }
}