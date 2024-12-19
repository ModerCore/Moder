﻿// SPDX-License-Identifier: MIT

/*
MIT License
Copyright (c) 2023 dSPACE GmbH, Germany, Contributed by Carsten Igel <CIgel@dspace>
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Moder.Hosting;

public static class HostBuilderExtensions
{
    public static IHostApplicationBuilder ConfigureAvalonia<TApplication>(
        this IHostApplicationBuilder hostBuilder,
        Func<TApplication> applicationResolver,
        Action<AppBuilder> configureAppBuilder,
        IHostedLifetime? lifetime = null
    )
        where TApplication : Application
    {
        return hostBuilder.ConfigureAvaloniaAppBuilder<TApplication>(
            () => AppBuilder.Configure(applicationResolver),
            configureAppBuilder,
            lifetime
        );
    }

    public static IHostApplicationBuilder ConfigureAvaloniaAppBuilder<TApplication>(
        this IHostApplicationBuilder hostBuilder,
        Action<AppBuilder> configureAppBuilder,
        IHostedLifetime? lifetime = null
    )
        where TApplication : Application
    {
        ArgumentNullException.ThrowIfNull(configureAppBuilder);

        hostBuilder.Services.AddSingleton<TApplication>();
        hostBuilder.Services.AddSingleton(sp => AppBuilder.Configure(sp.GetRequiredService<TApplication>));
        
        hostBuilder.Services.AddSingleton<Application>(sp => sp.GetRequiredService<TApplication>());
        hostBuilder.Services.AddSingleton<IHostedLifetime>(provider =>
            lifetime
            ?? HostedLifetime.Select(
                provider.GetRequiredService<ILoggerFactory>(),
                provider.GetRequiredService<Application>().ApplicationLifetime
            )
        );
        return hostBuilder;
    }
}