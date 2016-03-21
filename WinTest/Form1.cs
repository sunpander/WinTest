using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;
using System.IO;
using org.pdfbox.pdmodel;
using org.pdfbox.util;
using OfficeOpenXml;
using System.Threading;
 

namespace WinTest
{
    public partial class Form1 : Form
    {

        public static List<ResultOk> listResultOk = new List<ResultOk>();

        public static List<ResultOk> listResultError = new List<ResultOk>();
  
        private static string strTxtFolderName = "";
        public Form1()
        {
            InitializeComponent();
        }
        #region 按钮事件
        private void Form1_Load(object sender, EventArgs e)
        {
            txtHelp.Text = "1.选择pdf所在的目录; \r\n2.点击执行按钮.\r\n \r\n程序首先将pdf转换为txt文件,然后遍历txt文件进行分析,分析结果会生成excel文件";

            try
            {
                //试图记下,,,,最后一次选择的目录 路径..尤其是调试时不断输入耽误时间
                StreamReader CodeReader = new StreamReader("config.txt", Encoding.Default);
                string oldCodeText = CodeReader.ReadToEnd();
                CodeReader.Close();

                if (oldCodeText.IndexOf("#") >= 0)
                {
                    string[] a = oldCodeText.Split('#');
                    if (a.Length > 2)
                    {
                        txtPdfFolderName.Text = a[0];
                        txtTxtFolder.Text = a[1];
                        txtResult.Text = a[2];
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private void btnChooseTxtFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog selFolder = new FolderBrowserDialog();
            selFolder.ShowDialog();
            string rootDir = selFolder.SelectedPath;
            if (String.IsNullOrEmpty(rootDir))
            {
                return;
            }
            
            this.txtTxtFolder.Text = rootDir;
        }
        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog selFolder = new FolderBrowserDialog();
            selFolder.ShowDialog();
            string rootDir = selFolder.SelectedPath;
            if (String.IsNullOrEmpty(rootDir))
            {
                return;
            }
            DirectoryInfo root = new DirectoryInfo(rootDir);
            FileInfo[] files = root.GetFiles("*.pdf");
            if (files.Length == 0)
                MessageBox.Show("文件夹下不包含pdf文件.");
            this.txtPdfFolderName.Text = rootDir;
        }

      
        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtPdfFolderName.Text) || !Directory.Exists(this.txtPdfFolderName.Text))
                {
                    MessageBox.Show("选择pdf文件目录");
                    return;
                }
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    btnRead.Enabled = false;
                    btnConvert.Enabled = false;
                    btnAnaylse.Enabled = false;
                }));
                SetLableText("pdf文件转txt文件", lblInfo);
                string rootDir = convertPdfToTxt(this.txtPdfFolderName.Text);
                SetLableText("pdf文件转txt文件.完成", lblInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAnayle_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtTxtFolder.Text) || !Directory.Exists(this.txtTxtFolder.Text))
                {
                    MessageBox.Show("选择pdf文件目录");
                    return;
                }
                SetLableText("分析txt文件", lblInfo);
                strTxtFolderName = this.txtTxtFolder.Text;

                //多线程,防止假死
              

                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    btnRead.Enabled = false;
                    btnConvert.Enabled = false;
                    btnAnaylse.Enabled = false;
                }));
                Thread thread = new Thread(new ThreadStart(DoAnaylese));
                thread.IsBackground = true;
                thread.Start();
               // DoAnaylese(this.txtTxtFolder.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //一键处理
        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                //多线程,防止假死
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    btnRead.Enabled = false;
                    btnConvert.Enabled = false;
                    btnAnaylse.Enabled = false;
                }));
                Thread thread = new Thread(new ThreadStart(processRun));
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        delegate void labDelegate(string str,Control box);
        private void SetLableText(string str,Control box)
        {
            if (box.InvokeRequired)
            {
                Invoke(new labDelegate(SetLableText), new object[] { str ,box});
            }
            else
            {
                if (box is TextBox)
                {
                    (box as TextBox).Text = str;
                }
                else if (box is Label)
                {
                   ( box as Label).Text = str;
                }
                // progressBar1.Value =    (int) (  100.0 /  files.Length   *(i+1));
            }
        }


        delegate void pbDelegate(int value);
        private void SetPbValue(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                Invoke(new pbDelegate(SetPbValue), new object[] { value });
            }
            else
            {
                progressBar1.Value = value;
            }
        }

 
        public void processRun(){

            DateTime dtStartTime = DateTime.Now;
 
         
            if (string.IsNullOrEmpty(this.txtPdfFolderName.Text) || !Directory.Exists(this.txtPdfFolderName.Text))
            {
                MessageBox.Show("选择pdf文件目录");
                return;
            }
            SetLableText("pdf文件转txt文件",lblInfo);
            strTxtFolderName = convertPdfToTxt(this.txtPdfFolderName.Text);
            //string rootDir = @"H:\2014\wobo\2016\数据样本（徐祯）\pdf\20160301102704";
            //2.分析txt文件内容
            if (string.IsNullOrEmpty(strTxtFolderName))
            {
                return;
            }
            DoAnaylese();
                 
               string msg = "耗时" +  (DateTime.Now - dtStartTime).TotalSeconds +"秒";

               SetLableText(msg, lblInfo);
        }

        private   void processVoteRate(string txtYiti, string oldCodeText, ResultOk stock)
        {
            if (txtYiti.Length < 20)
                return;
            if (txtYiti.IndexOf("%") < 0 && txtYiti.IndexOf("％") < 0 && txtYiti.IndexOf("票") < 0)
                return;
            string doWhat = ServiceHelp.getDoWhat(txtYiti);
            if (string.IsNullOrEmpty(doWhat))
                return;
            doWhat = "";
            int begin = 0;
            int tmpLen = 5;

            //Console.WriteLine(txtYiti);

            string splitKeyWord = "";

            //如果只有  一个 ,则不用再细分
            if (ServiceHelp.getStringCount(txtYiti, "弃权") > 1)// || (txtYiti.IndexOf("如下") > 0 && txtYiti.IndexOf("关于") > 0))
            {

                //按"审议通过"  区分
                if (txtYiti.IndexOf("审议通过") < 0)
                {
                    if (txtYiti.IndexOf("议案 ") > 0)
                    {
                        splitKeyWord = "议案 ";

                    }
                    if (txtYiti.IndexOf("如下") > 0 && txtYiti.IndexOf("关于") > 0 && oldCodeText != txtYiti)
                    {
                        splitKeyWord = "关于";
                    }

                    //if (txtYiti.IndexOf("的议案") > 0)
                    //{
                    //    splitKeyWord = "的议案 ";
                    //}
                }
                else
                {
                    splitKeyWord = "审议通过";
                    if (txtYiti.IndexOf("如下") > 0 && txtYiti.IndexOf("关于") > 0 && oldCodeText != txtYiti)
                    {
                        splitKeyWord = "关于";
                    }
                }
            }
            int start = 0;
            while (true)
            {
                if (string.IsNullOrEmpty(splitKeyWord) && start == 0)
                {
                    begin = 0;
                    tmpLen = txtYiti.Length;
                    start = txtYiti.Length + 10;
                }
                else
                {
                    if (start >= txtYiti.Length)
                        break;
                    begin = txtYiti.IndexOf(splitKeyWord, start);
                    if (begin < 0)
                        break;

                    tmpLen = txtYiti.IndexOf(splitKeyWord, begin + 4) - begin;
                    if (tmpLen < 0 && txtYiti.Length >( begin + 20))
                    {

                        //给最后一次机会(处理最后不包含  审议通过的特殊情况)
                        //比如1股权2选举  20150627-000007.SZ-零七股份：2014年年度股东大会决议公告.pdf中第14条
                        if (ServiceHelp.getStringCount(txtYiti.Substring(10, txtYiti.Length - 11), "《") == 1)
                        {
                            tmpLen = txtYiti.IndexOf("《", begin + 20) - begin;
                            if (tmpLen < 0)
                            {
                                tmpLen = txtYiti.Length - begin;
                                start = begin + 4;
                            }
                            else //if( txtYiti.IndexOf("《", begin + 20) ==txtYiti.LastIndexOf("《", begin + 20))
                            {

                                //再加一个判断,截取后应该含有%,否则不算
                                string abc = txtYiti.Substring(begin, tmpLen);
                                if (abc.IndexOf("%") > 0 || abc.IndexOf("％") > 0)
                                {

                                    splitKeyWord = "《";
                                    start = begin + 20;
                                }
                                else
                                {
                                    tmpLen = txtYiti.Length - begin;
                                    start = begin + 4;
                                }
                            }
                        }
                        else
                        {
                            tmpLen = txtYiti.Length - begin;
                            start = begin + 4;
                        }
                    }
                    else
                    {

                        start = begin + 4;
                    }
                }
                if (tmpLen <= 0)
                    break;
                string txt1 = txtYiti.Substring(begin, tmpLen);

                //有可能不带百分比
                if (txt1.IndexOf("%") < 0 && txt1.IndexOf("％") < 0 && txt1.IndexOf("票") < 0)
                    continue;


                if (splitKeyWord.IndexOf("议案") >= 0)
                {
                    txt1 = txtYiti.Substring(begin - 20, tmpLen - 20);
                }


                doWhat = ServiceHelp.getDoWhat(txt1);
                if (doWhat == "")
                    continue;
                //Console.WriteLine(txt1);
                //Console.WriteLine(doWhat);

                //最后一步,查找 同意 反对 弃权比例
                //类型1:
                //                同意 53,594,429 股，占出席会议所有有表决权股东所持表决权 100％；反对 0 股，占
                //出席会议所有有表决权股东所持表决权 0％；弃权 0 股，占出席会议所有有表决权股东所持
                //表决权 0％。 
                if (txt1.IndexOf("同意  反对   弃权") < 0)
                {
                    txt1 = txt1.Replace(" ", "");
                }

                decimal agree = 0;
                decimal notagree = 0;
                decimal forget = 0;
                //是同意和反对的
                #region 特殊处理表格
                if (txt1.IndexOf("同意  反对   弃权") >= 0 && txt1.IndexOf("股份总数  同意股数 比例% 反对股数 比例%  弃权股数  比例%") > 0)
                {
                    //出去数字中间的逗号
                    txt1 = txt1.Replace(",", "");
                    //特殊处理表格方式的,表格的格式为

                    int titleIndex = txt1.IndexOf("股份总数  同意股数 比例% 反对股数 比例%  弃权股数  比例%") + "股份总数  同意股数 比例% 反对股数 比例%  弃权股数  比例%".Length;
                    //查找数字开头
                    decimal pp = 0;
                    int a = titleIndex;
                    while (!decimal.TryParse(txt1.Substring(a, 1), out pp))
                    {
                        a++;
                    }
                    int b = a;
                    while (decimal.TryParse(txt1.Substring(b, 1), out pp) || txt1.Substring(b, 1) == "." || txt1.Substring(b, 1) == " " || txt1.Substring(b, 1) == "%")
                    {
                        b++;
                    }
                    agree = ServiceHelp.getPersent(txt1, "同意");


                    string data = txt1.Substring(a, b - a);
                    data = data.Replace("%", "");
                    string[] datas = data.Split(' ');
                    if (datas.Length >= 7)
                    {
                        decimal.TryParse(datas[2], out agree);

                        decimal.TryParse(datas[4], out notagree);
                        decimal.TryParse(datas[6], out forget);

                    }
                    if (agree == 100)
                    {
                        //同意为100 则反对,弃权不用再查了
                        notagree = 0;
                        forget = 0;
                    }
                    else if (agree > 100 || agree < 30)
                    {
                        //如果同意的票数 大于100,认为不是百分比,,或者同意小于30,也认为不是百分比

                        notagree = ServiceHelp.getPersent(txt1, "反对", "num");
                        forget = ServiceHelp.getPersent(txt1, "弃权", "num");

                    }

                    else
                    {
                        notagree = ServiceHelp.getPersent(txt1, "反对");
                        forget = ServiceHelp.getPersent(txt1, "弃权");
                        //不等于100,继续找其他的
                    }

                }
                #endregion


                #region 同意 反对 弃权  带百分比的
                else if (txt1.IndexOf("票同意") >= 0 && txt1.IndexOf("票反对") >= 0 && txt1.IndexOf("票弃权") >= 0)
                {
                    agree = ServiceHelp.getPersent(txt1, "票同意");
                    if (agree == 100)
                    {
                        //同意为100 则反对,弃权不用再查了
                        notagree = 0;
                        forget = 0;
                    }
                    else
                    {
                        notagree = ServiceHelp.getPersent(txt1, "票反对");
                        forget = ServiceHelp.getPersent(txt1, "票弃权");
                        //不等于100,继续找其他的
                    }

                }
                #endregion
                    
                #region 同意 反对 弃权  带百分比的
                else if (txt1.IndexOf("同意") >= 0 && (txt1.IndexOf("％", txt1.IndexOf("同意")) > 0 || txt1.IndexOf("%", txt1.IndexOf("同意")) > 0))
                {
                    agree = ServiceHelp.getPersent(txt1, "同意");
                    if (agree == 100)
                    {
                        //同意为100 则反对,弃权不用再查了
                        notagree = 0;
                        forget = 0;
                    }
                    else if (agree > 100 || agree < 30)
                    {
                        //如果同意的票数 大于100,认为不是百分比,,或者同意小于30,也认为不是百分比

                        notagree = ServiceHelp.getPersent(txt1, "反对", "num");
                        forget = ServiceHelp.getPersent(txt1, "弃权", "num");
                    }
                    else
                    {
                        notagree = ServiceHelp.getPersent(txt1, "反对");
                        forget = ServiceHelp.getPersent(txt1, "弃权");
                        //不等于100,继续找其他的
                    }

                }
                #endregion
                #region 赞成   反对 弃权 带%比的
                else if (txt1.IndexOf("赞成") >= 0 && (txt1.IndexOf("％", txt1.IndexOf("赞成")) > 0 || txt1.IndexOf("%", txt1.IndexOf("赞成")) > 0))
                {
                    //查找同意后的第一个 百分比
                    agree = ServiceHelp.getPersent(txt1, "赞成");
                    if (agree == 100)
                    {
                        //同意为100 则反对,弃权不用再查了
                        notagree = 0;
                        forget = 0;
                    }
                    else if (agree > 100 || agree < 30)
                    {
                        //如果同意的票数 大于100,认为不是百分比,,或者同意小于30,也认为不是百分比

                        notagree = ServiceHelp.getPersent(txt1, "反对","num");
                        forget = ServiceHelp.getPersent(txt1, "弃权","num");

                    }
                    else
                    {
                        //不等于100,继续找其他的
                        //同样的方法查找 反对


                        notagree = ServiceHelp.getPersent(txt1, "反对");
                        forget = ServiceHelp.getPersent(txt1, "弃权");
                    }

                }
                #endregion
                #region 同意 反对 弃权  不带百分比的
                else if (txt1.IndexOf("同意") >= 0 && (txt1.IndexOf("％", txt1.IndexOf("同意")) < 0 && txt1.IndexOf("%", txt1.IndexOf("同意")) < 0))
                {
                    agree = ServiceHelp.getPersent(txt1, "同意");
                    if (agree == 100)
                    {
                        //同意为100 则反对,弃权不用再查了
                        notagree = 0;
                        forget = 0;
                    }
                    else if (agree > 100 || agree < 30)
                    {
                        //如果同意的票数 大于100,认为不是百分比,,或者同意小于30,也认为不是百分比

                        notagree = ServiceHelp.getPersent(txt1, "反对", "num");
                        forget = ServiceHelp.getPersent(txt1, "弃权", "num");

                    }
                    else
                    {
                        notagree = ServiceHelp.getPersent(txt1, "反对");
                        forget = ServiceHelp.getPersent(txt1, "弃权");
                        //不等于100,继续找其他的
                    }

                }
                #endregion
   
                #region 赞成票
                else if (txt1.IndexOf("赞成票") >= 0 && txt1.IndexOf("反对票") >= 0 && txt1.IndexOf("弃权票") >= 0)
                {
                    //查找同意后的第一个 百分比
                    agree = ServiceHelp.getPersent(txt1, "赞成票");
                    if (agree == 100)
                    {
                        //同意为100 则反对,弃权不用再查了
                        notagree = 0;
                        forget = 0;
                    }
 
                    else
                    {
                        //不等于100,继续找其他的
                        //同样的方法查找 反对
                        notagree = ServiceHelp.getPersent(txt1, "反对票");
                        forget = ServiceHelp.getPersent(txt1, "弃权票");
                    }

                }
                #endregion
                #region 同意票 弃权票
                else if (txt1.IndexOf("同意票") >= 0 && txt1.IndexOf("反对票") >= 0 && txt1.IndexOf("弃权票") >= 0)
                {
                    //查找同意后的第一个 百分比
                    agree = ServiceHelp.getPersent(txt1, "同意票");
                    if (agree == 100)
                    {
                        //同意为100 则反对,弃权不用再查了
                        notagree = 0;
                        forget = 0;
                    }
       
                    else
                    {
                        //不等于100,继续找其他的
                        //同样的方法查找 反对
                        notagree = ServiceHelp.getPersent(txt1, "反对票");
                        forget = ServiceHelp.getPersent(txt1, "弃权票");
                    }

                }
                #endregion
                else
                {
                   // Console.WriteLine("不包含同意");
                }
                ResultOk newStock = new ResultOk(stock.fileName, stock.code, stock.name, stock.date);
                newStock.doWhat = doWhat;

                if ((notagree + forget + agree) <0)
                {
                    agree = 100;
                }
                if (agree == 0 && notagree == 0 && forget == 0)
                {
                    agree = 100;
                } else if (notagree == 0 && forget == 0)
                {
                    agree = 100;
                }
                else if ((notagree + forget + agree) > 100 || (notagree + forget + agree)<90)
                {
                    if ((notagree + forget + agree) > 100 && (notagree + agree) == 100)
                    {
                        forget = 0;
                    }
                    else
                    {
                        decimal agree1 = agree;
                        decimal notagree2 = notagree;

                        agree = (agree / (notagree2 + forget + agree1)) * 100;
                        notagree = (notagree / (notagree2 + forget + agree1)) * 100;
                        forget = (forget / (notagree2 + forget + agree1)) * 100;

                        agree = Math.Round(agree, 4);
                        notagree = Math.Round(notagree, 4);
                        forget = Math.Round(forget, 4);
                    }
                }
                
                newStock.agree = agree + "";
                newStock.notagree = notagree + "";
                newStock.forget = forget + "";
                
                listResultOk.Add(newStock);
                continue;
            }
        }

       
        #region  pdf转txt文件
        private   string convertPdfToTxt(string rootDir)
        {
            DirectoryInfo root = new DirectoryInfo(rootDir);
            FileInfo[] files = root.GetFiles("*.pdf");
            if (files.Length == 0)
                MessageBox.Show("文件夹下不包含pdf文件.");

            string bakFolderName = rootDir + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
            DirectoryInfo rootBak = new DirectoryInfo(bakFolderName);
            if (!rootBak.Exists)
            {
                rootBak.Create();
            }
            SetPbValue(0);
                    
            //1.pdf转txt
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    SetLableText(string.Format("pdf转txt.已处理{0},共{1},完成比例:{2}%", i + 1, files.Length , decimal.Round((Convert.ToDecimal(i + 1) / Convert.ToDecimal(files.Length) * 100), 2).ToString()), lblInfo);
                    SetPbValue((int)decimal.Round((Convert.ToDecimal(i + 1) / Convert.ToDecimal(files.Length) * 100), 2));
                    
                    FileInfo pdfFile = files[i];

                    string txtFilePath = bakFolderName + "\\" + pdfFile.Name.Substring(0, pdfFile.Name.Length - 4) + ".txt";
                     
                    FileInfo txtFile = new FileInfo(txtFilePath);

                    if (!txtFile.Exists)
                    {
                        FileStream stream = txtFile.Create();
                        stream.Close();
                    }
                    PDDocument doc = PDDocument.load(pdfFile.FullName);
                    PDFTextStripper pdfStripper = new PDFTextStripper();
                    string text = pdfStripper.getText(doc);

                    StreamWriter swPdfChange = new StreamWriter(txtFile.FullName, false, Encoding.GetEncoding("gb2312"));
                    swPdfChange.Write(text);

                    swPdfChange.Close();
                }
                catch (Exception)
                {
                    continue;
                }

            }
           //this.txtTxtFolder.Text = bakFolderName;

            this.SetLableText(bakFolderName, txtTxtFolder);
            return bakFolderName;
        }
        #endregion


        #region 分析txt文件
        private void DoAnaylese()
        {
            string rootDir = strTxtFolderName;
            listResultOk.Clear();
            listResultError.Clear();

            DirectoryInfo root = new DirectoryInfo(rootDir);
            FileInfo[] files = root.GetFiles("*.txt");
            if (files.Length == 0)
            {
                MessageBox.Show("文件夹下不包含txt文件.");
                return;
            }
            this.BeginInvoke(new MethodInvoker(delegate()
            {
                btnRead.Enabled = false;
                btnConvert.Enabled = false;
                btnAnaylse.Enabled = false;
            }));
            string code = "";
            string name = "";
            string date = "";

            int begin = 0;
            int tmpLen = 5;

            SetLableText("转换完成,开始分析txt文件", lblInfo);
            SetPbValue(0);
            for (int i = 0; i < files.Length; i++)
            {
                SetLableText(string.Format("分析文件.已处理{0},共:{1},完成比例:{2}%", i + 1, files.Length , decimal.Round((Convert.ToDecimal(i + 1) / Convert.ToDecimal(files.Length) * 100), 2).ToString()), lblInfo);
                bool hasError = false;
                int beginListCount = listResultOk.Count;

                SetPbValue((int)decimal.Round((Convert.ToDecimal(i + 1) / Convert.ToDecimal(files.Length) * 100), 2));
                code = "";
                name = "";
                date = "";
                FileInfo txtFile = files[i];
                try
                {
                    #region
                    string oldCodeText = "";
                    StreamReader CodeReader = new StreamReader(txtFile.FullName, Encoding.Default);
                    oldCodeText = CodeReader.ReadToEnd();
                    CodeReader.Close();
                    //根据标题做第一次筛选.截取前200位,任务里面还有标题
                    string titleContext = oldCodeText.Replace(" ", "");
                    if (titleContext.Length > 200)
                    {
                        titleContext = titleContext.Substring(0, 200);
                    }
                    if (titleContext.IndexOf("决议公告") < 0 && titleContext.IndexOf("决议的公告") < 0)
                    {
                        if (titleContext.IndexOf("大会通知的公告") > 0)
                        {
                            throw new Exception("大会通知的公告,不用处理.");
                        }
                        if (titleContext.IndexOf("股东大会的公告") > 0 )
                        {
                            throw new Exception("股东大会的公告,不用处理.");
                        }
                        if (titleContext.IndexOf("提示性公告") > 0 )
                        {
                            throw new Exception("提示性公告,不用处理.");
                        }
                        if (titleContext.IndexOf("补充通知的公告") > 0 )
                        {
                            throw new Exception("补充通知的公告,不用处理.");
                        }
                        if (titleContext.IndexOf("通知的公告") > 0 )
                        {
                            throw new Exception("通知的公告,不用处理.");
                        }
                   
                        throw new Exception("不包含[决议公告]和[决议的公告],不用处理.");
                    }

                    if (oldCodeText.IndexOf("附件 1：") > 0)
                    {
                        //如果有附件,,则附件不考虑,直接截取掉
                        oldCodeText = oldCodeText.Substring(0, oldCodeText.IndexOf("附件 1："));
                    } if (oldCodeText.IndexOf("附件一：") > 0)
                    {
                        //如果有附件,,则附件不考虑,直接截取掉
                        oldCodeText = oldCodeText.Substring(0, oldCodeText.IndexOf("附件一："));
                    }
                     if (oldCodeText.IndexOf("附件 一 ：") > 0)
                    {
                        //如果有附件,,则附件不考虑,直接截取掉
                        oldCodeText = oldCodeText.Substring(0, oldCodeText.IndexOf("附件 一 ："));
                    }

                   //先getdowhat找不到算了
                    if (string.IsNullOrEmpty(ServiceHelp.getDoWhat(oldCodeText)))
                    {
                        throw new Exception("没找到关键字,不用处理.");
                    }


                    //查找code
                    code = ServiceHelp.getCode(oldCodeText);
                    //查找name
                    name = ServiceHelp.getName(oldCodeText);
                    //查找时间
                    date = ServiceHelp.getDate(oldCodeText);
                    if (date.Length > 10 && date.IndexOf("年")>0)
                    {
                        //对日期再一次处理  //2010年7/18
                        date = date.Substring(0, 4) + "/" + date.Substring(date.LastIndexOf("年")+1);

                    }

                    ResultOk stock = new ResultOk(txtFile.FullName, code, name, date);
                    //查找关键字
                    //规律1:都在提审议案和表决情况     提案审议
                    //选举	1提名
                    //    2增补
                    //    3选举
                    //担保	4担保
                    //并购	5股权            竞购  30%股权              -表决结果
                    //    6资产    
               
                    //Console.WriteLine(txtFile.FullName);
                    //Console.WriteLine(code + "__" + name + "__" + date);
                    string txtYiti = "";
                    if (oldCodeText.IndexOf("议案的审议和表决情况") > 0 || oldCodeText.IndexOf("议案的审议和表决情况") > 0 || oldCodeText.IndexOf("提案审议") > 0 || oldCodeText.IndexOf("提审议案") > 0 || oldCodeText.IndexOf("议案审议") > 0)
                    {
                       // 提案审议情况
                        begin = oldCodeText.IndexOf("提案审议");
                        if (begin < 0)
                        {
                            begin = oldCodeText.IndexOf("提审议案");
                        }
                        if (begin < 0)
                        {
                            begin = oldCodeText.IndexOf("议案审议");
                        }
                        if (begin < 0)
                        {
                            begin = oldCodeText.IndexOf("议案的审议和表决情况");
                        }
                        if (oldCodeText.IndexOf("律师出具", begin) > 0)
                        {
                            tmpLen = oldCodeText.IndexOf("律师出具", begin) - begin;
                        }
                        else if (oldCodeText.IndexOf("律师见证情况", begin) > 0)
                        {
                            tmpLen = oldCodeText.IndexOf("律师见证情况", begin) - begin;
                        }

                        if (tmpLen > 0)
                        {
                            if (tmpLen > oldCodeText.Length)
                            {
                                tmpLen = oldCodeText.Length;
                            }
                            try
                            {
                                txtYiti = oldCodeText.Substring(begin + 5, tmpLen);
                            }
                            catch
                            {
                                txtYiti = oldCodeText;// 针对不包含 具体项的
                            }
                        }
                    }
                    else
                    {
                        txtYiti = oldCodeText;// 针对不包含 具体项的
                    }
                    if (txtYiti.Length < 200)
                    {
                        txtYiti = oldCodeText;// 针对不包含 具体项的
                    }

                    //去空格,,,针对 一些关键字加空格的情况
                    if (txtYiti.IndexOf("审议 通过") >= 0)
                    {
                        txtYiti = txtYiti.Replace(" ", "");
                    }
                    string splitKeyWord = "";
                    //按"审议通过"  区分
                    if (txtYiti.IndexOf("审议通过") < 0)
                    {

                        //
                        //Console.WriteLine("不包含   审议通过");
                        if (txtYiti.IndexOf("议案 ") > 0)
                        {
                            splitKeyWord = "议案 ";

                        }
                        //Console.WriteLine("不包含   议案 ");
                        //if (txtYiti.IndexOf("的议案") > 0)
                        //{
                        //    splitKeyWord = "的议案 ";
                        //}
                    }
                    else
                    {
                        splitKeyWord = "审议通过";
                    }

                    int start = 0;
                    while (true)
                    {
                        if (string.IsNullOrEmpty(splitKeyWord) && start == 0)
                        {
                            begin = 0;
                            tmpLen = txtYiti.Length;
                            start = txtYiti.Length + 10;
                        }
                        else
                        {
                            if (start >= txtYiti.Length)
                                break;
                            begin = txtYiti.IndexOf(splitKeyWord, start);
                            if (begin < 0)
                                break;

                            tmpLen = txtYiti.IndexOf(splitKeyWord, begin + 4) - begin;

                            string txt2 = txtYiti.Substring(begin, tmpLen > 0 ? tmpLen : txtYiti.Length - begin);
                            if (tmpLen < 0)
                            {
                                //给最后一次机会(处理最后不包含  审议通过的特殊情况)
                                //比如1股权2选举  20150627-000007.SZ-零七股份：2014年年度股东大会决议公告.pdf中第14条
                                if (ServiceHelp.getStringCount(txt2, "关于") > 1)
                                {
                                    tmpLen = txtYiti.IndexOf("《", begin + 20) - begin;
                                    if (tmpLen < 0)
                                    {
                                        tmpLen = txtYiti.Length - begin;
                                        start = begin + 4;
                                    }
                                    else //if( txtYiti.IndexOf("《", begin + 20) ==txtYiti.LastIndexOf("《", begin + 20))
                                    {

                                        //再加一个判断,截取后应该含有%,否则不算
                                        string abc = txtYiti.Substring(begin, tmpLen);
                                        if (abc.IndexOf("%") > 0 || abc.IndexOf("％") > 0)
                                        {

                                            splitKeyWord = "《";
                                            start = begin + 20;
                                        }
                                        else
                                        {
                                            tmpLen = txtYiti.Length - begin;
                                            start = begin + 4;
                                        }
                                    }
                                }
                                else
                                {
                                    tmpLen = txtYiti.Length - begin;
                                    start = begin + 4;
                                }
                            }
                            else
                            {

                                start = begin + 4;
                            }
                        }
                        string txt1 = txtYiti.Substring(begin, tmpLen);

                        if (splitKeyWord.IndexOf("议案") >= 0)
                        {
                            txt1 = txtYiti.Substring(begin - 20, tmpLen);
                        }
                        processVoteRate(txt1, oldCodeText, stock);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("不用处理") < 0)
                    {
                        Console.WriteLine(txtFile.FullName);
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                    hasError = true;
                    //TODO:这个地方加了个容错,就是如果一个文件有正确结果后再出错则忽略
                    if (beginListCount == listResultOk.Count)
                    {
                        //没找到
                        listResultError.Add(new ResultOk(txtFile.FullName, code, name, date, ex.Message));
                    }
                    else
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    //listResultError.Add(new ResultOk(txtFile.FullName, code, name, date,"分析中出错"));
                }
                if (beginListCount == listResultOk.Count && !hasError)
                {
                    //没找到
                    listResultError.Add(new ResultOk(txtFile.FullName, code, name, date,"没找到关键字,不用处理"));
                }
            }

            //将结果输出到excel中
            string resultFileName = ServiceHelp.printResult(rootDir, listResultOk, listResultError);
            SetLableText(resultFileName, txtResult);

            this.BeginInvoke(new MethodInvoker(delegate()
            {
                btnRead.Enabled = true;
                btnConvert.Enabled = true;
                btnAnaylse.Enabled = true;
            }));
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //试图记下,,,,最后一次选择的目录 路径..尤其是调试时不断输入耽误时间
                StreamWriter sw = new StreamWriter("config.txt",false,  Encoding.Default);
                string w =  txtPdfFolderName.Text+"#"+txtTxtFolder.Text +"#"+txtResult.Text; 
                sw.Write(w);
                sw.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnOpenResult_Click(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txtResult.Text))
                {
                    string path=txtResult.Text;
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
