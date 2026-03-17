using System.Collections.Immutable;
using Tiledriver.FormatModels.Common;
using Tiledriver.FormatModels.Common.Reading;

namespace Tiledriver.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

public sealed record IntTuple(FilePosition StartLocation, ImmutableArray<IntegerToken> Values);
