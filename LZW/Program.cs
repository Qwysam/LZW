using System;
using System.Collections.Generic;
using System.IO;

namespace LZW
{
    public class Program
    {
        /* Функция для сжатия
        Принимает: строку ввода и таблицу ASCII в виде списка строк
        Возвращает: массив строк в состав которого входят:
        - строка в виде целых неотрицательных чисел,раздеоённых пробелами
        - строка хранящая размер до сжатия
        - строка хранящая размер после сжатия */
        static string[] Compress(string input, List<string> root_table)
        {
            //Объявление целочисленного списка,который будет содержать коды символов
            List<int> compressed = new List<int>();
            //Объявление новой таблицы для последующего сжатия
            List<string> compress_table = new List<string>();
            //Заполнение новой таблицы символами ASCII
            foreach (string element in root_table)
                compress_table.Add(element);
            //Объявление строки и присваивание ей значения первого символа ввода
            string temp_s = input[0].ToString();
            //Цикл,число итераций которого на 1 меньше,чем элементов в строке
            for (int i = 0; i < input.Length - 1; i++)
            {
                //Следующий символ
                char temp_char = input[i + 1];
                //Добавление следующего символа к temp_s,если их комбинация уже есть в таблице
                if (compress_table.Contains(temp_s + temp_char))
                {
                    temp_s = temp_s + temp_char;
                }
                else
                {
                    //Добавление кода первого символа в список
                    compressed.Add(compress_table.IndexOf(temp_s));
                    //Добавление в таблицу комбинации символов
                    compress_table.Add(temp_s + temp_char);
                    //Присваивание значения следующего символа строке
                    temp_s = temp_char.ToString();
                }
                //Решение проблемы с отсутствием кода последнего символа
                if (i == input.Length - 2)
                {
                    compressed.Add(compress_table.IndexOf(temp_s));
                }

            }
            string output = "";
            //Добавление всех кодов в строку output
            foreach (int elem in compressed)
                output += $"{elem} ";
            //Избавлерие от пробела на конце строки
            output = output.Trim();
            //Определение размера до и после сжатия
            int size_before = sizeof(char) * input.Length;
            int size_after = sizeof(int) * compressed.Count;
            //Возвращение массива строк
            string[] res = { output, $"{size_before}", $"{size_after}" };
            return res;
        }
        /* Функция для декомпрессии
        Принимает: строку ввода и таблицу ASCII в виде списка строк
        Возвращает: Строку в виде целых неотрицательных чисел,раздеоённых пробелами */
        static string Decompress(string input, List<string> root_table)
        {
            //Перевод строки в массив строк с пробелом в роли разделителя
            string[] arr = input.Split(' ');
            //Перевод строчного массива в целочисленный
            int[] Ints = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                Ints[i] = int.Parse(arr[i]);
            //Объявление таблицы для декомпрессии
            List<string> decompress_table = new List<string>();
            string output = "";
            //Заполнение таблицы символами ASCII
            foreach (string element in root_table)
                decompress_table.Add(element);
            //Перевод первого числа в символ
            int temp_i = Ints[0];
            string res = "";
            output += decompress_table[temp_i];
            res += output;
            string temp_s = "";
            char C = ' ';
            for (int i = 1; i < Ints.Length; i++)
            {
                //Перевод второго числа в символ
                int next_temp = Ints[i];
                //Добавление комбинации символов в тиблицу
                if (decompress_table.Count - 1 < next_temp)
                {
                    temp_s = output;
                    temp_s = temp_s + C;
                }
                else
                {
                    temp_s = decompress_table[next_temp];
                }
                res += temp_s;
                C = temp_s[0];
                decompress_table.Add(output + C);
                output = temp_s;

            }
            return res;
        }

        static void Main(string[] args)
        {
            //explain what it should do
            Console.WriteLine("This is and educational program to show LZW compression algorithm.");
            Console.WriteLine("It can also be used for text compression in advanced mode.");
            List<string> root_table = new List<string>();
            for (int i = 0; i < 256; i++)
            {
                char temp = (char)i;
                root_table.Add(temp.ToString());
            }
            Console.WriteLine("Input 'exit' to stop the application.");
            Console.WriteLine("Input 'change' after choosing the mode to change it.");
            bool go = true;
            int version = 0;
            while (go)
            {
                Console.WriteLine("Input 1 for educational version or 2 for advanced version.");
                string temp = Console.ReadLine();
                if (temp == "exit")
                {
                    go = false;
                    break;
                }
                bool IsANum = int.TryParse(temp, out version);
                if (IsANum && ((version == 1)||(version==2)))
                    break;
            }
            while (go)
            {
              
                if(version == 1)
                {
                    Console.WriteLine("Input text: ");
                    string input = Console.ReadLine();
                    if (input == "exit")
                    {
                        break;
                    }
                    if (input == "change")
                    {
                        version = 2;
                        continue;
                    }
                    string[] res = Compress(input, root_table);
                    string output = res[0];
                    Console.WriteLine("Compression codes: ");
                    Console.WriteLine(output);
                    Console.WriteLine($"Size before the compression : {res[1]} bytes");
                    Console.WriteLine($"Size after the compression : {res[2]} bytes");
                    Console.WriteLine("Decompression.....");
                    output = Decompress(output, root_table);
                    Console.WriteLine(output);

                }
                else
                {
                    bool usual_version_go = true,fileread = true;
                    int action_num = 0;
                    Console.WriteLine("Input 'file' to read text from a file");
                    Console.WriteLine("Input text or codes: ");
                    string input = Console.ReadLine();
                    if(input == "file")
                    {
                        while (fileread)
                        {
                            Console.WriteLine("Input file name if it's in the app's folder");
                            Console.WriteLine("Or input file path");
                            string path = Console.ReadLine();
                            if (path == "exit")
                                break;
                            if (path == "change")
                            {
                                input = "change";
                            }
                            else
                            {
                                if (File.Exists(path))
                                    input = File.ReadAllText(path);
                            }
                        }
                    }
                    if (input == "change")
                    {
                        version = 1;
                        Console.WriteLine("Application mode has been changed.");
                        continue;
                    }
                    if (input == "exit")
                    {
                        break;
                    }
                    do
                    {
                        Console.WriteLine("Input 1 to compress 2 to decompress");
                        string action = Console.ReadLine();
                        if (action == "change")
                        {
                            version = 1;
                            break;
                        }
                        bool IsANum = int.TryParse(action, out action_num);
                        if (IsANum && ((action_num == 1) || (action_num == 2)))
                        {
                            break;
                        }
                    }
                    while (usual_version_go);
                    if (version == 1)
                    {
                        Console.WriteLine("Application mode has been changed.");
                        continue;
                    }
                    if (action_num == 1)
                    {
                        string[] res = Compress(input, root_table);
                        string output = res[0];
                        Console.WriteLine("Compression codes: ");
                        Console.WriteLine(output);
                        Console.WriteLine($"Size before the compression : {res[1]} bytes");
                        Console.WriteLine($"Size after the compression : {res[2]} bytes");
                    }
                    if (action_num == 2)
                    {
                        string output = Decompress(input, root_table);
                        Console.WriteLine("Decompressed: ");
                        Console.WriteLine(output);
                    }
                }

            }
        }
    }
}

