using System.Collections.Generic;

namespace TwitterHashtag
{
    class TagFinder
    {
        private readonly List<byte> sb = new List<byte>();
        int stage = 0;

        public IEnumerable<string> Find(char[] chars)
        { 
            for (int i = 0; i < chars.Length; i++)
            {
                char curr = chars[i];
                // Finite-state machine to find a sequence from streaming characters ("tag":"xxxxxx")
                switch (stage)
                {
                    case 0:
                        stage = (curr == '"') ? stage + 1 : 0;
                        break;
                    case 1:
                        stage = (curr == 't') ? stage + 1 : 0;
                        break;
                    case 2:
                        stage = (curr == 'a') ? stage + 1 : 0;
                        break;
                    case 3:
                        stage = (curr == 'g') ? stage + 1 : 0;
                        break;
                    case 4:
                        stage = (curr == '"') ? stage + 1 : 0;
                        break;
                    case 5:
                        stage = (curr == ':') ? stage + 1 : 0;
                        break;
                    case 6:
                        stage = (curr == '"') ? stage + 1 : 0;
                        break;
                    case 7:
                        if (curr == '"')
                        {
                            yield return System.Text.Encoding.UTF8.GetString(sb.ToArray());
                            sb.Clear();
                            stage = 0;
                        }
                        else
                        {
                            sb.Add((byte)curr);
                        }
                        break;
                }
            }
        }

        public int TweetCount(char[] chars)
        {
            int count = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                char curr = chars[i];
                // Finite-state machine to find a sequence from streaming characters ("data":)
                switch (stage)
                {
                    case 0:
                        stage = (curr == '"') ? stage+1 : 0;
                        break;
                    case 1:
                        stage = (curr == 'd') ? stage + 1 : 0;
                        break;
                    case 2:
                        stage = (curr == 'a') ? stage + 1 : 0;
                        break;
                    case 3:
                        stage = (curr == 't') ? stage + 1 : 0;
                        break;
                    case 4:
                        stage = (curr == 'a') ? stage + 1 : 0;
                        break;
                    case 5:
                        stage = (curr == '"') ? stage + 1 : 0;
                        break;
                    case 6:
                        if (curr == ':')
                        { 
                            count++;
                            stage = 0;
                        }
                        else
                        {
                            sb.Add((byte)curr);
                        }
                        break;
                }
            }
            return count;
        }
    }
}
