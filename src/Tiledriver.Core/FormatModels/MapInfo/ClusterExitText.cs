// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.FormatModels.MapInfo
{
    public sealed class ClusterExitText
    {
        public string Message { get; }
        public bool MessageIsLookup { get; }

        private ClusterExitText(string message, bool messageIsLookup)
        {
            Message = message;
            MessageIsLookup = messageIsLookup;
        }

        public static ClusterExitText FromMessage(string message) => new(message, false);

        public static ClusterExitText FromLookup(string lookup) => new(lookup, true);
    }
}
