using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitterHashtag
{
    public class HashTags
    {
        int topCount = 10;
        List<string> top = new List<string>();
        int min;
        Dictionary<string, int> topIndex = new Dictionary<string, int>();
        Dictionary<string, int> counts = new Dictionary<string, int>();

        public IEnumerable<string> GetTopTen()
        {
            return top;
        }

        public IEnumerable<Tuple<string, int>> GetTopTenWithCounts()
        {
            return top.Select(x => new Tuple<string, int>(x, counts[x]));
        }

        private void PromoteTag(string hashtag)
        {
            var count = counts[hashtag];
            if (topIndex[hashtag] > 0)
            {
                string nextTag = top[topIndex[hashtag] - 1];
                while (counts[nextTag] < count)
                {
                    topIndex[hashtag]--;
                    topIndex[nextTag]++;
                    top[topIndex[hashtag]] = hashtag;
                    top[topIndex[nextTag]] = nextTag;
                    if (topIndex[hashtag] == 0)
                    {
                        break;
                    }
                    nextTag = top[topIndex[hashtag] - 1];
                }
            }
        }

        public void Add(string hashtag)
        {
            int count = 1;
            if (counts.ContainsKey(hashtag))
            {
                count = ++counts[hashtag];
            } 
            else
            {
                counts[hashtag] = 1;
            }
            if (top.Count < topCount || count > min)
            {
                if (topIndex.ContainsKey(hashtag))
                {
                    PromoteTag(hashtag);
                } 
                else
                {
                    if (top.Count < topCount)
                    {
                        top.Add(hashtag);
                        topIndex.Add(hashtag, top.Count() - 1);
                    } 
                    else
                    {
                        topIndex.Remove(top[top.Count() - 1]);
                        top[top.Count() - 1] = hashtag;
                        topIndex.Add(hashtag, top.Count() - 1);
                        PromoteTag(hashtag);
                    }
                    
                }
                min = counts[top[top.Count - 1]];
            }
        }
    }
}
