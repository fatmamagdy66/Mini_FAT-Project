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
    public class Directory : Directory_Entery
    {

        public List<Directory_Entery> directory_File;
        public Directory parent;
        public Directory(string Dir_Name, byte dir_atr, Directory parent)
        {
            this.Directory_Name = Dir_Name;
            this.dir_attr = dir_atr;
            this.parent = parent;
        }

        public Directory(string Dir_Name, byte dir_atr, int dir_frst_cluster, Directory parent)
        {
            this.Directory_Name = Dir_Name;
            this.dir_attr = dir_atr;
            this.dir_frist_cluster[0] = dir_frst_cluster;
            this.parent = parent;
            if (this.dir_attr == 0)
                this.dir_file_size[0] = 0;
            
            directory_File = new List<Directory_Entery>();
        }
        public void Rename_Dir(string Old_Name, string New_Name)
        {

            int index_of_old_dir = search_about_Directory(Old_Name);
            Console.WriteLine(index_of_old_dir);
            if (index_of_old_dir != -1)
            {
                directory_File[index_of_old_dir].Directory_Name = New_Name;
                write_Directory();
                Mini_Fat.Write_Fat();
            }
            else Console.WriteLine($"error : cannot find the file which called {Old_Name}. ");

        }
        public byte[] Convert_dirEntery_to_byte(Directory_Entery file_or_directory_information)
        {

            string s = file_or_directory_information.Directory_Name;
            // convert  char to byte 


            byte[] bytes = Encoding.ASCII.GetBytes(s);
            int r = 0;
            byte[] b = new byte[32];
            if (file_or_directory_information.Directory_Name.Length == 11)
            {
                for (int i = 0; i < file_or_directory_information.Directory_Name.Length; i++)
                {
                    b[r] = bytes[i];

                    r++;
                }
            }
            else if (file_or_directory_information.Directory_Name.Length > 11)
            {
                for (int i = 0; i < 11; i++)
                {
                    b[r] = bytes[i];

                    // convert each char to byte 
                    r++;
                }
            }
            else if (file_or_directory_information.Directory_Name.Length < 11)
            {
                for (int i = 0; i < file_or_directory_information.Directory_Name.Length; i++)
                {
                    // convert each char to byte 
                    b[r] = bytes[i];
                    r++;
                }
                for (int i = 0; i < 11 - file_or_directory_information.Directory_Name.Length; i++)
                {
                    b[r] = Convert.ToByte(null);
                    // convert each char to byte 
                    r++;
                }

            }
            // BitConverter.GetBytes(r).CopyTo(b, 0);
            b[r] = file_or_directory_information.dir_attr;
            r++;
            for (int i = 0; i < 12; i++)
            {
                b[r] = file_or_directory_information.directory_empty[i];
                r++;
            }
            // convert dir_frist_cluster
            System.Buffer.BlockCopy(file_or_directory_information.dir_frist_cluster, 0, b, 24, 4);
            // convert dir_file_size
            System.Buffer.BlockCopy(file_or_directory_information.dir_file_size, 0, b, 28, 4);


            // another way
            //byte[] fc = BitConverter.GetBytes(dir_frist_cluster);
            //for (int i = 0; i < fc.Length ; i++)
            //{
            //    b[r] = fc[i];
            //    r++;
            //}

            //byte[] size = BitConverter.GetBytes(dir_file_size);
            //for (int i =  0; i < size.Length; i++)
            //{
            //    b[r] = size[i];
            //}
            return b;
        }

        public Directory_Entery Convert_byte_to_dirEntery(byte[] byte32)
        {
            if (byte32[0] == 0)
            {
                return null;
            }
            byte[] nameByte = new byte[11];

            int r = 0;
            for (int i = 0; i < 11; i++)
            {
                nameByte[i] = byte32[i];
                r++;
            }
            Directory_Entery d = new Directory_Entery();
            string name = Encoding.ASCII.GetString(nameByte);

            // convert each char to byte                                  //convert byte to char                       System.Text.Encoding.ChooseYourEncoding.GetString(bytes).ToCharArray();
            d.Directory_Name = name;
            d.dir_attr = byte32[r];
            r++;
            for (int i = 0; i < 12; i++)
            {
                d.directory_empty[i] = byte32[r];
                r++;
            }
            System.Buffer.BlockCopy(byte32, 24, d.dir_frist_cluster, 0, 4);
            System.Buffer.BlockCopy(byte32, 28, d.dir_file_size, 0, 4);

            return d;
        }
        public void empty_all_its_cluster()
        {
            if (this.dir_frist_cluster[0] != 0)
            {
                int cluster = this.dir_frist_cluster[0];
                int next = Mini_Fat.Get_Cluster_statu(cluster);
                if (cluster == 5 && next == 0)
                    return;
                do
                {
                    Mini_Fat.set_Cluster_statu(cluster, 0);
                    cluster = next;
                    if (cluster != -1)
                        next = Mini_Fat.Get_Cluster_statu(cluster);

                } while (cluster != -1);
            }
        }
        public Directory_Entery GetDirectory_Entery()
        {
            Directory_Entery me = new Directory_Entery(this.Directory_Name, this.dir_attr, this.dir_frist_cluster[0],0);
            me.Directory_Name = this.Directory_Name;
            me.dir_attr = this.dir_attr;
            me.dir_frist_cluster[0] = this.dir_frist_cluster[0];
            if (this.dir_attr == 0x10)
            {
                me.dir_file_size[0] = 0;
            }
            else me.dir_file_size[0] = this.dir_file_size[0];
            return me;
        }
        public void update_info(Directory_Entery old, Directory_Entery nneeww)
        {
            Read_Directory();
            int index = search_about_Directory(old.Directory_Name);
            if (index != -1)
            {
                directory_File.RemoveAt(index);
                directory_File.Insert(index, nneeww);
                write_Directory();
            }
            else Console.WriteLine($"No such Directory or file called '{old.Directory_Name}'");
        }
        public int search_about_Directory(string name)
        {
            Read_Directory();
            for (int i = 0; i < directory_File.Count; i++)
            {
                string n = directory_File[i].Directory_Name;
                if (n.Contains(name))
                    return i;
            }
            return -1;
        }
        public void remove_Directory_Entry(Directory_Entery d)
        {
            Read_Directory();
            int index = search_about_Directory(d.Directory_Name);
            if (index != -1)
            {
                directory_File.RemoveAt(index);
                write_Directory();
            }
           // else Console.WriteLine($"No such file or Directory called '{d.Directory_Name}'");

        }
        public void Add_Directory_Entry(Directory_Entery d)
        {
            if (search_about_Directory(d.Directory_Name) == -1)
                directory_File.Add(d);
            write_Directory();
        }
        public void Delet_Directoty()
        {

            if (Program.current == this)
            {
                Console.WriteLine("You can't delet this directory ,Becouse it is the current Directory.");
                return;
            }
            empty_all_its_cluster();
            if (parent != null)
            {
                this.parent.remove_Directory_Entry(GetDirectory_Entery());
            }
            Mini_Fat.Write_Fat();
        }
        public int Get_mySize_on_Disk()
        {
            int size = 0;
            if (this.dir_frist_cluster[0] != 0)
            {
                int cluster = this.dir_frist_cluster[0];
                int next = Mini_Fat.Get_Cluster_statu(cluster);
                do
                {
                    size++;
                    cluster = next;
                    if (cluster != -1)
                        next = Mini_Fat.Get_Cluster_statu(cluster);
                } while (cluster != -1);

            }
            return size;
            
        }
        public bool Can_Add_entry(Directory_Entery d)
        {
            bool can = false;
            int needed_Size = (directory_File.Count + 1) * 32;
            int needed_Cluster = needed_Size / 1024;
            int rem = needed_Size % 1024;
            if (rem > 0)
                needed_Cluster++;

            needed_Cluster += d.dir_file_size[0] / 1024;
            int rem1 = d.dir_file_size[0] % 1024;
            if (rem1 > 0)
                needed_Cluster++;
            if (Get_mySize_on_Disk() + Mini_Fat.Get_Avilable_cluster() >= needed_Cluster)
                can = true;
            return can;

        }
        public void write_Directory()
        {
            Directory_Entery old = GetDirectory_Entery();
            List<byte[]> arrayListof1024 = new List<byte[]>();
            int Frist_Cluster = this.dir_frist_cluster[0];
            int x = 0;
            byte[] b = new byte[directory_File.Count * 32];
            // byte[] temparr1 = new byte[1024];
            int last_cluster = -1, cluster_index;

            for (int i = 0; i < directory_File.Count; i++)
            {
                byte[] f = new byte[32];
                f = Convert_dirEntery_to_byte(directory_File[i]);
                for (int j = 0; j < 32; j++)
                {
                    NewMethod(x, b, f, j);
                    x++;
                }
            }
            int y = 0;
            byte[] arr1024 = new byte[1024];
            //split b to list of arrays each array of size 1024            
            for (int i = 0; i < directory_File.Count * 32; i++)
            {

                if (y % 1024 == 0 && y != 0)
                {
                    y = 0;
                    arrayListof1024.Add(arr1024);
                }
                arr1024[y] = b[i];
                if (i + 1 == directory_File.Count * 32)
                {
                    arrayListof1024.Add(arr1024);
                    break;
                }
                y++;
            }

            if (this.dir_frist_cluster[0] == 0)
            {
                cluster_index = Mini_Fat.Get_Empty_Cluster();
                this.dir_frist_cluster[0] = cluster_index;
            }
            else
            {
                //empty all its cluster
                empty_all_its_cluster();
                cluster_index = Mini_Fat.Get_Empty_Cluster();
                this.dir_frist_cluster[0] = cluster_index;
            }
            for (int i = 0; i < arrayListof1024.Count; i++)
            {
                Virtual_Disk.Write_cluster(cluster_index, arrayListof1024[i]);
                Mini_Fat.set_Cluster_statu(cluster_index, -1);
                if (last_cluster != -1)
                {
                    Mini_Fat.set_Cluster_statu(last_cluster, cluster_index);
                }
                last_cluster = cluster_index;
                cluster_index = Mini_Fat.Get_Empty_Cluster();
            }
            Directory_Entery new1 = GetDirectory_Entery();
            if (this.parent != null)
            {
                this.parent.update_info(old, new1);                                  
                this.parent.write_Directory();
            }
            Mini_Fat.Write_Fat();
        }
        private static void NewMethod(int x, byte[] b, byte[] f, int j)
        {
            b[x] = f[j];
        }
        public void Read_Directory()
        {
            if (dir_frist_cluster[0] != 0)
            {
                directory_File = new List<Directory_Entery>();      
                int cluster_index = dir_frist_cluster[0];
                int next = Mini_Fat.Get_Cluster_statu(cluster_index);
                if (cluster_index == 5 && next == 0)
                    return;
                List<byte> c = new List<byte>();
                byte[] b = new byte[32];
                Directory_Entery f = new Directory_Entery();
                int l = 0;
                do
                {
                    c.AddRange(Virtual_Disk.Read_Cluster(cluster_index));
                    cluster_index = next;
                    if (cluster_index != -1)
                        next = Mini_Fat.Get_Cluster_statu(cluster_index);

                }
                while (cluster_index != -1);
                for (int i = 0; i < c.Count; i++)
                {

                    if (i % 32 == 0)
                    {
                        l = 0;
                        f = Convert_byte_to_dirEntery(b);
                        if (f != null)
                            directory_File.Add(f);
                    }
                    b[l++] = (byte)c[i];

                }
            }
        }
    }
}