using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
 
namespace WinTest
{
    public static class ServiceHelp
    {

        public static string printResult(string folder, List<ResultOk> listResultOk, List<ResultOk> listResultError)
        {
            string resultXlsFileName = "result" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string strFilePath = System.IO.Path.Combine(folder, resultXlsFileName);
            FileInfo file = new FileInfo(strFilePath);
            //应该是只有一条记录
            string tmp = "";

            //将连着的一样的文件,单独列入一个sheet内
            List<ResultOk> listResultTheSame = new List<ResultOk>();
            for (int i = 1; i < listResultOk.Count; i++)
            {
                //如果连着两条记录完全一样,则加到 listResultTheSame 中
                ResultOk before = listResultOk[i - 1];
                ResultOk now = listResultOk[i];

                //比较是否一样
                if (before.fileName == now.fileName)
                {
                    if (before.code == now.code)
                    {
                        if (before.date == now.date)
                        {
                            if (before.remark == now.remark)
                                    {
                                        if (before.agree == now.agree)
                                        {
                                            if (before.notagree == now.notagree)
                                            {
                                                if (before.forget == now.forget)
                                                {
                                                    //认为是一样的,则加入到 另一个列表中
                                                    listResultTheSame.Add(now);
                                                }
                                            }
                                        }
                            }
                        }
                    }
                }
            }

            using (ExcelPackage package = new ExcelPackage(file))
            {

                bool isPdfExit = false;
                string parentPath = "";
                if (listResultOk.Count > 0 && !string.IsNullOrEmpty(listResultOk[0].fileName))
                {
                    //查看上层目录下的pdf是否存在,存在则文件 用pdf的全路径
                    string fileTmp = listResultOk[0].fileName;
                    parentPath = fileTmp.Substring(0,fileTmp.LastIndexOf("\\"));
                    if (parentPath.LastIndexOf("\\")>0)
                    {
                        parentPath=parentPath.Substring(0,parentPath.LastIndexOf("\\"))+"\\";
                    }  

                    string pdf =  parentPath+ fileTmp.Substring(fileTmp.LastIndexOf("\\"),fileTmp.LastIndexOf(".")-fileTmp.LastIndexOf("\\"))+".pdf"    ;

                    if (File.Exists(pdf))
                    {
                        isPdfExit = true;
                    }
                }

                //第一个sheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("result");
                //特殊的几个位置
                worksheet.Cells["A1"].Value = "name";
                worksheet.Cells["B1"].Value = "date";
                worksheet.Column(2).Width = 20;
                worksheet.Cells["C1"].Value = "关键字";
                worksheet.Cells["D1"].Value = "赞成%";
                worksheet.Cells["E1"].Value = "反对%";
                worksheet.Cells["F1"].Value = "弃权%";
                worksheet.Cells["G1"].Value = "备注";

               // worksheet.Cells["H1"].Value = "证券名";
                worksheet.Cells["H1"].Value = "txt文件名";
                worksheet.Cells["I1"].Value = "pdf文件名";

                worksheet.Column(8).Width = 60;
                worksheet.Column(9).Width = 60;                                                
                //将listResult打印出来
                for (int i = 0; i < listResultOk.Count; i++)
                {
                    ResultOk stock = listResultOk[i];
                    //Console.WriteLine(stock.code + "__" + stock.name + "__" + stock.date + "__" + stock.doWhat + "___" + stock.agree + "__" + stock.notagree + "__" + stock.forget + "__" + stock.remark);
                    worksheet.Cells[i + 2, 1].Value = stock.code;
                    worksheet.Cells[i + 2, 2].Value = stock.date;
                    worksheet.Cells[i + 2, 3].Value = stock.doCode;
                    worksheet.Cells[i + 2, 4].Value = stock.agree;
                    worksheet.Cells[i + 2, 5].Value = stock.notagree;
                    worksheet.Cells[i + 2, 6].Value = stock.forget;
                    worksheet.Cells[i + 2, 7].Value = stock.remark;
                    //worksheet.Cells[i + 2, 8].Value = stock.name;

                    string fileName1 = stock.fileName.Substring(stock.fileName.LastIndexOf("\\") + 1, stock.fileName.LastIndexOf(".") - stock.fileName.LastIndexOf("\\")-1);
                    worksheet.Cells[i + 2, 8].Formula = "HYPERLINK(\"" + stock.fileName + "\",\"" + fileName1 + ".txt\")";
                    //worksheet.Cells[i + 2, 9].Value = "=HYPERLINK(\"" + stock.fileName + "\")";

                    try
                    {
                        if (isPdfExit)
                        {
                            string pdf = parentPath + fileName1  + ".pdf";
                            worksheet.Cells[i + 2, 9].Formula = "HYPERLINK(\"" + parentPath + fileName1 + ".pdf\",\"" + fileName1 + ".pdf\")";
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                }
 

                //第2个sheet
                ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add("Error");
                //特殊的几个位置
                worksheet2.Cells["A1"].Value = "name";
                worksheet2.Cells["B1"].Value = "date";
                worksheet2.Column(2).Width = 20;
            
                worksheet2.Cells["C1"].Value = "备注";
                worksheet2.Column(3).Width = 30;
                worksheet2.Cells["D1"].Value = "文件名";
                worksheet2.Column(2).Width = 40;
                // worksheet.Cells["I1"].Value = "文件名";
                //CEMS运维联系人：                                                                   联系电话/手机： 
                //将listResult打印出来
                for (int i = 0; i < listResultError.Count; i++)
                {
                    ResultOk stock = listResultError[i];
 
                    if(true)// (stock.remark.IndexOf("不用处理") < 0)
                    {
                        if (!Directory.Exists(folder + "\\a\\"))
                        {
                            Directory.CreateDirectory(folder + "\\a\\");
                        }
                        File.Copy(stock.fileName, folder + "\\a\\" + stock.fileName.Replace(folder, ""), true);
                    }


                    string fileName1 = stock.fileName.Substring(stock.fileName.LastIndexOf("\\") + 1, stock.fileName.LastIndexOf(".") - stock.fileName.LastIndexOf("\\") - 1);
                
                   // Console.WriteLine(stock.code + "__" + stock.name + "__" + stock.date + "__" + stock.doWhat + "___" + stock.agree + "__" + stock.notagree + "__" + stock.forget + "__" + stock.remark);

                    worksheet2.Cells[i + 2, 1].Value = stock.code;
                    worksheet2.Cells[i + 2, 2].Value = stock.date;
                    worksheet2.Cells[i + 2, 4].Formula = "HYPERLINK(\"" + stock.fileName + "\",\""+ fileName1 +".txt\")";
                   // worksheet2.Cells[i + 2, 4].Value = "=HYPERLINK(\"" + stock.fileName + "\")";
                    worksheet2.Cells[i + 2, 3].Value = stock.remark;
                    //worksheet.Cells[i + 2, 1].Value = stock.code;
                    //将错误文件移到到一个独立的文件夹中
                      try
                    {
                        if (isPdfExit)
                        {
                            string pdf = parentPath + fileName1  + ".pdf";
                            worksheet.Cells[i + 2, 3].Formula = "HYPERLINK(\"" + parentPath + fileName1 + "\",\"" + fileName1 + ".pdf\")";
                        }
                    }
                    catch (Exception)
                    {
                        
                    }


                }







                //第一个sheet
                ExcelWorksheet worksheet3 = package.Workbook.Worksheets.Add("resultSame");
                //特殊的几个位置
                worksheet3.Cells["A1"].Value = "name";
                worksheet3.Cells["B1"].Value = "date";
                worksheet3.Column(2).Width = 20;
                worksheet3.Cells["C1"].Value = "关键字";
                worksheet3.Cells["D1"].Value = "赞成%";
                worksheet3.Cells["E1"].Value = "反对%";
                worksheet3.Cells["F1"].Value = "弃权%";
                worksheet3.Cells["G1"].Value = "备注";

                // worksheet3.Cells["H1"].Value = "证券名";
                worksheet3.Cells["H1"].Value = "txt文件名";
                worksheet3.Cells["I1"].Value = "pdf文件名";

                worksheet3.Column(8).Width = 60;
                worksheet3.Column(9).Width = 60;
                //将listResult打印出来
                for (int i = 0; i < listResultTheSame.Count; i++)
                {
                    ResultOk stock = listResultTheSame[i];
                    //Console.WriteLine(stock.code + "__" + stock.name + "__" + stock.date + "__" + stock.doWhat + "___" + stock.agree + "__" + stock.notagree + "__" + stock.forget + "__" + stock.remark);
                    worksheet3.Cells[i + 2, 1].Value = stock.code;
                    worksheet3.Cells[i + 2, 2].Value = stock.date;
                    worksheet3.Cells[i + 2, 3].Value = stock.doCode;
                    worksheet3.Cells[i + 2, 4].Value = stock.agree;
                    worksheet3.Cells[i + 2, 5].Value = stock.notagree;
                    worksheet3.Cells[i + 2, 6].Value = stock.forget;
                    worksheet3.Cells[i + 2, 7].Value = stock.remark;
                    //worksheet3.Cells[i + 2, 8].Value = stock.name;

                    string fileName1 = stock.fileName.Substring(stock.fileName.LastIndexOf("\\") + 1, stock.fileName.LastIndexOf(".") - stock.fileName.LastIndexOf("\\") - 1);
                    worksheet3.Cells[i + 2, 8].Formula = "HYPERLINK(\"" + stock.fileName + "\",\"" + fileName1 + ".txt\")";
                    //worksheet3.Cells[i + 2, 9].Value = "=HYPERLINK(\"" + stock.fileName + "\")";

                    try
                    {
                        if (isPdfExit)
                        {
                            string pdf = parentPath + fileName1 + ".pdf";
                            worksheet3.Cells[i + 2, 9].Formula = "HYPERLINK(\"" + parentPath + fileName1 + ".pdf\",\"" + fileName1 + ".pdf\")";
                        }
                    }
                    catch (Exception)
                    {

                    }
                }



                package.Save();

            }
            return strFilePath;
           // SetLableText(strFilePath, txtResult);
            //this.txtResult.Text = strFilePath;
        }


        public static int getStringCount(string txtString, string keyWord)
        {
            if (txtString.IndexOf(keyWord) < 0)
            {
                return 0;
            }
            int count = 0;
            int start = 0;
            while (txtString.IndexOf(keyWord, start) >= 0)
            {
                count++;
                start = txtString.IndexOf(keyWord, start) + 1;
            }

            return count;
        }
        //关键字一个都没有,直接return
        public static string getDoWhat(string txt1)
        {
            string doWhat = "";
            txt1 = txt1.Replace(" ", "");
            txt1 = txt1.Replace("\r\n", "");
            if (txt1.IndexOf("提名") > 0 && txt1.IndexOf("选举") > 0)
            {
                if (txt1.IndexOf("提名") < txt1.IndexOf("选举"))
                    doWhat = "1提名";
                else
                    doWhat = "3选举";

                if (txt1.IndexOf("非独立董事") < 0 && txt1.IndexOf("独立董事") > 0)
                {
                    doWhat += "_独立董事";
                }
            }
            else if (txt1.IndexOf("提名") > 0)
            {
                doWhat = "1提名";

                if (txt1.IndexOf("非独立董事") < 0 && txt1.IndexOf("独立董事") > 0)
                {
                    doWhat += "_独立董事";
                }
            }
            //增补  2增补
            else if (txt1.IndexOf("增补") > 0 && (txt1.IndexOf("增补审议") != txt1.IndexOf("增补")))
            {


                doWhat = "2增补";
                if (txt1.IndexOf("非独立董事") < 0 && txt1.IndexOf("独立董事") > 0)
                {
                    doWhat += "_独立董事";
                }
            }
            //选举  3选举
            else if (txt1.IndexOf("选举") > 0)
            {
                doWhat = "3选举";
                if (txt1.IndexOf("非独立董事") < 0 && txt1.IndexOf("独立董事") > 0)
                {
                    doWhat += "_独立董事";
                }
            }
            //担保	4担保
            else if (txt1.IndexOf("提供担保") > 0 || txt1.IndexOf("担保") > 0)
            {
                doWhat = "4担保";
            }
            //并购	5股权            竞购  30%股权              -表决结果


            else if (txt1.IndexOf("竞购") > 0 || txt1.IndexOf("处置") > 0 || txt1.IndexOf("受让") > 0
          || txt1.IndexOf("购买") > 0 || txt1.IndexOf("回购") > 0 || txt1.IndexOf("收购") > 0)
            {

                if (txt1.IndexOf("股权") > 0)
                {
                    doWhat = "5股权";
                }

                else
                {
                    if (txt1.IndexOf("资产") > 0 && txt1.IndexOf("资产管理") < 0)
                    {
                        doWhat = "6资产";
                    }
                }

            }
            else if (txt1.IndexOf("股权") > 0)
            {
                doWhat = "5股权";
            }
            else if (txt1.IndexOf("资产") > 0 && txt1.IndexOf("资产管理") < 0)
            {
                doWhat = "6资产";
            }


            return doWhat;
        }










        public static string getName(string oldCodeText)
        {

            string name = "";


            int begin = 0;
            int tmpLen = 5;
            string nameTitle = "";
            oldCodeText = oldCodeText.Replace(" ", "");
            if (oldCodeText.IndexOf("证券简称") >= 0)
            {
                nameTitle = "证券简称";
            }
            else if (oldCodeText.IndexOf("股票简称") > 0)
            {
                nameTitle = "股票简称";
            }
            else if (oldCodeText.IndexOf("证券减持") > 0)
            {
                nameTitle = "证券减持";
            }

            if (!string.IsNullOrEmpty(nameTitle))
            {
                begin = oldCodeText.IndexOf(nameTitle) + 5;
                tmpLen = 3;
                while (!string.IsNullOrEmpty(oldCodeText.Substring(begin + tmpLen, 1).Trim())     )
                {
                    tmpLen++;
                    if (tmpLen > 15)
                        break;
                }
                name = oldCodeText.Substring(begin, tmpLen).Trim();
                if (tmpLen < 3 || name.IndexOf("ST") >= 0)
                    tmpLen += 4;
                name = oldCodeText.Substring(begin, tmpLen).Trim();


            }
            if (name.IndexOf("公告") > 0)
            {
                name = name.Substring(0, name.IndexOf("公告"));
            }
            return name;
        }
        public static string getDate(string oldCodeText)
        {

                string date = "";

            int begin = 0;
            int tmpLen = 5;
            try
            {

                oldCodeText = oldCodeText.Trim().Replace(" ", "").Replace("\r\n", "");

                if (oldCodeText.IndexOf("召开日期") > 0)
                {

                    begin = oldCodeText.IndexOf("召开日期") + 5;
                    tmpLen = oldCodeText.IndexOf("日", begin) + 1 - begin;
                    date = oldCodeText.Substring(begin, tmpLen).Trim().Replace(" ", "").Replace("\r\n", "");
                    //时间在  召开关键字之后的
                    date = date.Substring(date.IndexOf("年") - 4, 4) + "/" + date.Substring(date.IndexOf("年") + 1, date.IndexOf("月") - date.IndexOf("年") - 1) + "/" + date.Substring(date.IndexOf("月") + 1, date.IndexOf("日") - date.IndexOf("月") - 1);
                }
                else if (oldCodeText.IndexOf("会议于") > 0)
                {

                    begin = oldCodeText.IndexOf("会议于") + 3;
                    tmpLen = oldCodeText.IndexOf("日", begin) + 1 - begin;
                    date = oldCodeText.Substring(begin, tmpLen).Trim().Replace(" ", "").Replace("\r\n", "");
                    //时间在  召开关键字之后的
                    date = date.Substring(date.IndexOf("年") - 4, 4) + "/" + date.Substring(date.IndexOf("年") + 1, date.IndexOf("月") - date.IndexOf("年") - 1) + "/" + date.Substring(date.IndexOf("月") + 1, date.IndexOf("日") - date.IndexOf("月") - 1);
                }
                else if (oldCodeText.IndexOf("召开时间") > 0)
                {

                    begin = oldCodeText.IndexOf("召开时间") + 5;
                    tmpLen = oldCodeText.IndexOf("日", begin) + 1 - begin;
                    date = oldCodeText.Substring(begin, tmpLen).Trim().Replace(" ", "").Replace("\r\n", "");
                    //时间在  召开关键字之后的
                    date = date.Substring(date.IndexOf("年") - 4, 4) + "/" + date.Substring(date.IndexOf("年") + 1, date.IndexOf("月") - date.IndexOf("年") - 1) + "/" + date.Substring(date.IndexOf("月") + 1, date.IndexOf("日") - date.IndexOf("月") - 1);
                }
                else if (oldCodeText.IndexOf("会议时间") > 0)
                {

                    begin = oldCodeText.IndexOf("会议时间") + 5;
                    tmpLen = oldCodeText.IndexOf("日", begin) + 1 - begin;
                    date = oldCodeText.Substring(begin, tmpLen).Trim().Replace(" ", "").Replace("\r\n", "");
                    //时间在  召开关键字之后的
                    date = date.Substring(date.IndexOf("年") - 4, 4) + "/" + date.Substring(date.IndexOf("年") + 1, date.IndexOf("月") - date.IndexOf("年") - 1) + "/" + date.Substring(date.IndexOf("月") + 1, date.IndexOf("日") - date.IndexOf("月") - 1);
                }
               
                else if (oldCodeText.IndexOf("会于") > 0)
                {

                    begin = oldCodeText.IndexOf("会于") + 2;
                    tmpLen = oldCodeText.IndexOf("日", begin) + 1 - begin;
                    date = oldCodeText.Substring(begin, tmpLen).Trim().Replace(" ", "").Replace("\r\n", "");
                    //时间在  召开关键字之后的
                    date = date.Substring(date.IndexOf("年") - 4, 4) + "/" + date.Substring(date.IndexOf("年") + 1, date.IndexOf("月") - date.IndexOf("年") - 1) + "/" + date.Substring(date.IndexOf("月") + 1, date.IndexOf("日") - date.IndexOf("月") - 1);
                }
                
                if (!string.IsNullOrEmpty(date))
                {
                    return date;
                }

                string alterKey = "方式召开，";


                if (oldCodeText.IndexOf("现场召开，") > 0)
                {
                    alterKey = "现场召开，";
                }
                else if (oldCodeText.IndexOf("方式召开，") > 0)
                {
                    alterKey = "方式召开，";
                }
                else if (oldCodeText.IndexOf("方式召开") > 0)
                {
                    alterKey = "方式召开";
                }
                else
                {
                    date = oldCodeText.Trim().Replace(" ", "").Replace("\r\n", "");

                    int tt = 0;
                    string year = date.Substring(date.IndexOf("年") - 4, 4);
                    string month = date.Substring(date.IndexOf("月") - 2, 2);
                    if (!int.TryParse(month, out tt))
                    {
                        month = date.Substring(date.IndexOf("月") - 1,1);
                    }

                    string day = date.Substring(date.IndexOf("月") + 1, date.IndexOf("日", date.IndexOf("月")) - date.IndexOf("月") - 1);
                    date = year + "/" + month + "/" + day;
                    return date;
                }

                begin = oldCodeText.IndexOf(alterKey);

                date = oldCodeText.Substring(0, begin).Trim().Replace(" ", "").Replace("\r\n", "");
                //时间在  召开关键字之前
                //TODO:一个错误忽略的处理,,,遇到召开之前没有年月日的,已之后的为准
                if (date.LastIndexOf("年") - 4 < 0 ||
                    date.LastIndexOf("月") < 0 || date.LastIndexOf("月") < 0)
                {
                    // 以第一个时间开始处理
                    date = oldCodeText;
                    date = date.Substring(date.IndexOf("年") - 4, 4) + "/" + date.Substring(date.IndexOf("年") + 1, date.IndexOf("月") - date.IndexOf("年") - 1) + "/" + date.Substring(date.IndexOf("月") + 1, date.IndexOf("日") - date.IndexOf("月") - 1);

                }
                else
                {
                    date = date.Substring(date.LastIndexOf("年") - 4, 4) + "/" + date.Substring(date.LastIndexOf("年") + 1, date.LastIndexOf("月") - date.LastIndexOf("年") - 1) + "/" + date.Substring(date.LastIndexOf("月") + 1, date.LastIndexOf("日") - date.LastIndexOf("月") - 1);
                }

                return date;
            }
            catch (Exception ex)
            {
                Console.WriteLine("日期出错");


                // string  date = oldCodeText.Trim().Replace(" ", "").Replace("\r\n","");
                //时间在  召开关键字之后的
                // date = date.Substring(date.IndexOf("年") - 4, 4) + "/" + date.Substring(date.IndexOf("年") + 1, date.IndexOf("月") - date.IndexOf("年") - 1) + "/" + date.Substring(date.IndexOf("月") + 1, date.IndexOf("日") - date.IndexOf("月") - 1);

                return "";
            }
        }

        public static string getCode(string oldCodeText)
        {
            string code = "";
            int begin = 0;
            int tmpLen = 5;
            string keyWord = "证券代号";
            oldCodeText = oldCodeText.Replace(" ", "");
            if (oldCodeText.IndexOf("证券代码") >= 0)
            {
                keyWord = "证券代码";
                
            }
            else if (oldCodeText.IndexOf("证券代号") >= 0)
            {
                keyWord = "证券代号";
            }
            else if (oldCodeText.IndexOf("股票代码") >= 0)
            {
                keyWord = "股票代码";
            }
           
            else if (oldCodeText.IndexOf("股票代号") >= 0)
            {
                keyWord = "股票代号";
            }
            else
            {
                return "未找到关键字:证券代码,证券代号,股票代码,股票代号";
            }
            if (oldCodeText.IndexOf(keyWord) >= 0)
            {
                begin = oldCodeText.IndexOf(keyWord) + 4;
                while (!int.TryParse(oldCodeText.Substring(begin, 1), out tmpLen))
                {
                    begin++;
                }
                int len = 1;
                while (int.TryParse(oldCodeText.Substring(begin, len), out tmpLen))
                {
                    len++;
                }
                if (len > 6)
                {
                    len = 6;
                }
                code = oldCodeText.Substring(begin, len).Trim();
            }
            return code;
        }


        public static decimal getPersentNum(string txt1, string keyWord)
        {
            try
            {
                txt1 = txt1.Replace(",", "").Replace("\r\n", "").Replace(" ", "");
                if (keyWord.StartsWith("票"))
                {
                    //则直接读取票前面的数字
                    //查找同意后的第一个 百分比
                    int s = txt1.IndexOf(keyWord);
                    if (s < 0)
                        return 0;
                    //查找%前的数字
                    decimal pp = 0;
                    int a = s - 1;
                    while (decimal.TryParse(txt1.Substring(a, 1), out pp) || txt1.Substring(a, 1) == ".")
                    {
                        a--;
                    }
                    if (decimal.TryParse(txt1.Substring(a + 1, s - a - 1), out pp))
                    {
                        //Console.WriteLine(pp);
                        return pp;
                    }
                    return 0;
                }


                //如果 同意需要再反对前,,如果是 反对需要在弃权之前
                if (keyWord.IndexOf("反对") >= 0)
                {
                    if (txt1.IndexOf("弃权", txt1.IndexOf(keyWord)) - txt1.IndexOf(keyWord) > 0)
                    {
                        txt1 = txt1.Substring(txt1.IndexOf(keyWord), txt1.IndexOf("弃权", txt1.IndexOf(keyWord)) - txt1.IndexOf(keyWord));
                    }
                }
                else if (keyWord.IndexOf("弃权") >= 0)
                {
                    // txt1 = txt1.Substring(txt1.IndexOf(keyWord), txt1.IndexOf("弃权"));
                }
                else
                {
                    if (txt1.IndexOf("反对", txt1.IndexOf(keyWord)) - txt1.IndexOf(keyWord) > 0)
                    {

                        txt1 = txt1.Substring(txt1.IndexOf(keyWord), txt1.IndexOf("反对", txt1.IndexOf(keyWord)) - txt1.IndexOf(keyWord));
                    }
                }

                if (txt1.IndexOf(keyWord) >= 0)
                {
                    //查找同意后的第一个 百分比
                    int s = txt1.IndexOf("％", txt1.IndexOf(keyWord) + keyWord.Length);
                    if (s < 0)
                    {
                        s = txt1.IndexOf("%", txt1.IndexOf(keyWord) + keyWord.Length);
                    }
                    if (s < 0)
                    {
                        s = txt1.IndexOf("票", txt1.IndexOf(keyWord) + keyWord.Length);
                    }
                    if (s < 0)
                    {
                        s = txt1.IndexOf("股", txt1.IndexOf(keyWord) + keyWord.Length);
                    }
                    if (s < 0)
                        return 0;
                    //查找%前的数字
                    decimal pp = 0;
                    int a = s - 1;
                    while (decimal.TryParse(txt1.Substring(a, 1), out pp) || txt1.Substring(a, 1) == ".")
                    {
                        a--;
                    }
                    if (decimal.TryParse(txt1.Substring(a + 1, s - a - 1), out pp))
                    {
                        //Console.WriteLine(pp);
                        return pp;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

         public static decimal getPersent(string txt1, string keyWord )
        {
            return getPersent(txt1, keyWord, "");
         }
        public static decimal getPersent(string txt1, string keyWord,string numOrPersent )
        {
            try
            {
            txt1 = txt1.Replace(",", "").Replace("\r\n", "").Replace(" ", "");
            if (keyWord.StartsWith("票"))
            {
                //则直接读取票前面的数字
                //查找同意后的第一个 百分比
                int s = txt1.IndexOf(keyWord);
                if (s < 0)
                    return 0;
                //查找%前的数字
                decimal pp = 0;
                int a = s - 1;
                while (decimal.TryParse(txt1.Substring(a, 1), out pp) || txt1.Substring(a, 1) == ".")
                {
                    a--;
                }
                if (decimal.TryParse(txt1.Substring(a + 1, s - a - 1), out pp))
                {
                    //Console.WriteLine(pp);
                    return pp;
                }
                return 0;
            }


            //如果 同意需要再反对前,,如果是 反对需要在弃权之前
            if (keyWord.IndexOf("反对") >= 0)
            {
                if (txt1.IndexOf("弃权", txt1.IndexOf(keyWord)) - txt1.IndexOf(keyWord) > 0)
                {
                    txt1 = txt1.Substring(txt1.IndexOf(keyWord), txt1.IndexOf("弃权", txt1.IndexOf(keyWord)) - txt1.IndexOf(keyWord));
                }
            }
            else if (keyWord.IndexOf("弃权") >= 0)
            {
                // txt1 = txt1.Substring(txt1.IndexOf(keyWord), txt1.IndexOf("弃权"));
            }
            else
            {
                if (txt1.IndexOf("反对", txt1.IndexOf(keyWord)) - txt1.IndexOf(keyWord) > 0)
                {

                    txt1 = txt1.Substring(txt1.IndexOf(keyWord), txt1.IndexOf("反对", txt1.IndexOf(keyWord)) - txt1.IndexOf(keyWord));
                }
            }

            if (txt1.IndexOf(keyWord) >= 0)
            {
                //查找同意后的第一个 百分比
                int s = -1;
                bool isPersent = true;

                if (string.IsNullOrEmpty(numOrPersent))
                {
                    s = txt1.IndexOf("％", txt1.IndexOf(keyWord) + keyWord.Length);
                    if (s < 0)
                    {
                        s = txt1.IndexOf("%", txt1.IndexOf(keyWord) + keyWord.Length);
                    }
                    if (s < 0)
                    {
                        isPersent = false;
                        s = txt1.IndexOf("票", txt1.IndexOf(keyWord) + keyWord.Length);
                    }
                    if (s < 0)
                    {
                        isPersent = false;
                        s = txt1.IndexOf("股", txt1.IndexOf(keyWord) + keyWord.Length);
                    }
                }
                else
                {
                    isPersent = false;
                    s = txt1.IndexOf("票", txt1.IndexOf(keyWord) + keyWord.Length);
                    if (s < 0)
                    {
                        s = txt1.IndexOf("股", txt1.IndexOf(keyWord) + keyWord.Length);
                    }
                }
                if (s < 0)
                    return 0;
                //查找%前的数字
                decimal pp = 0;
                int a = s - 1;
                while (decimal.TryParse(txt1.Substring(a, 1), out pp) || txt1.Substring(a, 1) == ".")
                {
                    a--;
                }


                string strPP = txt1.Substring(a + 1, s - a - 1);
                decimal.TryParse(strPP, out pp);
                if (isPersent)
                {
                    //是persent不能大于100
                    if (strPP.IndexOf(".") > 0  && pp>100)
                    {
                        strPP = strPP.Substring(strPP.IndexOf(".") - 2);
                        decimal.TryParse(strPP, out pp);
                    }
                }

                return pp;
               
            }
            return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
