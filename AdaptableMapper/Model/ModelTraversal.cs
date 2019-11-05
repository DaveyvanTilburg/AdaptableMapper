﻿using AdaptableMapper.Model.Language;
using AdaptableMapper.Traversals;

namespace AdaptableMapper.Model
{
    public sealed class ModelTraversal : Traversal
    {
        public ModelTraversal(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public object Traverse(object target)
        {
            if (!(target is ModelBase model))
            {
                Process.ProcessObservable.GetInstance().Raise("MODEL#21; target is not of expected type Model", "error", Path, target);
                return new NullModel();
            }

            ModelBase pathTarget = model.NavigateToModel(Path.ToQueue());

            return pathTarget;
        }
    }
}