using System;
using System.Linq;

namespace DotNetExtensions.AspNetCore.Internal
{
    public class TripleGuid
    {
        private readonly string concatenatedGuids;

        public TripleGuid()
        {
            this.concatenatedGuids = string.Join(
                string.Empty, 
                Enumerable.Range(1, 3).Select(n => 
                    Guid.NewGuid().ToString("N")));
        }

        public override string ToString()
        {
            return this.concatenatedGuids;
        }
    }
}
