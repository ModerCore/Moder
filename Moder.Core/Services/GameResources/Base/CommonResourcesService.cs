﻿using Moder.Core.Extensions;
using Moder.Core.Parser;
using ParadoxPower.Process;

namespace Moder.Core.Services.GameResources.Base;

public abstract class CommonResourcesService<TType, TContent>
    : ResourcesService<TType, TContent, Node>
    where TType : CommonResourcesService<TType, TContent>
{
    protected CommonResourcesService(string folderRelativePath, WatcherFilter filter)
        : base(folderRelativePath, filter) { }

    ///<inheritdoc />
    protected abstract override TContent? ParseFileToContent(Node rootNode);

    protected override Node? GetParseResult(string filePath)
    {
        if (!TextParser.TryParse(filePath, out var rootNode, out var error))
        {
            Logger.LogParseError(error);
            return null;
        }
        return rootNode;
    }
}
