using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MP3_Finder
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declaring how to enter address of website
            Console.WriteLine("Enter WebSite. like : 'http://Music.com'");

            //Getting address of website
            string websiteAddress = Console.ReadLine();

            //Using of our class and functions in it
            Stalker stalker = new Stalker();
            List<string> MP3LINKS = stalker.Stalk(websiteAddress);

            //Writing all mp3 links of website in MP3LINKS.txt file
            StreamWriter SW = new StreamWriter("MP3LINKS.txt");
            foreach (var item in MP3LINKS)
            {
                SW.WriteLine(item);
            }
        }
    }
}
