using Microsoft.VisualBasic.FileIO;
using System.Collections;

namespace 文件删除demo
{
    public partial class 主页面 : Form
    {

        //程序运行的路径
        string exepath = "",Guanjianci="";
        string[] filesdir;//目录下全部文件列表，相对路径
        //ArrayList filesdir = new ArrayList();
        ArrayList dirdir = new ArrayList();//目录下完整目录，完整路径
        ArrayList Target=new ArrayList();//目录下目标列表，相对路径
        bool StandBy=false;//系统状态，作为保护机制
        public 主页面()
        {
            InitializeComponent();
        }

        //文件删除到回收站（文件路径）
        private bool DeleteFile(String filepath)
        {
            try
            {
                FileSystem.DeleteFile(filepath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //删除文件夹到回收站（文件夹路径）
        private bool DeleteBolck(String dirpath)
        {
            try
            {
                FileSystem.DeleteDirectory(dirpath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                return true;
            }
            catch (Exception)
            {
                return false;                
            }
            
        }

        //获取指定路径下的文件列表
        private string[] GetFileList(String dir)
        {
            try
            {
                string[] filedir = Directory.GetFiles(dir, "*", System.IO.SearchOption.AllDirectories);
                //去除文件名中的绝对地址“exepath部分”
                for (int i = 0; i < filedir.Length; i++)
                {
                    dirdir.Add(filedir[i]);
                    filedir[i] = filedir[i].Replace(exepath + "\\", "");
                }
                //将修改后的文件名输出到textbox1
                for (int i = 0; i < filedir.Length; i++)
                {
                    textBox1.Text += filedir[i] + Environment.NewLine;
                }
                filesdir = filedir;
                button1.Enabled = true;
                button2.Enabled = true;
                label7.Text = "共" + filedir.Length + "项";
                return filedir;
            }
            catch (System.UnauthorizedAccessException)
            {
                string[] filedir= Array.Empty<string>();
                button1.Enabled = false;
                button2.Enabled = false;
                MessageBox.Show("当前路径需要管理员权限，请以管理员身份重启程序。");
                return filedir;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            exepath = Directory.GetCurrentDirectory();
            Fresh();
        }

        private void Fresh()
        {
            StandBy = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            label7.Text = "";
            label8.Text = "";
            Guanjianci = "";
            dirdir.Clear();
            Target.Clear();
            label2.Text = exepath.ToString();
            GetFileList(exepath);
            StandBy = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Guanjianci = textBox2.Text;
        }
        //切换路径
        private void button3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择一个文件夹";
            dialog.SelectedPath = "C:\\";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                exepath = dialog.SelectedPath;
                //MessageBox.Show("选择了文件夹路径：" + dialog.SelectedPath);
                Fresh();
            }
        }
        //移动文件到回收站
        private void button1_Click(object sender, EventArgs e)
        {
            int success = 0, fail = 0;
            ArrayList FailList=new ArrayList();
            //MessageBox.Show(Convert.ToString(textBox3.Lines.Length - 1));//当前行数
            string[] str = new string[textBox3.Lines.Length];
            for (int i = 0; i < textBox3.Lines.Length - 1; i++)
            {
                if(textBox3.Lines[i].Trim()!=""&& textBox3.Lines[i].Trim() != "未找到相关内容。。。")
                {
                    str[i] = exepath + "\\" + textBox3.Lines[i];
                    if (DeleteFile(str[i]))
                    {
                        success++;
                    }
                    else
                    {
                        FailList.Add(textBox3.Lines[i]);
                        fail++;
                    }
                }
                
            }
            if (fail == 0)
            {
                MessageBox.Show("共删除"+success+"项文件，全部成功！");
            }
            else
            {
                MessageBox.Show("共删除"+success+fail+"项文件，"+success+"项成功,"+fail+"项失败。\n"+FailList.ToString());
            }
            Fresh();
            
        }

        private void label5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("允许搜索空格");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Fresh();
        }

        //查找
        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            Guanjianci = textBox2.Text;
            if (Guanjianci != null && Guanjianci != "")
            {
                if (!StandBy)
                {
                    MessageBox.Show("请等待文件路径加载完成再操作！");
                }
                else
                {
                    for (int i = 0; i < filesdir.Length; i++)
                    {
                        if (filesdir[i].Contains(Guanjianci))
                        {
                            Target.Add(dirdir[i]);
                            textBox3.Text += filesdir[i] + Environment.NewLine;
                        }
                    }
                    if (Target.Count == 0)
                    {
                        textBox3.Text = "未找到相关内容。。。";
                        button1.Enabled = false;
                    }
                }
                label8.Text = "共" + Target.Count + "项";
            }
            else
            {
                MessageBox.Show("请先输入关键词");
            }
        }
    }
}