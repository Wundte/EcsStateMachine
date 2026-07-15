using Microsoft.CodeAnalysis;

namespace SourceGenerators;

public static class StableId
{
    /// <summary>
    /// Get hash from system name.
    /// </summary>
    public static int Get(INamedTypeSymbol symbol)
    {
        var text = symbol.ToDisplayString(); // Avoid collision between system with the same name but in different namespaces.

        unchecked
        {
            var hash = 17;

            foreach (var c in text)
            {
                hash = hash * 31 + c;
            }

            return hash;
        }
    }
}