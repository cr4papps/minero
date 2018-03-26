using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Minero
{
    public class Miner
    {
        public Miner()
        {
        }
        
        public string GetSelectedMiningInfo(string url)
        {
            try {
            using (WebResponse response = WebRequest.Create(url).GetResponse()) {
                using (var reader = new StreamReader(response.GetResponseStream())) {
                    var o = JObject.Parse(reader.ReadToEnd());
                    return o["miningInfo"].ToString();
                }
            }
            } catch {
                return "";
            }
        }

        void ProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
        
        public void Mine(string miningInfo)
        {
            string cpuMiner = "cpuminer\\cpuminer-gw64-core2.exe";
            if (Environment.ProcessorCount > 4) {
                cpuMiner = "cpuminer\\cpuminer-gw64-corei7.exe";
            }
            var cpuProcess = GetProcess(cpuMiner, miningInfo);
            cpuProcess.Start();
            cpuProcess.BeginOutputReadLine();
            
            var gpuProcess = GetProcess("ccminer\\ccminer-x64.exe", miningInfo);
            gpuProcess.Start();
            gpuProcess.BeginOutputReadLine();
        }
        
        Process GetProcess(string minerFileName, string arguments)
        {
            var process = new Process();
            string fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), minerFileName);
            try {
                process.StartInfo = new ProcessStartInfo {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                process.EnableRaisingEvents = true;
                process.OutputDataReceived += ProcessOutputDataReceived;
            } catch (Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
            }
            return process;
        }
    }
}
