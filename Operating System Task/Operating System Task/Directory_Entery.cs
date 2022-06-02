using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

namespace Operating_System_Task
{
    public class Directory_Entery
    {
        public string Directory_Name;
        public byte dir_attr;//its size is 1 byte
        public byte[] directory_empty = new byte[12];
        public int[] dir_frist_cluster = new int[1];// 4 byte   make them arr of integer with 1 element
        public int[] dir_file_size = new int[1]; //4 byte       make them arr of integer with 1 element

        //                                      empty constrctor  
        public Directory_Entery()
        {
           

        }
        //                                      parameterize constrctor 
        public Directory_Entery(string name, byte dir_attr, int dir_firstCluster,int size)
        {
            this.dir_attr = dir_attr;
            //regular file
            name = CleanTheName(name);
            if (dir_attr == 0x0)
            {
                string[] fileName = name.Split('.');
                assignFileName(fileName[0].ToCharArray(), fileName[1].ToCharArray());
            }
            else if (dir_attr == 0x10)
            {
                assignDIRName(name.ToCharArray());
            }
            this.dir_frist_cluster[0] = dir_firstCluster;
        }
        public string CleanTheName(string s)
        {
            if (!s.Equals("R:"))
            {
                string n = string.Empty;
                foreach (char c in s)
                {
                    if (c != (' ') && c != ('?') && c != ('؟') && c != ('>') && c != ('<') && c != ('|') && c != (':') && c != ('*') && c != ('\'') && c != ('\\') && c != ('/'))
                    {
                        n += c;
                    }
                }
                return n;
            }
            else
                return s;
        }
        public void assignFileName(char[] name, char[] extension)
        {
            if (name.Length <= 7 && extension.Length <= 3)
            {
                int j = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    j++;
                    this.Directory_Name += name[i];
                }
                j++;
                this.Directory_Name += '.';
                for (int i = 0; i < extension.Length; i++)
                {
                    j++;
                    this.Directory_Name += extension[i];
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    this.Directory_Name += name[i];
                }
                this.Directory_Name += '.';
                for (int i = 0; i < extension.Length && Directory_Name.Length < 11; i++)
                {
                    this.Directory_Name += extension[i];
                }
            }
        }
        public void assignDIRName(char[] name)
        {
            if (name.Length <= 11)
            {
                int j = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    j++;
                    this.Directory_Name += name[i];
                }
                //for (int i = ++j; i < dir_name.Length; i++)
                //{
                //    this.dir_name[i] = ' ';
                //}
            }
            else
            {
                int j = 0;
                for (int i = 0; i < 11; i++)
                {
                    j++;
                    this.Directory_Name += name[i];
                }
            }
        }
    }
}