// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tiledriver.Core.Uwmf;

namespace Tiledriver.Core.Tests.Uwmf.Parsing
{
    public static partial class UwmfComparison
    {
        public static void AssertEqual(IEnumerable<UnknownProperty> actual, IEnumerable<UnknownProperty> expected, string context)
        {
            Func<IEnumerable<UnknownProperty>, string> toString =
                list => string.Join("\n", list.Select(up => $"{(string)up.Name} = {up.Value}"));
            Assert.That(
                toString(actual),
                Is.EqualTo(toString(expected)),
                "Different unknown properties in {0}", context);
        }

        public static void AssertEqual(UnknownBlock actual, UnknownBlock expected)
        {
            Assert.That(
                actual.Name,
                Is.EqualTo(expected.Name),
                "Different name on unknown property in unknown block.");
            AssertEqual(actual.Properties, expected.Properties, "Unknown Block");
        }
    }
}