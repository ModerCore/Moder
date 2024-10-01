﻿using Microsoft.Extensions.DependencyInjection;
using Moder.Core.Services.GameResources;
using ParadoxPower.Parser;

// ReSharper disable once CheckNamespace
namespace Moder.Core.Models.Vo;

public sealed partial class CountryTagLeafVo(string key, Types.Value value, NodeVo? parent) : LeafVo(key, value, parent)
{
    public static IReadOnlyCollection<string> CountryTags => CountryTagService.CountryTags;

    public override string Value
    {
        get => LeafValue;
        set
        {
            SetProperty(ref LeafValue, value);
            OnPropertyChanged(nameof(CountryName));
        }
    }

    public string CountryName => LocalisationService.GetValue(Value);

    private static readonly CountryTagService CountryTagService = App
        .Current.Services.GetRequiredService<GameResourcesService>()
        .CountryTagsService;
}