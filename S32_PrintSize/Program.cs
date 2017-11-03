using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// create by kuan 2017年11月2日21:24:46
/// </summary>
namespace S32_PrintSize
{
    class Program
    {

        static void Main(string[] args)
        {
            int i;
            string tmpExePath = string.Empty;
            string tmpPara = string.Empty;
            //Console.WriteLine("-------------------[Merge plug-in exe ing...]-----------------------------");
            //Console.WriteLine();
            //Console.WriteLine("argc={0}", args.Length);
            for (i = 0; i < args.Length; i++)
            {
                //Console.WriteLine("[argv{0}]={1}", i, args[i]);
                if (i >= 1)
                {
                    if (i == (args.Length-1))
                    {
                        tmpPara += (args[i] + ".elf ");
                    }
                    else
                    {
                        tmpPara += (args[i] + " ");
                    }
                }
                 
            }
            tmpExePath = args[0];

            Console.WriteLine("ExePath={0}", tmpExePath);
            Console.WriteLine("ExePara={0}", tmpPara);


            execFun(tmpExePath, tmpPara);

            //DisposeFun("hehe");

            //Console.ReadKey();
        }
        /// <summary>
        /// 执行exe ref：http://blog.csdn.net/yinxing2008/article/details/18823991 选择同步模式，异步有问题
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="parameters"></param>
        public static void execFun(string exePath, string parameters)
        {
            System.Diagnostics.ProcessStartInfo psi =
            new System.Diagnostics.ProcessStartInfo();
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            psi.FileName = exePath;
            psi.Arguments = parameters;
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi);
            System.IO.StreamReader outputStreamReader = process.StandardOutput;
            System.IO.StreamReader errStreamReader = process.StandardError;
            //process.WaitForExit(500);
            // if (process.HasExited)
            while (false == process.HasExited) { };
            {
                string output = outputStreamReader.ReadToEnd();
                string error = errStreamReader.ReadToEnd();
                Console.WriteLine(output);
                DisposeFun(output);
                Console.WriteLine(error);
            }
        }
  
        /// <summary>
        /// 处理字符串
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="parameters"></param>
        public static void DisposeFun(string exePath)
        {
            string para = exePath;
            int RAM_size = 0;
            int Flash_size = 0;
            //para = "   text	   data	    bss	    dec	    hex	filename\r\n     105204	   2440	  19568	  37312	   91c0	lpspi_transfer_s32k144.elf";
            List<string> tmpList = str2lst(para);

            //RAM size = .data + .bss(不包含堆栈(heap and stack)大小)
            //Flash size = .text + .data
            RAM_size = Convert.ToInt32(tmpList[1]) + Convert.ToInt32(tmpList[2]);
            Flash_size= Convert.ToInt32(tmpList[0]) + Convert.ToInt32(tmpList[1]);

            Console.WriteLine("Flash size =.text +.data =>{0,-8}+{1,-8}={2,-8} B ={3,-12} KB", tmpList[0], tmpList[1], Flash_size, (Flash_size * 1.0 / 1024));
            Console.WriteLine("RAM size   =.data +.bss  =>{0,-8}+{1,-8}={2,-8} B ={3,-12} KB", tmpList[1], tmpList[2], RAM_size,(RAM_size*1.0/1024));



        }
        /// <summary>
        /// ref:https://zhidao.baidu.com/question/872082699778978252.html
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> str2lst(string str)
        {
            List<string> alist = new List<string>();

            System.Text.RegularExpressions.MatchCollection match = System.Text.RegularExpressions.Regex.Matches(str, @"(?<number>(\+|-)?(0|[1-9]\d*)(\.\d*[0-9])?)");


            foreach (System.Text.RegularExpressions.Match mc in match)
            {

                alist.Add(mc.Result("${number}"));
                //Console.WriteLine(alist.Last());
            }
            return alist;
        }
    }
}
