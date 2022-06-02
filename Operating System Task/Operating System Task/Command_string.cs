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
    class Command_string
    {
        string command_line;
        string command_name;
        string[] words;
        //virsual->real                       .txt              dir
        public static void export_File(string source, string destination)
        {
            int index = Program.current.search_about_Directory(source);
            if (index != -1)
            {
               
                if (System.IO.Directory.Exists(destination))
                {
                   
                    int first_cluster = Program.current.directory_File[index].dir_frist_cluster[0];
                    int file_size = Program.current.directory_File[index].dir_file_size[0];
                    Directory_Entery f = new Directory_Entery(source, 0x00, first_cluster,file_size);                    
                    string new_file = destination + "\\" + source;
                    // in real disk
                    StreamWriter sw = new StreamWriter(new_file);
                   sw.Write(new_file);
                    sw.Flush();
                    sw.Close();
                    Console.WriteLine("1 file is export");
                }
                else Console.WriteLine("the system can not find  the path specified  in your computer disk ");
            }
            else Console.WriteLine("this file is not exist in your virsual disk");

           

        }
        //real ->virsual
        public static void import_File(string file_path)
        {
          
            if (File.Exists(file_path))
            {
                
               // string content = File.ReadAllText(file_path);
                //int size = content.Length;//كل حرف ب byte
                //  last "\" علشان اوصل لاسم الفايل
                int name_start_index= file_path.LastIndexOf("\\");
                string file_name = file_path.Substring(name_start_index + 1);
                int index = Program.current.search_about_Directory(file_name);
                if (index == -1)
                {
                    int first_cluster=0;
                    //if (size > 0)
                    //{
                        first_cluster = Mini_Fat.Get_Avilable_cluster();
                   // }
                    //File_Entery f = new File_Entery(file_name, 0x00, first_cluster, Program.current,0);
                  //  f.write_File_Content();
                    Directory_Entery d = new Directory_Entery(file_name, 0x00, first_cluster, 0);
                   // Program.current.Add_Directory_Entry(d);
                    Program.current.directory_File.Add(d);
                    Program.current.write_Directory();
                }
                else Console.WriteLine("this file is already exist in your virsual disk");
            }
            else Console.WriteLine("this file not exist ");

        }
        
        public static void delet_file(string file_name)
        {
           
            int index = Program.current.search_about_Directory(file_name);
            if (index != -1&& Program.current.directory_File[index].dir_attr == 0x00)//file
            {
                    int first_cluster = Program.current.directory_File[index].dir_frist_cluster[0];
                    int file_size = Program.current.directory_File[index].dir_file_size[0];
                    Directory_Entery f = new Directory_Entery(file_name, 0x00, first_cluster, 0);
                  //  f.Delet_file();
                    Program.current.remove_Directory_Entry(f);
                    Program.current.Read_Directory();
                Console.WriteLine($" the file '{file_name}' is sucessful delet :)");
            }
            else Console.WriteLine("the system canot  find  the file ");
        }
        public static void copy_command(string source, string destentation)//name +size
        {
            int index_source = Program.current.search_about_Directory(source);
            if (index_source != -1)
            {
                int name_start_index = destentation.LastIndexOf("\\");
                string name = destentation.Substring(name_start_index + 1);
                int index_destenation = Program.current.search_about_Directory(name);
                if (index_destenation != -1)

                {
                    if (Program.current.directory_File[index_destenation].dir_attr == 0x10)
                    {
                        int f_c = Program.current.directory_File[index_destenation].dir_frist_cluster[0];
                        int first_cluster = Program.current.directory_File[index_source].dir_frist_cluster[0];
                        int file_size = Program.current.directory_File[index_source].dir_file_size[0];
                        Directory_Entery e = new Directory_Entery(source, 0x00, first_cluster, 0);
                        Directory d = new Directory(name, 0x10, f_c, Program.current);
                        Directory last_dir = d;
                        // Directory last = Program.current;
                       // d.Add_Directory_Entry(e);
                        // d.write_Directory();
                        //Program.current.write_Directory();
                        if (d.parent != null)
                        {
                           // d.update_info(d, last_dir);
                            //   Program.current.parent.update(Program.current);
                            d.parent.write_Directory();


                        }
                        Console.WriteLine(" 1 file(s) copied.");
                    }

                    else
                    {
                        Console.WriteLine("override? Y or N ");
                        string input = Console.ReadLine();
                        if (input == "Y")
                        {

                            Console.WriteLine(" 1 file(s) copied.");
                        }
                        else if (input == "N")
                        {
                            Console.WriteLine(" 0 file(s) copied.");
                        }
                        else
                            Console.WriteLine("error input ");
                    }

                }
                else Console.WriteLine($"the system can not find  the folder specified  in your virsual disk ' {destentation} '");

                }
                else Console.WriteLine($"the system can not find  the file specified  in your virsual disk ' {source} '");

            
        }
        public static void Type(string file_name)
        {
           
            int index = Program.current.search_about_Directory(file_name);
            if (index != -1&& Program.current.directory_File[index].dir_attr==0x00)
            {
                int first_cluster = Program.current.directory_File[index].dir_frist_cluster[0];
                int file_size = Program.current.directory_File[index].dir_file_size[0];
                string name = Program.current.directory_File[index].Directory_Name;
                Console.WriteLine($"{name}    {file_size} bytes");
            }
            else Console.WriteLine("this file not exist ");

        }
       
        public static void make_directory(string name)
        {

            if (Program.current.search_about_Directory(name) == -1)
            {
                Console.WriteLine("Yah, you can add this ");
                Directory_Entery x = Program.current;
                int f_c = Mini_Fat.Get_Avilable_cluster();
                Directory_Entery d = new Directory_Entery(name, 0x10,f_c,0);
                Program.current.Add_Directory_Entry(d);
                //Program.current.directory_File.Add(d);
                Program.current.write_Directory();
                if (Program.current.parent != null)
                {
                    Program.current.parent.update_info(Program.current, x);
                    
                    Program.current.parent.write_Directory();

                }
            }
            else Console.WriteLine($"Can't creat a dir, there is a Directory / file has the same name ");
        }
        public static void remove_directory(string name)
        {
            int index = Program.current.search_about_Directory(name);
            if (index != -1 && Program.current.directory_File[index].dir_attr==0x10)
            {
                int first_cluster = Program.current.directory_File[index].dir_frist_cluster[0];
                Directory d = new Directory(name, 0x10, first_cluster, Program.current);//directory  هنا انا غيرت ال النوع بتاعها
                //d.Delet_Directoty();
               Program.current.remove_Directory_Entry(d);
                Console.WriteLine($"the dir ' {name} ' is succesful remove");
            }
            else Console.WriteLine("Directory not found");
        }
        public static void rename_directory(string old_Name,string new_Name)
        {
            int index_old_Name = Program.current.search_about_Directory(old_Name);
            int index_new_name = Program.current.search_about_Directory(new_Name);
            if (index_old_Name != -1)
            {
                if (index_new_name == -1)
                {
                    int first_cluster = Program.current.directory_File[index_old_Name].dir_frist_cluster[0];
                    Directory d = new Directory(old_Name, 0x10, first_cluster, Program.current);
                    Program.current.directory_File[index_old_Name].Directory_Name = new_Name;
                    //delet and insert as
              /* Directory_Entery e = Program.current.directory_File[index_old_Name];
                    e.Directory_Name = new_Name;
                    Program.current.directory_File.RemoveAt(index_old_Name);
                    Program.current.directory_File.Insert(index_new_name,d);
              */

                   // Program.current.Rename_Dir(old_Name, new_Name);
                    Program.current.write_Directory();
                    Console.WriteLine($"'{old_Name}' is sucessful change to ' {new_Name}' :) ");
                }
                else Console.WriteLine($" can not change this name is exist ' {new_Name} ' ");
            }
            else Console.WriteLine($"The system cannot find the folder specified ' { old_Name} '");

        }
        public static void change_directory(string name)
        {
            int index = Program.current.search_about_Directory(name);
            if (index != -1)
            {
                int first_cluster = Program.current.directory_File[index].dir_frist_cluster[0];
                Program.current_path = Program.current.Directory_Name+"\\" +name;  
                Directory d = new Directory(Program.current_path, 0x10, first_cluster, Program.current);
                Program.current = d;
             
                Program.current.Read_Directory();
                Console.WriteLine("change directory is Done :)");
            }
            else if(name=="..")
            {
                Program.current = Program.root;
                Program.current_path = "\\" + Program.root.Directory_Name; 
                Program.current.Read_Directory();
                Console.WriteLine("change directory is Done :)");
            }

            else Console.WriteLine($"'{name}' is not found.");

        }
        public static void Dir(string name)
        {
            int count_dir = 0;
            int count_file = 0;
            int free_size=0;
            int index = Program.current.search_about_Directory(name);
            if (Program.current.Directory_Name == name)
            {
                int x = Program.current.directory_File.Count();
                for (int i = 0; i < x; i++)
                {
                    if (Program.current.directory_File[i].dir_attr == 0x10)
                    {
                        Console.Write("<DIR>   ");
                        count_dir++;
                    }
                    if (Program.current.directory_File[i].dir_attr == 0x00)
                    {

                        count_file++;
                    }
                    Console.WriteLine(Program.current.directory_File[i].Directory_Name);
                }
                free_size = Program.current.Get_mySize_on_Disk();
            }
            else if(index!=-1)
            {
                int first_cluster = Program.current.directory_File[index].dir_frist_cluster[0];
                Directory d = new Directory(name, 0x10, first_cluster, Program.current);
                int x =d.directory_File.Count();
                for (int i = 0; i < x; i++)
                {
                    if (d.directory_File[i].dir_attr == 0x10)
                    {
                        Console.Write("<DIR>    ");
                        count_dir++;
                            }
                    if (Program.current.directory_File[i].dir_attr == 0x00)
                    {

                        count_file++;
                    }
                    Console.WriteLine(d.directory_File[i].Directory_Name);
                }
                free_size = d.Get_mySize_on_Disk();
            }
            Console.WriteLine($"{count_file} file(s)   {0}   size ");
            Console.WriteLine($"{count_dir} Dir(s)   {(1024*1024)-(free_size*1024)}   bytes free ");
         
        }
    public bool Is_this_command_exist(string command)
        {
            command = command.ToLower();
            if (command == "help" || command == "cls" || command == "dir" || command == "quit" ||
              command == "copy" || command == "del" || command == "md" || command == "rd" ||
              command == "rename" || command == "type" || command == "import" || command == "export"
              || command == "cd")
                return true;
            return false;


        }
        public void help_fun_No_arg()
        {

            Console.WriteLine("cd                                      Change the current default directory to . If the argument is not present, report the current directory. If the directory does not exist an appropriate error should be reported.");
            Console.WriteLine("cls                                     Clear the screen.");
            Console.WriteLine("dir                                     List the contents of directory.");
            Console.WriteLine("quit                                    Quit the shell.");
            Console.WriteLine("copy                                    Copies one or more files to another location.");
            Console.WriteLine("del                                     Deletes one or more files.");
            Console.WriteLine("help                                    Provides Help information for commands.");
            Console.WriteLine("md                                      Creates a directory.");
            Console.WriteLine("rd                                      Removes a directory.");
            Console.WriteLine("rename                                  Removes a directory.");
            Console.WriteLine("type                                    Displays the contents of a text file.");
            Console.WriteLine("import                                  import text file(s) from your computer.");
            Console.WriteLine("export                                  export text file(s) to your computer.");

        }
        public void help_fun_with_arg()
        {

            if (words.Length == 2)
            {
                bool ok = Is_this_command_exist(words[1]);

                if (ok)
                {
                    string x = words[1];
                    switch (x.ToLower())
                    {
                        case "cd":
                            Console.WriteLine("cd                       Change the current default directory to . If the argument is not present, report the current" +
                                              "\n                      directory. If the directory does not exist an appropriate error should be reported.");
                            break;
                        case "cls":
                            Console.WriteLine("cls                      Clear the screen.");
                            break;

                        case "dir":
                            Console.WriteLine("dir                      List the contents of directory.");
                            break;
                        case "quit":
                            Console.WriteLine("quit                     Quit the shell.");
                            break;
                        case "copy":
                            Console.WriteLine("copy                     Copies one or more files to another location.");
                            break;
                        case "del":
                            Console.WriteLine("del                      Deletes one or more files.");
                            break;
                        case "help":
                            Console.WriteLine("help                     Provides Help information for commands.");
                            break;
                        case "md":
                            Console.WriteLine("md                       Creates a directory.");
                            break;

                        case "rd":
                            Console.WriteLine("rd                       Removes a directory.");
                            break;
                        case "rename":
                            Console.WriteLine("rename                   Removes a directory.");
                            break;
                        case "type":
                            Console.WriteLine("type                     Displays the contents of a text file.");
                            break;
                        case "import":
                            Console.WriteLine("import                   import text file(s) from your computer.");
                            break;
                        case "export":
                            Console.WriteLine("export                   export text file(s) to your computer.");
                            break;

                    }
                }
                else Console.WriteLine($"' {words[1]} ' This command is not supported by the help utility.");
            }
            else
            {
                Console.WriteLine("Provides help information for Windows commands.\n" +
                    "HELP has two way to  use \n" +
                    "1) HELP   Provides Help information for all avilable commands\n" +
                    "2) HELP[command]: command - displays help information on that command.");
            }


        }
        public string[] prepar_arg_command(string sub_command)
        {
            // string temp_command_line = sub_command;
            // temp_command_line.ToLower();
            string s = "";
            int index = 0;
            string[] arg;
            for (int i = 0; i < sub_command.Length; i++)
            {
                // if string have space in front 1 , in end 
                if (sub_command[i] != ' ' && i != 0 && sub_command[i - 1] == ' ' && s.Length != 0)
                {

                    s += ' ';//prepar command to split each item using single space 
                    s += sub_command[i];

                    index++;
                }
                else if (sub_command[i] != ' ')
                    s += sub_command[i];
            }

            arg = s.Split(' ');
            return arg;
            //[0] = words[0].ToLower();
        }
        public void prepar_command()
        {
            try
            {
                string temp_command_line = command_line;
                // temp_command_line.ToLower();
                string s = "";
                int index = 0;

                for (int i = 0; i < this.command_line.Length; i++)
                {
                    // if string have space in front 1 , in end 
                    if (this.command_line[i] != ' ' && i != 0 && this.command_line[i - 1] == ' ' && s.Length != 0)
                    {

                        s += ' ';//prepar command to split each item using single space 
                        s += this.command_line[i];

                        index++;
                    }
                    else if (this.command_line[i] != ' ')
                        s += this.command_line[i];
                }

                words = s.Split(' ');
                words[0] = words[0].ToLower();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void chek_command()
        {
            prepar_command();
            bool yes = Is_this_command_exist(words[0]);
            if (yes)
            {
                switch (words[0])
                {
                    case "cls":
                        {
                            if (words.Length == 1)
                            {
                                Console.Clear();
                                // Console.Write(Program.current.Directory_Name);
                            }
                            else
                                Console.WriteLine("error : cls command doesn't take arguments");
                            break;
                        }
                    case "quit":
                        {
                            if (words.Length == 1)
                            {

                                System.Environment.Exit(2);

                            }
                            else
                                Console.WriteLine("error : exit command doesn't take arguments");
                            break;
                        }
                    case "help":
                        {
                            if (words.Length == 1)
                                help_fun_No_arg();
                            if (words.Length >= 2)
                                help_fun_with_arg();
                            break;
                        }

                    case "cd":
                        {
                            if (words.Length == 2)
                                change_directory(words[1]);
                            else Console.WriteLine($"'{words[2]}' The filename, directory name, or volume label syntax is incorrect");
                            break;
                        }
                    case "rd":
                        {
                            if (words.Length == 2)
                                remove_directory(words[1]);
                            else Console.WriteLine($"'{words[2]}' The filename, directory name, or volume label syntax is incorrect.");
                            break;

                        }
                    case "rename":
                        {
                            if (words.Length == 3)

                            {
                                rename_directory(words[1], words[2]);
                               
                            }
                            else Console.WriteLine($"The syntax of the command is incorrect.");
                            break;

                        }
                    case "md":
                        {
                            if (words.Length >= 2)
                            {
                                for (int i = 1; i < words.Length; i++)
                                {
                                    make_directory(words[i]);
                                }

                            }
                            else if (words.Length == 1)
                                Console.WriteLine("The syntax of the command is incorrect.");
                            break;
                        }
                    case "type":
                        {
                            if (words.Length == 2)
                                Type(words[1]);
                            else if (words.Length == 1)
                                Console.WriteLine("The syntax of the command is incorrect.");
                            break;
                        }
                    case "dir":
                        {
                            if (words.Length == 2)
                                Dir(words[1]);
                            else if (words.Length == 1)
                                Dir(Program.current.Directory_Name);
                            break;
                        }
                    case "import":
                        {
                           
                            if (words.Length == 2)
                            {
                               
                                import_File(words[1]);
                            }
                            else if (words.Length == 1)
                                Console.WriteLine("The syntax of the command is incorrect.");
                            break;
                        }
                    case "export":
                        {
                            if (words.Length == 3)
                                export_File(words[1],words[2]);
                            else if (words.Length == 1)
                                Console.WriteLine("The syntax of the command is incorrect.");
                            break;
                        }
                    case "del":
                        {
                            if (words.Length == 2)
                                delet_file(words[1]);
                            else if (words.Length == 1)
                                Console.WriteLine("The syntax of the command is incorrect.");
                            break;
                        }
                    case "copy":
                        {
                            if (words.Length == 3)
                            {
                                copy_command(words[1], words[2]);
                            }

                            else if (words.Length == 1)
                                Console.WriteLine("The syntax of the command is incorrect.");
                                
                                    break;
                        }
                }
            }
            else Console.WriteLine($"'{words[0]}' is not recognized command");
        }
        public Command_string(string command_line)

        {
            this.command_line = command_line;
        }
    }
}