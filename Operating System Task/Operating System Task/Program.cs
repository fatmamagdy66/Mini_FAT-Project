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
    public class Program
    {
        public static Directory current,root;
        public static string current_path;

        static void Main(string[] args)
        {
            Virtual_Disk.Initialization();
            current = new Directory("R", 0x10, 5, null);
             current.Read_Directory();
            root = current;



            //Mini_Fat.print();
            //write_Directory();


            string begin = "                            *****     Wellcome     *****       \n" +
                           "       We want to say hello from our frist implimentation for our OS project < ^_^ > \n" +
                           "                                                                              \n\n \n";
            string director = Environment.CurrentDirectory;

            Console.Write(begin);
            //Console.WriteLine(director);
            while (true)
            {
                Console.Write($"{current.Directory_Name}:\\>");
                string command_line = Console.ReadLine();
                Command_string command_String = new Command_string(command_line);

                command_String.chek_command();
                //command_line = command_line.Replace(" ", String.Empty);
                //Console.WriteLine(command_line);
                // command_line.Tolower();
            }
        }
    }
}