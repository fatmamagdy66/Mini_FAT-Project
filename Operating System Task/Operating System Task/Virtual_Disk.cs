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
    public class Virtual_Disk
    {
        static string fileName = @"C:\Users\All In One\Downloads\Disk";
        static FileStream Disk;
        public static void Initialization()
        {

            try
            {
                // Check if file isn't  exists. If yes, creat file .     
                if (!File.Exists(fileName))
                {
                    Disk = File.Create(fileName);

                    byte[] b = new byte[1024];

                    Mini_Fat.Prepare_Fat();
                    Write_cluster(0, b);

                    Mini_Fat.Write_Fat();

                }
                // Check if file is  exists.then open it.     
                else
                {
                    Disk = new FileStream(fileName, FileMode.Open);

                    Mini_Fat.Read_Fat();

                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());

            }
        }
        public static void Write_cluster(int cluster_index, byte[] b)
        {
            Disk.Seek(cluster_index * 1024, SeekOrigin.Begin);
            Disk.Write(b);
            Disk.Flush();
        }
        public static byte[] Read_Cluster(int cluster_index)
        {
            byte[] b = new byte[1024];
            Disk.Seek(cluster_index * 1024, SeekOrigin.Begin);
            Disk.Read(b);
            return b;
        }
        public static int Get_logical_free_space()
        {
            return Mini_Fat.Get_Avilable_cluster() * 1024;
        }


    }

}
