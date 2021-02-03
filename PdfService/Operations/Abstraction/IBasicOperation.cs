using System.Collections.Generic;

namespace PdfService.Operations.Abstraction
{
    public interface IBasicOperation
    {
        void OnInit(Dictionary<string, object> appSettingsItems);
        void OnRelease();
    }
}
