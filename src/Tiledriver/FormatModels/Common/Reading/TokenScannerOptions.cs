namespace Tiledriver.FormatModels.Common.Reading;

public sealed record TokenScannerOptions(
	bool ReportNewlines = false,
	bool AllowDollarIdentifiers = false,
	bool AllowPipes = false
);
