using System;
using System.Net;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace TwitterHashtag
{
    class Program
    {
        static void Main(string[] args)
        {
            TagFinder finder = new TagFinder();
            HashTags tags = new HashTags();
            
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddUserSecrets<Program>()
                .Build();

            IConfigurationSection authorizationToken = config.GetSection("Authorization:BearerToken");
            IConfigurationSection twitterUrl = config.GetSection("TwitterUrl");

            string authHeader = $"Authorization:Bearer {authorizationToken.Value}";
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(twitterUrl.Value);
            request.Headers.Add(authHeader);
            request.KeepAlive = false;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream streamResponse = response.GetResponseStream())
                {
                    using (StreamReader streamRead = new StreamReader(streamResponse))
                    {
                        char[] readBuff = new char[256];
                        int count = 0;
                        int tweetCount = 0;
                        int iteration = 0;
                        do
                        {
                            tweetCount += finder.TweetCount(readBuff);
                            foreach (var tag in finder.Find(readBuff))
                            {
                                tags.Add(tag);
                            }
                            count = streamRead.Read(readBuff, 0, 256);
                            iteration = (iteration + 1) % 20;
                            if (iteration == 19)
                            {
                                Console.Clear();
                                Console.WriteLine("Tweet count: " + tweetCount);
                                foreach (var topTen in tags.GetTopTenWithCounts())
                                {
                                    Console.WriteLine(topTen.Item1 + " - " + topTen.Item2);
                                }
                            }
                        } while (count > 0);
                    }
                }
            }
        }
            
    }
}
