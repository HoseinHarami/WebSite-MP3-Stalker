using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;




namespace MP3_Finder
{
    class Stalker
    {
        string siteAddress = "http://nex1music.ir";
        List<string> pages = new List<string>();

        //Receives source of a website and returns it in string format
        private string SourceReceiver(string address)
        {
            string sourceCode = "";
            try
            {
                using (WebClient client = new WebClient())
                {
                    sourceCode = client.DownloadString(address);
                }
            }
            catch { }
            return sourceCode;
        }

        //Extracts all links of a page in a website and returns it in List<string> format
        private List<string> LinkExtractor(string inputLink)
        {
            List<string> outputLinks = new List<string>();

            //Receiving source of page
            string siteSource = SourceReceiver(inputLink);

            //finding all <a ... > tags (<a .......> contains links)
            string regex = @"(<a)(.*?)(>)";
            MatchCollection matches = Regex.Matches(siteSource, regex, RegexOptions.IgnoreCase);
            List<string> links = new List<string>();
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                links.Add(match.Groups[0].Value);
            }

            foreach (string link in links)
            {
                //Checking for if link is directed to the site
                if (link.Contains(siteAddress.Substring(8)) && !link.Contains("google") && !link.Contains("twitter") && !link.Contains("facebook"))
                {
                    try
                    {
                        //Puring links (Splitting some letters like : href)
                        string s = link.Substring(link.IndexOf("href") + 5);

                        if (s.IndexOf('"') == 0)
                        {
                            s = s.Substring(s.IndexOf('"') + 1);
                        }
                        s = s.Remove(s.IndexOf('"'));

                        //Checking for if the link is duplicate or not
                        int check = 0;
                        foreach (var item in outputLinks)
                        {
                            if (s == item)
                            {
                                check = 1;
                                break;
                            }
                            else { }
                        }
                        if (check == 0)
                        {
                            //Adding new links (not duplicate links)
                            outputLinks.Add(s);
                        }
                        else { }
                    }
                    catch { }
                }
            }

            //Returning all links that is directed to the website in List<string> format
            return outputLinks;
        }

        //Extracts all pages of a website and returns it in List<string> format
        private List<string> PageExtractor(List<string> inputPages)
        {
            //Last check for recursive function
            if (inputPages.Count == 0)
            {
                //Last return of recursive function , this is what we get from this function in return
                return pages;
            }
            else
            {
                List<string> temp = new List<string>();
                
                //Extracting all links of all input pages and adds it to List<string> pages
                foreach (string page in inputPages)
                {
                    //Extracting all links of a page in inputpages
                    temp = LinkExtractor(page);
                    List<string> tempRemove = new List<string>();

                    //Checking for duplicate links 
                    foreach (string item in temp)
                    {
                        foreach (string item2 in pages)
                        {
                            if (item == item2)
                            {
                                //Adding duplicate items to List<string> tempRemove
                                tempRemove.Add(item);
                            }
                            else { }
                        }
                    }
                    //Removing duplicate links
                    foreach (string item in tempRemove)
                    {
                        temp.Remove(item); 
                    }

                    //Adding links to List<string> pages
                    pages.AddRange(temp);
                }

                //This return is where this function changes to recursive
                return PageExtractor(temp);
            }
        }

        //Extracts all mp3 links of a website and returns it in List<string> format
        private List<string> MP3Extractor(string Site)
        {
            //Changing string Site to List<string> site
            List<string> site = new List<string>();
            site.Add(Site);
            //Extracting all pages of website
            List<string> pages = PageExtractor(site);
            List<string> mp3Links = new List<string>();

            //Extracting all mp3 links of every pages
            foreach (string page in pages)
            {
                //Receiving page source
                string siteSource = SourceReceiver(page);

                //Finding all mp3 links in page with Regular Expression
                string regex = @"(http://)?(www)?[-a-zA-Z0-9@:%_+.~#?/=ا-ی]+.mp3";
                MatchCollection matches = Regex.Matches(siteSource, regex, RegexOptions.IgnoreCase);
                List<string> temp = new List<string>();
                //First checking for duplicate links and then adding to List<string> temp
                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    //Checking for duplicate links
                    int x = 0;
                    foreach (string item in mp3Links)
                    {
                        if (item == match.Groups[0].Value)
                        {
                            x = 1;
                        }
                        else { }
                    }
                    if (x != 1)
                    {
                        //Adding not duplicate links
                        mp3Links.Add(match.Groups[0].Value);
                    }
                    else { }
                }
            }
            //returning all mp3 links of website
            return mp3Links;
        }

        //The function that we use in Program.cs
        public List<string> Stalk (string SiteAddress)
        {
            //Set's SiteAddress to siteAddress for using in other functions
            siteAddress = SiteAddress;
            
            //Returns all mp3 links
            return MP3Extractor(SiteAddress);
        }
    }
}
