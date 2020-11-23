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
        public static string newAppArgs = null;
        public static string newAppName = null;
        static List<string> separateAppPath = new List<string>();
        static List<string> appLaunchPath = new List<string>();
        static List<string> appLaunchArgs = new List<string>();
        static List<int> appID = new List<int>();
        static List<Control> controlsList = new List<Control>();
        static string pathAppExecuting = Process.GetCurrentProcess().MainModule.FileName;
        static string pathAppFolder = pathAddSlash(Path.GetDirectoryName(pathAppExecuting));
        static string appINI = pathAppFolder + "AllPrograms.ini";
        string appDisk = Path.GetPathRoot(pathAppFolder);
        int totalFiles = 0;
        int mouseWindowX = 0;
        int mouseWindowY = 0;
        int formDefaultX = 522;
        int formDefaultY = 127;
        int iconPosX = 35;
        int iconPosY = 48;
        int labelPosX = 12;
        int labelPosY = 83;
        int offSetX = 84;
        int offSetY = 68;
        int itemsOnLine = 0;
        int maxItemsOnLine = 6;
        bool nextLine = false;
        bool windgetOpen = false;
        const int CS_DBLCLKS = 0x8;
        const int WS_MINIMIZEBOX = 0x20000;

        public FormMain()
        {
            InitializeComponent();
            string[] allFolders = pathAppFolder.Split(new string[] { "/", @"\" }, StringSplitOptions.None);
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
                int wLeft = FuncParser.intRead(appINI, "General", "POS_WindowLeft");
                int wTop = FuncParser.intRead(appINI, "General", "POS_WindowTop");
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
                numericUpDown1.ValueChanged -= new System.EventHandler(numericUpDown1_ValueChanged);
                numericUpDown1.Value = maxItemsOnLine;
                numericUpDown1.ValueChanged += new System.EventHandler(numericUpDown1_ValueChanged);
                resetForm();
                parseINI();
            }
            else
            {
                closeApp(this, new EventArgs());
            }
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(closeApp);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;
                return cp;
            }
        }

        private void closeApp(object sender, EventArgs e)
        {
            if (!File.Exists(appINI))
            {
                FuncParser.writeToFile(appINI, new List<string>() {
                "[Files]",
                "",
                "[General]",
                "POS_WindowTop=100",
                "POS_WindowLeft=100",
                "TotalFiles=0",
                "MaxItemsOnLine=6" });
            }
            else
            {
                if (Top >= 0 && Left >= 0)
                {
                    FuncParser.iniWrite(appINI, "General", "POS_WindowTop", Top.ToString());
                    FuncParser.iniWrite(appINI, "General", "POS_WindowLeft", Left.ToString());
                }
            }
            AppDomain.CurrentDomain.ProcessExit -= new EventHandler(closeApp);
        }

        private void parseINI()
        {
            appID.Clear();
            appLaunchArgs.Clear();
            appLaunchPath.Clear();
            for (int i = 1; i <= totalFiles; i++)
            {
                if (FuncParser.keyExists(appINI, "Files", "ShortcutFile_" + i.ToString()))
                {
                    createShortcut(i, FuncParser.stringRead(appINI, "Files", "ShortcutFile_" + i.ToString()));
                }
                else
                {
                    totalFiles = i;
                    FuncParser.iniWrite(appINI, "General", "TotalFiles", i.ToString());
                    break;
                }
            }
        }

        private void buttonAdd_Click(object sender, System.EventArgs e)
        {
            formShowDialog(new FormName());
            DialogResult dialog = MessageBox.Show("Будет выбрана папка?", "Выбор источника", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.No)
            {
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    formShowDialog(new FormArgs());
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
            newAppArgs = null;
            newAppName = null;
        }

        private void createShortcut(int number, string line)
        {
            itemsOnLine++;
            if (nextLine)
            {
                ClientSize = new System.Drawing.Size(Width, Height + offSetY);
                label1.Size = new System.Drawing.Size(label1.Width, label1.Height + offSetY);
                nextLine = false;
            }
            appID.Add(number);
            string[] parseLine = line.Split(new string[] { "|" }, StringSplitOptions.None);
            if (parseLine[0][1].ToString() == "A")
            {
                appLaunchPath.Add(parseLine[2]);
            }
            else if (parseLine[0][1].ToString() == "R")
            {
                appLaunchPath.Add(trimAppPath(FuncParser.stringToInt(parseLine[0][2].ToString())) + parseLine[2]);
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
            newPictureBox.Name = "NewPictureBox_" + number.ToString();
            newPictureBox.Size = new System.Drawing.Size(32, 32);
            newPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            newPictureBox.Tag = number;
            newPictureBox.DoubleClick += clickItem;
            if (parseLine[0][0].ToString() == "F")
            {
                Image image = iconFromFile(appLaunchPath[appLaunchPath.Count - 1]);
                if (image.Width > 32 || image.Height > 32)
                {
                    image = resizeIcon(image, 32, 32);
                }
                newPictureBox.Image = image;
            }
            else if (parseLine[0][0].ToString() == "D")
            {
                newPictureBox.Image = Properties.Resources.folder;
            }
            Controls.Add(newPictureBox);
            newPictureBox.BringToFront();
            controlsList.Add(newPictureBox);
            Label newLabel = new Label();
            newLabel.BackColor = System.Drawing.Color.Transparent;
            newLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            newLabel.Location = new System.Drawing.Point(labelPosX, labelPosY);
            newLabel.Name = "NewLabel_" + number.ToString();
            newLabel.Size = new System.Drawing.Size(78, 30);
            newLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            newLabel.Text = parseLine[1];
            newLabel.Tag = number;
            Controls.Add(newLabel);
            newLabel.BringToFront();
            controlsList.Add(newLabel);
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

        private void clickItem(object sender, EventArgs e)
        {
            PictureBox clickPictureBox = (PictureBox)sender;
            int id = FuncParser.stringToInt(clickPictureBox.Tag.ToString());
            if (!windgetOpen)
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
                            tempList.Add(FuncParser.stringRead(appINI, "Files", "ShortcutFile_" + i.ToString()));
                        }
                        FuncParser.deleteKey(appINI, "Files", "ShortcutFile_" + i.ToString());
                    }
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        FuncParser.iniWrite(appINI, "Files", "ShortcutFile_" + (i + 1).ToString(), tempList[i]);
                    }
                    totalFiles = tempList.Count;
                    FuncParser.iniWrite(appINI, "General", "TotalFiles", totalFiles.ToString());
                    tempList.Clear();
                    clearAll();
                }
            }
        }

        private void processStart(string path, string args)
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
                catch
                {
                    MessageBox.Show("Не удалось запустить: " + path);
                }
            }
            else if (Directory.Exists(path))
            {
                Process.Start(path);
            }
        }

        private string trimAppPath(int count)
        {
            string line = pathAppFolder;
            for (int i = 0; i < count; i++)
            {
                line = pathAddSlash(Path.GetFullPath(line + @".."));
            }
            return line;
        }

        private void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            maxItemsOnLine = FuncParser.stringToInt(numericUpDown1.Value.ToString());
            FuncParser.iniWrite(appINI, "General", "MaxItemsOnLine", numericUpDown1.Value.ToString());
            clearAll();
        }

        private void clearAll()
        {
            resetForm();
            foreach (Control line in controlsList)
            {
                Controls.Remove(line);
                line.Dispose();
            }
            controlsList.Clear();
            iconPosX = 35;
            iconPosY = 48;
            labelPosX = 12;
            labelPosY = 83;
            itemsOnLine = 0;
            nextLine = false;
            parseINI();
        }

        private void resetForm()
        {
            ClientSize = new System.Drawing.Size(formDefaultX + ((maxItemsOnLine - 6) * offSetX), formDefaultY);
            label1.Size = new System.Drawing.Size(formDefaultX + ((maxItemsOnLine - 6) * offSetX), formDefaultY);
        }

        private void addFileToINI(string line)
        {
            totalFiles++;
            FuncParser.iniWrite(appINI, "Files", "ShortcutFile_" + totalFiles.ToString(), line);
            createShortcut(totalFiles, line);
            FuncParser.iniWrite(appINI, "General", "TotalFiles", totalFiles.ToString());
        }

        private static Image iconFromFile(string path)
        {
            Image result = null;
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

        private static Bitmap resizeIcon(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        private static string pathAddSlash(string path)
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

        private void formShowDialog(Form form)
        {
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                form.Dispose();
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseWindowX = e.X + 1;
            mouseWindowY = e.Y + 1;
            label1.MouseMove += MainForm_MouseMove;
            label1.MouseLeave += MainForm_MouseLeave;
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            label1.MouseMove -= MainForm_MouseMove;
            label1.MouseLeave -= MainForm_MouseLeave;
        }

        private void MainForm_MouseLeave(object sender, EventArgs e)
        {
            label1.MouseMove -= MainForm_MouseMove;
            label1.MouseLeave -= MainForm_MouseLeave;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            Location = new Point(Cursor.Position.X - mouseWindowX, Cursor.Position.Y - mouseWindowY);
        }

        private void button_Widget_Click(object sender, System.EventArgs e)
        {
            label1.Focus();
            if (!windgetOpen)
            {
                windgetOpen = true;
                optionsShowHide(true);
                button_Widget.BackgroundImage = Properties.Resources.buttonWidgetPressed;
            }
            else
            {
                windgetOpen = false;
                optionsShowHide(false);
                button_Widget.BackgroundImage = Properties.Resources.buttonWidget;
            }
        }

        private void optionsShowHide(bool visible)
        {
            buttonAdd.Enabled = visible;
            buttonAdd.Visible = visible;
            numericUpDown1.Enabled = visible;
            numericUpDown1.Visible = visible;
        }

        private void button_Close_Click(object sender, System.EventArgs e)
        {
            label1.Focus();
            Application.Exit();
        }

        private void button_Minimize_Click(object sender, System.EventArgs e)
        {
            label1.Focus();
            WindowState = FormWindowState.Minimized;
        }

        private void button_Close_MouseEnter(object sender, System.EventArgs e)
        {
            button_Close.BackgroundImage = Properties.Resources.buttonCloseGlow;
        }

        private void button_Close_MouseLeave(object sender, System.EventArgs e)
        {
            button_Close.BackgroundImage = Properties.Resources.buttonClose;
        }

        private void button_Minimize_MouseEnter(object sender, System.EventArgs e)
        {
            button_Minimize.BackgroundImage = Properties.Resources.buttonMinimizeGlow;
        }

        private void button_Minimize_MouseLeave(object sender, System.EventArgs e)
        {
            button_Minimize.BackgroundImage = Properties.Resources.buttonMinimize;
        }

        private void button_Widget_MouseEnter(object sender, System.EventArgs e)
        {
            if (windgetOpen)
            {
                button_Widget.BackgroundImage = Properties.Resources.buttonWidgetPressed;
            }
            else
            {
                button_Widget.BackgroundImage = Properties.Resources.buttonWidgetGlow;
            }
        }

        private void button_Widget_MouseLeave(object sender, System.EventArgs e)
        {
            if (windgetOpen)
            {
                button_Widget.BackgroundImage = Properties.Resources.buttonWidgetPressed;
            }
            else
            {
                button_Widget.BackgroundImage = Properties.Resources.buttonWidget;
            }
        }
    }
}
