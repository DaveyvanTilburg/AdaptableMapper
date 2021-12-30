using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;

namespace MappingFramework.MappingInterface.AvalonEdit.FoldingStrategies
{
    internal interface IFoldingStrategy
    {
        void UpdateFoldings(FoldingManager manager, TextDocument document);
    }
}