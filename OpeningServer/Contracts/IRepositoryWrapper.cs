﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        Task SaveChangesAsync();
    }
}