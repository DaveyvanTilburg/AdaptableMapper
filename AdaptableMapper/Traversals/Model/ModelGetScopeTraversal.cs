﻿using AdaptableMapper.Model;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableMapper.Traversals.Model
{
    public sealed class ModelGetScopeTraversal : GetScopeTraversal
    {
        public ModelGetScopeTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public IEnumerable<object> GetScope(object source)
        {
            if (!(source is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#12; source is not of expected type Model", "error", Path, source);
                return new List<object>();
            }

            var modelPathContainer = PathContainer.Create(Path);

            List<ModelBase> pathTargets = model.NavigateToAllModels(modelPathContainer.CreatePathQueue()).ToList();
            List<object> modelScopes = pathTargets
                .Where(m => m.IsValid())
                .Select(p => p.GetListProperty(modelPathContainer.LastInPath))
                .SelectMany(l => (IEnumerable<object>)l)
                .ToList();

            return modelScopes;
        }
    }
}