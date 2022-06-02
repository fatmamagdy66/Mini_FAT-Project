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
    public class File_Entery : Directory_Entery
    {
        public Directory parent;
        public string content;
        byte[] data;
        public File_Entery(string name, byte dir_attr, int dir_firstCluster, Directory pa, string cont,int size) : base(name, dir_attr, dir_firstCluster,size)
        {
            this.content = cont;
           this.dir_file_size[0] = cont.Length;
            if (pa != null)
                this.parent = pa;
        }
        public File_Entery(string name, byte dir_attr, int dir_firstCluster, Directory pa, int size) : base(name, dir_attr, dir_firstCluster, size)
        {
           
            this.dir_file_size[0] = 0;
            if (pa != null)
                this.parent = pa;
        }
        public byte[] convert_content_to_byte(string cont)
        {
            byte[] ret = new byte[cont.Length];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (byte)cont[i];
            }
            return ret;
        }
        public Directory_Entery Get_File_Entery()
        {
            Directory_Entery me = new Directory_Entery(this.Directory_Name, this.dir_attr, this.dir_frist_cluster[0],this.dir_file_size[0]);
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
        public void write_File_Content()
        {
            Directory_Entery old = Get_File_Entery();
            byte[] contentBYTES = convert_content_to_byte(content);
            List<byte[]> List_ofArraysof_1024 = new List<byte[]>();
            int Frist_Cluster = this.dir_frist_cluster[0];
            int last_cluster = -1, cluster_index;


            int y = 0;
            byte[] arr1024 = new byte[1024];
            //split b to list of arrays each array of size 1024            
            for (int i = 0; i < contentBYTES.Length; i++)
            {

                if (y % 1024 == 0 && y != 0)
                {
                    y = 0;
                    List_ofArraysof_1024.Add(arr1024);
                }
                arr1024[y] = data[i];
                if (i + 1 == contentBYTES.Length)
                {
                    List_ofArraysof_1024.Add(arr1024);
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
            for (int i = 0; i < List_ofArraysof_1024.Count; i++)
            {
                Virtual_Disk.Write_cluster(cluster_index, List_ofArraysof_1024[i]);
                Mini_Fat.set_Cluster_statu(cluster_index, -1);
                if (last_cluster != -1)
                {
                    Mini_Fat.set_Cluster_statu(last_cluster, cluster_index);
                }
                last_cluster = cluster_index;
                cluster_index = Mini_Fat.Get_Empty_Cluster();
            }
            Directory_Entery new1 = Get_File_Entery();
            if (this.parent != null)
            {
                this.parent.update_info(old, new1);       //    دي بتاخد القديم و الجديد صح update_info  مش ال                              
                this.parent.write_Directory();
            }
            Mini_Fat.Write_Fat();

        }
        public void read_File_Content()
        {

            if (dir_frist_cluster[0] != 0)
            {
                content = string.Empty;
                int cluster_index = dir_frist_cluster[0];
                int next = Mini_Fat.Get_Cluster_statu(cluster_index);
                if (cluster_index == 5 && next == 0)
                    return;
                List<byte> c = new List<byte>();
                do
                {
                    c.AddRange(Virtual_Disk.Read_Cluster(cluster_index));
                    cluster_index = next;
                    if (cluster_index != -1)
                        next = Mini_Fat.Get_Cluster_statu(cluster_index);

                }
                while (cluster_index != -1);


                this.content += Encoding.ASCII.GetString(c.ToArray());
            }
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
                // write_Directory();
            }
        }
        public void Delet_file()
        {


           
            if (parent != null)
            {
                this.parent.remove_Directory_Entry(Get_File_Entery());
            }
            empty_all_its_cluster();
            Mini_Fat.Write_Fat();
        }
        public void print_contant()
        {
            read_File_Content();
            Console.Write($"{this.content}");
        }
    }
}


