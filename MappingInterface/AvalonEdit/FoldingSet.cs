using System;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using MappingFramework.ContentTypes;
using MappingFramework.MappingInterface.AvalonEdit.FoldingStrategies;
using XmlFoldingStrategy = MappingFramework.MappingInterface.AvalonEdit.FoldingStrategies.XmlFoldingStrategy;

namespace MappingFramework.MappingInterface.AvalonEdit
{
    internal struct FoldingSet
        {
            private readonly FoldingManager _foldingManager;
            private readonly IFoldingStrategy _foldingStrategy;
            private readonly TextEditor _textEditor;

            private FoldingSet(FoldingManager foldingManager, IFoldingStrategy foldingStrategy, TextEditor textEditor)
            {
                _foldingManager = foldingManager;
                _foldingStrategy = foldingStrategy;
                _textEditor = textEditor;
            }

            public void Update()
                => _foldingStrategy.UpdateFoldings(_foldingManager, _textEditor.Document);

            public static FoldingSet Create(TextEditor textEditor, ContentType contentType)
            {
                var foldingManager = FoldingManager.Install(textEditor.TextArea);
                
                switch (contentType)
                {
                    case ContentType.Xml:
                        return new FoldingSet(foldingManager, new XmlFoldingStrategy(), textEditor);
                    case ContentType.Json:
                        return new FoldingSet(foldingManager, new BraceFoldingStrategy(), textEditor);
                }

                throw new Exception();
            }
        }
}
