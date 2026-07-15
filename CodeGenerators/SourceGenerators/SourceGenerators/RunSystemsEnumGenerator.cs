using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerators;

[Generator]
public class RunSystemsEnumGenerator : IIncrementalGenerator
{
    private const string EcsRunSystemInterface = "Leopotam.EcsLite.IEcsRunSystem";
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var systems = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, _) =>
                    {
                        return node is ClassDeclarationSyntax;
                    },

                    transform: static (ctx, _) =>
                    {
                        return ctx.SemanticModel
                            .GetDeclaredSymbol(ctx.Node)
                            as INamedTypeSymbol;
                    })
                .Where(static symbol => symbol != null);


        var ecsSystems = systems.Where(static symbol =>
            {
                return symbol!
                    .Interfaces
                    .Any(i =>
                        i.ToDisplayString()
                        ==
                        EcsRunSystemInterface);
            });
        
        context.RegisterSourceOutput(
            ecsSystems.Collect(),
            static (ctx, symbols) =>
            {
                GenerateEnum(ctx, symbols);
            });
    }


    private static void GenerateEnum(SourceProductionContext context, ImmutableArray<INamedTypeSymbol?> systems)
    {
        var builder = new StringBuilder();

        builder.AppendLine(
            """
            // Generated enum

            namespace Generated
            {
                public enum EcsRunSystemsIds
                {
            """);

        var generatedIds = new Dictionary<int, string>();
        
        foreach (var system in systems)
        {
            if (system == null)
            {
                continue;
            }
            
            // Avoid hash collisions
            var id = StableId.Get(system);
            
            if (generatedIds.TryGetValue(id, out var existingSystem))
            {
                throw new Exception($"ECS System ID collision between {existingSystem} and {system.ToDisplayString()}");
            }

            generatedIds.Add(id, system.ToDisplayString());
            
            builder.AppendLine($"        {system.Name} = {id},");
        }


        builder.AppendLine(
            """
                }
            }
            """);

        context.AddSource("EcsSystemId.g.cs", builder.ToString());
    }
}