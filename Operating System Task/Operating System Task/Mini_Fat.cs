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
    public class Mini_Fat
    {
        static int[] fat = new int[1024];
        static byte[] b = new byte[4096];
        public static void Prepare_Fat()
        {
            for (int i = 0; i < 1024; i++)
            {
                if (i == 0 || i == 4)
                    fat[i] = -1;
                else if (i >= 1 && i <= 3)
                    fat[i] = i + 1;
                else
                    fat[i] = 0;

            }
        }
        public static void Write_Fat()
        {

            System.Buffer.BlockCopy(fat, 0, b, 0, b.Length);
            for (int i = 0; i < 4; i++)
            {
                byte[] cluster = new byte[1024];
                for (int j = i * 1024, k = 0; k < 1024; k++, j++)
                {
                    cluster[k] = b[j];
                }
                Virtual_Disk.Write_cluster(i + 1, cluster);
            }
        }
        public static void Read_Fat()///
        {
            byte[] bytes = new byte[4096];
            for (int i = 0; i < 4; i++)
            {
                byte[] R_Cluster = Virtual_Disk.Read_Cluster(i + 1);
                for (int j = i * 1024, k = 0; k < 1024; j++, k++)
                {
                    bytes[j] = R_Cluster[k];
                }

            }
            System.Buffer.BlockCopy(bytes, 0, fat, 0, bytes.Length);

        }
        public static void print()
        {
            for (int i = 0; i < fat.Length; i++)
            {
                Console.WriteLine($"fat[{i}] = {fat[i]}");
            }
        }
        public static int Get_Empty_Cluster()
        {
            for (int i = 5; i < 1024; i++)
            {
                if (fat[i] == 0)
                    return i;
            }

            return -1;
        }
        public static void set_Cluster_statu(int Cluster_index, int stat)
        {
            fat[Cluster_index] = stat;
        }
        public static int Get_Cluster_statu(int Cluster_index)
        {
            return fat[Cluster_index];
        }
        public static int Get_Avilable_cluster()
        {
            //if it mean that he need empty cluster
            int count = 0;
            for (int i = 5; i < 1024; i++)
            {
                if (fat[i] == 0)
                    count++;

            }
            return count;


        }
    }
}