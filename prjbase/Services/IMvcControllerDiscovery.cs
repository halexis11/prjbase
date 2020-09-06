using prjbase.Controllers;
using System.Collections.Generic;

namespace prjbase.Services
{
    public interface IMvcControllerDiscovery
    {
        IEnumerable<MvcControllerInfo> GetControllers();
    }
}