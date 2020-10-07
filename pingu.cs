using System;
using System.Text;
 
using System.Net;
using System.Net.NetworkInformation;
 
using System.Collections.Generic;
 
namespace PingUtility
{
    
    public class BoxBlock{
        public string Title { get; set; }
        public string LineClear { get; set; }
        public string Bar { get; set; }
        public int Length { get; set; }
        public int ProgressBar { get; set; }
        public BoxBlock(string address){
            Title = "--- Ping: " + address + " ----";
            Length = Title.Length + 2;
            ProgressBar = Title.Length - 2;
        }
    }

    public class PingBlock
    {   
        public int X { get; set; }
        public int Y { get; set; }
        public string Address { get; set; }
        public int ProgressBar { get; set; }
        public int BoxLength { get; set; }
        public string BoxBar { get; set; }
        public string BoxLineClear { get; set; }
        public string Title { get; set; }
        public int Faults { get; set; }
        public decimal TotalPings { get; set; }
        public decimal SuccessfulPings { get; set; }
        public decimal FailedPings { get; set; }
        public string LastSuccessful { get; set; }
        public string LastFailure { get; set; }
        public BoxBlock Block { get; set; }
	public long TripTime { get; set; }
        public PingBlock(string address, int x, int y)
        {
            Address = address;
            X = x;
            Y = y;
            ProgressBar = 1;
            Faults = 0;
            SuccessfulPings = 0;
            FailedPings = 0;
            TotalPings = 0;
            Block = new BoxBlock(Address);
            //BuildBlock();

        }
        protected static int origRow;
        protected static int origCol;
        public void BuildBlock(){
            string title = "--- Ping: " + Address + " ----";
            Title = title;
            BoxLength = Title.Length;
            string boxBar = "";
            for (int i=0; i<title.Length-1; i++){
                //boxBar += "-";
                //BoxBar += "-";
                //BoxLineClear += " ";

                Block.Bar += "-";
                Block.LineClear += " ";
            }
            
            // Build the Block
            WriteAt("+", 0, 0);
            WriteAt("|", 0, 1);
            WriteAt("|", 0, 2);
            WriteAt("|", 0, 3);
            WriteAt("+", 0, 4);
 
            WriteAt(Title, 1, 0);
            WriteAt(Block.LineClear, 1, 1);
            //WriteAt("Trip Time: ", 2, 2);
            WriteAt(Block.Bar, 1, 4);
 
            WriteAt("+", Title.Length, 0);
            WriteAt("|", Title.Length, 1);
            WriteAt("|", Title.Length, 2);
            WriteAt("|", Title.Length, 3);
            WriteAt("+", Title.Length, 4);
        }
        public void WriteAt(string s, int x, int y, string color="black")
        {
            if (color == "black"){
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            } else if (color == "red"){
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
            } else if (color == "green"){
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
            } else if (color == "green-text"){
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
            } else if (color == "red-text"){
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
            }
 
        try
            {
            Console.SetCursorPosition(X+x, Y+y);
            Console.Write(s);
            }
        catch (ArgumentOutOfRangeException e)
            {
            Console.Clear();
            Console.WriteLine(e.Message);
            }
 
            // Reset Colors when done
            Console.SetCursorPosition(0,0);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        public bool DoThatPing(string address){
            Ping pingSender = new Ping ();
            PingOptions options = new PingOptions ();
 
            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;
 
            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes (data);
            int timeout = 1000;
	    // Add a try/catch here and send a different notifcation if the ping fails to send
	    try {
            PingReply reply = pingSender.Send (address, timeout, buffer, options);
            
            if (reply.Status == IPStatus.Success)
            {
		        TripTime = reply.RoundtripTime;
                return true;
            } else {
                return false;
            }
            } catch (Exception e) {
                Faults += 1;
                TotalPings += 1;
                WriteAt("Error sending pings!!", Block.Length, 4, "red");
                return false;
            }
        }
        public void Progress(){
            string CurrentTime = DateTime.Now.ToString("hh:mm:ss tt");
            decimal Uptime;
            if (ProgressBar >= Block.ProgressBar){
                WriteAt(Block.LineClear, 1, 3);
                ProgressBar = 1;
                WriteAt(String.Format("Updated: {0}", CurrentTime), Block.Length, 1);
                WriteAt(LastSuccessful, Block.Length, 2, "green-text");
                WriteAt(LastFailure, Block.Length, 3, "red-text");
            }
            string result = "red";
            TotalPings +=1;
            ProgressBar += 1;
            if (DoThatPing(Address)){
                result = "green";
                SuccessfulPings += 1;
                LastSuccessful = String.Format("{0} @ ping #{1}", CurrentTime, TotalPings);
            } else {
                result = "red";
                Faults += 1;
		
                //WriteAt(Faults.ToString(), 12, 2);
                FailedPings += 1;
                LastFailure = String.Format("{0} @ ping #{1}", CurrentTime, TotalPings);
            }
            Uptime = 100 - ((FailedPings / TotalPings) * 100);
            WriteAt(String.Format("{2}/{1} {0:0}%   ", Uptime, TotalPings, Faults), 2, 1);
	        WriteAt(String.Format("{0}ms   ", TripTime), 2, 2);
            WriteAt(" ", ProgressBar, 3, result);
        }
    }
    class PingUtility
    {
        static void Main(string[] args)
        {
 
            string Version = "2020.10.7.11.55";
            string WindowTitle = String.Format("Ping Utility Version {0} Started: {1}", Version, DateTime.Now.ToString("hh:mm:ss tt")); //result 11:11:45 PM
            Console.Title = WindowTitle;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            
            int BlockHeight = 6; // Set the Height of each box and spacer
            int DefaultWidth = 16 + 30; // Block Width + Status message width 
            int ActualWidth = WindowTitle.Length; // Default to the length of the title, so that we can put it at the top/
 
            List<PingBlock> PingList = new List<PingBlock>();
            for (int i = 0; i < args.Length; i++){
                // Ignore anything past 10, since that is all this handles at the moment
                if (i > 10){
                    continue;
                }
                // Set the width to the widest box
                if (args[i].Length + DefaultWidth > ActualWidth){
                    ActualWidth = args[i].Length + DefaultWidth;
                }
                PingList.Add(new PingBlock(args[i], 0, (i * BlockHeight)));
                Console.WindowHeight = (i * BlockHeight) + BlockHeight;
            }

            // Set the Width and redraw
            Console.WindowWidth = ActualWidth;
            Console.BufferWidth = ActualWidth;
            Console.Clear();

            /* Not able to get this to work yet

            // Write the title on the first line
            Console.SetCursorPosition(0, 0);
            Console.Write(WindowTitle);

            */

            // Redraw the blocks
            foreach (PingBlock block in PingList){
                block.BuildBlock();
            }

            // This is to only get rid of the scroll bar
            Console.BufferHeight = Console.WindowHeight;
 
            // The real work
            while (true){
                foreach (PingBlock block in PingList){
                    block.Progress();
                }
                System.Threading.Thread.Sleep(1500);
            }
        }
    }
}