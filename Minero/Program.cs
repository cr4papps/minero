using System;
using System.Configuration;

namespace Minero
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                var miner = new Miner();
                string miningInfo = miner.GetSelectedMiningInfo(ConfigurationManager.AppSettings["SelectedMiningAPI"]);
                if (miningInfo != "") {
                    miner.Mine(miningInfo);
                } else {
                    Console.Write("Empty mining info, nothing to do. ");
                }
                Console.Write("Press any key to continue...");
                Console.ReadLine();
            } catch (Exception ex) {
                Console.WriteLine("[ERROR]: " + ex.Message);
            }
        }
    }
}
