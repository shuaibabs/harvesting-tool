using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScholarCounter
{
    class GTUtility
    {
        public string  SaveDataToFile(string FilePath, string FileName, string data)
        {
            string Overright = "";
            try
            {
                if(data == "Error")
                {
                    // Nothing
                }
                else
                {
                    System.IO.Directory.CreateDirectory(FilePath);
                    string pathString = System.IO.Path.Combine(FilePath, FileName);
                    if (!System.IO.File.Exists(pathString))
                    {
                        using (System.IO.FileStream fs = System.IO.File.Create(pathString))
                        {
                            for (byte i = 0; i < 100; i++)
                            {
                                fs.WriteByte(i);
                            }
                        }
                        TextWriter txt = new StreamWriter(pathString);
                        txt.Write(data);
                        txt.Close();
                    }
                    else
                    {
                        Overright = "Overrited";
                        return Overright;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SaveDataToFile: " + ex.Message);
            }
            return Overright;
        }
    }
}
