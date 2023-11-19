using System;

namespace QuickShipParser
{
    public class Choice : IPattern
    {
        private IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text)
        {
            foreach (var pattern in patterns)
            {
                IMatch match = pattern.Match(text);

                if (match.Success())
                {
                    return match;
                }
            }

            return new FailedMatch(text);
        }

        public void Add(IPattern newPattern)
        {
            Array.Resize(ref this.patterns, this.patterns.Length + 1);
            this.patterns[^1] = newPattern;
        }
    }
}
