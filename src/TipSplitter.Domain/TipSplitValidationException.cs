namespace TipSplitter.Domain;

public sealed class TipSplitValidationException(string message) : Exception(message);
