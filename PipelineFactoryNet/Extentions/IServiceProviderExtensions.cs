#nullable enable
using System;

namespace PipelineFactoryNet.Extentions
{
    internal static class IServiceProviderExtensions
    {
        public static T? GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T?)serviceProvider.GetService(typeof(T));
        }
    }
}