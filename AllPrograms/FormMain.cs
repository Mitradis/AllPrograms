using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace AllPrograms
{
    public partial class FormMain : Form
    {
        public static string newAppArgs;
        public static string newAppName;
        static List<string> separateAppPath = new List<string>();
        static List<string> appLaunchPath = new List<string>();
        static List<string> appLaunchArgs = new List<string>();
        static List<int> appID = new List<int>();
        static string pathAppFolder = pathAddSlash(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
        static string appINI = pathAppFolder + "AllPrograms.ini";
        string appDisk = Path.GetPathRoot(pathAppFolder);
        int totalFiles = 0;
        int iconPosX = 35;
        int iconPosY = 17;
        int labelPosX = 12;
        int labelPosY = 49;
        int offSetX = 83;
        int offSetY = 68;
        int itemsOnLine = 0;
        int maxItemsOnLine = 6;
        bool nextLine = false;

        public FormMain()
        {
            InitializeComponent();
            string[] allFolders = pathAppFolder.Split(new string[] { "/", @"\" }, StringSplitOptions.RemoveEmptyEntries);
            separateAppPath.Add(pathAppFolder);
            if (allFolders.Length - 1 > 0)
            {
                for (int i = 0; i < allFolders.Length - 1; i++)
                {
                    separateAppPath.Add(pathAddSlash(Path.GetFullPath(separateAppPath[separateAppPath.Count - 1] + @"..")));
                }
            }
            allFolders = null;
            if (File.Exists(appINI))
            {
                string titleName = FuncParser.stringRead(appINI, "General", "WindowTitleName");
                if (titleName != null)
                {
                    Text = titleName;
                }
                int wLeft = FuncParser.intRead(appINI, "General", "WindowLeft");
                int wTop = FuncParser.intRead(appINI, "General", "WindowTop");
                if (wLeft < 0 || wTop < 0)
                {
                    StartPosition = FormStartPosition.CenterScreen;
                }
                else
                {
                    if (wLeft > (Screen.PrimaryScreen.Bounds.Width - Size.Width))
                    {
                        wLeft = Screen.PrimaryScreen.Bounds.Width - Size.Width;
                    }
                    if (wTop > (Screen.PrimaryScreen.Bounds.Height - Size.Height))
                    {
                        wTop = Screen.PrimaryScreen.Bounds.Height - Size.Height;
                    }
                    StartPosition = FormStartPosition.Manual;
                    Location = new Point(wLeft, wTop);
                }
                totalFiles = FuncParser.intRead(appINI, "General", "TotalFiles");
                if (totalFiles < 0)
                {
                    totalFiles = 0;
                }
                maxItemsOnLine = FuncParser.intRead(appINI, "General", "MaxItemsOnLine");
                if (maxItemsOnLine < 1)
                {
                    maxItemsOnLine = 6;
                }
                ClientSize = new System.Drawing.Size(ClientSize.Width + ((maxItemsOnLine - 6) * offSetX), ClientSize.Height);
                toolStripMenuItem3.Enabled = maxItemsOnLine > 2;
                parseINI();
            }
            else
            {
                closeApp(this, new EventArgs());
            }
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(closeApp);
        }

        void closeApp(object sender, EventArgs e)
        {
            if (!File.Exists(appINI))
            {
                FuncParser.writeToFile(appINI, new List<string>() {
                "[General]",
                "WindowTitleName=Все программы",
                "MaxItemsOnLine=6",
                "TotalFiles=0",
                "WindowLeft=100",
                "WindowTop=100",
                "",
                "[Files]"});
            }
            else
            {
                if (Top >= 0 && Left >= 0)
                {
                    FuncParser.iniWrite(appINI, "General", "WindowTop", Top.ToString());
                    FuncParser.iniWrite(appINI, "General", "WindowLeft", Left.ToString());
                }
            }
            AppDomain.CurrentDomain.ProcessExit -= new EventHandler(closeApp);
        }

        void parseINI()
        {
            appID.Clear();
            appLaunchArgs.Clear();
            appLaunchPath.Clear();
            for (int i = 1; i <= totalFiles; i++)
            {
                if (FuncParser.keyExists(appINI, "Files", "ShortcutFile_" + i))
                {
                    createShortcut(i, FuncParser.stringRead(appINI, "Files", "ShortcutFile_" + i));
                }
                else
                {
                    totalFiles = i--;
                    FuncParser.iniWrite(appINI, "General", "TotalFiles", i.ToString());
                    break;
                }
            }
        }

        void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {
            contextMenuStrip1.Show(PointToScreen(radioButton1.Location));
        }

        void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form form1 = new FormAdd(false);
            if (form1.ShowDialog(this) == DialogResult.OK)
            {
                form1.Dispose();
                DialogResult dialog = MessageBox.Show("Будет выбрана папка?", "Выбор источника", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    DialogResult result = openFileDialog1.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        form1 = new FormAdd(true);
                        form1.ShowDialog(this);
                        if (Path.GetPathRoot(pathAddSlash(Path.GetDirectoryName(openFileDialog1.FileName))) == appDisk)
                        {
                            for (int i = 0; i < separateAppPath.Count; i++)
                            {
                                if (openFileDialog1.FileName.IndexOf(separateAppPath[i], StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    openFileDialog1.FileName = openFileDialog1.FileName.Remove(0, separateAppPath[i].Length);
                                    addFileToINI("FR" + i + "|" + newAppName + "|" + openFileDialog1.FileName + "|" + newAppArgs);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            addFileToINI("FA0|" + newAppName + "|" + openFileDialog1.FileName + "|" + newAppArgs);
                        }
                    }
                }
                else
                {
                    DialogResult result = folderBrowserDialog1.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        folderBrowserDialog1.SelectedPath = pathAddSlash(folderBrowserDialog1.SelectedPath);
                        if (Path.GetPathRoot(folderBrowserDialog1.SelectedPath) == appDisk)
                        {
                            for (int i = 0; i < separateAppPath.Count; i++)
                            {
                                if (folderBrowserDialog1.SelectedPath.IndexOf(separateAppPath[i], StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    if (folderBrowserDialog1.SelectedPath != separateAppPath[i])
                                    {
                                        folderBrowserDialog1.SelectedPath = folderBrowserDialog1.SelectedPath.Remove(0, separateAppPath[i].Length);
                                        addFileToINI("DR" + i + "|" + newAppName + "|" + folderBrowserDialog1.SelectedPath);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            addFileToINI("DA0|" + newAppName + "|" + folderBrowserDialog1.SelectedPath);
                        }
                    }
                }
            }
            form1.Dispose();
            newAppArgs = null;
            newAppName = null;
        }

        void createShortcut(int number, string line)
        {
            itemsOnLine++;
            if (nextLine)
            {
                ClientSize = new System.Drawing.Size(ClientSize.Width, ClientSize.Height + offSetY);
                nextLine = false;
            }
            appID.Add(number);
            string[] parseLine = !String.IsNullOrEmpty(line) ? line.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries) : new string[] { "" };
            if (parseLine.Length >= 3)
            {
                if (parseLine[0][1] == 'A')
                {
                    appLaunchPath.Add(parseLine[2]);
                }
                else if (parseLine[0][1] == 'R')
                {
                    appLaunchPath.Add(trimAppPath(FuncParser.stringToInt(parseLine[0][2].ToString())) + parseLine[2]);
                }
            }
            else
            {
                appLaunchPath.Add("");
            }
            if (parseLine.Length == 4 && !String.IsNullOrEmpty(parseLine[3]))
            {
                appLaunchArgs.Add(parseLine[3]);
            }
            else
            {
                appLaunchArgs.Add("");
            }
            PictureBox newPictureBox = new PictureBox();
            newPictureBox.BackColor = System.Drawing.Color.Transparent;
            newPictureBox.Location = new System.Drawing.Point(iconPosX, iconPosY);
            newPictureBox.Name = "NewPictureBox_" + number;
            newPictureBox.Size = new System.Drawing.Size(32, 32);
            newPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            newPictureBox.Tag = number;
            newPictureBox.DoubleClick += clickItem;
            if (parseLine.Length >= 3)
            {
                if (parseLine[0][0] == 'F')
                {
                    Image image = iconFromFile(appLaunchPath[appLaunchPath.Count - 1]);
                    if (image.Width > 32 || image.Height > 32)
                    {
                        image = resizeIcon(image, 32, 32);
                    }
                    newPictureBox.BackgroundImage = image;
                }
                else if (parseLine[0][0] == 'D')
                {
                    newPictureBox.BackgroundImage = Properties.Resources.folder;
                }
            }
            else
            {
                newPictureBox.BackgroundImage = Properties.Resources.error;
            }
            newPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            Controls.Add(newPictureBox);
            newPictureBox.BringToFront();
            Label newLabel = new Label();
            newLabel.BackColor = System.Drawing.Color.Transparent;
            newLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            newLabel.Location = new System.Drawing.Point(labelPosX, labelPosY);
            newLabel.Name = "NewLabel_" + number;
            newLabel.Size = new System.Drawing.Size(78, 30);
            newLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            if (parseLine.Length >= 3)
            {
                newLabel.Text = parseLine[1];
            }
            else
            {
                newLabel.Text = "ОШИБКА";
            }
            newLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            newLabel.Tag = number;
            newLabel.DoubleClick += clickItem;
            Controls.Add(newLabel);
            newLabel.BringToFront();
            if (itemsOnLine < maxItemsOnLine)
            {
                iconPosX += offSetX;
                labelPosX += offSetX;
            }
            else
            {
                iconPosX = 35;
                labelPosX = 12;
                iconPosY += offSetY;
                labelPosY += offSetY;
                itemsOnLine = 0;
                nextLine = true;
            }
        }

        void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            addRemoveMenu(1);
        }

        void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            addRemoveMenu(-1);
        }

        void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Для удаления дважды щелкнуть по ярлыку с одной из зажатых клавиш: Alt, Ctrl, Shift.");
        }

        void addRemoveMenu(int value)
        {
            maxItemsOnLine = maxItemsOnLine + value;
            FuncParser.iniWrite(appINI, "General", "MaxItemsOnLine", maxItemsOnLine.ToString());
            Application.Restart();
        }

        void clickItem(object sender, EventArgs e)
        {
            int id = FuncParser.stringToInt(((Control)sender).Tag.ToString());
            if (Control.ModifierKeys == Keys.None)
            {
                int index = appID.IndexOf(id);
                if (index != -1)
                {
                    processStart(appLaunchPath[index], appLaunchArgs[index]);
                }
            }
            else
            {
                DialogResult dialog = MessageBox.Show("Удалить выбранный ярлык?", "Удаление элемента", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    List<string> tempList = new List<string>();
                    for (int i = 1; i <= totalFiles; i++)
                    {
                        if (i != id)
                        {
                            tempList.Add(FuncParser.stringRead(appINI, "Files", "ShortcutFile_" + i));
                        }
                        FuncParser.deleteKey(appINI, "Files", "ShortcutFile_" + i);
                    }
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        FuncParser.iniWrite(appINI, "Files", "ShortcutFile_" + (i + 1), tempList[i]);
                    }
                    totalFiles = tempList.Count;
                    FuncParser.iniWrite(appINI, "General", "TotalFiles", totalFiles.ToString());
                    tempList.Clear();
                    Application.Restart();
                }
            }
        }

        void processStart(string path, string args)
        {
            if (File.Exists(path))
            {
                Process process = new Process();
                process.StartInfo.FileName = path;
                if (!String.IsNullOrEmpty(args))
                {
                    process.StartInfo.Arguments = args;
                }
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
                process.StartInfo.UseShellExecute = true;
                try
                {
                    process.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось запустить: " + path + Environment.NewLine + ex.Message);
                }
            }
            else if (Directory.Exists(path))
            {
                Process.Start(path);
            }
        }

        void addFileToINI(string line)
        {
            totalFiles++;
            FuncParser.iniWrite(appINI, "Files", "ShortcutFile_" + totalFiles, line);
            createShortcut(totalFiles, line);
            FuncParser.iniWrite(appINI, "General", "TotalFiles", totalFiles.ToString());
        }

        static Image iconFromFile(string path)
        {
            Image result;
            if (File.Exists(path))
            {
                try
                {
                    result = Icon.ExtractAssociatedIcon(path).ToBitmap();
                }
                catch
                {
                    result = Properties.Resources.noicon;
                }
            }
            else
            {
                result = Properties.Resources.noicon;
            }
            return result;
        }

        static Bitmap resizeIcon(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics graphics = Graphics.FromImage(destImage);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            ImageAttributes wrapMode = new ImageAttributes();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            wrapMode.Dispose();
            graphics.Dispose();
            return destImage;
        }

        string trimAppPath(int count)
        {
            string line = pathAppFolder;
            for (int i = 0; i < count; i++)
            {
                line = pathAddSlash(Path.GetFullPath(line + @".."));
            }
            return line;
        }

        static string pathAddSlash(string path)
        {
            if (!path.EndsWith("/") && !path.EndsWith(@"\"))
            {
                if (path.Contains("/"))
                {
                    path += "/";
                }
                else if (path.Contains(@"\"))
                {
                    path += @"\";
                }
            }
            return path;
        }
    }
}
