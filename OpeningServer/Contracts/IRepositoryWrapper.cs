using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IProjectRepository Project { get; }
        IDrawingRepository Drawing { get; }
        IElementManagementRepository ElementManagement { get; }
        IElementRepository Element { get; }
        IGeometryVersionRepository GeometryVersion { get; }
        ICheckoutVersionRepository CheckoutVersion { get; }
        IRevisionRepository Revision { get; }

        void Save();
    }
}