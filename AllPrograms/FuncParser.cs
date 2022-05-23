using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AllPrograms
{
    public class FuncParser
    {
        static List<string> cacheFile = new List<string>();
        static int startIndex = -1;
        static int enbIndex = -1;
        static int lineIndex = -1;
        static bool blockClearKE = false;
        static bool blockClearSR = false;

        public static bool keyExists(string path, string section, string key)
        {
            startIndex = -1;
            enbIndex = -1;
            lineIndex = -1;
            bool findSection = false;
            bool findKey = false;
            if (File.Exists(path))
            {
                cacheFile.AddRange(File.ReadAllLines(path));
                for (int i = 0; i < cacheFile.Count; i++)
                {
                    if (!findSection && cacheFile[i].Equals("[" + section + "]", StringComparison.OrdinalIgnoreCase))
                    {
                        findSection = true;
                        startIndex = i;
                        enbIndex = i;
                    }
                    else if (findSection && cacheFile[i].StartsWith("[") && cacheFile[i].EndsWith("]"))
                    {
                        break;
                    }
                    else if (findSection && cacheFile[i].Length != 0)
                    {
                        if (cacheFile[i].StartsWith(key + "=", StringComparison.OrdinalIgnoreCase))
                        {
                            findKey = true;
                            lineIndex = i;
                            break;
                        }
                        else
                        {
                            enbIndex = i;
                        }
                    }
                }
            }
            if (blockClearKE)
            {
                blockClearKE = false;
            }
            else
            {
                cacheFile.Clear();
            }
            return findKey;
        }

        public static string stringRead(string path, string section, string key)
        {
            string outString = null;
            blockClearKE = true;
            if (keyExists(path, section, key))
            {
                outString = cacheFile[lineIndex].Remove(0, (key + "=").Length);
                if (outString.Length == 0)
                {
                    outString = null;
                }
            }
            if (blockClearSR)
            {
                blockClearSR = false;
            }
            else
            {
                cacheFile.Clear();
            }
            return outString;
        }

        public static void iniWrite(string path, string section, string key, string value)
        {
            bool readyToWrite = false;
            blockClearSR = true;
            string line = stringRead(path, section, key);
            if (lineIndex != -1)
            {
                if (value != null && value.Length == 0)
                {
                    value = null;
                }
                if (!string.Equals(line, value, StringComparison.OrdinalIgnoreCase))
                {
                    cacheFile[lineIndex] = key + "=" + value;
                    readyToWrite = true;
                }
            }
            else
            {
                if (startIndex != -1 && enbIndex != -1)
                {
                    cacheFile[enbIndex] += Environment.NewLine + key + "=" + value;
                    readyToWrite = true;
                }
                else if (File.Exists(path))
                {
                    try
                    {
                        File.AppendAllText(path, Environment.NewLine + "[" + section + "]" + Environment.NewLine + key + "=" + value + Environment.NewLine, new UTF8Encoding(false));
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось добавить параметры в файл: " + path);
                    }
                }
            }
            if (readyToWrite)
            {
                writeToFile(path, cacheFile);
            }
            cacheFile.Clear();
        }

        public static int intRead(string path, string section, string key)
        {
            return stringToInt(stringRead(path, section, key));
        }

        public static int stringToInt(string input)
        {
            int value = -1;
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Contains("."))
                {
                    Int32.TryParse(input.Remove(input.IndexOf('.')), out value);
                }
                else if (input.Contains(","))
                {
                    Int32.TryParse(input.Remove(input.IndexOf(',')), out value);
                }
                else
                {
                    Int32.TryParse(input, out value);
                }
            }
            return value;
        }

        public static void deleteKey(string path, string section, string key)
        {
            blockClearKE = true;
            if (keyExists(path, section, key))
            {
                cacheFile.RemoveAt(lineIndex);
                writeToFile(path, cacheFile);
            }
            cacheFile.Clear();
        }

        public static void writeToFile(string path, List<string> list)
        {
            try
            {
                File.WriteAllLines(path, list, new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось записать файл: " + path + Environment.NewLine + ex.Message);
            }
        }
    }
}